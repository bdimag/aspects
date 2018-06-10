using System;

namespace Aspects
{
    public interface IMethodAspectAdvisor<T>
         where T : IMethodAspectAdvice
    {
        T Advice { get; }
    }

    public interface IMethodAspectAdvice
    {
        void AfterCall(object target, string method, object[] arguments);

        void BeforeCall(object target, string method, object[] arguments);
    }


    public class GenericMethodAspectAdvice : IMethodAspectAdvice
    {

        public Action<object, string, object[]> AfterCallAdvice { get; set; }
        public Action<object, string, object[]> BeforeCallAdvice { get; set; }

        public void AfterCall(object target, string method, object[] arguments)
        {
            AfterCallAdvice?.Invoke(target, method, arguments);
        }

        public void BeforeCall(object target, string method, object[] arguments)
        {
            BeforeCallAdvice?.Invoke(target, method, arguments);
        }
    }
}