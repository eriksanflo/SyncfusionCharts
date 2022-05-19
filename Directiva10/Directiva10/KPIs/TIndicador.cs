using Syncfusion.SfChart.XForms;
using System.Collections.Generic;

namespace Directiva10.KPIs
{
    internal class TIndicador
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Tipo { get; set; }
        public bool EsFavorita { get; set; }
        public string Medicion { get; set; }
        public string FormatodeMedicion { get; set; }
        public string FormatoPorcentaje { get; set; }
        public bool MostrarLeyenda { get; set; }
        public bool MostrarLeyendaSecundaria { get; set; }
        public bool MostrarPorcentajeSobrelaGrafica { get; set; }
        public double ValorMaximo { get; set; }
        public LegendPlacement LegendPlacementPosicionLeyenda { get; set; }
        public List<TSerie> ListSeries { get; set; }
        public List<TEtiqueta> ListDetalles { get; set; }
        public List<TFiltroAplicado> FiltrosAplicados { get; set; }
        public List<TFiltro> Filtros { get; set; }

        public TIndicador()
        {
            ListSeries = new List<TSerie>();
            ListDetalles = new List<TEtiqueta>();
        }
    }
}
