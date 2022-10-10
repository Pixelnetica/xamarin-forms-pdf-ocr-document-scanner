using ImageSdkWrapper;
using ImageSdkWrapper.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace XamarinFormsDemoApplication
{
    public partial class ShellPage : Shell
    {
        internal MainRecord _Record = new MainRecord();

        ImageSdkWrapper.Forms.PxlCropImageView _CropImageFormsView;

        MenuItem _CropButton;
        StackLayout _TopLayout = new StackLayout();
        
        public ShellPage(MetaImage InitMetaImage)
        {
            CreateMenuItems();
            
            
            _Record.LoadPreferencies();
            
            /*if (InitMetaImage != null)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        OpenImage("", InitMetaImage);
                    });
                });
            }*/

            if(InitMetaImage!=null) OpenImage("", InitMetaImage);
#if DEBUG2
            if (Device.RuntimePlatform == Device.Android)
            {
                Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(1000);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        OpenDemo();
                    });
                });
            }
#endif
        }

       
        protected override bool OnBackButtonPressed()
        {
            if(_CropImageFormsView.Active)
            {
                _CropImageFormsView.Active = false;
                return true;
            }

#if _USE_V3_
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Xamarin.Forms.Application.Current.MainPage = new CameraMainPage();
                });
            });
            return true;
#else
            return false;
#endif

        }

        void StatusLog(string s)
        {
            Console.WriteLine(s);
        }

        void CreateMenuItems()
        {
            if (Items.Count > 0)
            {
                while (Items.Count > 1) Items.RemoveAt(1);
            }
            else
            {
                //Items.Clear();
                Items.Add(new FlyoutItem
                {
                    Title = Xamarin.Essentials.AppInfo.Name,
                    Items =
                    {
                        new Tab
                        {
                          Items = { new ShellContent {Content = CreatePicturePage()} }
                        }
                    }
                });
            };

            Items.Add(new MenuItem
            {
                Text = "Open",
                Command = new Command(() => { base.FlyoutIsPresented = false; OpenImage_ClickedAsync(); })
            });

            bool isImagePressent = _Record.IsSourceImagePrssent;

            if (isImagePressent)
            {
                Items.Add(_CropButton = new MenuItem
                {
                    Text = "Manual Crop",
                    Command = new Command(() => { base.FlyoutIsPresented = false; ManualCropButton_Clicked(); })
                });

                Items.Add(_CropButton = new MenuItem
                {
                    Text= "Color Profile",
                    Command = new Command(() => { base.FlyoutIsPresented = false; CropButton_Clicked(); })
                });
                
                Items.Add(new MenuItem
                {
                    Text = "Save to file",
                    Command = new Command(() => { base.FlyoutIsPresented = false; SaveButton_Clicked(true); })
                });
            }

            Items.Add(new MenuItem
            {
                Text = "About",
                Command = new Command(() => { base.FlyoutIsPresented = false;

                    

                    //MyTools.AboutMeMessageBox(); 
                    Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Popup.AboutDialogPage());
                })
                //Text = "License info",
                //Command = new Command(() => { base.FlyoutIsPresented = false; LicenseInfo_Clicked(); })
            });

            
        }

        ContentPage CreatePicturePage()
        {
            ContentPage resultPage = new ContentPage();
            
            var mainLayout = new StackLayout() { VerticalOptions = LayoutOptions.FillAndExpand,HorizontalOptions= LayoutOptions.FillAndExpand };
            mainLayout.Spacing = 0;
            
            _CropImageFormsView = new ImageSdkWrapper.Forms.PxlCropImageView(){ VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };            
            
            _CropImageFormsView.ActveChanged += _CropImageFormsView_ActveChanged;

            mainLayout.Children.Add(_CropImageFormsView);
            
            resultPage.Content = mainLayout;

            _CropImageFormsView.ToolBarBackgroundColor= Color.Transparent;
            Shell.SetTitleView(resultPage, (View)_TopLayout);
            _TopLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            var v = _CropImageFormsView.TakeToolBar();
            v.VerticalOptions = LayoutOptions.CenterAndExpand;
            _TopLayout.Children.Add(v);

            //customization of CropImageFormsView
            _CropImageFormsView.ToolBarHeight = 25;
            _CropImageFormsView.ToolBarBackgroundColor = _CropImageFormsView.ToolBarBackgroundColor;
                        
            _CropImageFormsView.NonActiveEdgeColor = Color.FromUint(4278229452u);
            _CropImageFormsView.EdgeActiveColor = Color.FromUint(4278246911u);
            _CropImageFormsView.EdgeMoveColor = Color.FromUint(1342234111u);
            _CropImageFormsView.EdgeInvalidColor = Color.FromUint(4294919236u);
            _CropImageFormsView.CornerActiveColor = Color.FromUint(4281519410u);
            _CropImageFormsView.CornerMoveColor = Color.FromUint(2150812978u);
            _CropImageFormsView.CornerInvalidColor = Color.FromUint(4294919236u);

            _CropImageFormsView.TouchRadius = _CropImageFormsView.TouchRadius;

            _CropImageFormsView.CornderRadius = _CropImageFormsView.CornderRadius;
            _CropImageFormsView.EdgeThickness = _CropImageFormsView.EdgeThickness;

            Color buttonColor = Color.Transparent;
            _CropImageFormsView.RotateLeftButton.BackgroundColor = buttonColor;
            _CropImageFormsView.RotateRightButton.BackgroundColor = buttonColor;
            _CropImageFormsView.CloseButton.BackgroundColor = buttonColor;
            _CropImageFormsView.SelectAllButton.BackgroundColor = buttonColor;
            _CropImageFormsView.SelectOrigenalButton.BackgroundColor = buttonColor;
            
            //_CropImageFormsView.RotateLeftButton.Source = MyLoadImg("ic_action_rotate_left");
            //_CropImageFormsView.RotateRightButton.Source = MyLoadImg("ic_action_rotate_right");
            //_CropImageFormsView.CloseButton.Source = MyLoadImg("ic_action_accept");
            //_CropImageFormsView.SelectAllButton.Source = MyLoadImg("ic_action_select_all");
            //_CropImageFormsView.SelectOrigenalButton.Source = MyLoadImg("ic_action_view_as_grid");

            return resultPage;
        }
        ImageSource MyLoadImg(string s)
        {
            return ImageSource.FromResource("XamarinFormsDemoApplication.Resources." + s + ".png", typeof(ShellPage).Assembly);
        }


        bool _bSkipActveChanged = false;
        private void _CropImageFormsView_ActveChanged(object sender, EventArgs e)
        {
            Device.InvokeOnMainThreadAsync(() => _TopLayout.IsVisible = _CropImageFormsView.Active);
            if (!_CropImageFormsView.Active)
            {
                if (!_bSkipActveChanged)
                {
                    DoCropImage(false);
                }
            }
        }


#region OpenImage
        private async void OpenImage_ClickedAsync()
        {
            try
            {
                BeforeProcess();

                var options = new PickOptions
                {
                    PickerTitle = "Please select a picture file",
                    FileTypes = FilePickerFileType.Images
                };

                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    OpenImage(result.FullPath,null);
                }
                else
                {
                    UpdateView(false,true,false);
                }
            }
            catch (Exception ex)
            {
                MyTools.MessageBox("Error!", ex.ToString());
            }
        }

        void OpenDemo()
        {
            OpenImage("/storage/emulated/0/sdcard/Download/demo2l.jpg", null);
        }

        public void OpenImage(string uri,MetaImage source)
        {
            bool old = _Record.IsSourceImagePrssent;
            BeforeProcess();

#if DEBUG
            Console.WriteLine("open uri "+uri.ToString());
#endif

            StatusLog("Opening image. Please wait...");
            Task.Run(() =>
            {
                _Record.OpenSourceImage(uri,source);                

                
                if(!string.IsNullOrEmpty(_Record.LastStateText))
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MyTools.MessageBox(_Record.LastStateText);
                    });
                }
                
                if(!_Record.DetectedDocumentCorners.IsInited)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CreateMenuItems();
                    });
                    DoCropImage(true);
                }
                else
                {
                    UpdateView(old != _Record.IsSourceImagePrssent, true, false);
                }
            });
        }

#endregion OpenImage

#region Crop

        async void CropButton_Clicked()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Popup.ProfileDialogPage(this));            
        }

        void ManualCropButton_Clicked()
        {
            DoCropImage(true);
        }

        public void DoCropImage(bool source)
        {
            BeforeProcess();
            StatusLog("Processing...");
            Task.Run(() =>
            {                
                //if (source)
                //{
                //    // Special case: reset to source
                //    _Record.OnShowSource();
                //}
                //else
                {
                    _Record.OnCropImage(!source);
                }

                UpdateView(false, true, source);
            });
        }
                
#endregion


#region Save image
        async void SaveButton_Clicked(bool needShare)
        {
            var page = new Popup.SaveDialogPage(this);
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(page);
        }

        internal async void DoSaveImage(SaveImageTask.Params prm, bool needShare)
        {           

            if (await CheckPermission())
            {
                _ = Task.Run(() =>
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        StatusLog("Saving image. Please wait...");
                        BeforeProcess();
                    });

                    _Record.WriterType = prm.writerType;
                    _Record.PaperFormat = prm.paperFormat;
                    _Record.MultiPages = prm.multiPages;

                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

#if DEBUG2
                    needShare = false;


                    if (!needShare)
                    {
                        path = "/sdcard/Download/";

                        //var p = DependencyService.Get<Utils.IGetPicturesFolder>();
                        //if (p != null)
                        //{
                        //    path = p.GetPicturesFolder();
                        //}
                    }
#endif

                    prm.filePath = System.IO.Path.Combine(path, string.Format("Pixelnetica-SdkDemo-{0:X08}.jpg", DateTime.UtcNow.Ticks));
                    //*/
                    //string fileName = System.IO.Path.Combine(path, string.Format("temp.jpg", DateTime.UtcNow.Ticks));
                    //if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);

                    _Record.OnSaveImage(prm,needShare);
                    UpdateView(false,false,_CropImageFormsView.Active);
                });
            }
            else
            {
                MyTools.MessageBox("Error!", "No write access!");
            }
        }

        async Task<bool> CheckPermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                }
                return status == PermissionStatus.Granted;

            }
            catch (Exception ex)
            {
                //Something went wrong
                MyTools.MessageBox("Error!", ex.ToString());
            }

            return await Task.FromResult(false);
        }

#endregion Save image

#region Update picture and state

        DateTime _StartUtcTine = DateTime.UtcNow;

        void UpdateView(bool needUpdateMenu,bool needUpdateMetaImage,bool editMode)
        {
            DateTime updateTime = DateTime.UtcNow;


            if (needUpdateMenu)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CreateMenuItems();
                });
            }

            //bool editMode = _Record.ImageMode == MainRecord.ImageState.Source;
            if (needUpdateMetaImage)
            {
                _CropImageFormsView.UiSetMetaImage(_Record.DisplayBitmap, !editMode, _Record.DocumentCorners, _Record.DetectedDocumentCorners);
            }

            _bSkipActveChanged = true;
            //_CropImageFormsView.ActveChanged -= _CropImageFormsView_ActveChanged; //not update second time
            _CropImageFormsView.Active = editMode;
            _bSkipActveChanged = false;
            //_CropImageFormsView.ActveChanged += _CropImageFormsView_ActveChanged;

            var now = DateTime.UtcNow;
            StatusLog(string.Format("[{0}+{1}ms] {2}", (int)(now - _StartUtcTine).TotalMilliseconds,
                (int)(now - updateTime).TotalMilliseconds, _Record.LastStateText));
            //});
        }

        
        void BeforeProcess()
        {
            _StartUtcTine = DateTime.UtcNow;
        }
#endregion Update picture and state

        void LicenseInfo_Clicked()
        {
            var info=Main.GetLicenseInfo();
            if(info==null)MyTools.MessageBox("None");
            else
            {
                string msg = string.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5:X}", info.Status, info.ClientName, info.ClientExtraInfo, info.UtcValidSubscriptionTs, info.UtcValidTs, info.Features);
                MyTools.MessageBox(msg);
            }
        }

        void Test_Clicked()
        {
            var info = Main.GetLicenseInfo();
            if (info == null) MyTools.MessageBox("None");
            else
            {
                string msg = string.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5:X}", info.Status, info.ClientName, info.ClientExtraInfo, info.UtcValidSubscriptionTs, info.UtcValidTs, info.Features);
                MyTools.MessageBox(msg);
            }
        }
    }
}