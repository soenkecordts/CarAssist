using Acr.UserDialogs;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarAssist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewTripPage : ContentPage
	{
        bool _cancel = false;
        string _fileName = null;

		public ViewTripPage (string fileName)
		{
			InitializeComponent ();
            _fileName = fileName;
		}

        async protected override void OnAppearing()
        {
            Title = Path.GetFileNameWithoutExtension(_fileName);

            using (var progress = UserDialogs.Instance.Loading("CarAssist", OnCancel, "Abbrechen", maskType: MaskType.Black))
            {
                using (var reader = new StreamReader(File.OpenRead(_fileName)))
                {
                    string line = null;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (_cancel == true)
                            break;
                        editor.Text += line + "\r\n";
                    }
                }
            }
            base.OnAppearing();
        }

        private void OnCancel()
        {
            _cancel = true;
        }
    }
}