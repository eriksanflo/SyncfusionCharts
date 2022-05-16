using Directiva10.Plantilla;
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
    public partial class CPPermisosdeUsuario : ContentPage
    {
        private double Ancho, Alto;
        TViewModelPermisosdeUsuario ViewModelPermisosdeUsuario;

        public CPPermisosdeUsuario(int ID_USUARIO, string NOMBRE_DE_USUARIO)
        {
            ViewModelPermisosdeUsuario = new TViewModelPermisosdeUsuario(ID_USUARIO, NOMBRE_DE_USUARIO);
            InitializeComponent();
            BindingContext = ViewModelPermisosdeUsuario;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModelPermisosdeUsuario != null)
            {
                ViewModelPermisosdeUsuario.ObtenerPermisos();
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
                        ViewModelPermisosdeUsuario.MostrarVistaVertical(false, Ancho);
                        XamlCVBarradeNavegacion.EsVistaVertical = false;
                    }
                    else
                    {
                        ViewModelPermisosdeUsuario.MostrarVistaVertical(true, Ancho);
                        XamlCVBarradeNavegacion.EsVistaVertical = true;
                    }
                }
            }
        }
    }

    class TViewModelPermisosdeUsuario : INotifyPropertyChanged
    {
        private string EsVistaVertical;
        private CancellationTokenSource CancellationTokenSourcePeticion;
        private CancellationToken CancellationTokenCancelar;
        private int IdUsuario;

        public string Nombre { get; set; }
        public ObservableCollection<TPermiso> ObservableCollectionPermisosdeUsuario { get; set; }
        public bool TienePermisoUsuario { get; set; }
        public bool EstaActualizando { get; set; }

        public TViewModelPermisosdeUsuario(int ID_USUARIO, string NOMBRE_DE_USUARIO)
        {
            EsVistaVertical = "";
            IdUsuario = ID_USUARIO;
            Nombre = NOMBRE_DE_USUARIO;
            ObservableCollectionPermisosdeUsuario = new ObservableCollection<TPermiso>();
        }

        /* ICommand lo ocupamos en una IsPullToRefreshEnabled de una ListView */
        public ICommand CommandObtenerPermisos
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
                    await ObtenerPermisos();
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

        public async Task ObtenerPermisos()
        {
            TControlUsuarios ControlUsuarios = new TControlUsuarios();
            ObservableCollectionPermisosdeUsuario = await ControlUsuarios.ObtenerPermisosdeUsuario(IdUsuario, CancellationTokenCancelar);
            TienePermisoUsuario = ObservableCollectionPermisosdeUsuario.Count > 0;
            OnPropertyChanged(nameof(ObservableCollectionPermisosdeUsuario));
            OnPropertyChanged(nameof(TienePermisoUsuario));
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