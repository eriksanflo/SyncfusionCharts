namespace ElementosComunes.Clases
{
    public class TDataSet
    {
        /// <summary>
        /// Función que copia la BD de la app a la carpeta de descargas del dispositivo
        /// </summary>
        /// <param name="SISTEMA_OPERATIVO">asdasd</param>
        /// <param name="NOMBRE_BASE_DATOS">asdasd</param>
        public static void CopiarBaseDatos(string SISTEMA_OPERATIVO, string NOMBRE_BASE_DATOS)
        {
            string nombreBaseDatos = NOMBRE_BASE_DATOS.Replace(".", "");
            if (SISTEMA_OPERATIVO.ToUpper() == "ANDROID")
            {
                /*** Descomentar estas lineas para compilar en Android ***/
                /*
                string rutaLibreria = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string rutaBasedeDatos = Path.Combine(rutaLibreria, NOMBRE_BASE_DATOS);
                var rutaOrigen = File.ReadAllBytes(rutaBasedeDatos);
                var rutaCarpetaDescargas = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
                // En la rutaDestino no debe tener mas de un guion bajo consecutivo por que causa problemas en android 7 y superior
                var rutaDestino = string.Format(rutaCarpetaDescargas + "/"+ nombreBaseDatos + "_{0:yyyy-MM-dd_HH-mm-ss-tt}.db3", System.DateTime.Now);

                File.WriteAllBytes(rutaDestino, rutaOrigen);
                */
                /*** ***/
            }
        }
    }
}
