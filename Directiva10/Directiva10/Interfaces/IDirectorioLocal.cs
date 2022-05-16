namespace Directiva10.Interfaces
{
    public interface IDirectorioLocal
    {
        /// <summary>
        /// Solo en Android regresa la ruta de la carperta de imagenes definida por el sistema
        /// </summary>
        /// <param name="NOMBRE_ALBUM">Puedes ingresar el nombre de la carpeta o carpeta con nombre y extension del archivo</param>
        /// <returns>string</returns>
        string ObtenerRutaAlbumMultimedia(string NOMBRE_ALBUM);

        /// <summary>
        /// Obtiene la ruta donde se descargan los archivos en el dispositivo
        /// </summary>
        /// <param name="NOMBRE_DEL_ARCHIVO">Puedes ingresar el nombre de la carpeta o carpeta con nombre y extension del archivo</param>
        /// <param name="DISTRIBUCION"></param>
        /// <returns>string</returns>
        string ObtenerRutaArchivoDescargado(string NOMBRE_DEL_ARCHIVO, string DISTRIBUCION);
    }
}
