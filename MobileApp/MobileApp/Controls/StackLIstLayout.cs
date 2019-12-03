using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileApp.Controls
{
    public class StackLIstLayout : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(StackLIstLayout),
                default(IEnumerable<object>),
                BindingMode.TwoWay, propertyChanged: ItemsSourceChanged);


        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(StackLayout), default(DataTemplate));


        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsLayout = (StackLIstLayout)bindable;
            itemsLayout.SetItems();
        }

        public StackLIstLayout()
        {
            
        }

        protected virtual void SetItems()
        {
            Children.Clear();

           
            if (ItemsSource == null)
            {
                return;
            }

            var counter = 0;
            foreach (var item in ItemsSource)
            {
               
                var v = GetItemView(item);
                Children.Add(v);
                counter++;
            }
        }


        protected virtual View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();

            if (!(content is View view))
            {
                return null;
            }

            view.BindingContext = item;
            return view;
        }
    }
}
