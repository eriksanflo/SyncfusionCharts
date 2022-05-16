using Rg.Plugins.Popup.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Directiva10.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PPModalMensaje : Rg.Plugins.Popup.Pages.PopupPage
    {
        private object ObjectBloqueo;
        private bool BotonOcupado;
        private string MessagingCenterLlavedeComunicacion;

        TViewModelModalMensaje ViewModelModalMensaje;

        public PPModalMensaje(string MENSAJE)
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            ViewModelModalMensaje = new TViewModelModalMensaje(MENSAJE, false);
            InitializeComponent();
            BindingContext = ViewModelModalMensaje;
        }

        /// <summary>
        /// Responde true o false de una pregunta, pero es necesario llamarlo de la siguiente manera:
        /// string llavedeComunicacion = "claveCPNombredeContentPage";
        /// await Navigation.PushPopupAsync(new PPModalMensaje(llavedeComunicacion, "¿Desea continuar?"));
        /// MessagingCenter.Subscribe<PPModalMensaje, bool>(this, llavedeComunicacion, async (remitente, argumento) =>
        /// {
        ///     bool respuesta = argumento;
        ///     if (respuesta == true) { Aquí va el texto de respuesta }
        ///     MessagingCenter.Unsubscribe<PPModalMensaje, bool>(this, llavedeComunicacion);
        /// });
        /// </summary>
        /// <param name="LLAVE_DE_COMUNICACION"></param>
        /// <param name="MENSAJE"></param>
        public PPModalMensaje(string LLAVE_DE_COMUNICACION, string MENSAJE)
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            MessagingCenterLlavedeComunicacion = LLAVE_DE_COMUNICACION;
            ViewModelModalMensaje = new TViewModelModalMensaje(MENSAJE, true);
            InitializeComponent();
            BindingContext = ViewModelModalMensaje;
        }

        private async void XamlTapGestureRecognizerAceptarTapped(object sender, System.EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
            }
            try
            {
                MessagingCenter.Send(this, MessagingCenterLlavedeComunicacion, true);
                if (PopupNavigation.Instance.PopupStack.Count > 0)
                    await PopupNavigation.PopAsync();
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                }
            }
        }

        private async void XamlTapGestureRecognizerCancelarTapped(object sender, System.EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
            }
            try
            {
                MessagingCenter.Send(this, MessagingCenterLlavedeComunicacion, false);
                if (PopupNavigation.Instance.PopupStack.Count > 0)
                    await PopupNavigation.PopAsync();
            }
            finally
            {
                lock (ObjectBloqueo)
                {
                    BotonOcupado = false;
                }
            }
        }

        private async void XamlTapGestureRecognizerCerrarTapped(object sender, System.EventArgs e)
        {
            lock (ObjectBloqueo)
            {
                if (BotonOcupado)
                    return;
                BotonOcupado = true;
            }
            try
            {
                await PopupNavigation.PopAsync();
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

    class TViewModelModalMensaje : INotifyPropertyChanged
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string TextoBotonAceptar { get; set; }
        public string TextoBotonCancelar { get; set; }
        public bool EsperaRespuestadeUsuario { get; set; }

        public TViewModelModalMensaje(string MENSAJE, bool ESPERA_RESPUESTA_DE_USUARIO)
        {
            Titulo = (string)Application.Current.Resources["StringMensajeTitulo"];
            TextoBotonAceptar = (string)Application.Current.Resources["StringMensajeBoton"];
            TextoBotonCancelar = (string)Application.Current.Resources["StringMensajeBotonCancelar"];
            Mensaje = MENSAJE;
            EsperaRespuestadeUsuario = ESPERA_RESPUESTA_DE_USUARIO;
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