
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Directiva10.Cajas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPCajasSucursal : ContentPage
    {
        private double Ancho, Alto;
        private object ObjectBloqueo;
        private bool BotonOcupado;

        TViewModelCajasSucursal ViewModelCajasSucursal;

        public CPCajasSucursal(int ID_SUCURSAL, string NOMBRE_DE_SUCURSAL, double TOTAL_SUCURSAL_DIARIO)
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            ViewModelCajasSucursal = new TViewModelCajasSucursal(ID_SUCURSAL, TOTAL_SUCURSAL_DIARIO);
            InitializeComponent();
            XamlCVBarradeNavegacion.Titulo = NOMBRE_DE_SUCURSAL;
            BindingContext = ViewModelCajasSucursal;
            IniciarVista();
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
                    {
                        ViewModelCajasSucursal.MostrarVistaVertical(false, Ancho);
                        XamlCVBarradeNavegacion.EsVistaVertical = false;
                    }
                    else
                    {
                        ViewModelCajasSucursal.MostrarVistaVertical(true, Ancho);
                        XamlCVBarradeNavegacion.EsVistaVertical = true;
                    }
                }
            }
        }

        private async void XamlCollectionViewCajasSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlCollectionViewCajas.SelectionChanged -= XamlCollectionViewCajasSelectionChanged;
            }
            try
            {
                TBalancedeUsuario BalancedeUsuario = e.CurrentSelection[0] as TBalancedeUsuario;
                if (BalancedeUsuario != null)
                {
                    ViewModelCajasSucursal.BuscarySeleccionarCaja(BalancedeUsuario);
                }
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    if (XamlCollectionViewCajas.SelectedItems.Count > 0)
                        XamlCollectionViewCajas.SelectedItem = null;
                    XamlCollectionViewCajas.SelectionChanged += XamlCollectionViewCajasSelectionChanged;
                }
            }
        }

        private async void IniciarVista()
        {
            if (ViewModelCajasSucursal != null)
            {
                ViewModelCajasSucursal.ObtenerBalanceporUsuariosdeSucursal();
            }
        }
    }

    class TViewModelCajasSucursal : INotifyPropertyChanged
    {
        private string EsVistaVertical;
        private int IdSucursal;

        public double Total { get; set; }
        public ObservableCollection<TBalancedeUsuario> ObservableCollectionBalancedeUsuarios { get; set; }
        public bool TieneUsuarios { get; set; }
        public bool EstaActualizando { get; set; }

        public TViewModelCajasSucursal(int ID_SUCURSAL, double TOTAL_SUCURSAL_DIARIO)
        {
            IdSucursal = ID_SUCURSAL;
            Total = TOTAL_SUCURSAL_DIARIO;
            EsVistaVertical = "";
            ObservableCollectionBalancedeUsuarios = new ObservableCollection<TBalancedeUsuario>();
        }

        /* ICommand lo ocupamos en una IsPullToRefreshEnabled de una ListView */
        public ICommand CommandObtenerBalanceporUsuariosdeSucursal
        {
            get
            {
                return new Command(async () =>
                {
                    EstaActualizando = true;
                    OnPropertyChanged(nameof(EstaActualizando));
                    await Task.Run(() =>
                    {
                        ObtenerBalanceporUsuariosdeSucursal();
                    });
                    EstaActualizando = false;
                    OnPropertyChanged(nameof(EstaActualizando));
                });
            }
        }

        public void MostrarVistaVertical(bool ES_VERTICAL, double ANCHO_DE_PANTALLA)
        {
        }

        public void ObtenerBalanceporUsuariosdeSucursal()
        {
            TControlCajas ControlCajas = new TControlCajas();
            ObservableCollectionBalancedeUsuarios = ControlCajas.ObtenerBalanceporUsuariosdeSucursal(IdSucursal);
            TieneUsuarios = ObservableCollectionBalancedeUsuarios.Count > 0;
            OnPropertyChanged(nameof(ObservableCollectionBalancedeUsuarios));
            OnPropertyChanged(nameof(TieneUsuarios));
        }

        public void BuscarySeleccionarCaja(TBalancedeUsuario BalancedeUsuarioSeleccionado)
        {
            foreach (TBalancedeUsuario BalancedeUsuario in ObservableCollectionBalancedeUsuarios)
            {
                if (BalancedeUsuario == BalancedeUsuarioSeleccionado)
                    BalancedeUsuario.ActualizarColores(true);
                else
                    BalancedeUsuario.ActualizarColores(false);
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