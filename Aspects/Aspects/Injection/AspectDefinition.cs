using Mono.Cecil;
using System.Linq;

namespace Aspects.Injection
{
    public abstract class AspectDefinition
    {
        internal readonly PropertyDefinition aspectAdvice;
        internal readonly TypeDefinition aspectAdviceType;
        internal readonly TypeDefinition aspectAdvisor;
        internal readonly ModuleDefinition aspectModule;
        internal readonly ModuleDefinition targetModule;

        /// <summary>
        /// Getter for the Advice property of the AspectAttribute
        /// </summary>
        public MethodReference Advice { get; private set; }

        /// <summary>
        /// Constructor for the AspectAttribute
        /// </summary>
        public MethodReference Attribute { get; private set; }

        public AspectDefinition(string assembly, string type, ModuleDefinition targetModule) : this(ModuleDefinition.ReadModule(assembly), type, targetModule)
        {
        }

        public AspectDefinition(ModuleDefinition aspectModule, string type, ModuleDefinition targetModule)
        {
            this.targetModule = targetModule;
            aspectAdvisor = aspectModule.Types.Single(t => t.Name == type);
            aspectAdvice = aspectAdvisor.Properties.Single(p => p.Name == "Advice");
            aspectAdviceType = aspectModule.Types.Single(t => t.Name == aspectAdvice.PropertyType.Name);

            Advice = targetModule.ImportReference(aspectAdvice.GetMethod);
            Attribute = targetModule.ImportReference(aspectAdvisor.Methods.Single(m => m.Name == ".ctor" && !m.HasParameters));

            LoadAdvice();
        }

        internal abstract void LoadAdvice();
    }
}