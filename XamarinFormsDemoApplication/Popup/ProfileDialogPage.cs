using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinFormsDemoApplication.Popup
{
    internal class ProfileDialogPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        ShellPage _Owner;
        EProcessing _CurrentProfile;
        MyCheckBox _StrongShadows;

        public ProfileDialogPage(ShellPage a)
        {
            _Owner = a;

            var m = new ScrollView() { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
            var f = new Frame();
            m.Content = f;
            var mainlist = new StackLayout();// { Spacing = 10 };
            f.Content = mainlist;

            var profilesLayout = new StackLayout();
            var label = new Label() { Text = "Color Profile", Margin = new Thickness(0, 0, 0, 10) };
            profilesLayout.Children.Add(label);
            mainlist.Children.Add(profilesLayout);

            _CurrentProfile = _Owner._Record.LastProcessingMode;
            foreach (var it in Processing.Instance.Profiles)
            {
                var r = new RadioButton() { Content = it.Value, IsChecked = _CurrentProfile == it.Key };
                r.FontSize = label.FontSize;
                r.CheckedChanged += R_CheckedChanged;
                profilesLayout.Children.Add(r);
            }
                  
            mainlist.Children.Add(_StrongShadows = new MyCheckBox("Strong shadows", a._Record.StrongShadow));
            _StrongShadows.CheckedChanged += _StrongShadows_CheckedChanged;

            base.Content = m;
        }

        private void _StrongShadows_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            _Owner._Record.StrongShadow = e.Value;
            DoCropImage();
        }

        private void R_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!e.Value) return ;

            string value = (string)((RadioButton)sender).Content;
            foreach(var p in Processing.Instance.Profiles)
            {
                if(p.Value==value)
                {
                    _CurrentProfile=p.Key;
                    _Owner._Record.LastProcessingMode = p.Key;
                    DoCropImage();
                    break;
                }
            }
        }

        async void DoCropImage()
        {
            try
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            }
            catch { };

            _Owner.DoCropImage(false);
        }
    }
}
