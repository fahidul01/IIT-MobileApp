using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public List<MenuItem> MenuItems { get; private set; }
        public MainViewModel()
        {
            MenuItems = new List<MenuItem>()
            {
                new MenuItem("Home","",typeof(HomeViewModel)),
                new MenuItem("Courses","",typeof(CoursesViewModel)),
                new MenuItem("Notices","",typeof(NoticesViewModel))
            };
        }

        public ICommand FlyoutCommand => new RelayCommand<MenuItem>(FlyoutAction);

        private void FlyoutAction(MenuItem obj)
        {
            _nav.NavigateTo(obj.Type);
        }
    }

    public class MenuItem
    {
        public string Title { get; private set; }
        public string Icon { get; private set; }
        public Type Type { get; private set; }

        public MenuItem(string title, string icon, Type type)
        {
            Title = title;
            Icon = icon;
            Type = type;
        }
    }
}
