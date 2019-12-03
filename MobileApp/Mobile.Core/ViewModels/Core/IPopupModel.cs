using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Mobile.Core.ViewModels.Core
{
    public interface IPopupModel
    {
        ICommand DataCommand { get; set; }
    }
}
