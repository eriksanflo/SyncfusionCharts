using Directiva10.A_Compartir;
using Directiva10.A_General;
using ElementosComunes.Clases;
using ElementosComunes.Conexiones;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Directiva10.IniciodeSesion
{
    class TControlIniciodeSesion
    {
        private JObject JObjectParametros;

        public TControlIniciodeSesion()
        {
            JObjectParametros = TVariables.ObtenerParesComunes();
        }

        /* Métodos públicos */

        public async Task<JObject> IniciarSesion()
        {
            JObject JObjectRespuesta = new JObject();
            JObjectRespuesta["Respuesta"] = false;
            JObjectRespuesta["Titulo"] = (string)Application.Current.Resources["StringMensajeTitulo"];
            TModeloIniciodeSesion ModeloIniciodeSesion = new TModeloIniciodeSesion();
            TModeloBasedeDatos ModeloBasedeDatos = new TModeloBasedeDatos();
            try
            {
                //JObject JObjectParametros = new JObject();
                //JObjectParametros["DISTRIBUCION"] = TCaracteres.CodificarMensajeWeb(DISTRIBUCION);
                //JObjectParametros["SISTEMA"] = TControlPrincipal.Sistema;
                //JObjectParametros["USUARIO"] = TCaracteres.CodificarMensajeWeb(NOMBRE_DE_USUARIO);
                //JObjectParametros["PASSWORD"] = TCaracteres.CodificarMensajeWeb(CONTRASENA);
                //JObjectParametros["TIPO_SISTEMA"] = TVariables.TipoSistema;
                //JObjectParametros["VERSION_APP"] = TVariables.VersionApp;
                //JObjectParametros["SISTEMA_OPERATIVO"] = TVariables.SistemaOperativo;

                //TConexion.CalcularPuertoAleatorio();
                //JObject JObjectServidor = await TConexion.RealizarPeticionGetWebData("TSM_IniciodeSesion", "iniciarSesionAppDirectiva", JObjectParametros, new CancellationToken());
                //if (JObjectServidor.Value<string>("Respuesta") == "true")
                //{
                //    if (JObjectServidor.Value<bool>("USUARIO_VALIDO"))
                //    {
                //        if (JObjectServidor.Value<bool>("VERSION_VALIDA"))
                //        {
                //            // Es necesario que en Procap tenga el permiso APP DIRECTIVA - Ingresar
                //            if (JObjectServidor.Value<bool>("PERMISO_INGRESAR"))
                //            {
                //                if (JObjectServidor.Value<string>("SISTEMA_REAL") != "")
                //                {
                                    if (ModeloBasedeDatos.CrearBasedeDatos())
                                    {
                //                        if (ModeloIniciodeSesion.InsertarConfiguracionInicial(JObjectServidor.Value<string>("NOMBRE_EMPRESA"), NOMBRE_DE_USUARIO, CONTRASENA, JObjectServidor.Value<string>("SISTEMA_REAL"), DISTRIBUCION, new JObject(), new JObject(), TControlPrincipal.VersionBDRequerida))
                                            JObjectRespuesta["Respuesta"] = true;
                                    }
                //                    else
                //                        JObjectRespuesta["Mensaje"] = "No se pudo crear la base de datos";
                //                }
                //                else
                //                    JObjectRespuesta["Mensaje"] = "El sistema no es válido";
                //            }
                //            else
                //                JObjectRespuesta["Mensaje"] = "El usuario no tiene acceso al sistema";
                //        }
                //        else
                //            JObjectRespuesta["Mensaje"] = "La versión del sistema no es válida";
                //    }
                //    else
                //        JObjectRespuesta["Mensaje"] = "Los datos del usuario no son correctos";
                //}
                //else
                //    JObjectRespuesta["Mensaje"] = TMensaje.ObtenerMensajeDefault(JObjectServidor.Value<string>("Respuesta"));
            }
            catch (Exception e)
            {
                JObjectRespuesta["Titulo"] = (string)Application.Current.Resources["StringMensajeErrorTitulo"];
                JObjectRespuesta["Mensaje"] = e.ToString();
            }
            finally
            {
                ModeloIniciodeSesion.CerrarConexion();
                ModeloBasedeDatos.CerrarConexion();
            }
            return JObjectRespuesta;
        }

        public bool SesionIniciada()
        {
            bool respuesta = false;
            TModeloIniciodeSesion ModeloIniciodeSesion = new TModeloIniciodeSesion();
            try
            {
                if (ModeloIniciodeSesion.SesionIniciada())
                    respuesta = true;
            }
            catch (Exception e)
            {
                respuesta = false;
            }
            finally
            {
                ModeloIniciodeSesion.CerrarConexion();
            }
            return respuesta;
        }

        public bool AnalizandoIntegridadBD()
        {
            bool respuesta = false;
            TModeloBasedeDatos ModeloBasedeDatos = new TModeloBasedeDatos();
            try
            {
                int versionInstalada = 0;
                int versionRequerida = TControlPrincipal.VersionBDRequerida;
                if (int.TryParse(ModeloBasedeDatos.ObtenerVersionBDInstalada(), out versionInstalada))
                {
                    if (versionInstalada == versionRequerida)
                        respuesta = true;
                }
                /* Ejecutar script de base de datos */
                if (respuesta == false)
                {
                    respuesta = true;
                    for (int i = versionInstalada; i < versionRequerida && respuesta; i++)
                    {
                        switch (i + 1)
                        {
                            case 2:
                                respuesta = ModeloBasedeDatos.EjecutarScriptBDVersion2();
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], e.ToString(), (string)Application.Current.Resources["StringMensajeBoton"]); });
                respuesta = false;
            }
            finally
            {
                ModeloBasedeDatos.CerrarConexion();
            }
            return respuesta;
        }
    }
}
