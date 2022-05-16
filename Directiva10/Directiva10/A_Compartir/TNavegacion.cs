using Xamarin.Forms;

namespace Directiva10.A_Compartir
{
    /// <summary>
    /// Versión 2.0
    /// Modificación 16/07/2020
    /// </summary>
    class TNavegacion
    {
        public static bool RedireccionarPage(string NOMBRE_DE_PAGINA_DESTINO, INavigation NavigationPiladePlataformaNativa)
        {
            var PilaPaginasNavegadas = NavigationPiladePlataformaNativa.NavigationStack;

            /* Primero encontramos el indice de la pagina destino dentro de la pila de navegacion y si no lo encuentra regresa como valor -1 */
            int indicePaginaDestino = -1;
            for (int i = 0; i < PilaPaginasNavegadas.Count; i++)
            {
                if (PilaPaginasNavegadas[i].GetType().Name == NOMBRE_DE_PAGINA_DESTINO)
                {
                    indicePaginaDestino = i;
                    break;
                }
            }
            /* Recorremos la lista y todos los indices mayor que nuestra pagina destino se eliminan de la pila de navegacion excepto el ultimo
             * porque es la pagina donde nos encontramos, entonces devemos salir con el metodo PopAsync*/
            if (indicePaginaDestino > -1)
            {
                int i = 0;
                while (i < PilaPaginasNavegadas.Count)
                {
                    if (indicePaginaDestino < i)
                    {
                        if (i < (PilaPaginasNavegadas.Count - 1))
                            NavigationPiladePlataformaNativa.RemovePage(PilaPaginasNavegadas[i]);
                        else
                        {
                            NavigationPiladePlataformaNativa.PopAsync();
                            i = i + 1;
                        }
                    }
                    else
                        i = i + 1;
                }
                return true;
            }
            else
                return false;
        }
    }
}
