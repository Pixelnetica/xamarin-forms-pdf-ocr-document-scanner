using ImageSdkWrapper;
using ImageSdkWrapper.Forms;
using ImageSdkWrapper.Forms.Camera;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinFormsDemoApplication
{
    public partial class CameraMainPage : ContentPage
    {
        ImageSdkWrapper.Forms.PxlScannerView _PxlScannerView;

        public MetaImage OldMetaImage;

        public Thickness SafeAreaPadding { get => _PxlScannerView.SafeAreaPadding; set { _PxlScannerView.SafeAreaPadding = value; } }
        public CameraMainPage()
        {
            _PxlScannerView = new ImageSdkWrapper.Forms.PxlScannerView(); ;
            _PxlScannerView.VerticalOptions = LayoutOptions.FillAndExpand;
            _PxlScannerView.HorizontalOptions = LayoutOptions.FillAndExpand;
            
            _PxlScannerView.PictureReceiver = MyPictureReceiver;
            _PxlScannerView.OnReadyForStart += OnReadyForStart;
            _PxlScannerView.CloseClicked += OnCloseClicked;

            //PxlScannerView.Msg.shot_busy = "Camera busy";
            //PxlScannerView.Msg.shot_not_stable = "Camera is shaking";
            //PxlScannerView.Msg.looking_for_document = "Looking for document";
            //PxlScannerView.Msg.small_area = "Move Camera closer to document";
            //PxlScannerView.Msg.distorted = "Hold camera parallel to document";
            //PxlScannerView.Msg.unstable = "Don\'t move camera";

            this.Content = _PxlScannerView;
        }

        protected override void OnDisappearing()
        {
            _PxlScannerView.Dispose();
            base.OnDisappearing();
        }

        void OnReadyForStart(object sender, System.EventArgs e)        
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await CheckPermissions();
                if (!_PxlScannerView.StartCamera())
                {
                    MyTools.MessageBox("Start camera error!");
                }
            });            
        }

        protected override bool OnBackButtonPressed()
        {            
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new MainPage(OldMetaImage);
                });
            });
            return true;

        }

        public async Task CheckPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    MyTools.MessageBox("No permissions for Camera");
                }
            }
        }

        void OnCloseClicked(object sender,EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Xamarin.Forms.Application.Current.MainPage = new MainPage(OldMetaImage);
            });
        }

        void MyPictureReceiver(MetaImage img, string errorTextOrNull)
        {
#if DEBUG
            Console.WriteLine(String.Format("time:Receive picture {0}ms", (int)(DateTime.UtcNow - _PxlScannerView.DebugTakePictureUtcTime).TotalMilliseconds));
#endif   

            if (img == null)
            {
                _PxlScannerView.Restart();
                MyTools.MessageBox(errorTextOrNull);
            }
            else
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    Xamarin.Forms.Application.Current.MainPage = new MainPage(img);
#if DEBUG
                    Console.WriteLine(String.Format("time:Afte shllpage {0}ms", (int)(DateTime.UtcNow - _PxlScannerView.DebugTakePictureUtcTime).TotalMilliseconds));
#endif
                }
                );
            }
        }

    }
}
