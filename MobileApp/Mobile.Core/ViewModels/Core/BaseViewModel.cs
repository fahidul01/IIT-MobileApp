using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Core.ViewModels
{
    public abstract class BaseViewModel : ViewModelBase
    {
        public abstract void OnAppear(params object[] args);
    }
}
