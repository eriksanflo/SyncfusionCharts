
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Rg.Plugins.Popup.Services;
using Android;
using System.Threading.Tasks;

namespace Directiva10.Droid
{
    [Activity(Label = "Directiva", 
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme", 
        MainLauncher = false,
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            /* Inicializando Plugins Inicio */
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            /* Inicializando Plugins Fin */

            /* Solicita permiso al usuario */
            await IntentarObtenerPermisosdelUsuario();

            LoadApplication(new App());
        }

        // Método necesario para el Paquete NutGet Rg.Plugins.Popup
        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
                PopupNavigation.PopAsync();
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        #region PermisosenTiempodeEjecucion

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /* Permisos para acceder a la memoria externa */
        async Task IntentarObtenerPermisosdelUsuario()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await ObtenerPermisoAsync();
                return;
            }
        }

        const int solicitarIdUbicacion = 0;

        readonly string[] ArrayGrupoPermisos =
        {
            /* Agrega todo los permisos requeridos */
            Manifest.Permission.Internet,
        };

        async Task ObtenerPermisoAsync()
        {
            RequestPermissions(ArrayGrupoPermisos, solicitarIdUbicacion);
        }

        #endregion
    }
}