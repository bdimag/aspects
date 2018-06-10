using Aspects;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Aspects_Example
{
    public class Vm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Property First
        private string first;

        /// <summary>
        /// PropertyChanged event handled normally--by defining the getter/setter and
        /// invoking the PropertyChanged event manually. The Elvis operator `?.` helps
        /// a bit by not requiring the more verbose `if (PropertyChanged != null)`
        /// </summary>
        public string First
        {
            get
            {
                return first;
            }
            set
            {
                first = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(First)));
            }
        }
        #endregion

        #region Property Second
        private string second;

        void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// PropertyChanged event handling can be moved to a central location, but still
        /// requires that the getter/setter are defined and some code is invoked.
        /// </summary>
        public string Second
        {
            get
            {
                return second;
            }
            set
            {
                second = value;
                OnPropertyChanged(nameof(Second));
            }
        }
        #endregion

        #region Property Third
        /// <summary>
        /// PropertyChanged event handled by an aspect--invocation injected during compile.
        /// </summary>
        [NotifyPropertyChanged]
        public string Third { get; set; }
        #endregion

        #region DoWork
        #region Additional DoWork `ICommand` Setup
        private string _doWorkResult;
        
        public ICommand DoWorkCommand { get; set; }

        public string DoWorkResult
        {
            get
            {
                return _doWorkResult;
            }
            set
            {
                _doWorkResult = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DoWorkResult)));
            }
        }

        public Vm()
        {
            DoWorkCommand = new View.ViewModel.RelayCommand(
                (param) =>
                {
                    try
                    {
                        DoWork();
                    }
                    catch (Exception ex)
                    {
                        DoWorkResult = $"Exception {ex.GetType().Name}: {ex.Message}";
                    }
                },
                (param) => { return true; }
            );
        }
        #endregion

        /// <summary>
        /// It can be assumed that `First` and `Second` are set -- if not, an aspect can be configured to, e.g., throw a well handled exception before the code is reached.
        /// </summary>
        [Requires(nameof(First), nameof(Second))]
        public void DoWork()
        {
            DoWorkResult = $"Values are {First}, {Second}.";
        }
        #endregion
    }
}