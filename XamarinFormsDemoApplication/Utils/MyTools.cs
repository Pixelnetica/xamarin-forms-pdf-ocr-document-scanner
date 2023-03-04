using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinFormsDemoApplication
{
    public class MyTools
    {
        public static void Log(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                Console.WriteLine(s);
            }
        }

        public static void MessageBox(string caption,string msg)
        {
            App.Current.MainPage.DisplayAlert(caption,msg, "OK");
        }

        public static void MessageBox( string msg)
        {
            App.Current.MainPage.DisplayAlert("", msg, "OK");
        }

        public async static void AboutMeMessageBox()
        {
            string appname;
            string msg = string.Format("Version {0} ({1})",GetAppVersion(out appname), ImageSdkWrapper.Main.GitHashValue.Substring(0,7));

            var result = await App.Current.MainPage.DisplayAlert(appname, msg, "More info", "OK");
            if (result)
            {
                Xamarin.Essentials.Browser.OpenAsync("https://www.pixelnetica.com/products/document-scanning-sdk/document-scanner-sdk.html");
            }
        }

        public static string GetAppVersion(out string appname)
        {
            appname = "Error!";
            try
            {
                appname = Xamarin.Essentials.AppInfo.Name;
                return Xamarin.Essentials.AppInfo.VersionString;
            }
            catch { }
            return "";
        }
    }
}
