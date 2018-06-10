using System;

namespace Aspects
{
    public interface IPropertyAspectAdvisor<T>
     where T : IPropertyAspectAdvice
    {
        T Advice { get; }
    }

    public interface IPropertyAspectAdvice
    {
        void AfterPropertyGet(object target, string property);

        void AfterPropertySet(object target, string property);

        void BeforePropertyGet(object target, string property);

        void BeforePropertySet(object target, string property);
    }

    public class GenericPropertyAspectAdvice : IPropertyAspectAdvice
    {
        public Action<object, string> AfterPropertyGetAdvice { get; set; }
        public Action<object, string> AfterPropertySetAdvice { get; set; }
        public Action<object, string> BeforePropertyGetAdvice { get; set; }
        public Action<object, string> BeforePropertySetAdvice { get; set; }

        public void AfterPropertyGet(object target, string property)
        {
            AfterPropertyGetAdvice?.Invoke(target, property);
        }

        public void AfterPropertySet(object target, string property)
        {
            AfterPropertySetAdvice?.Invoke(target, property);
        }

        public void BeforePropertyGet(object target, string property)
        {
            BeforePropertyGetAdvice?.Invoke(target, property);
        }

        public void BeforePropertySet(object target, string property)
        {
            BeforePropertySetAdvice?.Invoke(target, property);
        }
    }
}