using CarAssist.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(CarAssist.UWP.IO.Folder))]
namespace CarAssist.UWP.IO
{
    public class Folder : IFolder
    {
        public string Personal => ApplicationData.Current.LocalFolder.Path;
    }
}
