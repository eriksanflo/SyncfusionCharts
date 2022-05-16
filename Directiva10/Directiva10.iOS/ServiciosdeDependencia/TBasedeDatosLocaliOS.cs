using Directiva10.Interfaces;
using Directiva10.iOS.ServiciosdeDependencia;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(TBasedeDatosLocaliOS))]
namespace Directiva10.iOS.ServiciosdeDependencia
{
    class TBasedeDatosLocaliOS : IBasedeDatos
    {
        public bool ExportarBasedeDatosLocal(string NOMBRE_DE_LA_BASE_DE_DATOS)
        {
            bool respuesta = false;
            try
            {
                // iOS No lo permite
                respuesta = false;
            }
            catch (Exception)
            {
                respuesta = false;
            }
            return respuesta;
        }
    }
}