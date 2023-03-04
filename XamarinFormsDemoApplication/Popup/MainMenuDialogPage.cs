using ImageSdkWrapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsDemoApplication.Popup
{
    internal class MyMenuItem
    {
        public string Caption;
        public System.Action OnClick;

        public MyMenuItem(string caption, System.Action ev)
        {
            Caption = caption;
            OnClick = ev;
        }
    }

    internal class MainMenuDialogPage : MyBaseDialogPage
    {

        public MainMenuDialogPage( List<MyMenuItem> menu,double top):base(top)
        {  
            foreach (var it in menu)
            {
                var r = new Label() { Text = it.Caption};
                
                r.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(
                    async () => {
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                        it.OnClick();
                        }) 
                });
                AddItem(r);
            }

        }
    }
}
