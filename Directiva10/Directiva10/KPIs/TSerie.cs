using Syncfusion.SfChart.XForms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Directiva10.KPIs
{
    public class TSerie
    {
        public string Nombre { get; set; }
        public Color Color { get; set; }
        public ObservableCollection<TPunto> ObservableCollectionPuntos { get; set; }
        public List<Color> ListColoresdePuntos { get; set; }
        public List<TEtiqueta> ListEtiquetas { get; set; }

        public TSerie()
        {
            ObservableCollectionPuntos = new ObservableCollection<TPunto>();
            ListColoresdePuntos = new List<Color>();
            ListEtiquetas = new List<TEtiqueta>();
        }
    }
}
