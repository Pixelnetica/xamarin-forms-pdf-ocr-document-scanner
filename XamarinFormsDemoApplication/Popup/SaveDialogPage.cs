using ImageSdkWrapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsDemoApplication.Popup
{
    internal class SaveDialogPage : MyBaseDialogPage
    {
        MainPage _Owner;

        MyCheckBox _MultiplePagesCB;
        StackLayout _PaperFormatLayout=new StackLayout();
        ImageWriter.EImageFileType _CurrentImageFileType;
        ImageWriter.EPaperFormatConfigValues _CurrentPaperFormat;

        Label _WidthLabel = new Label() { Text="Width", TextColor = MenuFontColor, FontSize = MenuDescriptionFontSize };
        Label _HeightLabel = new Label() { Text = "Height",TextColor=MenuFontColor, FontSize = MenuDescriptionFontSize };
        Entry _WidthEditor=new Entry() { TextColor = MenuFontColor, FontSize = MenuDescriptionFontSize };
        Entry _HeightEditor = new Entry() { TextColor = MenuFontColor, FontSize = MenuDescriptionFontSize };

        Dictionary<ImageWriter.EImageFileType, string> _Types = new Dictionary<ImageWriter.EImageFileType, string>();
        Dictionary<ImageWriter.EPaperFormatConfigValues, string> _PaperFormats = new Dictionary<ImageWriter.EPaperFormatConfigValues, string>();

        public SaveDialogPage(MainPage a,double top):base(top)
        {
            _Owner = a;
            MainLayout.Spacing = MenuDivSize;

            _Types.Add(ImageWriter.EImageFileType.Jpeg, "JPEG");
            _Types.Add(ImageWriter.EImageFileType.Tiff, "TIFF G4");
            _Types.Add(ImageWriter.EImageFileType.Png, "PNG");
            //_Types.Add(ImageWriter.EImageFileType.PngExt, "PNG (SDK)");
            _Types.Add(ImageWriter.EImageFileType.Pdf, "PDF");
            _Types.Add(ImageWriter.EImageFileType.PdfPng, "PDF/PNG");

            _PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.A4, "A4");
            _PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.A5, "A5");
            _PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.A6, "A6");
            //_PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.HalfLetter, "Predefined (half letter)");
            //_PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.Legal, "Legal)");
            //_PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.JuniorLegal, "Custom (eq. A5)");
            //_PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.Leger, "Leger");
            _PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.BusinessCard, "Business Card");
            //_PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.BusinessCard2, "BusinessCard2");
            //_PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.ReceiptMobile, "ReceiptMobile");
            //_PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.ReceiptStation, "ReceiptStation");
            _PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.ReceiptKitchen, "Receipt");
            _PaperFormats.Add(ImageWriter.EPaperFormatConfigValues.Terminator, "Custom in mm");


                        
            var fileFormats = new StackLayout();
            var label = new Label() { 
                Text = "Save format", 
                Margin = new Thickness(0, 0, 0, 10), 
                FontSize = MenuDescriptionFontSize 
            };

            fileFormats.Children.Add(label);
            MainLayout.Children.Add(fileFormats);

            _CurrentImageFileType = _Owner._Record.WriterType;
            if (_CurrentImageFileType == ImageWriter.EImageFileType.PngExt) _CurrentImageFileType = ImageWriter.EImageFileType.Png;
            fileFormats.Spacing = 0;
            foreach (var it in _Types.Keys)
            {
                var r = new RadioButton() { Content= _Types[it],IsChecked= _CurrentImageFileType == it };
                r.FontSize = MenuFontSize;
                r.TextColor = MenuFontColor;
                r.HeightRequest = MenuItemHeight;
                r.CheckedChanged += ImageType_CheckedChanged;
                fileFormats.Children.Add(r);
            }

            _PaperFormatLayout.Children.Add(new Label() { 
                Text = "Some PDF page configurations", 
                Margin = new Thickness(0, 0, 0, 5), 
                FontSize = MenuDescriptionFontSize });

            _PaperFormatLayout.Spacing = 0;
            _CurrentPaperFormat = _Owner._Record.PaperFormat;
            foreach (var it in _PaperFormats)
            {
                var r = new RadioButton() { Content = it.Value, IsChecked = _CurrentPaperFormat== it.Key };
                r.CheckedChanged += PaperFornat_CheckedChanged;
                r.FontSize=MenuFontSize;
                r.TextColor = MenuFontColor;
                r.HeightRequest = MenuItemHeight;
                _PaperFormatLayout.Children.Add(r);
            }
            

            var g = new Grid();
            g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            g.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = 100 });
            g.Children.Add(_WidthLabel, 0, 0);
            g.Children.Add(_HeightLabel, 0, 1);

            _WidthEditor.Text = _Owner._Record.CustomPageWidth.ToString();
            _HeightEditor.Text = _Owner._Record.CustomPageHeight.ToString();
            g.Children.Add(_WidthEditor, 1, 0);
            g.Children.Add(_HeightEditor, 1, 1);
            UpdateEditorsState();

            _PaperFormatLayout.Children.Add(g);
            _PaperFormatLayout.Margin = new Thickness(0, 10, 0, 0);
            MainLayout.Children.Add(_PaperFormatLayout);

            MainLayout.Children.Add(_MultiplePagesCB=new MyCheckBox("Simulate multiple pages",a._Record.MultiPages));
            
            var ok = new Button() { Text = "OK",HorizontalOptions=LayoutOptions.End, FontSize = MenuFontSize };
            ok.Clicked += OnOK;
            MainLayout.Children.Add(ok);

            UpdateVisible();
        }

        async private void OnOK(object sender, EventArgs e)
        {
            SaveImageTask.Params p = new SaveImageTask.Params();
            p.writerType = _CurrentImageFileType;
            p.multiPages = _MultiplePagesCB.IsChecked;
            p.paperFormat = _CurrentPaperFormat;
            int.TryParse(_WidthEditor.Text,out p.CustomPageWidth);
            int.TryParse(_HeightEditor.Text, out p.CustomPageHeight);

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
                    UpdateEditorsState();
                    break;
                }
            }            
        }

        void UpdateEditorsState()
        {
            bool custom = _CurrentPaperFormat == ImageWriter.EPaperFormatConfigValues.Terminator;
            _HeightEditor.IsEnabled = custom;
            _WidthEditor.IsEnabled = custom;
            _WidthLabel.IsEnabled = custom;
            _HeightLabel.IsEnabled = custom;
        }

        
    }
}
