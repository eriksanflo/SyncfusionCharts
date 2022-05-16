using Directiva10.Droid.ServiciosdeDependencia;
using Directiva10.Interfaces;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(TBasedeDatosLocalAndroid))]
namespace Directiva10.Droid.ServiciosdeDependencia
{
    class TBasedeDatosLocalAndroid : IBasedeDatos
    {
        public bool ExportarBasedeDatosLocal(string NOMBRE_DE_LA_BASE_DE_DATOS)
        {
            bool respuesta = false;
            try
            {
                string nombre = NOMBRE_DE_LA_BASE_DE_DATOS.Split('.')[0];
                string rutadeBasedeDatos = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), NOMBRE_DE_LA_BASE_DE_DATOS);
                var archivoCopiado = File.ReadAllBytes(rutadeBasedeDatos);
                var carpetadeDescargas = Android.App.Application.Context.GetExternalFilesDirs(Android.OS.Environment.DirectoryDownloads)[0].AbsolutePath;
                // En la rutaDestino no debe tener mas de un guion bajo consecutivo por que causa problemas en android 7 o superior
                var rutadeDestino = string.Format(carpetadeDescargas + "/" + nombre + "_{0:yyyyMMdd_HHmmss}.db3", System.DateTime.Now);
                File.WriteAllBytes(rutadeDestino, archivoCopiado);
                respuesta = true;
            }
            catch (Exception e)
            {
                respuesta = false;
            }
            return respuesta;
        }
    }
}