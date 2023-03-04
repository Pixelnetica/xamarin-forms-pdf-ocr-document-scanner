using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinFormsDemoApplication.Popup
{
    internal class MyBaseDialogPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        protected StackLayout MainLayout = new StackLayout();

        public static Color MenuFontColor = Color.Black;
        public static double MenuFontSize = 24;
        public static double MenuDescriptionFontSize = 24;
        
        public static double MenuItemHeight = 48;
        public static double MenuDivSize = 14;

        public MyBaseDialogPage(double top)
        {
            this.ControlTemplate = null;
            var myLabel = new Label();
            //MenuFontSize =  Device.GetNamedSize(NamedSize.Medium, myLabel);
            MenuDescriptionFontSize = 14;
            MenuFontSize = 14;

            var m = new ScrollView() { VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.Start };
            m.Margin = new Thickness(0, top, 0, 0);
            var f = new Frame() { Content = MainLayout,Padding=new Thickness(12,8) };
            
            m.Content = f;
            MainLayout.Spacing = 0;
            

            base.Content = m;        
        }

        protected void AddItem(View v)
        {
            v.HeightRequest = MenuItemHeight;
            if(v is Label)
            {
                var l = v as Label;
                l.VerticalOptions = LayoutOptions.Center;
                l.FontSize = MenuFontSize;
                l.TextColor = MenuFontColor;
            }
            else
            if (v is StackLayout)
            {
                var l = v as StackLayout;
                l.VerticalOptions = LayoutOptions.Center;
            }
            MainLayout.Children.Add(v);
        }
    }
}
