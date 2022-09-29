using ImageSdkWrapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsDemoApplication.Popup
{
    internal class SaveDialogPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        ShellPage _Owner;

        MyCheckBox _MultiplePagesCB;
        StackLayout _PaperFormatLayout=new StackLayout();
        ImageWriter.EImageFileType _CurrentImageFileType;
        ImageWriter.EPaperFormatConfigValues _CurrentPaperFormat;

        Dictionary<ImageWriter.EImageFileType, string> _Types = new Dictionary<ImageWriter.EImageFileType, string>();
        Dictionary<ImageWriter.EPaperFormatConfigValues, string> _PaperFormats = new Dictionary<ImageWriter.EPaperFormatConfigValues, string>();

        public SaveDialogPage(ShellPage a)
        {
            _Owner = a;

            _Types.Add(ImageWriter.EImageFileType.Jpeg, "JPEG");
            _Types.Add(ImageWriter.EImageFileType.Tiff, "TIFF G4");
            _Types.Add(ImageWriter.EImageFileType.Png, "PNG");
            //_Types.Add(ImageWriter.EImageFileType.PngExt, "PNG (SDK)");
            _Types.Add(ImageWriter.EImageFileType.Pdf, "PDF");
            _Types.Add(ImageWriter.EImageFileType.PdfPng, "PDF/PNG");

            _PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.Unknown, "Default");
            _PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.HalfLetter, "Predefined (half letter)");
            _PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.A5, "Custom (eq. A5)");
            _PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.Terminator, "Extensible");


            var m = new ScrollView() { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
            var f = new Frame();
            m.Content = f;
            var mainlist = new StackLayout();// { Spacing = 10 };
            f.Content = mainlist;
                        
            var fileFormats = new StackLayout();
            var label = new Label() { Text = "Save format", Margin = new Thickness(0, 0, 0, 10) };
            fileFormats.Children.Add(label);
            mainlist.Children.Add(fileFormats);

            _CurrentImageFileType = _Owner._Record.WriterType;
            if (_CurrentImageFileType == ImageWriter.EImageFileType.PngExt) _CurrentImageFileType = ImageWriter.EImageFileType.Png;
            fileFormats.Spacing = 0;
            foreach (var it in _Types.Keys)
            {
                var r = new RadioButton() { Content= _Types[it],IsChecked= _CurrentImageFileType == it };
                r.FontSize = label.FontSize;
                r.CheckedChanged += ImageType_CheckedChanged;
                fileFormats.Children.Add(r);
            }

            _PaperFormatLayout.Children.Add(new Label() { Text = "Some PDF page configurations", Margin = new Thickness(0, 0, 0, 5) });
            _PaperFormatLayout.Spacing = 0;
            _CurrentPaperFormat = _Owner._Record.PaperFormat;
            foreach (var it in _PaperFormats)
            {
                var r = new RadioButton() { Content = it.Value, IsChecked = _CurrentPaperFormat== it.Key };
                r.CheckedChanged += PaperFornat_CheckedChanged;
                r.FontSize=label.FontSize;
                _PaperFormatLayout.Children.Add(r);
            }
            _PaperFormatLayout.Margin = new Thickness(0, 10, 0, 0);
            mainlist.Children.Add(_PaperFormatLayout);


            mainlist.Children.Add(_MultiplePagesCB=new MyCheckBox("Simulate multiple pages",a._Record.MultiPages));
            
            var ok = new Button() { Text = "OK",HorizontalOptions=LayoutOptions.End };
            ok.Clicked += OnOK;
            mainlist.Children.Add(ok);

            UpdateVisible();

            base.Content = m;
        }

        async private void OnOK(object sender, EventArgs e)
        {
            SaveImageTask.Params p = new SaveImageTask.Params();
            p.writerType = _CurrentImageFileType;
            p.multiPages = _MultiplePagesCB.IsChecked;
            p.paperFormat = _CurrentPaperFormat;

            if (p.writerType == ImageWriter.EImageFileType.Png)
            {
                if (_Owner._Record.LastProcessingMode == EProcessing.BW)
                {
                    p.writerType = ImageWriter.EImageFileType.PngExt;
                }
            }
            
            try
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            }
            catch { };
                        

            _Owner.DoSaveImage(p, true);
        }


        private void ImageType_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string name = (string)((RadioButton)sender).Content;
            foreach(var p in _Types)
            {
                if (p.Value==name)
                {
                    _CurrentImageFileType = p.Key;
                    break;
                }
            }

            UpdateVisible();
        }

        void UpdateVisible()
        {
            _MultiplePagesCB.IsVisible = (_CurrentImageFileType == ImageWriter.EImageFileType.Pdf) || (_CurrentImageFileType == ImageWriter.EImageFileType.PdfPng)
                || (_CurrentImageFileType == ImageWriter.EImageFileType.Tiff);
            _PaperFormatLayout.IsVisible = (_CurrentImageFileType == ImageWriter.EImageFileType.Pdf) || (_CurrentImageFileType == ImageWriter.EImageFileType.PdfPng);
        }

        private void PaperFornat_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string name = (string)((RadioButton)sender).Content;
            foreach (var p in _PaperFormats)
            {
                if (p.Value == name)
                {
                    _CurrentPaperFormat = p.Key;
                    break;
                }
            }
            
        }

        
    }
}
