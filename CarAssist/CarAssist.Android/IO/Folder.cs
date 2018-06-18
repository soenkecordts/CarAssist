using System;
using Xamarin.Forms;
using CarAssist.IO;

[assembly: Dependency(typeof(CarAssist.Droid.IO.Folder))]
namespace CarAssist.Droid.IO
{
    public class Folder : IFolder
    {
        public string Personal => Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    }
}
