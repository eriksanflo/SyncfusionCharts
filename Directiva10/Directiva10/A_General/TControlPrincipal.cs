using Directiva10.A_Compartir;
using Directiva10.Interfaces;
using ElementosComunes.Conexiones;
using Mono.Data.Sqlite;
using System;
using System.IO;
using Xamarin.Forms;

namespace Directiva10.A_General
{
    class TControlPrincipal
    {
        //http://25.104.125.195/
        private static string IpServidor { get { return "25.104.125.195"; } }
        public static string VersionApp { get { return "0.0.0"; } }
        public static int VersionBDRequerida { get { return 1; } }
        public static string BasedeDatos { get { return "Directiva.db3"; } }
        public static string SistemaOperativo { get { return Device.RuntimePlatform.ToString().ToUpper(); } }
        public static string Sistema { get { return "SistemaMovil"; } }

        /* Métodos públicos */

        public static void SetVariablesdeProyecto()
        {
            /**
             * Versión 0.0.0 IP 54.241.35.219:15007
             * *
            */
            
            TConexion.EsConexionProductivo = false;         // Local
            TConexion.IpServidor = "25.104.125.195";        // Local
            TConexion.AsignarRangodePuertos(1007, 1007);    // Local
            

            TConexionLocal.BasedeDatos = CrearRutaBasedeDatos();
            TConexion.AsignarTiempoMaximodeEsperaenPeticion(300);
            TVariables.Sistema = Sistema;
            TVariables.VersionApp = VersionApp;
            TVariables.SistemaOperativo = SistemaOperativo;
        }

        public static void SetVariablesComunes()
        {
            TModeloPrincipal ModeloPrincipal = new TModeloPrincipal();
            try
            {
                LlenarVariablesdeSesion(ModeloPrincipal.ObtenerParametrosdeSesion());
                TVariables.VersionApp = VersionApp;
                TVariables.SistemaOperativo = SistemaOperativo;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ModeloPrincipal.CerrarConexion();
            }
        }

        public static bool ExportarBasedeDatos()
        {
            bool respuesta = false;
            try
            {
                string destino = DependencyService.Get<IDirectorioLocal>().ObtenerRutaArchivoDescargado("BasedeDatos", TVariables.Distribucion);
                respuesta = !string.IsNullOrEmpty(TArchivo.CopiaryRenombraArchivoalaCarpeta(CrearRutaBasedeDatos(), destino, "base.db"));
            }
            catch (Exception e) { }
            return respuesta;
        }

        public static void InicializarTemadeApp()
        {
            switch (Application.Current.RequestedTheme)
            {
                case OSAppTheme.Dark:
                    DependencyService.Get<IBarradeEstado>().AplicarColoraBarradeEstado((Color)Application.Current.Resources["ColorBarradeNavegacionOscuro"], true);
                    break;
                default:
                    DependencyService.Get<IBarradeEstado>().AplicarColoraBarradeEstado((Color)Application.Current.Resources["ColorBarradeNavegacionClaro"], false);
                    break;
            }
        }

        public static void EstablecerColorBarradeNotificaciones(Color COLOR_CLARO, Color COLOR_OSCURO)
        {
            switch (Application.Current.RequestedTheme)
            {
                case OSAppTheme.Dark:
                    DependencyService.Get<IBarradeEstado>().AplicarColoraBarradeEstado(COLOR_OSCURO, true);
                    break;
                default:
                    DependencyService.Get<IBarradeEstado>().AplicarColoraBarradeEstado(COLOR_CLARO, false);
                    break;
            }
        }

        public static Color ObtenerColorAppThemeBinding(string KEY_STATIC_RESOURCE_COLOR_CLARO, string KEY_STATIC_RESOURCE_COLOR_OSCURO)
        {
            Color ColorRespuesta = Color.Black;
            switch (Application.Current.RequestedTheme)
            {
                case OSAppTheme.Dark:
                    ColorRespuesta = (Color)Application.Current.Resources[KEY_STATIC_RESOURCE_COLOR_OSCURO];
                    break;
                default:
                    ColorRespuesta = (Color)Application.Current.Resources[KEY_STATIC_RESOURCE_COLOR_CLARO];
                    break;
            }
            return ColorRespuesta;
        }

        /* Métodos privados */

        private static string CrearRutaBasedeDatos()
        {
            string respuesta = "";
            try
            {
                string rutaLibreria = null;
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        string rutaDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        rutaLibreria = Path.Combine(rutaDocumentos, "..", "Library");
                        break;
                    case Device.Android:
                        rutaLibreria = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        break;
                    case Device.UWP:
                        rutaLibreria = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        break;
                }
                respuesta = Path.Combine(rutaLibreria, BasedeDatos);
            }
            catch (Exception e)
            {
                throw e;
            }
            return respuesta;
        }

        private static void LlenarVariablesdeSesion(SqliteDataReader SqliteDataReaderSesion)
        {
            if (SqliteDataReaderSesion != null && SqliteDataReaderSesion.HasRows)
            {
                while (SqliteDataReaderSesion.Read())
                {
                    switch (SqliteDataReaderSesion["Parametro"].ToString())
                    {
                        case "Distribucion":
                            TVariables.Distribucion = SqliteDataReaderSesion["Valor"].ToString();
                            break;
                        case "SistemaReal":
                            TVariables.Sistema = SqliteDataReaderSesion["Valor"].ToString();
                            break;
                        case "Usuario":
                            TVariables.Usuario = SqliteDataReaderSesion["Valor"].ToString();
                            TSesionPrincipal.Usuario = SqliteDataReaderSesion["Valor"].ToString();
                            break;
                        case "IdEntidad":
                            int idEntidad = 0;
                            int.TryParse(SqliteDataReaderSesion["Valor"].ToString(), out idEntidad);
                            TSesionPrincipal.IdEntidad = idEntidad;
                            break;
                        case "Nombre":
                            TSesionPrincipal.Nombre = SqliteDataReaderSesion["Valor"].ToString();
                            break;
                        case "Apellidos":
                            TSesionPrincipal.Apellidos = SqliteDataReaderSesion["Valor"].ToString();
                            break;
                        case "FotodePerfil":
                            TSesionPrincipal.FotoPerfil = SqliteDataReaderSesion["Valor"].ToString();
                            break;
                        case "TieneAcceso":
                            bool tieneAcceso = false;
                            if (!string.IsNullOrEmpty(SqliteDataReaderSesion["Valor"].ToString()))
                                tieneAcceso = SqliteDataReaderSesion["Valor"].ToString().ToUpper() == "SI";
                            TSesionPrincipal.TieneAccesoalSistema = tieneAcceso;
                            break;
                    }
                }
            }
        }
    }
}
