using Android.OS;
using Directiva10.Droid.ServiciosdeDependencia;
using Directiva10.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(TBarradeEstadoAndroid))]
namespace Directiva10.Droid.ServiciosdeDependencia
{
    public class TBarradeEstadoAndroid : IBarradeEstado
    {
        public void AplicarColoraBarradeEstado(System.Drawing.Color ColorBarradeEstado, bool PINTAR_OSCURO_BARRA_DE_ESTADO)
        {
            if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Lollipop)
                return;
            var activity = Platform.CurrentActivity;
            var windows = activity.Window;
            windows.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
            windows.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
            windows.SetStatusBarColor(ColorBarradeEstado.ToPlatformColor());

            if (Build.VERSION.SdkInt <= Android.OS.BuildVersionCodes.Q)
            {
                var bandera = (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LightStatusBar;
                windows.DecorView.SystemUiVisibility = PINTAR_OSCURO_BARRA_DE_ESTADO ? bandera : 0;
            }
        }
    }
}