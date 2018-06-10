using Mono.Cecil;
using System.Linq;

namespace Aspects.Injection
{
    public class MethodAspectDefinition : AspectDefinition
    {
        /// <summary>
        /// AfterPropertySet method of the Advice
        /// </summary>
        public MethodReference AfterCall { get; private set; }

        /// <summary>
        /// BeforePropertySet method of the Advice
        /// </summary>
        public MethodReference BeforeCall { get; private set; }

        public MethodAspectDefinition(string assembly, string type, ModuleDefinition targetModule) : base(assembly, type, targetModule)
        {
        }

        public MethodAspectDefinition(ModuleDefinition aspectModule, string type, ModuleDefinition targetModule) : base(aspectModule, type, targetModule)
        {
        }

        internal override void LoadAdvice()
        {
            AfterCall = targetModule.ImportReference(aspectAdviceType.Methods.Single(m => m.Name == "AfterCall"));
            BeforeCall = targetModule.ImportReference(aspectAdviceType.Methods.Single(m => m.Name == "BeforeCall"));
        }
    }
}