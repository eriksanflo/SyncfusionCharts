using Directiva10.A_Compartir;
using Directiva10.A_General;
using Directiva10.IniciodeSesion;
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

namespace Directiva10.Pin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPRestablecerPin : ContentPage
    {
        private double Ancho, Alto;
        private object ObjectBloqueo;
        private bool BotonOcupado;

        TViewModelRestablecerPin ViewModelRestablecerPin;

        public CPRestablecerPin()
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            ViewModelRestablecerPin = new TViewModelRestablecerPin();
            InitializeComponent();
            BindingContext = ViewModelRestablecerPin;
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
                        ViewModelRestablecerPin.MostrarVistaVertical(false, Ancho);
                    else
                        ViewModelRestablecerPin.MostrarVistaVertical(true, Ancho);
                }
            }
        }

        private async void XamlButtonRestablecerPinClicked(object sender, EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlButtonRestablecerPin.IsEnabled = false;
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(XamlEntryContrasena.Text) && !string.IsNullOrWhiteSpace(XamlEntryContrasena.Text))
                {
                    if (TConexion.TieneConectividadaInternet())
                    {
                        TControlPin ControlPin = new TControlPin();
                        JObject JObjectRespuesta = new JObject();
                        JObjectRespuesta = await ControlPin.RestablecerPin(XamlEntryContrasena.Text.Trim());
                        if (JObjectRespuesta.Value<bool>("Respuesta"))
                        {
                            await Navigation.PushPopupAsync(new PPModalMensaje(JObjectRespuesta.Value<string>("Mensaje")));
                            Application.Current.MainPage = new SMenuLateral();
                        }
                        else
                            await Navigation.PushPopupAsync(new PPModalMensaje(JObjectRespuesta.Value<string>("Mensaje")));
                    }
                    else
                        await Navigation.PushModalAsync(new CPSinConexionaInternet(true));
                }
                else
                {
                    await Navigation.PushPopupAsync(new PPModalMensaje("Falta ingresar una constraseña"));
                    TValidacion.ResaltarCamposRequeridos(XamlEntryContrasena);
                }
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    XamlButtonRestablecerPin.IsEnabled = true;
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

    class TViewModelRestablecerPin : INotifyPropertyChanged
    {
        private string EsVistaVertical;

        public StackOrientation StackOrientationContenedor { get; set; }
        public Thickness ThicknessPaddingTexto { get; set; }
        public double AnchoIconoAlerta { get; set; }
        public Thickness ThicknessPaddingLogin { get; set; }

        public TViewModelRestablecerPin()
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
                    AnchoIconoAlerta = Device.Idiom == TargetIdiom.Tablet ? 60 : 50;
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        ThicknessPaddingTexto = Device.RuntimePlatform == Device.iOS ? new Thickness(45, 45, 45, 45) : new Thickness(45, 45, 45, 45);
                        ThicknessPaddingLogin = Device.RuntimePlatform == Device.iOS ? new Thickness(90, 50, 90, 50) : new Thickness(90, 50, 90, 50);
                    }
                    else
                    {
                        ThicknessPaddingTexto = Device.RuntimePlatform == Device.iOS ? new Thickness(45, 45, 45, 45) : new Thickness(45, 45, 45, 45);
                        ThicknessPaddingLogin = Device.RuntimePlatform == Device.iOS ? new Thickness(55, 40, 55, 40) : new Thickness(55, 40, 55, 40);
                    }
                    EsVistaVertical = "true";
                }
                else
                {
                    StackOrientationContenedor = StackOrientation.Horizontal;
                    AnchoIconoAlerta = Device.Idiom == TargetIdiom.Tablet ? 60 : 50;
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        ThicknessPaddingTexto = Device.RuntimePlatform == Device.iOS ? new Thickness(45, 45, 45, 45) : new Thickness(45, 45, 45, 45);
                        ThicknessPaddingLogin = Device.RuntimePlatform == Device.iOS ? new Thickness(180, 60, 180, 60) : new Thickness(180, 60, 180, 60);
                    }
                    else
                    {
                        ThicknessPaddingTexto = Device.RuntimePlatform == Device.iOS ? new Thickness(40, 30, 40, 30) : new Thickness(40, 30, 40, 30);
                        ThicknessPaddingLogin = Device.RuntimePlatform == Device.iOS ? new Thickness(100, 40, 100, 40) : new Thickness(100, 40, 100, 40);
                    }
                    EsVistaVertical = "false";
                }
                OnPropertyChanged(nameof(StackOrientationContenedor));
                OnPropertyChanged(nameof(ThicknessPaddingTexto));
                OnPropertyChanged(nameof(AnchoIconoAlerta));
                OnPropertyChanged(nameof(ThicknessPaddingLogin));
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