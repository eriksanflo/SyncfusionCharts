
using Directiva10.Alertas;
using ElementosComunes.Conexiones;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Directiva10.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CVBarradeNavegacion : ContentView
    {
        private double Ancho, Alto;
        private object ObjectBloqueo;
        private bool BotonOcupado;

        TViewModelBarradeNavegacion ViewModelBarradeNavegacion;

        public static readonly BindableProperty BindablePropertyTitulo = BindableProperty.Create("Titulo", typeof(string), typeof(CVBarradeNavegacion), string.Empty);
        public static readonly BindableProperty BindablePropertyMostrarIconodeNavegacion = BindableProperty.Create("MostrarIconodeNavegacion", typeof(bool), typeof(CVBarradeNavegacion), false);
        public static readonly BindableProperty BindablePropertyEsIconoRegresar = BindableProperty.Create("EsIconoRegresar", typeof(bool), typeof(CVBarradeNavegacion), false);
        public static readonly BindableProperty BindablePropertyMostrarIconodeAlerta = BindableProperty.Create("MostrarIconodeAlerta", typeof(bool), typeof(CVBarradeNavegacion), false);
        public static readonly BindableProperty BindablePropertyNumerodeNotificaciones = BindableProperty.Create("NumerodeNotificaciones", typeof(int), typeof(CVBarradeNavegacion), 0);
        public static readonly BindableProperty BindablePropertyEsVistaVertical = BindableProperty.Create("EsVistaVertical", typeof(bool), typeof(CVBarradeNavegacion), true);

        public string Titulo
        {
            get => (string)GetValue(CVBarradeNavegacion.BindablePropertyTitulo);
            set 
            { 
                SetValue(CVBarradeNavegacion.BindablePropertyTitulo, value); 
                if(ViewModelBarradeNavegacion != null)
                    ViewModelBarradeNavegacion.ActualizarTitulo(Titulo);
            }
        }

        public bool MostrarIconodeNavegacion
        {
            get => (bool)GetValue(CVBarradeNavegacion.BindablePropertyMostrarIconodeNavegacion);
            set
            {
                SetValue(CVBarradeNavegacion.BindablePropertyMostrarIconodeNavegacion, value);
                if (ViewModelBarradeNavegacion != null)
                    ViewModelBarradeNavegacion.ActualizarMostrarIconodeNavegacion(MostrarIconodeNavegacion);
            }
        }

        public bool MostrarIconodeAlerta
        {
            get => (bool)GetValue(CVBarradeNavegacion.BindablePropertyMostrarIconodeAlerta);
            set
            {
                SetValue(CVBarradeNavegacion.BindablePropertyMostrarIconodeAlerta, value);
                if (ViewModelBarradeNavegacion != null)
                    ViewModelBarradeNavegacion.ActualizarMostrarIconodeAlerta(MostrarIconodeAlerta);
            }
        }

        public bool EsIconoRegresar
        {
            get => (bool)GetValue(CVBarradeNavegacion.BindablePropertyEsIconoRegresar);
            set
            {
                SetValue(CVBarradeNavegacion.BindablePropertyEsIconoRegresar, value);
                if (ViewModelBarradeNavegacion != null)
                    ViewModelBarradeNavegacion.ActualizarEsIconoRegresar(EsIconoRegresar);
            }
        }

        public int NumerodeNotificaciones
        {
            get => (int)GetValue(CVBarradeNavegacion.BindablePropertyNumerodeNotificaciones);
            set
            {
                SetValue(CVBarradeNavegacion.BindablePropertyNumerodeNotificaciones, value);
                if (ViewModelBarradeNavegacion != null)
                    ViewModelBarradeNavegacion.ActualizarNumerodeNotificaciones(NumerodeNotificaciones);
            }
        }

        // Este valor se modifica desde la vista que lo invoca, porque el ContentView solo detecta su ares, y siempre va ser mas grande el ancho que el alto, sin importar la orientación del dispositivo
        public bool EsVistaVertical
        {
            get => (bool)GetValue(CVBarradeNavegacion.BindablePropertyEsVistaVertical);
            set
            {
                SetValue(CVBarradeNavegacion.BindablePropertyEsVistaVertical, value);
                if (ViewModelBarradeNavegacion != null)
                    ViewModelBarradeNavegacion.MostrarVistaVertical(value);
            }
        }

        public CVBarradeNavegacion()
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            ViewModelBarradeNavegacion = new TViewModelBarradeNavegacion();
            InitializeComponent();
            BindingContext = ViewModelBarradeNavegacion;
            ViewModelBarradeNavegacion.MostrarVistaVertical(true);
        }

        private void XamlTapGestureRecognizerMostrarMenuLateral(object sender, System.EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        private void XamlTapGestureRecognizerCerrarModulo(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void XamlTapGestureRecognizerCerrarModal(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void XamlTapGestureRecognizerAbrirAlertas(object sender, System.EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
            }
            try
            {
                if (TConexion.TieneConectividadaInternet())
                    await Navigation.PushModalAsync(new CPAlertas());
                else
                    await Navigation.PushModalAsync(new CPSinConexionaInternet(true));
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

    class TViewModelBarradeNavegacion : INotifyPropertyChanged
    {
        private string EsVistaVertical;
        private bool MostrarIconodeNavegacion;

        public Thickness ThicknessPadding { get; set; }
        public double AnchoIconos { get; set; }
        public string Titulo { get; set; }
        public bool MostrarIconoHamburguesa { get; set; }
        public bool MostrarIconoRegresar { get; set; }
        public bool MostrarIconodeAlerta { get; set; }
        public bool MostrarCirculoRojo { get; set; }
        public int NumerodeNotificaciones { get; set; }

        public TViewModelBarradeNavegacion()
        {
            EsVistaVertical = "";
            Titulo = "";
        }

        public void ActualizarTitulo(string NUEVO_TITULO)
        {
            Titulo = NUEVO_TITULO;
            OnPropertyChanged(nameof(Titulo));
        }

        public void ActualizarMostrarIconodeNavegacion(bool MOSTRAR_ICONO_DE_NAVEGACION)
        {
            MostrarIconodeNavegacion = MOSTRAR_ICONO_DE_NAVEGACION;
            ActualizarEsIconoRegresar(MostrarIconoRegresar);
        }

        public void ActualizarEsIconoRegresar(bool ES_ICONO_REGRESAR)
        {
            if(MostrarIconodeNavegacion == true)
            {
                if(ES_ICONO_REGRESAR == true)
                {
                    MostrarIconoHamburguesa = false;
                    MostrarIconoRegresar = true;
                }
                else
                {
                    MostrarIconoHamburguesa = true;
                    MostrarIconoRegresar = false;
                }
            }
            else
            {
                MostrarIconoHamburguesa = false;
                MostrarIconoRegresar = false;
            }
            OnPropertyChanged(nameof(MostrarIconoHamburguesa));
            OnPropertyChanged(nameof(MostrarIconoRegresar));
        }

        public void ActualizarMostrarIconodeAlerta(bool MOSTRAR_ICONO_DE_ALERTA)
        {
            MostrarIconodeAlerta = MOSTRAR_ICONO_DE_ALERTA;
            OnPropertyChanged(nameof(MostrarIconodeAlerta));
        }

        public void ActualizarNumerodeNotificaciones(int NUMERO_DE_NOTIFICACIONES)
        {
            NumerodeNotificaciones = NUMERO_DE_NOTIFICACIONES;
            MostrarCirculoRojo = MostrarIconodeAlerta == true && NumerodeNotificaciones > 0;
            OnPropertyChanged(nameof(NumerodeNotificaciones));
            OnPropertyChanged(nameof(MostrarCirculoRojo));
        }

        public void MostrarVistaVertical(bool ES_VERTICAL)
        {
            if (EsVistaVertical != ES_VERTICAL.ToString())
            {
                if (ES_VERTICAL)
                {
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        ThicknessPadding = Device.RuntimePlatform == Device.iOS ? new Thickness(21, 0, 21, 0) : new Thickness(21, 0, 21, 0);
                        AnchoIconos = Device.RuntimePlatform == Device.iOS ? 25 : 25;
                    }
                    else
                    {
                        if(Device.RuntimePlatform == Device.iOS)
                        {
                            // En iOS 15 si tiene la propiedad Shell.NavBarIsVisible="False" se come un espacio superior de la pagina, con esta opción se compensa el espacio superior
                            string versionSistemaOperativo = DeviceInfo.VersionString.Split('.')[0];
                            if (double.TryParse(versionSistemaOperativo, out double version) && version >= 15)
                                ThicknessPadding = new Thickness(21, 27, 21, 0);
                            else
                                ThicknessPadding = new Thickness(21, 10, 21, 0);
                        }
                        else
                            ThicknessPadding = new Thickness(21, 0, 21, 0);
                        AnchoIconos = Device.RuntimePlatform == Device.iOS ? 25 : 25;
                    }
                    EsVistaVertical = "true";
                }
                else
                {
                    if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        ThicknessPadding = Device.RuntimePlatform == Device.iOS ? new Thickness(0, 20, 0, 0) : new Thickness(15, 0, 15, 0);
                        AnchoIconos = Device.RuntimePlatform == Device.iOS ? 20 : 20;
                    }
                    else
                    {
                        ThicknessPadding = Device.RuntimePlatform == Device.iOS ? new Thickness(48, 0, 48, 0) : new Thickness(48, 0, 48, 0);
                        AnchoIconos = Device.RuntimePlatform == Device.iOS ? 24 : 24;
                    }
                    EsVistaVertical = "false";
                }
                OnPropertyChanged(nameof(ThicknessPadding));
                OnPropertyChanged(nameof(AnchoIconos));
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