using System;
using Xamarin.Forms;
using CarAssist.IO;

[assembly: Dependency(typeof(CarAssist.iOS.IO.Folder))]
namespace CarAssist.iOS.IO
{
    public class Folder : IFolder
    {
        public string Personal => Environment.GetFolderPath(Environment.SpecialFolder.Personal);

    }
}
