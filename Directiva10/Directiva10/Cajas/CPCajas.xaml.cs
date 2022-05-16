
using Directiva10.Plantilla;
using ElementosComunes.Conexiones;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Directiva10.Cajas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPCajas : ContentPage
    {
        private double Ancho, Alto;
        private object ObjectBloqueo;
        private bool BotonOcupado;

        TViewModelCajas ViewModelCajas;

        public CPCajas()
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            ViewModelCajas = new TViewModelCajas();
            InitializeComponent();
            BindingContext = ViewModelCajas;
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
                        ViewModelCajas.MostrarVistaVertical(false, Ancho);
                        XamlCVBarradeNavegacion.EsVistaVertical = false;
                    }
                    else
                    {
                        ViewModelCajas.MostrarVistaVertical(true, Ancho);
                        XamlCVBarradeNavegacion.EsVistaVertical = true;
                    }
                }
            }
        }

        private async void XamlRefresViewSucursalesRefreshing(object sender, System.EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlButtonEgresos.IsEnabled = false;
            }
            try
            {
                if (ViewModelCajas != null)
                {
                    if (TConexion.TieneConectividadaInternet())
                        ViewModelCajas.CommandActualizarInformaciondeBalance.Execute(Navigation);
                    else
                    {
                        ViewModelCajas.CambiarEstadoActualizando(false);
                        await Navigation.PushModalAsync(new CPSinConexionaInternet(true));
                    }
                }
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    XamlButtonEgresos.IsEnabled = true;
                }
            }
        }

        private async void XamlButtonIngresosClicked(object sender, System.EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlButtonEgresos.IsEnabled = false;
            }
            try
            {
                if (ViewModelCajas != null)
                    ViewModelCajas.ActualizarBotonesActivos(true, false);
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    XamlButtonEgresos.IsEnabled = true;
                }
            }
        }

        private async void XamlButtonEgresosClicked(object sender, System.EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlButtonEgresos.IsEnabled = false;
            }
            try
            {
                if (ViewModelCajas != null)
                    ViewModelCajas.ActualizarBotonesActivos(false, true);
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    XamlButtonEgresos.IsEnabled = true;
                }
            }
        }

        private async void XamlCollectionViewSucursalesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlCollectionViewSucursales.SelectionChanged -= XamlCollectionViewSucursalesSelectionChanged;
            }
            try
            {
                TSucursal Sucursal = e.CurrentSelection[0] as TSucursal;
                if (TConexion.TieneConectividadaInternet())
                {
                    Page PageDestino = new CPCajasSucursal(Sucursal.Id, Sucursal.Nombre, Sucursal.Monto);
                    if (PageDestino != null)
                    {
                        PageDestino.Title = Sucursal.Nombre;
                        await Navigation.PushAsync(PageDestino);
                    }
                }
                else
                    await Navigation.PushModalAsync(new CPSinConexionaInternet(true));
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    //if (XamlCollectionViewSucursales.SelectedItems.Count > 0)
                    XamlCollectionViewSucursales.SelectedItem = null;
                    XamlCollectionViewSucursales.SelectionChanged += XamlCollectionViewSucursalesSelectionChanged;
                }
            }
        }

        private async void IniciarVista()
        {
            if (ViewModelCajas != null)
            {
                if (TConexion.TieneConectividadaInternet())
                {
                    Navigation.PushModalAsync(new PPLoading(ViewModelCajas.ObtenerNuevoTokendeCancelacion()));
                    JObject JObjectRespuesta = await ViewModelCajas.ActualizarInformaciondeBalance();
                    // Importante la siguiente validación porque el modal puede ser cerrado por el usuario
                    if (Navigation.ModalStack.Count > 0)
                        await Navigation.PopModalAsync();
                    if (JObjectRespuesta.Value<bool>("Respuesta") != true)
                        await Navigation.PushPopupAsync(new PPModalMensaje(JObjectRespuesta.Value<string>("Mensaje")));
                }
                else
                {
                    ViewModelCajas.ObtenerBalance();
                    await Navigation.PushModalAsync(new CPSinConexionaInternet(true));
                }
            }
        }
    }

    class TViewModelCajas : INotifyPropertyChanged
    {
        private string EsVistaVertical;
        private CancellationTokenSource CancellationTokenSourcePeticion;
        private CancellationToken CancellationTokenCancelar;
        private TControlCajas ControlCajas;
        private TBalance Balance;
        private Style StyleBotonActivo;
        private Style StyleBotonInactivo;
        private Color ColorCebradeListView;
        private bool ModulodeIngresosActivo;

        public double TotaldeHoy { get; set; }
        public double TotalSemana { get; set; }
        public double TotalMes { get; set; }
        public DateTime FechadeUltimaSincronizacion { get; set; }
        public TimeSpan HoradeUltimaSincronizacion { get; set; }
        public ObservableCollection<TSucursal> ObservableCollectionSucursales { get; set; }
        public bool TieneSucursales { get; set; }
        public bool EstaActualizando { get; set; }
        public Style StyleBotonIngresos { get; set; }
        public Style StyleBotonEgresos { get; set; }

        public TViewModelCajas()
        {
            EsVistaVertical = "";
            CancellationTokenSourcePeticion = new CancellationTokenSource();
            CancellationTokenCancelar = CancellationTokenSourcePeticion.Token;
            ControlCajas = new TControlCajas();
            Balance = new TBalance();
            ColorCebradeListView = Color.FromHex("eff3f6");
            ObservableCollectionSucursales = new ObservableCollection<TSucursal>();
            StyleBotonActivo = (Style)Application.Current.Resources["StyleBoton"];
            StyleBotonInactivo = (Style)Application.Current.Resources["StyleBotonSecundario"];
            ModulodeIngresosActivo = true;
            ActualizarBotonesActivos(true, false);
        }

        /* ICommand lo ocupamos en una IsPullToRefreshEnabled de una ListView */
        public ICommand CommandActualizarInformaciondeBalance
        {
            get
            {
                return new Command(async (NavigationOrigen) =>
                {
                   INavigation Navigation = NavigationOrigen as INavigation;
                    if (Navigation != null)
                        Navigation.PushModalAsync(new PPLoading(ObtenerNuevoTokendeCancelacion()));
                    EstaActualizando = true;
                    OnPropertyChanged(nameof(EstaActualizando));
                    JObject JObjectRespuesta = await ActualizarInformaciondeBalance();
                    EstaActualizando = false;
                    OnPropertyChanged(nameof(EstaActualizando));
                    // Importante la siguiente validación porque el modal puede ser cerrado por el usuario
                    if (Navigation != null && Navigation.ModalStack.Count > 0)
                        await Navigation.PopModalAsync();
                    if(JObjectRespuesta.Value<bool>("Respuesta") != true)
                        await Navigation.PushPopupAsync(new PPModalMensaje(JObjectRespuesta.Value<string>("Mensaje")));
                });
            }
        }

        public void CambiarEstadoActualizando(bool ESTA_ACTUALIZANDO)
        {
            EstaActualizando = ESTA_ACTUALIZANDO;
            OnPropertyChanged(nameof(EstaActualizando));
        }

        public void MostrarVistaVertical(bool ES_VERTICAL, double ANCHO_DE_PANTALLA)
        {
        }

        public CancellationTokenSource ObtenerNuevoTokendeCancelacion()
        {
            CancellationTokenSourcePeticion = new CancellationTokenSource();
            CancellationTokenCancelar = CancellationTokenSourcePeticion.Token;
            return CancellationTokenSourcePeticion;
        }

        public async Task<JObject> ActualizarInformaciondeBalance()
        {
            JObject JObjectRespuesta = await ControlCajas.ActualizarInformaciondeBalance(CancellationTokenCancelar);
            if (JObjectRespuesta.Value<bool>("Respuesta") == true)
                await Task.Run(() =>
                {
                    ObtenerBalance();
                });
            return JObjectRespuesta;
        }

        public void ObtenerBalance()
        {
            Balance = ControlCajas.ObtenerBalance();
            ActualizarBotonesActivos(ModulodeIngresosActivo, !ModulodeIngresosActivo);
            FechadeUltimaSincronizacion = Balance.FechaUltimaSincronizacion;
            HoradeUltimaSincronizacion = Balance.HoraUltimaSincronizacion;
            ObservableCollectionSucursales = Balance.ObservableCollectionSucursales;
            TieneSucursales = ObservableCollectionSucursales.Count > 0;
            EstaActualizando = false;
            OnPropertyChanged(nameof(FechadeUltimaSincronizacion));
            OnPropertyChanged(nameof(HoradeUltimaSincronizacion));
            OnPropertyChanged(nameof(ObservableCollectionSucursales));
            OnPropertyChanged(nameof(TieneSucursales));
            OnPropertyChanged(nameof(EstaActualizando));
        }

        public void ActualizarBotonesActivos(bool ACTIVAR_INGRESOS, bool ACTIVAR_EGRESOS)
        {
            StyleBotonIngresos = ACTIVAR_INGRESOS ? StyleBotonActivo : StyleBotonInactivo;
            StyleBotonEgresos = ACTIVAR_EGRESOS ? StyleBotonActivo : StyleBotonInactivo;
            ModulodeIngresosActivo = ACTIVAR_INGRESOS;
            if (ACTIVAR_INGRESOS == true)
            {
                TotaldeHoy = Balance.IngresoAcumuladodeHoy;
                TotalSemana = Balance.IngresoAcumuladoSemanal;
                TotalMes = Balance.IngresoAcumuladoMensual;
            }
            else
            {
                TotaldeHoy = Balance.EgresoAcumuladodeHoy;
                TotalSemana = Balance.EgresoAcumuladoSemanal;
                TotalMes = Balance.EgresoAcumuladoMensual;
            }
            OnPropertyChanged(nameof(TotaldeHoy));
            OnPropertyChanged(nameof(TotalSemana));
            OnPropertyChanged(nameof(TotalMes));
            OnPropertyChanged(nameof(StyleBotonIngresos));
            OnPropertyChanged(nameof(StyleBotonEgresos));
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