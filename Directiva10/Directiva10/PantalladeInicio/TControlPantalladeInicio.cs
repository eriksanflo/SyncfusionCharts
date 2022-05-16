using ElementosComunes.Clases;
using ElementosComunes.Conexiones;
using Mono.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Directiva10.PantalladeInicio
{
    internal class TControlPantalladeInicio
    {
        private JObject JObjectParametros;

        public TControlPantalladeInicio()
        {
            JObjectParametros = TVariables.ObtenerParesComunes();
        }

        // Métodos públicos

        public async Task<JObject> ActualizarInformaciondeBalance(int ID, string NOMBRE, CancellationToken CancellationTokenCancelar)
        {
            JObject JObjectRespuesta = new JObject();
            JObjectRespuesta["Respuesta"] = false;
            JObjectRespuesta["Titulo"] = (string)Application.Current.Resources["StringMensajeTitulo"];
            TModeloPantalladeInicio ModeloPantalladeInicio = new TModeloPantalladeInicio();
            try
            {
                JObjectParametros["ID"] = ID.ToString();
                JObjectParametros["NOMBRE"] = TCaracteres.CodificarMensajeWeb(NOMBRE);
                TConexion.CalcularPuertoAleatorio();
                JObject JObjectServidor = await TConexion.RealizarPeticionGetWebData("TSM_ReportesDirectivos", "getTestRespuesta_ADesarrollo", JObjectParametros, CancellationTokenCancelar);
                if (JObjectServidor.Value<string>("Respuesta") == "true")
                {
                    if (ModeloPantalladeInicio.InsertarTodaslasPersonas(JObjectServidor.Value<JObject>("SUCURSALES")))
                        JObjectRespuesta["Respuesta"] = true;
                    else
                        JObjectRespuesta["Mensaje"] = (string)Application.Current.Resources["StringMensajeNoSincronizo"];
                }
                else
                    JObjectRespuesta["Mensaje"] = TMensaje.ObtenerMensajeDefault(JObjectServidor.Value<string>("Respuesta"));
            }
            catch (Exception e)
            {
                JObjectRespuesta["Titulo"] = (string)Application.Current.Resources["StringMensajeErrorTitulo"];
                JObjectRespuesta["Mensaje"] = e.ToString();
            }
            finally
            {
                ModeloPantalladeInicio.CerrarConexion();
            }
            return JObjectRespuesta;
        }

        public async Task<JObject> ActualizarInformacion(int ID, string NOMBRE, CancellationToken CancellationTokenCancelar)
        {
            JObject JObjectRespuesta = new JObject();
            JObjectRespuesta["Respuesta"] = false;
            JObjectRespuesta["Titulo"] = (string)Application.Current.Resources["StringMensajeTitulo"];
            TModeloPantalladeInicio ModeloPantalladeInicio = new TModeloPantalladeInicio();
            try
            {
                JObjectParametros["ID"] = ID.ToString();
                JObjectParametros["NOMBRE"] = TCaracteres.CodificarMensajeWeb(NOMBRE);
                TConexion.CalcularPuertoAleatorio();
                JObject JObjectServidor = await TConexion.RealizarPeticionGetWebData("TSM_ReportesDirectivos", "getTestRespuesta_ADesarrollo", JObjectParametros, CancellationTokenCancelar);
                if (JObjectServidor.Value<string>("Respuesta") == "true")
                {
                    JObjectRespuesta["Respuesta"] = true;
                    JObjectRespuesta["Mensaje"] = "El dato se guardo.";
                }
                else
                    JObjectRespuesta["Mensaje"] = TMensaje.ObtenerMensajeDefault(JObjectServidor.Value<string>("Respuesta"));
            }
            catch (Exception e)
            {
                JObjectRespuesta["Titulo"] = (string)Application.Current.Resources["StringMensajeErrorTitulo"];
                JObjectRespuesta["Mensaje"] = e.ToString();
            }
            finally
            {
                ModeloPantalladeInicio.CerrarConexion();
            }
            return JObjectRespuesta;
        }

        public async Task<ObservableCollection<TPersona>> ObtenerPersonas(int ID_PERSONA, string NOMBRE, CancellationToken CancellationTokenCancelar)
        {
            ObservableCollection<TPersona> ObservableCollectionRespuesta = new ObservableCollection<TPersona>();
            TModeloPantalladeInicio ModeloPantalladeInicio = new TModeloPantalladeInicio();
            try
            {
                JObjectParametros["ID"] = ID_PERSONA.ToString();
                JObjectParametros["NOMBRE"] = TCaracteres.CodificarMensajeWeb(NOMBRE);
                TConexion.CalcularPuertoAleatorio();
                JObject JObjectServidor = await TConexion.RealizarPeticionGetWebData("TSM_ReportesDirectivos", "getTestPersonas_ADesarrollo", JObjectParametros, CancellationTokenCancelar);
                if (JObjectServidor.Value<string>("Respuesta") == "true")
                {
                    //ObservableCollectionRespuesta = JObjectToPersonas(JObjectServidor.Value<JObject>("PERSONAS"));
                    if (ModeloPantalladeInicio.InsertarTodaslasPersonas(JObjectServidor.Value<JObject>("PERSONAS")))
                    {
                        ObservableCollectionRespuesta = SqliteDataReaderToPersonas(ModeloPantalladeInicio.ObtenerPersonas(ID_PERSONA));
                    }
                    else
                        Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], "No se pudo ingresar los datos en el móvil", (string)Application.Current.Resources["StringMensajeBoton"]); });
                }
                else
                    Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], TMensaje.ObtenerMensajeDefault(JObjectServidor.Value<string>("Respuesta")), (string)Application.Current.Resources["StringMensajeBoton"]); });

            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], e.ToString(), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            finally
            {
                ModeloPantalladeInicio.CerrarConexion();
            }
            return ObservableCollectionRespuesta;
        }

        public ObservableCollection<TPersona> ObtenerOtrasPersonas(int ID_PERSONA)
        {
            ObservableCollection<TPersona> ObservableCollectionRespuesta = new ObservableCollection<TPersona>();
            TModeloPantalladeInicio ModeloPantalladeInicio = new TModeloPantalladeInicio();
            try
            {
                ObservableCollectionRespuesta = SqliteDataReaderToPersonas(ModeloPantalladeInicio.ObtenerPersonas(ID_PERSONA));
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], e.ToString(), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            finally
            {
                ModeloPantalladeInicio.CerrarConexion();
            }
            return ObservableCollectionRespuesta;
        }

        // Métodos privados

        private ObservableCollection<TPersona> JObjectToPersonas(JObject JObjectPersonas)
        {
            ObservableCollection<TPersona> ObservableCollectionRespuesta = new ObservableCollection<TPersona>();
            foreach (var persona in JObjectPersonas)
            {
                TPersona Persona = new TPersona();
                Persona.Brazos = persona.Value.Value<int>("ID_PERSONA");
                Persona.Tamano = persona.Value.Value<string>("TAMANO");
                ObservableCollectionRespuesta.Add(Persona);
            }
            return ObservableCollectionRespuesta;
        }

        private ObservableCollection<TPersona> SqliteDataReaderToPersonas(SqliteDataReader SqliteDataReaderPersonas)
        {
            ObservableCollection<TPersona> ObservableCollectionRespuesta = new ObservableCollection<TPersona>();
            if (SqliteDataReaderPersonas != null && SqliteDataReaderPersonas.HasRows)
            {
                while (SqliteDataReaderPersonas.Read())
                {
                    TPersona Persona = new TPersona();
                    Persona.Id = int.Parse(SqliteDataReaderPersonas["IdPersona"].ToString());
                    Persona.Nombre = SqliteDataReaderPersonas["Nombre"].ToString();
                    ObservableCollectionRespuesta.Add(Persona);
                }
            }
            return ObservableCollectionRespuesta;
        }
    }
}
