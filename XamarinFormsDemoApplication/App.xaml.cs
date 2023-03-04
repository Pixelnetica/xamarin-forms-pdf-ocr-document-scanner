using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsDemoApplication
{
    public partial class App : Application
    {
        public App()
        {
            //MainPage = new CameraMainPage();
            MainPage = new MainPage(null);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
