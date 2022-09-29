using Xamarin.Forms;

[assembly: Dependency(typeof(XamarinFormsDemoApplication.Utils.GetPicturesFolderService))]

namespace XamarinFormsDemoApplication.Utils
{
    class GetPicturesFolderService : Utils.IGetPicturesFolder
    {
        public string GetPicturesFolder()
        {

            var p = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            return p;
        }
    }
}