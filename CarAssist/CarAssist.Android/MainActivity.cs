using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Android.Bluetooth;
using CarAssist.Droid.Connection;

namespace CarAssist.Droid
{
    [Activity(Label = "CarAssist", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Acr.UserDialogs.UserDialogs.Init(this);

            if (!new Bluetooth().IsEnabled)
            {
                Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableBtIntent, 0);
            }

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.FormsMaps.Init(this, bundle);                           // Maps

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;    // Location
            var app = new App();
            LoadApplication(app);
        }

        // Location
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

