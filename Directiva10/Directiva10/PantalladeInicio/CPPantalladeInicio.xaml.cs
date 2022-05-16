using Directiva10.A_Compartir;
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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Directiva10.PantalladeInicio
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPPantalladeInicio : ContentPage
    {
        private double Ancho, Alto;
        private object ObjectBloqueo;
        private bool BotonOcupado;

        TViewModelPantalladeInicio ViewModelPantalladeInicio;

        public CPPantalladeInicio()
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            ViewModelPantalladeInicio = new TViewModelPantalladeInicio();
            InitializeComponent();
            BindingContext = ViewModelPantalladeInicio;
            IniciarVista();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModelPantalladeInicio != null)
                await ViewModelPantalladeInicio.ObtenerPersonas();
        }

        private async void XamlButtonPruebaClicked(object sender, EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlButtonPrueba.IsEnabled = false;
            }
            try
            {
                if (ViewModelPantalladeInicio != null)
                {
                    if (!string.IsNullOrWhiteSpace(XamlEntryUsuario.Text) && !string.IsNullOrWhiteSpace(XamlEntryUsuario.Text))
                    {
                        ViewModelPantalladeInicio.ActualizarTexto(XamlEntryUsuario.Text, true, new DateTime(), new TimeSpan(), new TPersona());
                    }
                    else
                    {
                        await Navigation.PushPopupAsync(new PPModalMensaje("Falta ingresar un usuario"));
                        TValidacion.ResaltarCamposRequeridos(XamlEntryUsuario);
                    }
                }
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    XamlButtonPrueba.IsEnabled = true;
                }
            }
        }

        private async void XamlButtonPrueba2Clicked(object sender, EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlButtonPrueba.IsEnabled = false;
            }
            try
            {
                if (ViewModelPantalladeInicio != null)
                    await ViewModelPantalladeInicio.ObtenerPersonas();
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    XamlButtonPrueba.IsEnabled = true;
                }
            }
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
                if (!string.IsNullOrWhiteSpace(XamlEntryUsuario.Text) && !string.IsNullOrWhiteSpace(XamlEntryUsuario.Text))
                {
                    if (!string.IsNullOrWhiteSpace(XamlEntryContrasena.Text) && !string.IsNullOrWhiteSpace(XamlEntryContrasena.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(XamlEntryDistribucion.Text) && !string.IsNullOrWhiteSpace(XamlEntryDistribucion.Text))
                        {
                            if (TConexion.TieneConectividadaInternet())
                            {
                                TControlPantalladeInicio ControlPantalladeInicio = new TControlPantalladeInicio();
                                JObject JObjectRespuesta = await ControlPantalladeInicio.ActualizarInformacion(0, "Parametro1", new CancellationToken());
                                if (JObjectRespuesta.Value<bool>("Respuesta"))
                                {
                                    await Navigation.PushPopupAsync(new PPModalMensaje(JObjectRespuesta.Value<string>("Mensaje")));
                                }
                                else
                                {
                                    await DisplayAlert(JObjectRespuesta.Value<string>("Titulo"), JObjectRespuesta.Value<string>("Mensaje"), (string)Application.Current.Resources["StringMensajeBoton"]);
                                }
                            }
                            else
                                await Navigation.PushModalAsync(new CPSinConexionaInternet(true));
                        }
                        else
                        {
                            await Navigation.PushPopupAsync(new PPModalMensaje("Falta ingresar una distribución"));
                            TValidacion.ResaltarCamposRequeridos(XamlEntryDistribucion);
                        }
                    }
                    else
                    {
                        await Navigation.PushPopupAsync(new PPModalMensaje("Falta ingresar una constraseña"));
                        TValidacion.ResaltarCamposRequeridos(XamlEntryContrasena, XamlEntryDistribucion);
                    }
                }
                else
                {
                    await Navigation.PushPopupAsync(new PPModalMensaje("Falta ingresar un usuario"));
                    TValidacion.ResaltarCamposRequeridos(XamlEntryUsuario, XamlEntryContrasena, XamlEntryDistribucion);
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

        private async void XamlCollectionViewPersonasSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
                XamlCollectionViewPersonas.SelectionChanged -= XamlCollectionViewPersonasSelectionChanged;
            }
            try
            {
                TPersona Persona = e.CurrentSelection[0] as TPersona;
                if (TConexion.TieneConectividadaInternet())
                {
                    //Acción
                }
                else
                    await Navigation.PushModalAsync(new CPSinConexionaInternet(true));
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                    XamlCollectionViewPersonas.SelectedItem = null;
                    XamlCollectionViewPersonas.SelectionChanged += XamlCollectionViewPersonasSelectionChanged;
                }
            }
        }

        private void IniciarVista()
        {

        }
    }

    class TViewModelPantalladeInicio : INotifyPropertyChanged
    {
        private string DatosLocales;
        private double GastosMensualesconImpuestos;

        public string Texto { get; set; }
        public ObservableCollection<TPersona> ObservableCollectionPersonas { get; set; }
        public bool TienePersonas { get; set; }
        
        public TViewModelPantalladeInicio()
        {
            Texto = "Hola";
        }

        public void ActualizarTexto(string TEXTO, bool ES_VALIDO, DateTime FECHA, TimeSpan HORA, TPersona PersonaNueva)
        {
            double otroPrecio = 15.5;
            Texto = TEXTO;
            OnPropertyChanged(nameof(Texto));
        }

        public async Task ObtenerPersonas()
        {
            TControlPantalladeInicio ControlPantalladeInicio = new TControlPantalladeInicio();
            ObservableCollectionPersonas = await ControlPantalladeInicio.ObtenerPersonas(0, "Juan", new CancellationToken());
            TienePersonas = ObservableCollectionPersonas.Count > 0;
            OnPropertyChanged(nameof(ObservableCollectionPersonas));
            OnPropertyChanged(nameof(TienePersonas));
        }

        public void ObtenerOtrasPersonas()
        {
            TControlPantalladeInicio ControlPantalladeInicio = new TControlPantalladeInicio();
            ObservableCollectionPersonas = ControlPantalladeInicio.ObtenerOtrasPersonas(0);
            TienePersonas = ObservableCollectionPersonas.Count > 0;
            OnPropertyChanged(nameof(ObservableCollectionPersonas));
            OnPropertyChanged(nameof(TienePersonas));
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