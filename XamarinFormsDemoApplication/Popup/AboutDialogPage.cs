using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinFormsDemoApplication.Popup
{
    internal class AboutDialogPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public AboutDialogPage()
        {
            this.ControlTemplate = null;
            

            string appname;
            string pkgName = Xamarin.Essentials.AppInfo.PackageName;
            string msg = pkgName+"\n"+string.Format("Version {0} ({1})", MyTools.GetAppVersion(out appname), ImageSdkWrapper.Main.GitHashValue.Substring(0, 7));

            var m = new ScrollView() { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
            var f = new Frame();
            m.Content = f;
            var mainlist = new StackLayout();// { Spacing = 10 };
            f.Content = mainlist;

            StackLayout hdr = new StackLayout() { Orientation = StackOrientation.Horizontal };
            mainlist.Children.Add(hdr);
                        
            Image img = new Image() { Source = ImageSource.FromFile("icon.png") };
            img.VerticalOptions = LayoutOptions.FillAndExpand;
            img.HeightRequest = 25;
            hdr.Children.Add(img);

            var headerTxt = new Label() { Text = appname };
            //header.FontSize= header.FontSize*1.5
            headerTxt.FontAttributes = FontAttributes.Bold;
            headerTxt.VerticalOptions= LayoutOptions.Center;
            hdr.Children.Add(headerTxt);
                        
            var label = new Label() { Text = msg };
            mainlist.Children.Add(label);

            {
                StackLayout s = new StackLayout() { Orientation = StackOrientation.Horizontal };
                s.Children.Add(new Label() { Text = "Powered by " });
                s.Spacing = 0;

                var url = new Label() { Text = "Document Scaning SDK" };
                s.Children.Add(url);
                url.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => { OnDocumentScaningSdkUrl(); }) });
                url.TextDecorations = TextDecorations.Underline;
                url.TextColor = Color.Blue;
                mainlist.Children.Add(s);
            }
            
            mainlist.Children.Add(new Label() { Text = "©Pixelnetica. All rights reserved." });
            mainlist.Children.Add(new Label() { Text = "" });
            {
                var url = new Label() { Text = "DSSDK Support" };
                url.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => { OnDSSDKkUrl(); }) });
                url.TextDecorations = TextDecorations.Underline;
                url.TextColor = Color.Blue;
                mainlist.Children.Add(url);
            }

            var ok = new Button() { Text = "OK", HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 10, 0, 0) };
            ok.Clicked += OnOK;
            mainlist.Children.Add(ok);

            base.Content = m;
        }

        async void OnOK(object o,EventArgs e)
        {
            try
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            }
            catch { };

        }

        void OnDocumentScaningSdkUrl()
        {
            Xamarin.Essentials.Browser.OpenAsync("https://www.pixelnetica.com/products/document-scanning-sdk/document-scanner-sdk.html?utm_source=EasyScan&utm_medium=src-xamarin_forms&utm_campaign=scr-about&utm_content=dssdk-overview");
        }

        void OnDSSDKkUrl()
        {
            Xamarin.Essentials.Browser.OpenAsync("https://www.pixelnetica.com/products/document-scanning-sdk/sdk-support.html?utm_source=EasyScan&utm_medium=src-xamarin_forms&utm_campaign=scr-about&utm_content=dssdk-support");
        }

    }
}
