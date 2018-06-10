using Aspects.Injection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System;
using System.Linq;
using System.Reflection;

public class Inject : Task
{
    [Required]
    public string Target { get; set; }


    public override bool Execute()
    {
        ModuleDefinition targetModule = ModuleDefinition.ReadModule(Target);
        ModuleDefinition aspectsModule = ModuleDefinition.ReadModule(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath);

        var changed = false;

        foreach (var type in targetModule.GetTypes().Skip(1))
        {
            foreach (var method in type.Methods)
            {
                foreach (var attr in method.CustomAttributes)
                {
                    // TODO need to be able to determine if attribute extends `MethodAspectAttribute` and implements `IMethodAspectAdvisor<>`
                    if (attr.AttributeType.Name == "RequiresAttribute")
                    {
                        var aspect = new MethodAspectDefinition(aspectsModule, attr.AttributeType.Name, targetModule);

                        var processor = method.Body.GetILProcessor();

                        method.Body.SimplifyMacros();

                        // create a new instance of the attribute and access the Advice property
                        var advisor = processor.Create(OpCodes.Newobj, aspect.Attribute);
                        var advice = processor.Create(OpCodes.Call, aspect.Advice);

                        // call the AfterPropertySet method on the Advice 
                        var callBeforeCall = processor.Create(OpCodes.Callvirt, aspect.BeforeCall);
                        var callAfterCall = processor.Create(OpCodes.Callvirt, aspect.AfterCall);

                        // passing in `this` and the property name
                        var arg1 = processor.Create(OpCodes.Ldarg_0);
                        var arg2 = processor.Create(OpCodes.Ldstr, method.Name);
                        var arg3 = processor.Create(OpCodes.Ldnull);

                        // inject instructions to the beggining of the method
                        processor.InsertBefore(method.Body.Instructions[0], advisor);
                        processor.InsertAfter(advisor, advice);
                        processor.InsertAfter(advice, arg1);
                        processor.InsertAfter(arg1, arg2);
                        processor.InsertAfter(arg2, arg3);
                        processor.InsertAfter(arg3, callBeforeCall);

                        // inject instructions before the end of the method
                        var ret = processor.Body.Instructions.Last();
                        var nop = processor.Create(OpCodes.Nop);
                        processor.InsertBefore(ret, advisor);
                        processor.InsertBefore(ret, advice);
                        processor.InsertBefore(ret, arg1);
                        processor.InsertBefore(ret, arg2);
                        processor.InsertBefore(ret, arg3);
                        processor.InsertBefore(ret, callAfterCall);
                        processor.InsertBefore(ret, nop);

                        method.Body.OptimizeMacros();

                        changed = true;
                    }
                }
            }

            foreach (var property in type.Properties)
            {
                foreach (var attr in property.CustomAttributes)
                {
                    // TODO need to be able to determine if attribute extends `PropertyAspectAttribute` and implements `IPropertyAspectAdvisor<>`
                    if (attr.AttributeType.Name == "NotifyPropertyChangedAttribute")
                    {
                        var aspect = new PropertyAspectDefinition(aspectsModule, attr.AttributeType.Name, targetModule);
                    
                        // get method body
                        var method = property.SetMethod;
                        var processor = method.Body.GetILProcessor();

                        // create a new instance of the attribute and access the Advice property
                        var advisor = processor.Create(OpCodes.Newobj, aspect.Attribute);
                        var advice = processor.Create(OpCodes.Call, aspect.Advice);

                        // call the AfterPropertySet method on the Advice 
                        var callProcessAfter = processor.Create(OpCodes.Callvirt, aspect.AfterPropertySet);

                        // passing in `this` and the property name
                        var arg1 = processor.Create(OpCodes.Ldarg_0);
                        var arg2 = processor.Create(OpCodes.Ldstr, property.Name);

                        // inject instructions before the end of the method
                        var ret = processor.Body.Instructions.Last();
                        processor.InsertBefore(ret, advisor);
                        processor.InsertBefore(ret, advice);
                        processor.InsertBefore(ret, arg1);
                        processor.InsertBefore(ret, arg2);
                        processor.InsertBefore(ret, callProcessAfter);

                        // TODO AfterPropertyGet
                        // TODO BeforePropertyGet
                        // TODO BeforePropertySet

                        changed = true;
                    }
                }
            }
        }
        
        // save assembly
        if (changed)
        {
            targetModule.Write(System.Text.RegularExpressions.Regex.Replace(Target, @"\.(\w+)$", ".new.$1"));
        }
    
        return true;
    }
}
 