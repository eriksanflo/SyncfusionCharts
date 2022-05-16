using System;
using System.Collections.ObjectModel;

namespace Directiva10.Cajas
{
    internal class TBalance
    {
        public DateTime FechaUltimaSincronizacion{ get; set; }
        public TimeSpan HoraUltimaSincronizacion{ get; set; }
        public double IngresoAcumuladodeHoy { get; set; }
        public double IngresoAcumuladoSemanal { get; set; }
        public double IngresoAcumuladoMensual { get; set; }
        public double EgresoAcumuladodeHoy { get; set; }
        public double EgresoAcumuladoSemanal { get; set; }
        public double EgresoAcumuladoMensual { get; set; }
        public ObservableCollection<TSucursal> ObservableCollectionSucursales { get; set; }

        public TBalance()
        {
            ObservableCollectionSucursales = new ObservableCollection<TSucursal>();
        }
    }
}
