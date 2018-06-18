
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{
        public ListView MenuList { get { return menuList; } }

        public MasterPage ()
		{
			InitializeComponent ();
		}
	}
}