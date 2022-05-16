using Directiva10.A_General;
using Directiva10.IniciodeSesion;
using Directiva10.Pin;
using Directiva10.Plantilla;
using ElementosComunes.Conexiones;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Directiva10
{
    /* NOTA IMPORTANTE:
     * Xamarin Forms de 5.0.0.2125 a 5.0.0.2337 en iOS se queda en blanco al crear el Menu Shell, se rompe con la propiedad FlyoutBackdrop, porque ya esta obsoleta.
     * Xamarin Forms de 5.0.0.2012 es la mas estable pero inferior a esta se pierde caracteristicas del menú Shell.
     * En Xamarin Forms 5 falla ListView en iOS, mejor usar ahora CollectionView.
    */
    public partial class App : Application
    {
        private object ObjectBloqueo;
        private bool BotonOcupado;
        private TControlIniciodeSesion ControlIniciodeSesion;

        public App()
        {
            InitializeComponent();
            ObjectBloqueo = new object();
            BotonOcupado = false;
            TControlPrincipal.SetVariablesdeProyecto();
            ControlIniciodeSesion = new TControlIniciodeSesion();
            MainPage = new CPDefault();
        }

        protected override void OnStart()
        {
            TControlPrincipal.InicializarTemadeApp();
            Application.Current.RequestedThemeChanged += CambiodeTemaRequestedThemeChanged;
            if (TConexionLocal.ExisteBasedeDatos() && ControlIniciodeSesion.SesionIniciada())
            {
                /* Se asignan la variable comunes de sesión 
                   En método App.xaml.cs -> OnStart y OnResume
                   En el contructor  CPInicio.xaml.cs -> TControlPrincipal.SetVariablesComunes(); ponerlo antes del InitializeComponent(); */
                TControlPrincipal.SetVariablesComunes();
                if (!ControlIniciodeSesion.AnalizandoIntegridadBD())
                    MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeTitulo"], "Se realizó una actualización de la App, le sugerimos relice estos pasos para mantener la integridad de su información:\n  Abra el módulo de ayuda y presione el botón de verificar integridad.", "OK");
                MainPage = new NavigationPage(new CPPin());
            }
            else
                MainPage = new CPIniciodeSesion();
        }

        protected override void OnSleep()
        {
            TControlPrincipal.InicializarTemadeApp();
            Application.Current.RequestedThemeChanged -= CambiodeTemaRequestedThemeChanged;
        }

        protected override void OnResume()
        {
            TControlPrincipal.InicializarTemadeApp();
            Application.Current.RequestedThemeChanged -= CambiodeTemaRequestedThemeChanged;
            /* Se asignan la variable comunes de sesión */
            if (TConexionLocal.ExisteBasedeDatos() && ControlIniciodeSesion.SesionIniciada())
            {
                TControlPrincipal.SetVariablesComunes();
            }
        }

        private void CambiodeTemaRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TControlPrincipal.InicializarTemadeApp();
            });
        }
    }
}
