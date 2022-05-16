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

namespace Directiva10.Usuarios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPUsuarios : ContentPage
    {
        private double Ancho, Alto;
        private object ObjectBloqueo;
        private bool BotonOcupado;

        TViewModelUsuarios ViewModelUsuarios;
        public CPUsuarios()
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            ViewModelUsuarios = new TViewModelUsuarios();
            InitializeComponent();
            BindingContext = ViewModelUsuarios;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModelUsuarios != null)
            {
                ViewModelUsuarios.ObtenerUsuarios();
            }
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
                        ViewModelUsuarios.MostrarVistaVertical(false, Ancho);
                        XamlCVBarradeNavegacion.EsVistaVertical = false;
                    }
                    else
                    {
                        ViewModelUsuarios.MostrarVistaVertical(true, Ancho);
                        XamlCVBarradeNavegacion.EsVistaVertical = true;
                    }
                }
            }
        }

        private async void XamlCollectionViewUsuariosSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlCollectionViewUsuarios.SelectionChanged -= XamlCollectionViewUsuariosSelectionChanged;
            }
            try
            {
                TUsuario Usuario = e.CurrentSelection[0] as TUsuario;
                if (Usuario != null && Usuario.Id > 0 && !string.IsNullOrWhiteSpace(Usuario.Nombre) && Usuario.EstaOnline == true)
                {
                    if (TConexion.TieneConectividadaInternet())
                    {
                        Page PageDestino = new CPPermisosdeUsuario(Usuario.Id, Usuario.Nombre);
                        if (PageDestino != null)
                        {
                            PageDestino.Title = "Usuarios";
                            await Navigation.PushAsync(PageDestino);
                        }
                    }
                    else
                        await Navigation.PushModalAsync(new CPSinConexionaInternet(true));
                }
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    if (XamlCollectionViewUsuarios.SelectedItems.Count > 0)
                        XamlCollectionViewUsuarios.SelectedItem = null;
                    XamlCollectionViewUsuarios.SelectionChanged += XamlCollectionViewUsuariosSelectionChanged;
                }
            }
        }
    }

    class TViewModelUsuarios : INotifyPropertyChanged
    {
        private string EsVistaVertical;
        private CancellationTokenSource CancellationTokenSourcePeticion;
        private CancellationToken CancellationTokenCancelar;

        public ObservableCollection<TUsuario> ObservableCollectionUsuarios { get; set; }
        public bool TieneUsuarios { get; set; }
        public bool EstaActualizando { get; set; }

        public TViewModelUsuarios()
        {
            EsVistaVertical = "";
            CancellationTokenSourcePeticion = new CancellationTokenSource();
            CancellationTokenCancelar = CancellationTokenSourcePeticion.Token;
            ObservableCollectionUsuarios = new ObservableCollection<TUsuario>();
        }

        /* ICommand lo ocupamos en una IsPullToRefreshEnabled de una ListView */
        public ICommand CommandObtenerUsuarios
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
                    await ObtenerUsuarios();
                    EstaActualizando = false;
                    OnPropertyChanged(nameof(EstaActualizando));
                    // Importante la siguiente validación porque el modal puede ser cerrado por el usuario
                    if (Navigation != null && Navigation.ModalStack.Count > 0)
                        await Navigation.PopModalAsync();
                });
            }
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

        public async Task ObtenerUsuarios()
        {
            TControlUsuarios ControlCajas = new TControlUsuarios();
            ObservableCollectionUsuarios = await ControlCajas.ObtenerUsuarios(CancellationTokenCancelar);
            TieneUsuarios = ObservableCollectionUsuarios.Count > 0;
            OnPropertyChanged(nameof(ObservableCollectionUsuarios));
            OnPropertyChanged(nameof(TieneUsuarios));
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