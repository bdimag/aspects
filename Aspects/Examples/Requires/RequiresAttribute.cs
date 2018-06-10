using System;
using System.Collections.Generic;
using System.Linq;

namespace Aspects
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequiresAttribute : MethodAspectAttribute, IMethodAspectAdvisor<GenericMethodAspectAdvice>
    {
        private readonly string[] requiredProperties;

        private GenericMethodAspectAdvice advice;

        public GenericMethodAspectAdvice Advice
        {
            get
            {
                return advice = (advice ?? new GenericMethodAspectAdvice
                {
                    BeforeCallAdvice = (target, method, arguments) =>
                    {
                        var attributes = target.GetType().GetMethod(method).GetCustomAttributes(typeof(RequiresAttribute), false).Cast<RequiresAttribute>();
                        foreach (var attribute in attributes)
                        {
                            foreach (var requiredProperty in attribute.requiredProperties)
                            {
                                var member = target.GetType().GetProperty(requiredProperty, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                                if (member == null || member.GetValue(target) == null)
                                {
                                    throw new MissingMemberException(target.GetType().Name, requiredProperty);
                                }
                            }
                        }
                    }
                });
            }
        }

        public RequiresAttribute() : this(new string[] { })
        {

        }

        public RequiresAttribute(params string[] requiredProperties)
        {
            this.requiredProperties = requiredProperties;
        }
    }
}