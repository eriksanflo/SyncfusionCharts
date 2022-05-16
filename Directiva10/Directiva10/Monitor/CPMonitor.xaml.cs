using Directiva10.Cajas;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Directiva10.Monitor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPMonitor : ContentPage
    {
        private double Ancho, Alto;
        private object ObjectBloqueo;
        private bool BotonOcupado;

        TViewModelCajas ViewModelCajas;

        public CPMonitor()
        {
            ObjectBloqueo = new object();
            BotonOcupado = false;
            ViewModelCajas = new TViewModelCajas();
            InitializeComponent();
            BindingContext = ViewModelCajas;
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
                        ViewModelCajas.MostrarVistaVertical(false, Ancho);
                    else
                        ViewModelCajas.MostrarVistaVertical(true, Ancho);
                }
            }
            XamlCVBarradeNavegacion.NumerodeNotificaciones = 3;
        }
    }
}