
using ElementosComunes.Conexiones;
using Directiva10.A_General;
using Directiva10.IniciodeSesion;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Directiva10.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SMenuLateral : Shell
    {
        private TViewModelMenuLateral ViewModelMenuLateral;
        public bool SesionIniciada { get; set; }

        public SMenuLateral()
        {
            TControlIniciodeSesion ControlIniciodeSesion = new TControlIniciodeSesion();
            ViewModelMenuLateral = new TViewModelMenuLateral();
            InitializeComponent();
            BindingContext = ViewModelMenuLateral;
            ViewModelMenuLateral.ActualizarMenuLateral();
            if (TConexionLocal.ExisteBasedeDatos() && ControlIniciodeSesion.SesionIniciada())
            {
                TControlPrincipal.SetVariablesComunes();
                ViewModelMenuLateral.ActualizarSesion(true);
            }
            //XamlFlyoutItemAccesoDirecto.CurrentItem = XamlShellContentElementoActivo;
        }

        // Shell no funcionan los métodos OnAppearing()
    }

    class TViewModelMenuLateral : INotifyPropertyChanged
    {
        public bool SesionIniciada { get; set; }
        public string NombredelUsuario { get; set; }
        public string ApellidosdelUsuario { get; set; }
        public string VersiondeApp { get; set; }

        public DataTemplate DataTemplateEstadodeCuenta { get; set; }

        public TViewModelMenuLateral()
        {
            DataTemplateEstadodeCuenta = new DataTemplate();
        }

        public void ActualizarVistaEstadodeCuenta(Type TypePaginaDestino)
        {
            DataTemplateEstadodeCuenta = new DataTemplate(TypePaginaDestino);
            OnPropertyChanged(nameof(DataTemplateEstadodeCuenta));
        }

        public void ActualizarSesion(bool SESION_INICIADA)
        {
            SesionIniciada = SESION_INICIADA;
            OnPropertyChanged(nameof(SesionIniciada));
        }

        public void ActualizarMenuLateral()
        {
            NombredelUsuario = string.IsNullOrWhiteSpace(TSesionPrincipal.Nombre) ? "Invitado" : TSesionPrincipal.Nombre;
            ApellidosdelUsuario = TSesionPrincipal.Apellidos;
            VersiondeApp = TVariables.VersionApp;
            OnPropertyChanged(nameof(NombredelUsuario));
            OnPropertyChanged(nameof(ApellidosdelUsuario));
            OnPropertyChanged(nameof(VersiondeApp));
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