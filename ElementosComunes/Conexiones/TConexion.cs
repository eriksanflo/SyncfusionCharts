using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElementosComunes.Conexiones
{
    public class TConexion
    {
        private static string _IpServidor;
        private static string _Puerto;

        private static bool _Conectado;
        private static bool _Accesible;

        private static int _PuertoRangoInicio;
        private static int _PuertoRangoFin;
        private static TimeSpan _TiempodeEsperaenPeticion;

        public static string IpServidor { set { _IpServidor = value; } }
        public static string Puerto { set { _Puerto = value; } }
        public static bool EsConexionProductivo { get; set; }

        public static void EstaConectado()
        {
            if (CrossConnectivity.Current.IsConnected)
                _Conectado = true;
        }

        public static bool HayConexion()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        public static void EsAccesible()
        {           
            _Accesible = false;
            try
            {
                if (EsConexionProductivo)
                {
                    HttpWebRequest HttpWebRequestPeticion = (HttpWebRequest)WebRequest.Create("https://www.amazon.com/");
                    HttpWebRequestPeticion.Timeout = 5000;
                    WebResponse WebResponseRespuesta = HttpWebRequestPeticion.GetResponse();
                    WebResponseRespuesta.Close();                
                    _Accesible = true;
                }
                else
                    _Accesible = true;
            }
            catch (WebException e)
            {
                _Accesible = false;
            }
        }

        public static bool TieneConectividadaInternet()
        {
            bool respuesta = false;
            EstaConectado();
            EsAccesible();
            respuesta = _Conectado && _Accesible;
            return respuesta;
        }

        public static void AsignarRangodePuertos(int PUERTO_RANGO_INICIO, int PUERTO_RANGO_FIN)
        {
            _Puerto = PUERTO_RANGO_INICIO.ToString();
            _PuertoRangoInicio = PUERTO_RANGO_INICIO;
            _PuertoRangoFin = PUERTO_RANGO_FIN;
        }

        public static void AsignarTiempoMaximodeEsperaenPeticion(double SEGUNDOS)
        {
            if(SEGUNDOS > 0)
                _TiempodeEsperaenPeticion = TimeSpan.FromSeconds(SEGUNDOS);
            else
                _TiempodeEsperaenPeticion = TimeSpan.FromMinutes(5);
        }

        public static void CalcularPuertoAleatorio()
        {
            Random RandomPuerto = new Random();
            _Puerto = RandomPuerto.Next(_PuertoRangoInicio, (_PuertoRangoFin + 1)).ToString();
        }

        public static async Task<JObject> RealizarPeticionGetWeb(string CLASE, string METODO, JObject JObjectJSON)
        {
            JObject JObjectRespuesta = new JObject();
            HttpClient httpClientCliente = new HttpClient();
            try
            {
                EstaConectado();
                EsAccesible();

                if (_Conectado)
                {
                    if (_Accesible)
                    {
                        string json = JObjectJSON.ToString();
                        Uri UriUrl = new Uri("http://" + _IpServidor + ":" + _Puerto + "/datasnap/rest/" + CLASE + "/" + METODO + "/" + json);

                        HttpResponseMessage HttpResponseMessagePeticion = await httpClientCliente.GetAsync(UriUrl);

                        if (HttpResponseMessagePeticion.IsSuccessStatusCode)
                        {
                            string respuestaPeticion = await HttpResponseMessagePeticion.Content.ReadAsStringAsync();
                            JObject JObjectRespuestaTemporal = JObject.Parse(respuestaPeticion);
                            JObjectRespuesta = JObject.Parse(JObjectRespuestaTemporal["result"][0].ToString());
                        }
                        else
                        {
                            JObjectRespuesta["Respuesta"] = "Error en el envío de los datos: " + HttpResponseMessagePeticion.StatusCode;
                        }
                    }
                    else
                    {
                        JObjectRespuesta["Respuesta"] = "No Accesible";
                    }
                }
                else
                {
                    JObjectRespuesta["Respuesta"] = "No Conectado";
                }
            }
            catch (Exception e)
            {
                JObjectRespuesta["Respuesta"] = "Sin Respuesta";
            }
            return JObjectRespuesta;
        }

        public static async Task<JObject> RealizarPeticionGetWebData(string CLASE, string METODO, JObject JObjectJSON, CancellationToken CancellationTokenCancelar)
        {
            JObject JObjectRespuesta = new JObject();
            HttpClient httpClientCliente = new HttpClient();
            try
            {
                /*if (EsConexionProductivo)
                    httpClientCliente.Timeout = _TiempodeEsperaenPeticion;
                else
                    httpClientCliente.Timeout = TimeSpan.FromSeconds(5);*/
                //await Task.Delay(500);
                EstaConectado();
                EsAccesible();

                if (_Conectado)
                {
                    if (_Accesible)
                    {
                        string json = JObjectJSON.ToString();
                        Uri UriUrl = new Uri("http://" + _IpServidor + ":" + _Puerto + "/datasnap/rest/" + CLASE + "/" + METODO + "/" + json);

                        HttpResponseMessage HttpResponseMessagePeticion = await httpClientCliente.GetAsync(UriUrl, CancellationTokenCancelar);

                        if (HttpResponseMessagePeticion.IsSuccessStatusCode)
                        {
                            string respuestaPeticion = await HttpResponseMessagePeticion.Content.ReadAsStringAsync();
                            JObject JObjectRespuestaTemporal = JObject.Parse(respuestaPeticion);
                            JObjectRespuesta = JObject.Parse(JObjectRespuestaTemporal["result"][0].ToString());
                        }
                        else
                        {
                            JObjectRespuesta["Respuesta"] = "Error en el envío de los datos: " + HttpResponseMessagePeticion.StatusCode;
                        }
                    }
                    else
                    {
                        JObjectRespuesta["Respuesta"] = "No Accesible";
                    }
                }
                else
                {
                    JObjectRespuesta["Respuesta"] = "No Conectado";
                }
            }
            
            catch (TaskCanceledException) when (CancellationTokenCancelar.IsCancellationRequested)
            {
                JObjectRespuesta["Respuesta"] = "Usuario Cancelo";
                Console.WriteLine("Usuario cancelo forma 1");
            }
            catch (TaskCanceledException)
            {
                JObjectRespuesta["Respuesta"] = "Tiempo Espera Agotado";
                Console.WriteLine("Tiempo agotado");
            }
            catch (OperationCanceledException ex)
            {
                JObjectRespuesta["Respuesta"] = "Usuario Cancelo";
                Console.WriteLine("Usuario cancelo forma 2");
            }
            catch (Exception e)
            {
                JObjectRespuesta["Respuesta"] = "Sin Respuesta";
                Console.WriteLine("Error no reconocido");
            }
            return JObjectRespuesta;
        }
    }
}
