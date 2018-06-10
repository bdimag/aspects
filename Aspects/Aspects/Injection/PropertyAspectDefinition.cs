using Mono.Cecil;
using System.Linq;

namespace Aspects.Injection
{
    public class PropertyAspectDefinition : AspectDefinition
    {
        /// <summary>
        /// AfterPropertyGet method of the Advice
        /// </summary>
        public MethodReference AfterPropertyGet { get; private set; }

        /// <summary>
        /// AfterPropertySet method of the Advice
        /// </summary>
        public MethodReference AfterPropertySet { get; private set; }

        /// <summary>
        /// BeforePropertyGet method of the Advice
        /// </summary>
        public MethodReference BeforePropertyGet { get; private set; }

        /// <summary>
        /// BeforePropertySet method of the Advice
        /// </summary>
        public MethodReference BeforePropertySet { get; private set; }

        public PropertyAspectDefinition(string assembly, string type, ModuleDefinition targetModule) : base(assembly, type, targetModule)
        {
        }

        public PropertyAspectDefinition(ModuleDefinition aspectModule, string type, ModuleDefinition targetModule) : base(aspectModule, type, targetModule)
        {
        }

        internal override void LoadAdvice()
        {
            AfterPropertySet = targetModule.ImportReference(aspectAdviceType.Methods.Single(m => m.Name == "AfterPropertySet"));
            BeforePropertySet = targetModule.ImportReference(aspectAdviceType.Methods.Single(m => m.Name == "BeforePropertySet"));
            AfterPropertyGet = targetModule.ImportReference(aspectAdviceType.Methods.Single(m => m.Name == "AfterPropertyGet"));
            BeforePropertyGet = targetModule.ImportReference(aspectAdviceType.Methods.Single(m => m.Name == "BeforePropertyGet"));
        }
    }
}