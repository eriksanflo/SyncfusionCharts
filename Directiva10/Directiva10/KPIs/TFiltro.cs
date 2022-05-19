using System.Collections.Generic;

namespace Directiva10.KPIs
{
    public class TFiltro
    {
        public string TipoFiltro { get; set; }
        public List<TFiltroValor> ValoresFiltro { get; set; }
    }
    public class TFiltroValor
    {
        public int IdFiltroValor { get; set; }
        public string FiltroValor { get; set; }
        public string Nombre { get; set; }
        public List<TFiltroValor> Valores { get; set; }
    }
}
