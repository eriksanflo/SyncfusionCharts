
namespace ElementosComunes.Clases
{
    public class TMensaje
    {
        public static string ObtenerMensajeDefault(string MENSAJE)
        {
            string respuesta = "";

            switch (MENSAJE)
            {
                case "Parametros Invalidos":
                    respuesta = "Error en los tipos de datos";
                    break;
                case "Sesion Invalida":
                    respuesta = "La sesión se ha cerrado";
                    break;
                case "Estructura Invalida":
                    respuesta = "Error en la estructura de la solicitud";
                    break;
                case "Error":
                    respuesta = "Error en el servidor";
                    break;
                case "Solicitud Invalida":
                    respuesta = "Error en la solicitud";
                    break;
                case "Sin Respuesta":
                    respuesta = "No se recibió respuesta del servidor, puede intentarlo de nuevo por favor";
                    break;
                case "No Conectado":
                    respuesta = "Favor de activar su wifi o sus datos móviles";
                    break;
                case "No Accesible":
                    respuesta = "Sin acceso a internet";
                    break;
                case "Credenciales Invalidas":
                    respuesta = "Usuario, password o distribución incorrecta, intente de nuevo.";
                    break;


                case "Datos Inválidos":
                    respuesta = "Error en los tipos de datos";
                    break;
                case "Sesion invalida":
                    respuesta = "La sesión se ha cerrado";
                    break;
                case "JSON Inválido":
                    respuesta = "Error en la estructura de la solicitud";
                    break;
                case "Solicitud Inválida":
                    respuesta = "Error en la solicitud";
                    break;

                case "Tiempo Espera 0":
                    respuesta = "Descarga las configuraciones de la empresa en el módulo de ayuda";
                    break;
                case "Excedió Limite Espera":
                    respuesta = "El dispositivo no pudo conectarse con el servidor.";
                    break;

                case "Usuario Cancelo":
                    respuesta = "El usuario cancelo la solicitud";
                    break;
                default:
                    respuesta = MENSAJE;
                    break;
            }

            return respuesta;
        }
    }
}
