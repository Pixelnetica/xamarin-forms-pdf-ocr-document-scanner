using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinFormsDemoApplication.Popup
{
    internal class MyCheckBox : StackLayout
    {
        CheckBox _CB;

        public event EventHandler<CheckedChangedEventArgs> CheckedChanged;
        internal bool IsChecked { get => _CB.IsChecked; set => _CB.IsChecked = value; }
        public MyCheckBox(string text,bool value)
        {
            Orientation = StackOrientation.Horizontal;
            Children.Add(_CB = new CheckBox() { IsChecked = value, VerticalOptions = LayoutOptions.Center });
            var l = new Label() { Text = text, VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center };
            l.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => { _CB.IsChecked = !_CB.IsChecked; }) });
            Children.Add(l);

            l.TextColor= Popup.MyBaseDialogPage.MenuFontColor;
            l.FontSize = Popup.MyBaseDialogPage.MenuFontSize;
            l.HeightRequest= Popup.MyBaseDialogPage.MenuItemHeight;

            _CB.CheckedChanged += _CB_CheckedChanged;
        }

        private void _CB_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (CheckedChanged != null) CheckedChanged(this, e);
        }
    }
}
