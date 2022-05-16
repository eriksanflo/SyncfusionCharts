
using Directiva10.PantalladeInicio;
using Directiva10.Plantilla;
using Rg.Plugins.Popup.Extensions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Directiva10.Pin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPPin : ContentPage
    {
        private double Ancho, Alto;
        private object ObjectBloqueo;
        private bool BotonOcupado;
        private string PIN = "";

        TViewModelPin ViewModelPin;

        public CPPin()
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            ViewModelPin = new TViewModelPin();
            InitializeComponent();
            BindingContext = ViewModelPin;
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
                        ViewModelPin.MostrarVistaVertical(false, Ancho);
                    else
                        ViewModelPin.MostrarVistaVertical(true, Ancho);
                }
            }
        }

        private void XamlButtonNumeroClicked(object sender, EventArgs e)
        {
            if (PIN.Length < 4)
            {
                Button ButtonNumero = (Button)sender;
                PIN = PIN + ButtonNumero.Text;
                ActualizarVistaNumerosIngresados();
                if (PIN.Length == 4)
                    ValidarPin();
            }
        }

        private async void ActualizarVistaNumerosIngresados()
        {
            switch (PIN.Length)
            {
                case 0:
                    XamlLabel1.Text = "";
                    XamlLabel2.Text = "";
                    XamlLabel3.Text = "";
                    XamlLabel4.Text = "";
                    break;
                case 1:
                    XamlLabel1.Text = "•";
                    XamlLabel2.Text = "";
                    XamlLabel3.Text = "";
                    XamlLabel4.Text = "";
                    break;
                case 2:
                    XamlLabel1.Text = "•";
                    XamlLabel2.Text = "•";
                    XamlLabel3.Text = "";
                    XamlLabel4.Text = "";
                    break;
                case 3:
                    XamlLabel1.Text = "•";
                    XamlLabel2.Text = "•";
                    XamlLabel3.Text = "•";
                    XamlLabel4.Text = "";
                    break;
                case 4:
                    XamlLabel1.Text = "•";
                    XamlLabel2.Text = "•";
                    XamlLabel3.Text = "•";
                    XamlLabel4.Text = "•";
                    break;
            }
        }

        private async void ValidarPin()
        {
            if (PIN.Length == 4)
            {
                TControlPin ControlPin = new TControlPin();
                bool esPinValido = false;

                await Task.Run(() => { esPinValido = ControlPin.EsPinValido(PIN); });
                if (esPinValido)
                {
                    SMenuLateral SMenuLateralPrincipal = new SMenuLateral();
                    SMenuLateralPrincipal.GoToAsync("//RutaInicio");
                    Application.Current.MainPage = SMenuLateralPrincipal;
                }
                else
                    await Navigation.PushPopupAsync(new PPModalMensaje("El PIN es incorrecto, vuelva intentarlo"));
                PIN = "";
                ActualizarVistaNumerosIngresados();
            }
        }

        private void XamlButtonCancelarPinClicked(object sender, EventArgs e)
        {
            PIN = "";
            ActualizarVistaNumerosIngresados();
        }

        private void XamlButtonBorrarPinClicked(object sender, EventArgs e)
        {
            if (PIN.Length > 0)
            {
                PIN = PIN.Remove(PIN.Length - 1);
            }
            ActualizarVistaNumerosIngresados();
        }

        private void TapGestureRecognizerOlvidoPINTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CPRestablecerPin() { Title = "" });
        }
    }

    class TViewModelPin : INotifyPropertyChanged
    {
        private string EsVistaVertical;

        public StackOrientation StackOrientationContenedor { get; set; }
        public bool VistaVertical { get; set; }
        public Thickness ThicknessPIN { get; set; }

        public TViewModelPin()
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
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        ThicknessPIN = Device.RuntimePlatform == Device.iOS ? new Thickness(40, 80, 160, 40) : new Thickness(40, 80, 40, 40);
                        EsVistaVertical = "true";
                    }
                    else
                    {
                        ThicknessPIN = Device.RuntimePlatform == Device.iOS ? new Thickness(20, 45, 20, 45) : new Thickness(20, 45, 20, 45);
                        EsVistaVertical = "true";
                    }
                }
                else
                {
                    StackOrientationContenedor = StackOrientation.Horizontal;
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        ThicknessPIN = Device.RuntimePlatform == Device.iOS ? new Thickness(40, 70, 40, 70) : new Thickness(40, 70, 40, 70);
                        EsVistaVertical = "false";
                    }
                    else
                    {
                        ThicknessPIN = Device.RuntimePlatform == Device.iOS ? new Thickness(20, 45, 20, 45) : new Thickness(20, 45, 20, 45);
                        EsVistaVertical = "false";
                    }
                }
                VistaVertical = string.IsNullOrWhiteSpace(EsVistaVertical) || EsVistaVertical == "true";
                OnPropertyChanged(nameof(StackOrientationContenedor));
                OnPropertyChanged(nameof(VistaVertical));
                OnPropertyChanged(nameof(ThicknessPIN));
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