using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Threading.Tasks;

namespace Directiva10.Droid
{
    [Activity(Label = "Directiva",
        Theme = "@style/MyTheme.Splash",
        MainLauncher = true,
        NoHistory = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task TaskInicio = new Task(() => { SimularInicio(); });
            TaskInicio.Start();
        }

        public override void OnBackPressed() { }

        private async void SimularInicio()
        {
            await Task.Delay(600);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}