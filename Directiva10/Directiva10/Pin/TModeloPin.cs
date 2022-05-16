using ElementosComunes.Conexiones;
using Mono.Data.Sqlite;
using System;

namespace Directiva10.Pin
{
    class TModeloPin
    {
        private TConexionLocal ConexionLocal;

        public TModeloPin()
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

        public bool ObtenerPin(string PIN)
        {
            bool respuesta = false;

            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;

                SqliteCommandComando.CommandText = "SELECT Valor FROM Configuracion WHERE Parametro = 'Pin' AND Valor = @Pin";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;

                SqliteCommandComando.Parameters.Add(new SqliteParameter("@Pin", PIN));

                SqliteDataReader SqliteDataReaderRespuesta = SqliteCommandComando.ExecuteReader();
                if (SqliteDataReaderRespuesta.HasRows)
                    respuesta = true;
            }

            return respuesta;
        }

        public bool ActualizarPin(string PIN)
        {
            bool respuesta = false;

            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;

                SqliteCommandComando.CommandText = "UPDATE Configuracion SET Valor = @Valor WHERE Parametro = 'Pin'";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;

                SqliteCommandComando.Parameters.Add(new SqliteParameter("@Valor", PIN));

                respuesta = SqliteCommandComando.ExecuteNonQuery() == 1;
            }

            return respuesta;
        }

        public bool ActualizarEstadoPinBloqueo(string ESTADO)
        {
            bool respuesta = false;

            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;

                SqliteCommandComando.CommandText = "UPDATE Configuracion SET Valor = @Valor WHERE Parametro = 'PinBloqueo'";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;

                SqliteCommandComando.Parameters.Add(new SqliteParameter("@Valor", ESTADO));

                respuesta = SqliteCommandComando.ExecuteNonQuery() == 1;
            }

            return respuesta;
        }
    }
}
