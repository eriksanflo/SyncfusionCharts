using Directiva10.Droid.ServiciosdeDependencia;
using Directiva10.Interfaces;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(TDirectorioLocalAndroid))]
namespace Directiva10.Droid.ServiciosdeDependencia
{
    class TDirectorioLocalAndroid : IDirectorioLocal
    {
        public TDirectorioLocalAndroid()
        {
            /* No se te olvide agregar el permiso WRITE_EXTERNAL_STORAGE en el manifiesto */
        }

        public string ObtenerRutaAlbumMultimedia(string NOMBRE_ALBUM)
        {
            string respuesta = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryPictures);
            respuesta = Path.Combine(respuesta, NOMBRE_ALBUM);
            return respuesta;
        }

        public string ObtenerRutaArchivoDescargado(string NOMBRE_DEL_ARCHIVO, string DISTRIBUCION)
        {
            string respuesta = "";
            string distribucion = DISTRIBUCION.ToUpper().Replace(" ", "_");
            string carpetadeDescargas = Android.App.Application.Context.GetExternalFilesDirs(Android.OS.Environment.DirectoryDownloads)[0].AbsolutePath;
            Directory.CreateDirectory(carpetadeDescargas + "/" + distribucion);
            respuesta = Path.Combine(carpetadeDescargas, distribucion, NOMBRE_DEL_ARCHIVO);
            return respuesta;
        }
    }
}