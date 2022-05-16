using Directiva10.A_Compartir;
using ElementosComunes.Clases;
using ElementosComunes.Conexiones;
using Mono.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Directiva10.Cajas
{
    internal class TControlCajas
    {
        private JObject JObjectParametros;

        public TControlCajas()
        {
            JObjectParametros = TVariables.ObtenerParesComunes();
        }

        /* Métodos públicos */

        public async Task<JObject> ActualizarInformaciondeBalance(CancellationToken CancellationTokenCancelar)
        {
            JObject JObjectRespuesta = new JObject();
            JObjectRespuesta["Respuesta"] = false;
            JObjectRespuesta["Titulo"] = (string)Application.Current.Resources["StringMensajeTitulo"];
            TModeloCajas ModeloCajas = new TModeloCajas();
            try
            {
                TConexion.CalcularPuertoAleatorio();
                JObject JObjectServidor = await TConexion.RealizarPeticionGetWebData("TSM_ReportesDirectivos", "getDetallesdeCajas", JObjectParametros, CancellationTokenCancelar);
                if (JObjectServidor.Value<string>("Respuesta") == "true")
                {
                    if (ModeloCajas.InsertarInformaciondeBalance(JObjectServidor.Value<JObject>("SUCURSALES"), JObjectServidor.Value<JObject>("BALANCE_ACTUAL_DE_USUARIOS"), JObjectServidor.Value<string>("FECHA_DE_SERVIDOR"), JObjectServidor.Value<string>("HORA_DE_SERVIDOR"), JObjectServidor.Value<double>("INGRESO_ACUMULADO_DE_HOY"), JObjectServidor.Value<double>("INGRESO_ACUMULADO_SEMANAL"), JObjectServidor.Value<double>("INGRESO_ACUMULADO_MENSUAL"), JObjectServidor.Value<double>("EGRESO_ACUMULADO_DE_HOY"), JObjectServidor.Value<double>("EGRESO_ACUMULADO_SEMANAL"), JObjectServidor.Value<double>("EGRESO_ACUMULADO_MENSUAL")))
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
                ModeloCajas.CerrarConexion();
            }
            return JObjectRespuesta;
        }

        public TBalance ObtenerBalance()
        {
            TBalance BalanceRespuesta = new TBalance();
            TModeloCajas ModeloCajas = new TModeloCajas();
            try
            {
                BalanceRespuesta = SqliteDataReaderToBalance(ModeloCajas.ObtenerBalanceTotalporSucursales(), ModeloCajas.ObtenerBalance());
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], e.ToString(), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            finally
            {
                ModeloCajas.CerrarConexion();
            }
            return BalanceRespuesta;
        }

        public ObservableCollection<TBalancedeUsuario> ObtenerBalanceporUsuariosdeSucursal(int ID_SUCURSAL)
        {
            ObservableCollection<TBalancedeUsuario> ObservableCollectionRespuesta = new ObservableCollection<TBalancedeUsuario>();
            TModeloCajas ModeloCajas = new TModeloCajas();
            try
            {
                ObservableCollectionRespuesta = SqliteDataReaderToBalancedeUsuarios(ModeloCajas.ObtenerBalanceporUsuariosdeSucursal(ID_SUCURSAL));
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], e.ToString(), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            finally
            {
                ModeloCajas.CerrarConexion();
            }
            return ObservableCollectionRespuesta;
        }

        /*public async Task<TReportedeCajas> ObtenerReportedeCajas(bool ES_MODULO_DE_INGRESOS, CancellationToken CancellationTokenCancelar)
        {
            TReportedeCajas ReportedeCajasRespuesta = new TReportedeCajas();
            TModeloCajas ModeloCajas = new TModeloCajas();
            try
            {
                string datos = TCifrado.Codificar(JObjectParametros.ToString());
                TConexion.CalcularPuertoAleatorio();
                JObject JObjectServidor = await TConexion.RealizarPeticionGetWebData("TSM_ReportesDirectivos", "getDetallesdeCajas", datos, CancellationTokenCancelar);
                if (JObjectServidor.Value<string>("Respuesta") == "true")
                {
                    if (ModeloCajas.InsertarInformaciondeBalance(JObjectServidor.Value<JObject>("SUCURSALES"), JObjectServidor.Value<JObject>("BALANCE_ACTUAL_DE_USUARIOS"), JObjectServidor.Value<string>("FECHA_DE_SERVIDOR"), JObjectServidor.Value<string>("HORA_DE_SERVIDOR"), JObjectServidor.Value<double>("INGRESO_ACUMULADO_DE_HOY"), JObjectServidor.Value<double>("INGRESO_ACUMULADO_SEMANAL"), JObjectServidor.Value<double>("INGRESO_ACUMULADO_MENSUAL"), JObjectServidor.Value<double>("EGRESO_ACUMULADO_DE_HOY"), JObjectServidor.Value<double>("EGRESO_ACUMULADO_SEMANAL"), JObjectServidor.Value<double>("EGRESO_ACUMULADO_MENSUAL")))
                    {

                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeTitulo"], (string)Application.Current.Resources["StringMensajeNoSincronizo"], (string)Application.Current.Resources["StringMensajeBoton"]); });
                    }
                    ReportedeCajasRespuesta.TotaldeHoy = JObjectServidor.Value<double>("IngresoTotaldeHoy");
                    ReportedeCajasRespuesta.TotalSemana = JObjectServidor.Value<double>("IngresoTotalSemanal");
                    ReportedeCajasRespuesta.TotalMes = JObjectServidor.Value<double>("IngresoTotalMensual");
                    ReportedeCajasRespuesta.ObservableCollectionSucursales = JObjectToSucursales(JObjectServidor.Value<JObject>("Sucursales"));
                }
                else
                    Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeTitulo"], TMensaje.ObtenerMensajeDefault(JObjectServidor.Value<string>("Respuesta")), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], e.ToString(), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            finally
            {
                ModeloCajas.CerrarConexion();
            }
            return ReportedeCajasRespuesta;
        }*/

        /* Métodos privados */

        /*private ObservableCollection<TSucursal> JObjectToSucursales(JObject JObjectSucursales)
        {
            ObservableCollection<TSucursal> ObservableCollectionRespuesta = new ObservableCollection<TSucursal>();
            foreach (var sucursal in JObjectSucursales)
            {
                TSucursal Sucursal = new TSucursal();
                Sucursal.Id = sucursal.Value.Value<int>("IdSucursal");
                Sucursal.Nombre = sucursal.Value.Value<string>("Nombre");
                Sucursal.Monto = sucursal.Value.Value<double>("Cantidad");
                ObservableCollectionRespuesta.Add(Sucursal);
            }
            return ObservableCollectionRespuesta;
        }*/

        private TBalance SqliteDataReaderToBalance(SqliteDataReader SqliteDataReaderBalanceTotalporSucursales, SqliteDataReader SqliteDataReaderBalance)
        {
            TBalance BalanceRespuesta = new TBalance();
            if (SqliteDataReaderBalanceTotalporSucursales != null && SqliteDataReaderBalanceTotalporSucursales.HasRows)
            {
                while (SqliteDataReaderBalanceTotalporSucursales.Read())
                {
                    TSucursal Sucursal = new TSucursal();
                    Sucursal.Id = int.Parse(SqliteDataReaderBalanceTotalporSucursales["IdSucursal"].ToString());
                    Sucursal.Nombre = SqliteDataReaderBalanceTotalporSucursales["Sucursal"].ToString();
                    Sucursal.Monto = double.Parse(SqliteDataReaderBalanceTotalporSucursales["TotalFinal"].ToString());
                    BalanceRespuesta.ObservableCollectionSucursales.Add(Sucursal);
                }
            }
            if (SqliteDataReaderBalance != null && SqliteDataReaderBalance.HasRows)
            {
                while (SqliteDataReaderBalance.Read())
                {
                    BalanceRespuesta.IngresoAcumuladodeHoy = double.Parse(SqliteDataReaderBalance["IngresoAcumuladodeHoy"].ToString());
                    BalanceRespuesta.IngresoAcumuladoSemanal = double.Parse(SqliteDataReaderBalance["IngresoAcumuladoSemanal"].ToString());
                    BalanceRespuesta.IngresoAcumuladoMensual = double.Parse(SqliteDataReaderBalance["IngresoAcumuladoMensual"].ToString());
                    BalanceRespuesta.EgresoAcumuladodeHoy = double.Parse(SqliteDataReaderBalance["EgresoAcumuladodeHoy"].ToString());
                    BalanceRespuesta.EgresoAcumuladoSemanal = double.Parse(SqliteDataReaderBalance["EgresoAcumuladoSemanal"].ToString());
                    BalanceRespuesta.EgresoAcumuladoMensual = double.Parse(SqliteDataReaderBalance["EgresoAcumuladoMensual"].ToString());
                    if (!string.IsNullOrWhiteSpace(SqliteDataReaderBalance["FechaUltimaSincronizacion"].ToString()))
                    {
                        // El formato debe ser YYYY/MM/DD
                        if (DateTime.TryParse(SqliteDataReaderBalance["FechaUltimaSincronizacion"].ToString(), out DateTime fecha))
                            BalanceRespuesta.FechaUltimaSincronizacion = fecha;
                    }
                    if (!string.IsNullOrWhiteSpace(SqliteDataReaderBalance["HoraUltimaSincronizacion"].ToString()))
                    {
                        // El formato debe ser 24hrs
                        if (TimeSpan.TryParse(SqliteDataReaderBalance["HoraUltimaSincronizacion"].ToString(), out TimeSpan hora) && BalanceRespuesta.FechaUltimaSincronizacion != new DateTime())
                            BalanceRespuesta.HoraUltimaSincronizacion = hora;
                    }
                }
            }
            return BalanceRespuesta;
        }

        private ObservableCollection<TBalancedeUsuario> SqliteDataReaderToBalancedeUsuarios(SqliteDataReader SqliteDataReaderBalancedeUsuarios)
        {
            ObservableCollection<TBalancedeUsuario> ObservableCollectionRespuesta = new ObservableCollection<TBalancedeUsuario>();
            if (SqliteDataReaderBalancedeUsuarios != null && SqliteDataReaderBalancedeUsuarios.HasRows)
            {
                while (SqliteDataReaderBalancedeUsuarios.Read())
                {
                    TBalancedeUsuario BalancedeUsuario = new TBalancedeUsuario();
                    BalancedeUsuario.Nombre = SqliteDataReaderBalancedeUsuarios["UsuariodeRegistro"].ToString();
                    BalancedeUsuario.TotalCobrado = double.Parse(SqliteDataReaderBalancedeUsuarios["TotaldeRecibos"].ToString());
                    BalancedeUsuario.TotalEfectivo = double.Parse(SqliteDataReaderBalancedeUsuarios["TotaldeRecibosEfectivo"].ToString());
                    BalancedeUsuario.Efectivo = double.Parse(SqliteDataReaderBalancedeUsuarios["TotaldeIngresos"].ToString());
                    BalancedeUsuario.Depositos = double.Parse(SqliteDataReaderBalancedeUsuarios["Depositos"].ToString());
                    BalancedeUsuario.Gastos = double.Parse(SqliteDataReaderBalancedeUsuarios["TotaldeEgresos"].ToString());
                    ObservableCollectionRespuesta.Add(BalancedeUsuario);
                }
            }
            return ObservableCollectionRespuesta;
        }
    }
}
