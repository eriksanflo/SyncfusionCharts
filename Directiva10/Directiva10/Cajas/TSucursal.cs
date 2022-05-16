using Xamarin.Forms;

namespace Directiva10.Cajas
{
    internal class TSucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Monto { get; set; }
        public Color ColordeFondo { get; set; }

        public TSucursal()
        {
            ColordeFondo = Color.Transparent;
        }
    }
}
