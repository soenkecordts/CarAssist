namespace CarAssist.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new CarAssist.App());
        }
    }
}
