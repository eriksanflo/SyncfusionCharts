using Directiva10.A_Compartir;
using ElementosComunes.Clases;
using ElementosComunes.Conexiones;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Directiva10.Usuarios
{
    internal class TControlUsuarios
    {
        private JObject JObjectParametros;

        public TControlUsuarios()
        {
            JObjectParametros = TVariables.ObtenerParesComunes();
        }

        // Métodos públicos

        public async Task<ObservableCollection<TUsuario>> ObtenerUsuarios(CancellationToken CancellationTokenCancelar)
        {
            ObservableCollection<TUsuario> ObservableCollectionRespuesta = new ObservableCollection<TUsuario>();
            try
            {
                TConexion.CalcularPuertoAleatorio();
                JObject JObjectServidor = await TConexion.RealizarPeticionGetWebData("TSM_ReportesDirectivos", "", JObjectParametros, CancellationTokenCancelar);
                if (JObjectServidor.Value<string>("Respuesta") == "true")
                    ObservableCollectionRespuesta = JObjectToUsuarios(JObjectServidor.Value<JObject>("USUARIOS"));
                else
                    Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeTitulo"], TMensaje.ObtenerMensajeDefault(JObjectServidor.Value<string>("Respuesta")), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], e.ToString(), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            return ObservableCollectionRespuesta;
        }

        public async Task<ObservableCollection<TPermiso>> ObtenerPermisosdeUsuario(int ID_USUARIO, CancellationToken CancellationTokenCancelar)
        {
            ObservableCollection<TPermiso> ObservableCollectionRespuesta = new ObservableCollection<TPermiso>();
            try
            {
                TConexion.CalcularPuertoAleatorio();
                JObject JObjectServidor = await TConexion.RealizarPeticionGetWebData("TSM_ReportesDirectivos", "", JObjectParametros, CancellationTokenCancelar);
                if (JObjectServidor.Value<string>("Respuesta") == "true")
                    ObservableCollectionRespuesta = JObjectToPermisos(JObjectServidor.Value<JObject>("PERMISOS"));
                else
                    Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeTitulo"], TMensaje.ObtenerMensajeDefault(JObjectServidor.Value<string>("Respuesta")), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], e.ToString(), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            return ObservableCollectionRespuesta;
        }

        // Métodos privados

        private ObservableCollection<TUsuario> JObjectToUsuarios(JObject JObjectUsuarios)
        {
            ObservableCollection<TUsuario> ObservableCollectionRespuesta = new ObservableCollection<TUsuario>();
            foreach (var usuario in JObjectUsuarios)
            {
                TUsuario Usuario = new TUsuario();
                Usuario.Id = usuario.Value.Value<int>("ID_USUARIO");
                Usuario.Nombre = usuario.Value.Value<string>("NOMBRE");
                Usuario.Sistema = usuario.Value.Value<string>("SISTEMA");
                Usuario.VersionInstalada = usuario.Value.Value<string>("VERSION_INSTALADA");
                Usuario.EstaOnline = usuario.Value.Value<string>("VERSION_INSTALADA") == "ONLINE";
                if (!string.IsNullOrWhiteSpace(usuario.Value.Value<string>("FECHA_ULTIMA_CONEXION")))
                {
                    // El formato debe ser YYYY/MM/DD
                    if (DateTime.TryParse(usuario.Value.Value<string>("FECHA_ULTIMA_CONEXION"), out DateTime fecha))
                        Usuario.FechadeConexion = fecha;
                }
                if (!string.IsNullOrWhiteSpace(usuario.Value.Value<string>("HORA_ULTIMA_CONEXION")))
                {
                    // El formato debe ser 24hrs
                    if (TimeSpan.TryParse(usuario.Value.Value<string>("HORA_ULTIMA_CONEXION"), out TimeSpan hora) && Usuario.FechadeConexion != new DateTime())
                        Usuario.HoradeConexion = hora;
                }
                ObservableCollectionRespuesta.Add(Usuario);
            }
            return ObservableCollectionRespuesta;
        }

        private ObservableCollection<TPermiso> JObjectToPermisos(JObject JObjectPermisos)
        {
            ObservableCollection<TPermiso> ObservableCollectionRespuesta = new ObservableCollection<TPermiso>();
            foreach (var permiso in JObjectPermisos)
            {
                TPermiso Permiso = new TPermiso();
                Permiso.Id = permiso.Value.Value<int>("ID_PERMISO");
                Permiso.Nombre = permiso.Value.Value<string>("NOMBRE");
                Permiso.EsEspecial = permiso.Value.Value<string>("SISTEMA") == "ESPECIAL";
                ObservableCollectionRespuesta.Add(Permiso);
            }
            return ObservableCollectionRespuesta;
        }
    }
}
