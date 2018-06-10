using System;
using System.ComponentModel;
using System.Reflection;

namespace Aspects
{
    public class NotifyPropertyChangedAdvice : IPropertyAspectAdvice
    { 
        public void AfterPropertyGet(object target, string propertyName)
        {
        }

        public void AfterPropertySet(object target, string propertyName)
        {
            var senderType = target.GetType();
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(senderType))
            {
                var propertyChanged = (PropertyChangedEventHandler)senderType.GetField(nameof(INotifyPropertyChanged.PropertyChanged), BindingFlags.NonPublic | BindingFlags.Instance).GetValue(target);
                if (propertyChanged != null)
                {
                    propertyChanged.DynamicInvoke(new object[] { target, new PropertyChangedEventArgs(propertyName) });
                }
            }
        }

        public void BeforePropertyGet(object target, string propertyName)
        {
        }

        public void BeforePropertySet(object target, string propertyName)
        {
        }
    }
}
