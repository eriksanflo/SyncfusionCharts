using ElementosComunes.Clases;
using ElementosComunes.Conexiones;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Directiva10.Pin
{
    class TControlPin
    {
        private JObject JObjectParametros;

        public TControlPin()
        {
            JObjectParametros = TVariables.ObtenerParesComunes();
        }

        /* Métodos públicos */

        public bool EsPinValido(string PIN)
        {
            bool respuesta = false;
            TModeloPin ModeloPin = new TModeloPin();
            try
            {
                if (ModeloPin.ObtenerPin(PIN))
                    respuesta = true;
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "OK"); });
            }
            finally
            {
                ModeloPin.CerrarConexion();
            }
            return respuesta;
        }

        public bool ActualizarPin(string NUEVO_PIN)
        {
            bool respuesta = false;
            TModeloPin ModeloPin = new TModeloPin();
            try
            {
                if (ModeloPin.ActualizarPin(NUEVO_PIN))
                    respuesta = true;
            }
            catch (Exception e)
            {
                respuesta = false;
                Device.BeginInvokeOnMainThread(async () => { await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "OK"); });
            }
            finally
            {
                ModeloPin.CerrarConexion();
            }
            return respuesta;
        }

        public void ActualizarEstadoPinBloqueo(bool ACTIVO)
        {
            TModeloPin ModeloPin = new TModeloPin();

            try
            {
                string esActivo = ACTIVO ? "SI" : "NO";
                ModeloPin.ActualizarEstadoPinBloqueo(esActivo);
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await App.Current.MainPage.DisplayAlert("Error", e.ToString(), "OK"); });
            }
            finally
            {
                ModeloPin.CerrarConexion();
            }
        }

        public async Task<JObject> RestablecerPin(string CONTRASENA)
        {
            JObject JObjectRespuesta = new JObject();
            JObjectRespuesta["Respuesta"] = false;

            TModeloPin ModeloPin = new TModeloPin();
            try
            {
                JObjectParametros["PASSWORD"] = TCaracteres.CodificarMensajeWeb(CONTRASENA);
                TConexion.CalcularPuertoAleatorio();

                JObject JObjectServidor = await TConexion.RealizarPeticionGetWeb("TSM_IniciodeSesion", "reestablecerPinOP", JObjectParametros);
                if (JObjectServidor.Value<bool>("Respuesta"))
                {
                    if (JObjectServidor.Value<bool>("USUARIO"))
                    {
                        if (JObjectServidor.Value<bool>("LICENCIA"))
                        {
                            if (ModeloPin.ActualizarPin("0000"))
                            {
                                JObjectRespuesta["Mensaje"] = "El pin de seguridad ha sido restaurado exitosamente a '0000'";
                                JObjectRespuesta["Respuesta"] = true;
                            }
                            else
                                JObjectRespuesta["Mensaje"] = "El pin de seguridad no se pudo restaurar";
                        }
                        else
                            JObjectRespuesta["Mensaje"] = "La licencia del sistema no es valida";
                    }
                    else
                        JObjectRespuesta["Mensaje"] = "Los datos del usuario no son correctos";
                }
                else
                    JObjectRespuesta["Mensaje"] = TMensaje.ObtenerMensajeDefault(JObjectServidor.Value<string>("Respuesta"));
            }
            catch (Exception e)
            {
                JObjectRespuesta["Mensaje"] = e.ToString();
            }
            finally
            {
                ModeloPin.CerrarConexion();
            }
            return JObjectRespuesta;
        }
    }
}
