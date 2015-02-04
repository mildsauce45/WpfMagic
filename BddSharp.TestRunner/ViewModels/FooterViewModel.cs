using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfMagic.Mvvm;

namespace BddSharp.TestRunner.ViewModels
{
    public class FooterViewModel : NotifyableObject
    {
        private bool? allTestsPassed;
        private string statusBarText;

        public bool? AllTestsPassed
        {
            get { return allTestsPassed; }
            set
            {
                allTestsPassed = value;
                NotifyPropertyChanged(() => AllTestsPassed);
            }
        }

        public string StatusBarText
        {
            get { return statusBarText; }
            set
            {
                statusBarText = value;
                NotifyPropertyChanged(() => StatusBarText);
            }
        }
    }
}
