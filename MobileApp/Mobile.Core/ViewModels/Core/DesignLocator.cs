using Mobile.Core.Engines.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Core.ViewModels
{
    public static  class DesignLocator
    {
        public static LoginViewModel LoginViewModel => Locator.GetInstance<LoginViewModel>();
        public static ForgetPassViewModel ForgetPassViewModel => Locator.GetInstance<ForgetPassViewModel>();
    }
}
