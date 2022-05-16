using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace Directiva10.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PPLoading : Rg.Plugins.Popup.Pages.PopupPage
    {
        private object ObjectBloqueo;
        private bool BotonOcupado;
        private string MessagingCenterLlavedeComunicacion;
        private CancellationTokenSource CancellationTokenSourcePeticion;

        TViewModelLoading ViewModelLoading;

        public PPLoading(CancellationTokenSource CancellationTokenSourcePeticionOrigen)
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            ViewModelLoading = new TViewModelLoading();
            CancellationTokenSourcePeticion = CancellationTokenSourcePeticionOrigen;
            InitializeComponent();
            BindingContext = ViewModelLoading;
            ViewModelLoading.InicializarObjetosdeAnimacion(this, XamlEllipseCirculoBlanco, XamlEllipseCirculoNaranja, XamlLabelTexto);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModelLoading != null)
                ViewModelLoading.IniciarAnimacion();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (ViewModelLoading != null)
                ViewModelLoading.DetenerAnimacion();
        }

        private async void XamlButtonCerrarClicked(object sender, EventArgs e)
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
                try
                {
                    CancellationToken CancellationTokenCancelar = CancellationTokenSourcePeticion.Token;
                    if (CancellationTokenCancelar.IsCancellationRequested == false && CancellationTokenCancelar.CanBeCanceled == true)
                    {
                        CancellationTokenCancelar.ThrowIfCancellationRequested();
                        CancellationTokenSourcePeticion.Cancel();
                    }
                }
                catch { }
                await Navigation.PopModalAsync();
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

    class TViewModelLoading: INotifyPropertyChanged
    {
        double Radio;
        double RadioalCuadrado;
        double EscalaMinima;
        private Page PagedeAnimacion;
        private Animation AnimationRotacionCirculoBlanco;
        private Animation AnimationRotacionCirculoNajaranja;
        private Animation AnimationEscalaCirculoBlanco;
        private Animation AnimationEscalaCirculoNajaranja;
        private Animation AnimationTexto;
        private Ellipse EllipseCirculoBlanco;
        private Ellipse EllipseCirculoNaranja;
        private Label LabelTexto;

        public TViewModelLoading()
        {
            Radio = 60;
            RadioalCuadrado = Radio * Radio;
            EscalaMinima = 0.5;
        }

        public void InicializarObjetosdeAnimacion(Page PagedeAnimacionOrigen, Ellipse EllipseCirculoBlancoOrigen, Ellipse EllipseCirculoNaranjaOrigen, Label LabelTextoOrigen)
        {
            PagedeAnimacion = PagedeAnimacionOrigen;
            EllipseCirculoBlanco = EllipseCirculoBlancoOrigen;
            EllipseCirculoNaranja = EllipseCirculoNaranjaOrigen;
            LabelTexto = LabelTextoOrigen;
            EllipseCirculoBlanco.TranslationX = -Radio;
            EllipseCirculoBlanco.TranslationY = Math.Sqrt(RadioalCuadrado - (-Radio * -Radio));
            // Primero fracción de tiempo en que se ejecuta, escala de tiempo (0,1) <- Si no se pone marca error finishAt must be greater than beginAt, 
            // Propiedad enlazada a modificar
            // Rango de valores que tomara la propiedad enlazada
            // La formula en la que me base fue en Movimiento Circular Uniforme
            AnimationRotacionCirculoBlanco = new Animation {
                { 0.0, 0.2, new Animation(valor => EllipseCirculoBlanco.TranslationX = valor, -Radio, 0) },
                { 0.0, 0.2, new Animation(valor => EllipseCirculoBlanco.TranslationY = Math.Sqrt(RadioalCuadrado - (valor * valor)), -Radio, 0) },
                { 0.2, 0.5, new Animation(valor => EllipseCirculoBlanco.TranslationX = valor, 0, Radio) },
                { 0.2, 0.5, new Animation(valor => EllipseCirculoBlanco.TranslationY = Math.Sqrt(RadioalCuadrado - (valor * valor)), 0, Radio) },
                { 0.5, 0.7, new Animation(valor => EllipseCirculoBlanco.TranslationX = valor, Radio, 0) },
                { 0.5, 0.7, new Animation(valor => EllipseCirculoBlanco.TranslationY = -Math.Sqrt(RadioalCuadrado - (valor * valor)), Radio, 0) },
                { 0.7, 1.0, new Animation(valor => EllipseCirculoBlanco.TranslationX = valor, 0, -Radio) },
                { 0.7, 1.0, new Animation(valor => EllipseCirculoBlanco.TranslationY = -Math.Sqrt(RadioalCuadrado - (valor * valor)), 0, -Radio) },
            };
            AnimationRotacionCirculoNajaranja = new Animation {
                { 0.0, 0.2, new Animation(valor => EllipseCirculoNaranja.TranslationX = valor, Radio, 0) },
                { 0.0, 0.2, new Animation(valor => EllipseCirculoNaranja.TranslationY = -Math.Sqrt(RadioalCuadrado - (valor * valor)), Radio, 0) },
                { 0.2, 0.5, new Animation(valor => EllipseCirculoNaranja.TranslationX = valor, 0, -Radio) },
                { 0.2, 0.5, new Animation(valor => EllipseCirculoNaranja.TranslationY = -Math.Sqrt(RadioalCuadrado - (valor * valor)), 0, -Radio) },
                { 0.5, 0.7, new Animation(valor => EllipseCirculoNaranja.TranslationX = valor, -Radio, 0) },
                { 0.5, 0.7, new Animation(valor => EllipseCirculoNaranja.TranslationY = Math.Sqrt(RadioalCuadrado - (valor * valor)), -Radio, 0) },
                { 0.7, 1.0, new Animation(valor => EllipseCirculoNaranja.TranslationX = valor, 0, Radio) },
                { 0.7, 1.0, new Animation(valor => EllipseCirculoNaranja.TranslationY = Math.Sqrt(RadioalCuadrado - (valor * valor)), 0, Radio) },
            };
            AnimationEscalaCirculoBlanco = new Animation {
                { 0.0, 0.5, new Animation(valor => EllipseCirculoBlanco.Scale = valor, 1, 0.6) },
                { 0.5, 1.0, new Animation(valor => EllipseCirculoBlanco.Scale = valor, 0.6, 1 ) }
            };
            AnimationEscalaCirculoNajaranja = new Animation {
                { 0.0, 0.5, new Animation(valor => EllipseCirculoNaranja.Scale = valor, EscalaMinima, 1) },
                { 0.5, 1.0, new Animation(valor => EllipseCirculoNaranja.Scale = valor, 1, EscalaMinima) }
            };
            AnimationTexto = new Animation
            {
                { 0.0, 0.1, new Animation(valor => { LabelTexto.Scale = valor; }, 1.0, 0.8) },
                { 0.0, 0.1, new Animation(valor => { LabelTexto.Opacity = valor; }, 1.0, 0.5) },
                { 0.1, 0.3, new Animation(valor => { LabelTexto.Scale = valor; LabelTexto.Text = "Espera un momento"; }, 0.8, 1.0) },
                { 0.1, 0.3, new Animation(valor => { LabelTexto.Opacity = valor; }, 0.5, 1.0) },
                { 0.3, 0.4, new Animation(valor => { LabelTexto.Scale = valor; }, 1.0, 0.8) },
                { 0.3, 0.4, new Animation(valor => { LabelTexto.Opacity = valor; }, 1.0, 0.5) },
                { 0.4, 0.6, new Animation(valor => { LabelTexto.Scale = valor; LabelTexto.Text = "Estamos preparando todo lo necesario de tu consulta"; }, 0.8, 1.0) },
                { 0.4, 0.6, new Animation(valor => { LabelTexto.Opacity = valor; }, 0.5, 1.0) },
                { 0.6, 0.7, new Animation(valor => { LabelTexto.Scale = valor; }, 1.0, 0.8) },
                { 0.6, 0.7, new Animation(valor => { LabelTexto.Opacity = valor; }, 1.0, 0.5) },
                { 0.7, 1.0, new Animation(valor => { LabelTexto.Scale = valor; LabelTexto.Text = "Recuerda que siempre estamos contigo"; }, 0.8, 1.0) },
                { 0.7, 1.0, new Animation(valor => { LabelTexto.Opacity = valor; }, 0.5, 1.0) }
            };
        }

        public async void IniciarAnimacion()
        {
            try
            {
                DetenerAnimacion();
                // Necesita los Efectos y valor inicial de la propiedad antes de animar
                AnimationRotacionCirculoBlanco.Commit(PagedeAnimacion, nameof(AnimationRotacionCirculoBlanco), length: 3500, easing: Easing.SinInOut, finished: (VALOR, SE_REPITE) => EllipseCirculoBlanco.TranslationX = -Radio, repeat: () => true);
                AnimationRotacionCirculoNajaranja.Commit(PagedeAnimacion, nameof(AnimationRotacionCirculoNajaranja), length: 3500, easing: Easing.SinInOut, finished: (VALOR, SE_REPITE) => EllipseCirculoNaranja.TranslationX = Radio, repeat: () => true);
                AnimationEscalaCirculoBlanco.Commit(PagedeAnimacion, nameof(AnimationEscalaCirculoBlanco), length: 3000, easing: Easing.SinInOut, finished: (VALOR, SE_REPITE) => EllipseCirculoBlanco.Scale = 1, repeat: () => true);
                AnimationEscalaCirculoNajaranja.Commit(PagedeAnimacion, nameof(AnimationEscalaCirculoNajaranja), length: 3000, easing: Easing.SinInOut, finished: (VALOR, SE_REPITE) => EllipseCirculoNaranja.Scale = EscalaMinima, repeat: () => true);
                AnimationTexto.Commit(PagedeAnimacion, nameof(AnimationTexto), length: 25000, easing: null, finished: (VALOR, SE_REPITE) => LabelTexto.Scale = 1, repeat: () => true);
            }
            catch (Exception ex) { }
        }

        public void DetenerAnimacion()
        {
            PagedeAnimacion.AbortAnimation(nameof(AnimationRotacionCirculoBlanco));
            PagedeAnimacion.AbortAnimation(nameof(AnimationRotacionCirculoNajaranja));
            PagedeAnimacion.AbortAnimation(nameof(AnimationEscalaCirculoBlanco));
            PagedeAnimacion.AbortAnimation(nameof(AnimationEscalaCirculoNajaranja));
            PagedeAnimacion.AbortAnimation(nameof(AnimationTexto));
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