using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Directiva10.A_Compartir
{
    /// <summary>
    /// Regresa ObservableCollection Agrupadas, por ejemplo contactos agrupados por su primer letra del nombre
    /// </summary>
    /// <typeparam name="TIPO_DE_KEY">Tipo de variable con la que se van agrupar</typeparam>
    /// <typeparam name="TIPO_DEL_OBJETO">Tipo de Objeto de la Colección</typeparam>

    /* Como poblar esta clase
     * Donde:
     *          MiObjetos es ObservableCollection<TMiObjeto>
     *          MiObjeto.Ordenar es la propiedad del objeto con el que se van a ORDENAR los elementos
     *          MiObjeto.Agrupar es la propiedad del objeto con el que se van a AGRUPAR los elementos
     * Uso:
     *          var ordenar =    from MiObjeto in MiObjetos
     *                        orderby MiObjeto.Ordenar
     *                          group MiObjeto by MiObjeto.Agrupar into GrupoTemporal
     *                         select new TAgrupando<string, TMiObjeto>(GrupoTemporal.Key, GrupoTemporal);
     *          return new ObservableCollection<TAgrupando<string, TMiObjeto>>(ordenar);  
     */

    public class TAgrupando<TIPO_DE_KEY, TIPO_DEL_OBJETO> : ObservableCollection<TIPO_DEL_OBJETO>
    {
        public TIPO_DE_KEY Llave { get; set; }

        public string EtiquetaparaMostarenVista { get; set; }

        public TAgrupando(string KeyLlave, IEnumerable<TIPO_DEL_OBJETO> EnumerableElementos)
        {
            if (KeyLlave != null)
            {
                EtiquetaparaMostarenVista = KeyLlave.ToString();
                foreach (var elemento in EnumerableElementos)
                {
                    Items.Add(elemento);
                }
            }
        }
    }
}
