using Directiva10.A_Compartir;
using Directiva10.A_General;
using Directiva10.Plantilla;
using ElementosComunes.Conexiones;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.OpenWhatsApp;
using Xamarin.Forms.Xaml;

namespace Directiva10.IniciodeSesion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPIniciodeSesion : ContentPage
    {
        private double Ancho, Alto;
        private object ObjectBloqueo;
        private bool BotonOcupado;

        TViewModelIniciodeSesion ViewModelIniciodeSesion;

        public CPIniciodeSesion()
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            TConexionLocal.EliminaBasedeDatos();
            ViewModelIniciodeSesion = new TViewModelIniciodeSesion();
            InitializeComponent();
            BindingContext = ViewModelIniciodeSesion;
        }

        protected async override void OnSizeAllocated(double width, double height)
        {
            await Task.Delay(300);
            base.OnSizeAllocated(width, height);
            if (width > 0 && height > 0)
            {
                if (Ancho != width && Alto != height)
                {
                    Ancho = width;
                    Alto = height;
                    if (Ancho > Alto)
                        ViewModelIniciodeSesion.MostrarVistaVertical(false, Ancho);
                    else
                        ViewModelIniciodeSesion.MostrarVistaVertical(true, Ancho);
                }
            }
        }

        private void XamlEntryUsuarioCompleted(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(XamlEntryUsuario.Text) && !string.IsNullOrWhiteSpace(XamlEntryUsuario.Text))
                XamlEntryContrasena.Focus();
        }

        private void XamlEntryContrasenaCompleted(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(XamlEntryContrasena.Text) && !string.IsNullOrWhiteSpace(XamlEntryContrasena.Text))
                XamlEntryDistribucion.Focus();
        }

        private async void XamlButtonIniciarSesionClicked(object sender, EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlButtonIniciarSesion.IsEnabled = false;
            }
            try
            {
                TControlIniciodeSesion ControlIniciodeSesion = new TControlIniciodeSesion();
                JObject JObjectRespuesta = await ControlIniciodeSesion.IniciarSesion();
                if (JObjectRespuesta.Value<bool>("Respuesta"))
                {
                    TControlPrincipal.SetVariablesComunes();
                    SMenuLateral SMenuLateralPrincipal = new SMenuLateral();
                    SMenuLateralPrincipal.GoToAsync("//RutaInicio");
                    Application.Current.MainPage = SMenuLateralPrincipal;
                }
                else
                {
                    TConexionLocal.EliminaBasedeDatos();
                    await DisplayAlert(JObjectRespuesta.Value<string>("Titulo"), JObjectRespuesta.Value<string>("Mensaje"), (string)Application.Current.Resources["StringMensajeBoton"]);
                }
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    XamlButtonIniciarSesion.IsEnabled = true;
                }
            }
        }

        private async void XamlTapGestureRecognizerTelefonoTapped(object sender, EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
            }
            try
            {
                TappedEventArgs TappedEventArgsEvento = (TappedEventArgs)e;
                if (TappedEventArgsEvento != null)
                {
                    string celular = (string)TappedEventArgsEvento.Parameter;
                    if (celular != null)
                    {
                        TMetodosComunes MetodosComunes = new TMetodosComunes();
                        MetodosComunes.LlamarporTelefono(celular);
                    }
                }
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                }
            }
        }

        private void XamlGestureRecognizerWhatsAppTapped(object sender, System.EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
            }
            try
            {
                TappedEventArgs TappedEventArgsEvento = (TappedEventArgs)e;
                if (TappedEventArgsEvento != null)
                {
                    string celular = (string)TappedEventArgsEvento.Parameter;
                    if (celular != null)
                    {
                        try
                        {
                            Chat.Open(celular, "");
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                }
            }
        }
    }

    class TViewModelIniciodeSesion : INotifyPropertyChanged
    {
        /* Actualización 07/03/2022 Es la mejor versión para rotar el inicio de sesión */

        private string EsVistaVertical;

        public StackOrientation StackOrientationContenedor { get; set; }
        public Thickness ThicknessPaddingLogo { get; set; }
        public double AnchoLogo { get; set; }
        public Thickness ThicknessPaddingFormulario { get; set; }

        public TViewModelIniciodeSesion()
        {
            EsVistaVertical = "";
        }

        public void MostrarVistaVertical(bool ES_VERTICAL, double ANCHO_DE_PANTALLA)
        {
            if (EsVistaVertical != ES_VERTICAL.ToString())
            {
                if (ES_VERTICAL)
                {
                    StackOrientationContenedor = StackOrientation.Vertical;
                    AnchoLogo = Device.Idiom == TargetIdiom.Tablet ? 150 : 180;
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        ThicknessPaddingLogo = Device.RuntimePlatform == Device.iOS ? new Thickness(160, 80, 160, 40) : new Thickness(160, 80, 160, 40);
                        ThicknessPaddingFormulario = Device.RuntimePlatform == Device.iOS ? new Thickness(140, 50, 140, 50) : new Thickness(140, 50, 140, 50);
                    }
                    else
                    {
                        ThicknessPaddingLogo = Device.RuntimePlatform == Device.iOS ? new Thickness(60, 120, 60, 60) : new Thickness(60, 120, 60, 60);
                        ThicknessPaddingFormulario = Device.RuntimePlatform == Device.iOS ? new Thickness(40, 40, 40, 40) : new Thickness(40, 40, 40, 40);
                    }
                    if (ANCHO_DE_PANTALLA != (-1))
                    {
                        AnchoLogo = Math.Round(ANCHO_DE_PANTALLA - 10);
                        EsVistaVertical = "true";
                    }
                }
                else
                {
                    StackOrientationContenedor = StackOrientation.Horizontal;
                    AnchoLogo = Device.Idiom == TargetIdiom.Tablet ? 150 : 180;
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        ThicknessPaddingLogo = Device.RuntimePlatform == Device.iOS ? new Thickness(60, 0, 30, 0) : new Thickness(60, 0, 30, 0);
                        ThicknessPaddingFormulario = Device.RuntimePlatform == Device.iOS ? new Thickness(30, 0, 60, 0) : new Thickness(30, 0, 60, 0);
                    }
                    else
                    {
                        ThicknessPaddingLogo = Device.RuntimePlatform == Device.iOS ? new Thickness(60, 0, 30, 0) : new Thickness(60, 0, 30, 0);
                        ThicknessPaddingFormulario = Device.RuntimePlatform == Device.iOS ? new Thickness(30, 0, 60, 0) : new Thickness(30, 0, 60, 0);
                    }
                    if (ANCHO_DE_PANTALLA != (-1))
                    {
                        AnchoLogo = Math.Round(ANCHO_DE_PANTALLA / 3);
                        EsVistaVertical = "false";
                    }
                }
                OnPropertyChanged(nameof(StackOrientationContenedor));
                OnPropertyChanged(nameof(ThicknessPaddingLogo));
                OnPropertyChanged(nameof(AnchoLogo));
                OnPropertyChanged(nameof(ThicknessPaddingFormulario));
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}