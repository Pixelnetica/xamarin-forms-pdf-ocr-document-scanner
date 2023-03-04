using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinFormsDemoApplication.Droid;

[assembly: ExportRenderer(typeof(Shell), typeof(MyShellRenderer))]
namespace XamarinFormsDemoApplication.Droid
{
    public class MyShellRenderer : ShellRenderer
    {
        public MyShellRenderer(Context context) : base(context)
        {
        }
        protected override IShellFlyoutRenderer CreateShellFlyoutRenderer()
        {
            var flyoutRenderer = base.CreateShellFlyoutRenderer();
            flyoutRenderer.AndroidView.Touch += AndroidView_Touch;
            return flyoutRenderer;
        }
        private void AndroidView_Touch(object sender, Android.Views.View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Move)
                e.Handled = true;
            else
                e.Handled = false;
        }
    }
}


