using Xamarin.Forms;

namespace CarAssist.IO
{
    public static class Folder
    {
        public static IFolder Current { get; } = DependencyService.Get<IFolder>();
        public static string Personal => Current.Personal;
    }
}
