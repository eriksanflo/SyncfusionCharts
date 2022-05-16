using Directiva10.Interfaces;
using Directiva10.iOS.ServiciosdeDependencia;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(TDirectorioLocaliOS))]
namespace Directiva10.iOS.ServiciosdeDependencia
{
    class TDirectorioLocaliOS : IDirectorioLocal
    {
        public TDirectorioLocaliOS()
        {
        }

        public string ObtenerRutaAlbumMultimedia(string NOMBRE_ALBUM)
        {
            string respuesta = NOMBRE_ALBUM;
            return respuesta;
        }

        public string ObtenerRutaArchivoDescargado(string NOMBRE_DEL_ARCHIVO, string DISTRIBUCION)
        {
            string respuesta = "";
            string directorioDescargas = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string directorioDistribucion = DISTRIBUCION.ToUpper().Replace(" ", "_");

            Directory.CreateDirectory(directorioDescargas + "/" + directorioDistribucion);
            respuesta = Path.Combine(directorioDescargas, directorioDistribucion, NOMBRE_DEL_ARCHIVO);
            return respuesta;
        }
    }
}