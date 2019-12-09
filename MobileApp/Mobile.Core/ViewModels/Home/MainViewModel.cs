using GalaSoft.MvvmLight.Command;
using Mobile.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public bool IsPresented { get; set; }
        public List<MenuItem> MenuItems { get; private set; }
        public MainViewModel()
        {
            MenuItems = new List<MenuItem>()
            {
                new MenuItem("Home",IconType.Home,typeof(HomeViewModel)),
                new MenuItem("Courses",IconType.Class,typeof(CoursesViewModel)),
                new MenuItem("Notices",IconType.Notifications,typeof(NoticesViewModel)),
                new MenuItem("Result",IconType.Grade,typeof(GradesViewModel))
            };
        }

        public ICommand FlyoutCommand => new RelayCommand<MenuItem>(FlyoutAction);

        private void FlyoutAction(MenuItem obj)
        {
            IsPresented = false;
            _nav.NavigateTo(obj.Type);
        }
    }

    public class MenuItem
    {
        public string Title { get; private set; }
        public IconType Icon { get; private set; }
        public Type Type { get; private set; }

        public MenuItem(string title, IconType icon, Type type)
        {
            Title = title;
            Icon = icon;
            Type = type;
        }
    }
}
