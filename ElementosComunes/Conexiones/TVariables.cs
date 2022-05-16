using Newtonsoft.Json.Linq;

namespace ElementosComunes.Conexiones
{
    public class TVariables
    {
        private static string _Distribucion;
        private static string _Sistema;
        private static string _Usuario;
        private static string _TipoSistema = "APP";
        private static string _VersionApp;
        private static string _SistemaOperativo;

        public static string Distribucion
        {
            set { _Distribucion = value; }
            get { return _Distribucion; }
        }
        public static string VersionApp
        {
            set { _VersionApp = value; }
            get { return _VersionApp; }
        }
        public static string SistemaOperativo
        {
            set { _SistemaOperativo = value; }
            get { return _SistemaOperativo; }
        }
        public static string Sistema { set { _Sistema = value; } }
        public static string Usuario { set { _Usuario = value; } }
        public static string TipoSistema { get { return _TipoSistema; } }
        

        public static JObject ObtenerParesComunes()
        {
            JObject JObjectRespuesta = new JObject();

            JObjectRespuesta["DISTRIBUCION"] = _Distribucion;
            JObjectRespuesta["SISTEMA"] = _Sistema;
            JObjectRespuesta["USUARIO"] = _Usuario;
            JObjectRespuesta["TIPO_SISTEMA"] = _TipoSistema;
            JObjectRespuesta["VERSION_APP"] = _VersionApp;
            JObjectRespuesta["SISTEMA_OPERATIVO"] = _SistemaOperativo;

            return JObjectRespuesta;
        }
    }
}
