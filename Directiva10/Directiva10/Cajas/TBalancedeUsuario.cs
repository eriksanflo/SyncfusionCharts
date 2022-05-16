using Directiva10.A_General;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Directiva10.Cajas
{
    internal class TBalancedeUsuario : INotifyPropertyChanged
    {
        public bool EstaSeleccionado { get; set; }
        public double RotacionIcono { get; set; }
        public string Nombre { get; set; }
        public double TotalCobrado { get; set; }
        public double TotalEfectivo { get; set; }
        public bool MostrarDetalles { get; set; }
        public double Efectivo { get; set; }
        public double Depositos { get; set; }
        public double Gastos { get; set; }

        public Color ColorNombre { get; private set; }
        public Color ColorTotalCobrado { get; private set; }
        public Color ColorTotalEfectivo { get; private set; }

        public TBalancedeUsuario()
        {
            ActualizarColores(false);
        }

        public void ActualizarColores(bool CAJA_SELECCIONADA)
        {
            EstaSeleccionado = CAJA_SELECCIONADA;
            if (EstaSeleccionado)
            {
                RotacionIcono = 90;
                ColorNombre = TControlPrincipal.ObtenerColorAppThemeBinding("ColorTextoClaro", "ColorTextoOscuro");
                ColorTotalCobrado = TControlPrincipal.ObtenerColorAppThemeBinding("ColorBotonPrimarioClaro", "ColorBotonPrimarioOscuro");
                ColorTotalEfectivo = TControlPrincipal.ObtenerColorAppThemeBinding("ColorTextoClaro", "ColorTextoOscuro");
            }
            else
            {
                RotacionIcono = 0;
                ColorNombre = (Color)Application.Current.Resources["ColorEtiquetaClaro"];
                ColorTotalCobrado = (Color)Application.Current.Resources["ColorEtiquetaClaro"];
                ColorTotalEfectivo = (Color)Application.Current.Resources["ColorEtiquetaClaro"];
            }
            OnPropertyChanged(nameof(EstaSeleccionado));
            OnPropertyChanged(nameof(RotacionIcono));
            OnPropertyChanged(nameof(ColorNombre));
            OnPropertyChanged(nameof(ColorTotalCobrado));
            OnPropertyChanged(nameof(ColorTotalEfectivo));
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
