using Mono.Data.Sqlite;
using System;
using System.IO;

namespace ElementosComunes.Conexiones
{
    public class TConexionLocal
    {
        private static string _BasedeDatos;

        private SqliteConnection _SqliteConnectionConexion;
        private SqliteTransaction _SqliteTransactionTransaccion;

        public static string BasedeDatos { set { _BasedeDatos = value; } }

        public SqliteCommand SqliteCommandComando { get { return _SqliteConnectionConexion.CreateCommand(); } }
        public bool ConexionAbierta { get { return _SqliteConnectionConexion.State == System.Data.ConnectionState.Open; } }

        public TConexionLocal()
        {
            if (!File.Exists(_BasedeDatos))
                Mono.Data.Sqlite.SqliteConnection.CreateFile(_BasedeDatos);

            if (File.Exists(_BasedeDatos))
            {
                _SqliteConnectionConexion = new SqliteConnection("Data Source=" + _BasedeDatos);
                _SqliteConnectionConexion.Open();
            }
        }

        public void IniciarTransaccion()
        {
            _SqliteTransactionTransaccion = _SqliteConnectionConexion.BeginTransaction();
        }

        public void CancelarTransaccion()
        {
            _SqliteTransactionTransaccion.Rollback();
        }

        public void TerminarTransaccion()
        {
            _SqliteTransactionTransaccion.Commit();
        }

        public void CerrarConexion()
        {
            _SqliteConnectionConexion.Close();
        }

        public static bool ExisteBasedeDatos()
        {
            return File.Exists(_BasedeDatos);
        }

        public static void EliminaBasedeDatos()
        {
            try
            {
                File.Delete(_BasedeDatos);
            }
            catch { }
        }

        /// <summary>
        /// Obtiene el proximo numero disponible de una columna de una tabla,
        /// recordemos que toma la conexion activa
        /// </summary>
        /// <param name="CAMPO"></param>
        /// <param name="TABLA_DE_BASE_DE_DATOS"></param>
        /// <returns></returns>
        public int ObtenerIdSiguiente(string CAMPO, string TABLA_DE_BASE_DE_DATOS)
        {
            int respuesta = -1;
            try
            {
                if (ConexionAbierta)
                {
                    SqliteCommand SqliteCommandComandoTemporal = SqliteCommandComando;
                    SqliteCommandComandoTemporal.CommandText = @"SELECT MAX(" + CAMPO + ") FROM " + TABLA_DE_BASE_DE_DATOS;

                    SqliteCommandComandoTemporal.CommandType = System.Data.CommandType.Text;

                    string respuestaConsulta = SqliteCommandComandoTemporal.ExecuteScalar().ToString();
                    if (string.IsNullOrEmpty(respuestaConsulta))
                        respuesta = 1;
                    else
                        respuesta = int.Parse(respuestaConsulta) + 1;
                }
            }
            catch (Exception e)
            {
                respuesta = -1;
                throw;
            }
            return respuesta;
        }

        /// <summary>
        /// Obtiene el proximo numero disponible de una columna de una tabla,
        /// recordemos que toma la conexion activa
        /// </summary>
        /// <param name="CAMPO"></param>
        /// <param name="TABLA_DE_BASE_DE_DATOS"></param>
        /// <returns></returns>
        public int ObtenerUltimoId(string CAMPO, string TABLA_DE_BASE_DE_DATOS)
        {
            int respuesta = -1;
            try
            {
                if (ConexionAbierta)
                {
                    SqliteCommand SqliteCommandComandoTemporal = SqliteCommandComando;
                    SqliteCommandComandoTemporal.CommandText = @"SELECT MAX(" + CAMPO + ") FROM " + TABLA_DE_BASE_DE_DATOS;

                    SqliteCommandComandoTemporal.CommandType = System.Data.CommandType.Text;

                    string respuestaConsulta = SqliteCommandComandoTemporal.ExecuteScalar().ToString();
                    if (string.IsNullOrEmpty(respuestaConsulta))
                        respuesta = -1;
                    else
                        respuesta = int.Parse(respuestaConsulta);
                }
            }
            catch (Exception e)
            {
                respuesta = -1;
                throw;
            }
            return respuesta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NOMBRE_TABLA"></param>
        /// <param name="NOMBRE_CAMPO"></param>
        /// <param name="VALOR">Es el valor exacto que debe tener el campo, respentando mayusculas y minusculas</param>
        /// <param name="SqliteCommandComando"></param>
        /// <returns></returns>
        public bool ExisteRegistro(string NOMBRE_TABLA, string NOMBRE_CAMPO, string VALOR)
        {
            bool respuesta = false;
            if (ConexionAbierta)
            {
                SqliteCommand SqliteCommandComandoTemporal = SqliteCommandComando;
                SqliteCommandComandoTemporal.CommandText = @"SELECT " + NOMBRE_CAMPO + " FROM " + NOMBRE_TABLA + " WHERE " + NOMBRE_CAMPO + " = @Valor";
                SqliteCommandComandoTemporal.CommandType = System.Data.CommandType.Text;
                SqliteCommandComandoTemporal.Parameters.Add(new SqliteParameter("@Valor", VALOR));
                SqliteDataReader SqliteDataReaderRespuesta = SqliteCommandComandoTemporal.ExecuteReader();
                if (SqliteDataReaderRespuesta != null && SqliteDataReaderRespuesta.HasRows)
                    respuesta = true;
                SqliteDataReaderRespuesta.Close();
            }
            return respuesta;
        }

        public bool BorrarTodoslosRegistros(string NOMBRE_TABLA, SqliteCommand SqliteCommandComando)
        {
            bool respuesta = false;
            if (ConexionAbierta)
            {
                SqliteCommandComando.CommandText = @"DELETE FROM " + NOMBRE_TABLA;
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                SqliteCommandComando.ExecuteNonQuery();
                respuesta = true;
            }
            return respuesta;
        }
    }
}
