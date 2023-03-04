using ImageSdkWrapper.Forms.Camera;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinFormsDemoApplication.Popup
{
    internal class CameraSettingsDialogPage :  MyBaseDialogPage
    {
        MyCheckBox ShakeDetection = new MyCheckBox("Shake detection",Settings.ShakeDetection);
        MyCheckBox DocumentArea = new MyCheckBox("Document area", Settings.DocumentArea);
        MyCheckBox TrapezoidDistortion = new MyCheckBox("Trapezoid distortion", Settings.TrapezoidDistortion);
        public CameraSettingsDialogPage(double top):base(top)
        {
            MainLayout.Spacing = MenuDivSize;
            AddItem(ShakeDetection);
            AddItem(DocumentArea);
            AddItem(TrapezoidDistortion);

            ShakeDetection.CheckedChanged += ShakeDetection_CheckedChanged;
            DocumentArea.CheckedChanged += DocumentArea_CheckedChanged;
            TrapezoidDistortion.CheckedChanged += TrapezoidDistortion_CheckedChanged;
        }

        private void TrapezoidDistortion_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Settings.TrapezoidDistortion = TrapezoidDistortion.IsChecked;
        }

        private void DocumentArea_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Settings.DocumentArea = DocumentArea.IsChecked;
        }

        private void ShakeDetection_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Settings.ShakeDetection = ShakeDetection.IsChecked;
        }
    }
}
