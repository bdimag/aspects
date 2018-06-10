using System;

namespace Aspects
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : PropertyAspectAttribute, IPropertyAspectAdvisor<NotifyPropertyChangedAdvice>
    {
        private NotifyPropertyChangedAdvice advice;
        public NotifyPropertyChangedAdvice Advice
        {
            get
            {
                return advice = (advice ?? new NotifyPropertyChangedAdvice());
            }
        }
    }
}
