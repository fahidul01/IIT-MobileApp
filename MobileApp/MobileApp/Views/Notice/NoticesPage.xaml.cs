﻿using Mobile.Core.ViewModels;
using MobileApp.Controls;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Notice
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoticesPage : CustomTabPage<NoticesViewModel>
    {
        public NoticesPage()
        {
            InitializeComponent();
        }
    }
}