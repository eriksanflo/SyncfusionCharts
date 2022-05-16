using Directiva10.A_General;
using ElementosComunes.Conexiones;
using Mono.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Directiva10.Cajas
{
    internal class TModeloCajas
    {
        private TConexionLocal ConexionLocal;

        public TModeloCajas()
        {
            ConexionLocal = new TConexionLocal();
        }

        public void CerrarConexion()
        {
            try
            {
                ConexionLocal.CerrarConexion();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /* Métodos públicos */

        public SqliteDataReader ObtenerBalance()
        {
            SqliteDataReader SqliteDataReaderRespuesta = null;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;
                SqliteCommandComando.CommandText = @"SELECT * 
                                                       FROM Balance";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                SqliteDataReaderRespuesta = SqliteCommandComando.ExecuteReader();
            }
            return SqliteDataReaderRespuesta;
        }

        public SqliteDataReader ObtenerBalanceTotalporSucursales()
        {
            // Nota importante: antes se tenia la instrucción IIF(Sucursales.Clave IS NULL, 'SIN SUCURSAL', Sucursales.Clave) AS Sucursal pero esta no funciona en iOS inferior a 13
            // Así que se coloco la versión viejita CASE WHEN Sucursales.Clave IS NULL THEN 'SIN SUCURSAL' ELSE Sucursales.Clave END as Sucursal y funciona muy bien en Android y iOS
            SqliteDataReader SqliteDataReaderRespuesta = null;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;
                SqliteCommandComando.CommandText = @"SELECT BalancedeUsuarios.IdSucursal, CASE WHEN Sucursales.Clave IS NULL THEN 'SIN SUCURSAL' ELSE Sucursales.Clave END as Sucursal, (SUM(TotaldeIngresos) + SUM(TotaldeRecibos)) - SUM(TotaldeEgresos)  AS TotalFinal 
                                                       FROM BalancedeUsuarios 
                                                  LEFT JOIN Sucursales 
                                                         ON Sucursales.IdSucursal = BalancedeUsuarios.IdSucursal 
                                                   GROUP BY Sucursales.Clave 
                                                   ORDER BY Sucursal";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                SqliteDataReaderRespuesta = SqliteCommandComando.ExecuteReader();
            }
            return SqliteDataReaderRespuesta;
        }

        public SqliteDataReader ObtenerBalanceporUsuariosdeSucursal(int ID_SUCURSAL)
        {
            SqliteDataReader SqliteDataReaderRespuesta = null;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;
                SqliteCommandComando.CommandText = @"SELECT *, (TotaldeRecibos - TotaldeRecibosEfectivo) AS Depositos 
                                                       FROM BalancedeUsuarios
                                                      WHERE IdSucursal = @IdSucursal
                                                   ORDER BY UsuariodeRegistro";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@IdSucursal", ID_SUCURSAL));
                SqliteDataReaderRespuesta = SqliteCommandComando.ExecuteReader();
            }
            return SqliteDataReaderRespuesta;
        }

        public bool InsertarInformaciondeBalance(JObject JObjectSucursales, JObject JObjectBalanceActualdeUsuarios, string FECHA_DE_SERVIDOR, string HORA_DE_SERVIDOR, double INGRESO_ACUMULADO_DE_HOY, double INGRESO_ACUMULADO_SEMANAL, double INGRESO_ACUMULADO_MENSUAL,
                                                 double EGRESO_ACUMULADO_DE_HOY, double EGRESO_ACUMULADO_SEMANAL, double EGRESO_ACUMULADO_MENSUAL)
        {
            bool respuesta = false;
            try
            {
                if (ConexionLocal.ConexionAbierta)
                {
                    TMetodosComunes MetodosComunes = new TMetodosComunes();
                    ConexionLocal.IniciarTransaccion();
                    SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;

                    respuesta = MetodosComunes.BorrarTodoslosRegistros(ConexionLocal, "Sucursales", SqliteCommandComando);
                    if (respuesta) respuesta = MetodosComunes.BorrarTodoslosRegistros(ConexionLocal, "Balance", SqliteCommandComando);
                    if (respuesta) respuesta = MetodosComunes.BorrarTodoslosRegistros(ConexionLocal, "BalancedeUsuarios", SqliteCommandComando);
                    if(respuesta) respuesta = InsertarSucursales(JObjectSucursales, SqliteCommandComando);
                    if(respuesta) respuesta = InsertarBalancedeUsuarios(JObjectBalanceActualdeUsuarios, SqliteCommandComando);
                    if(respuesta) respuesta = InsertarBalance(FECHA_DE_SERVIDOR, HORA_DE_SERVIDOR, INGRESO_ACUMULADO_DE_HOY, INGRESO_ACUMULADO_SEMANAL, INGRESO_ACUMULADO_MENSUAL, EGRESO_ACUMULADO_DE_HOY, EGRESO_ACUMULADO_SEMANAL, EGRESO_ACUMULADO_MENSUAL, SqliteCommandComando);
                    if (respuesta)
                        ConexionLocal.TerminarTransaccion();
                    else
                        ConexionLocal.CancelarTransaccion();
                }
            }
            catch (Exception e)
            {
                ConexionLocal.CancelarTransaccion();
                throw e;
            }
            return respuesta;
        }

        /* Métodos privados */

        private bool InsertarSucursales(JObject JObjectSucursales, SqliteCommand SqliteCommandComando)
        {
            bool respuesta = true;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommandComando.CommandText = @"INSERT INTO Sucursales (IdSucursal, Clave) 
                                                          VALUES (@IdSucursal , @Clave)";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                foreach (var sucursal in JObjectSucursales)
                {
                    if (respuesta)
                    {
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@IdSucursal", sucursal.Value.Value<int>("ID_SUCURSAL")));
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@Clave", sucursal.Value.Value<string>("CLAVE")));
                        respuesta = SqliteCommandComando.ExecuteNonQuery() == 1;
                    }
                    else
                        break;
                }
            }
            return respuesta;
        }

        private bool InsertarBalancedeUsuarios(JObject JObjectBalanceActualdeUsuarios, SqliteCommand SqliteCommandComando)
        {
            bool respuesta = true;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommandComando.CommandText = @"INSERT INTO BalancedeUsuarios (UsuariodeRegistro, IdSucursal, TotaldeIngresos, TotaldeRecibos, TotaldeRecibosEfectivo, TotaldeEgresos) 
                                                          VALUES (@UsuariodeRegistro , @IdSucursal, @TotaldeIngresos, @TotaldeRecibos, @TotaldeRecibosEfectivo, @TotaldeEgresos)";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                foreach (var balancedeUsuario in JObjectBalanceActualdeUsuarios)
                {
                    if (respuesta)
                    {
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@UsuariodeRegistro", balancedeUsuario.Value.Value<string>("USUARIO_DE_REGISTRO")));
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@IdSucursal", string.IsNullOrWhiteSpace(balancedeUsuario.Value.Value<string>("ID_SUCURSAL"))? 0 : balancedeUsuario.Value.Value<int>("ID_SUCURSAL")));
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@TotaldeIngresos", balancedeUsuario.Value.Value<double>("TOTAL_DE_INGRESOS")));
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@TotaldeRecibos", balancedeUsuario.Value.Value<double>("TOTAL_DE_RECIBOS")));
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@TotaldeRecibosEfectivo", balancedeUsuario.Value.Value<double>("TOTAL_DE_RECIBOS_EFECTIVO")));
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@TotaldeEgresos", balancedeUsuario.Value.Value<double>("TOTAL_DE_EGRESOS")));
                        respuesta = SqliteCommandComando.ExecuteNonQuery() == 1;
                    }
                    else
                        break;
                }
            }
            return respuesta;
        }

        private bool InsertarBalance(string FECHA_DE_SERVIDOR, string HORA_DE_SERVIDOR, double INGRESO_ACUMULADO_DE_HOY, double INGRESO_ACUMULADO_SEMANAL, double INGRESO_ACUMULADO_MENSUAL,
                                     double EGRESO_ACUMULADO_DE_HOY, double EGRESO_ACUMULADO_SEMANAL, double EGRESO_ACUMULADO_MENSUAL, SqliteCommand SqliteCommandComando)
        {
            bool respuesta = false;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommandComando.CommandText = @"INSERT INTO Balance (FechaUltimaSincronizacion, HoraUltimaSincronizacion, IngresoAcumuladodeHoy, IngresoAcumuladoSemanal, IngresoAcumuladoMensual, 
                                                                          EgresoAcumuladodeHoy, EgresoAcumuladoSemanal, EgresoAcumuladoMensual) 
                                                          VALUES (@FechaUltimaSincronizacion , @HoraUltimaSincronizacion, @IngresoAcumuladodeHoy, @IngresoAcumuladoSemanal, @IngresoAcumuladoMensual, 
                                                                  @EgresoAcumuladodeHoy, @EgresoAcumuladoSemanal, @EgresoAcumuladoMensual)";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@FechaUltimaSincronizacion", FECHA_DE_SERVIDOR));
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@HoraUltimaSincronizacion", HORA_DE_SERVIDOR));
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@IngresoAcumuladodeHoy", INGRESO_ACUMULADO_DE_HOY));
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@IngresoAcumuladoSemanal", INGRESO_ACUMULADO_SEMANAL));
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@IngresoAcumuladoMensual", INGRESO_ACUMULADO_MENSUAL));
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@EgresoAcumuladodeHoy", EGRESO_ACUMULADO_DE_HOY));
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@EgresoAcumuladoSemanal", EGRESO_ACUMULADO_SEMANAL));
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@EgresoAcumuladoMensual", EGRESO_ACUMULADO_MENSUAL));
                respuesta = SqliteCommandComando.ExecuteNonQuery() == 1;
            }
            return respuesta;
        }
    }
}
