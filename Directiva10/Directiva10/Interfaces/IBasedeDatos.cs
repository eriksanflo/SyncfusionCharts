namespace Directiva10.Interfaces
{
    public interface IBasedeDatos
    {
        /// <summary>
        /// Solo en Android copia la base de datos en la carpeta descargas
        /// </summary>
        /// <param name="NOMBRE_DE_LA_BASE_DE_DATOS"></param>
        /// <returns>bool</returns>
        bool ExportarBasedeDatosLocal(string NOMBRE_DE_LA_BASE_DE_DATOS);
    }
}
