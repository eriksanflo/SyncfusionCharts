
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// Tener cuidado con el simulador, si apagamos los datos móviles y el wifi, la app se cierra sin explicación
namespace Directiva10.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPSinConexionaInternet : ContentPage
    {
        private bool EsModal;
        private double Ancho, Alto;
        private object ObjectBloqueo;
        private bool BotonOcupado;

        TViewModelSinConexionaInternet ViewModelSinConexionaInternet;

        public CPSinConexionaInternet(bool ES_MODAL)
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            EsModal = ES_MODAL;
            ViewModelSinConexionaInternet = new TViewModelSinConexionaInternet();
            InitializeComponent();
            BindingContext = ViewModelSinConexionaInternet;
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
                        ViewModelSinConexionaInternet.MostrarVistaVertical(false, Ancho);
                    else
                        ViewModelSinConexionaInternet.MostrarVistaVertical(true, Ancho);
                }
            }
        }

        private void XamlButtonCerrarClicked(object sender, EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlButtonCerrar.IsEnabled = false;
            }
            try
            {
                if (EsModal)
                    Navigation.PopModalAsync();
                else
                    Navigation.PopAsync();
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    XamlButtonCerrar.IsEnabled = true;
                }
            }
        }
    }

    class TViewModelSinConexionaInternet : INotifyPropertyChanged
    {
        private string EsVistaVertical;

        public StackOrientation StackOrientationContenedor { get; set; }
        public Thickness ThicknessPaddingTexto { get; set; }
        public double AnchoIconoAlerta { get; set; }
        public double AnchoLogoWifi { get; set; }

        public TViewModelSinConexionaInternet()
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
                    AnchoIconoAlerta = Device.Idiom == TargetIdiom.Tablet? 60 : 50;
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        ThicknessPaddingTexto = Device.RuntimePlatform == Device.iOS ? new Thickness(45, 45, 45, 45) : new Thickness(45, 45, 45, 45);
                        if (ANCHO_DE_PANTALLA != (-1))
                        {
                            AnchoLogoWifi = Math.Round(ANCHO_DE_PANTALLA / 3);
                            EsVistaVertical = "true";
                        }
                    }
                    else
                    {
                        ThicknessPaddingTexto = Device.RuntimePlatform == Device.iOS ? new Thickness(45, 45, 45, 45) : new Thickness(45, 45, 45, 45);
                        if (ANCHO_DE_PANTALLA != (-1))
                        {
                            AnchoLogoWifi = Math.Round(ANCHO_DE_PANTALLA / 3);
                            EsVistaVertical = "true";
                        }
                    }
                }
                else
                {
                    StackOrientationContenedor = StackOrientation.Horizontal;
                    AnchoIconoAlerta = Device.Idiom == TargetIdiom.Tablet ? 60 : 50;
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        ThicknessPaddingTexto = Device.RuntimePlatform == Device.iOS ? new Thickness(45, 45, 45, 45) : new Thickness(45, 45, 45, 45);
                        if (ANCHO_DE_PANTALLA != (-1))
                        {
                            AnchoLogoWifi = Math.Round(ANCHO_DE_PANTALLA / 5);
                            EsVistaVertical = "false";
                        }
                    }
                    else
                    {
                        ThicknessPaddingTexto = Device.RuntimePlatform == Device.iOS ? new Thickness(40, 40, 40, 40) : new Thickness(40, 40, 40, 40);
                        if (ANCHO_DE_PANTALLA != (-1))
                        {
                            AnchoLogoWifi = Math.Round(ANCHO_DE_PANTALLA / 6);
                            EsVistaVertical = "false";
                        }
                    }
                }
                OnPropertyChanged(nameof(StackOrientationContenedor));
                OnPropertyChanged(nameof(ThicknessPaddingTexto));
                OnPropertyChanged(nameof(AnchoIconoAlerta));
                OnPropertyChanged(nameof(AnchoLogoWifi));
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