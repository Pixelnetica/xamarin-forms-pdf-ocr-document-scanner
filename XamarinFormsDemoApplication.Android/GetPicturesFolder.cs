using System.Runtime.CompilerServices;
using Xamarin.Forms;

#if DEBUG2
[assembly: System.Runtime.CompilerServices.Dependency(typeof(XamarinFormsDemoApplication.Utils.GetPicturesFolderService))]

namespace XamarinFormsDemoApplication.Utils
{
    class GetPicturesFolderService : Utils.IGetPicturesFolder
    {
        public string GetPicturesFolder()
        {

            var p = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
            if (p != null)
            {
                return p.Path;
            }
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
        }
    }
}
#endif