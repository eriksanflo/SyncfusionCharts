using ElementosComunes.Conexiones;
using Mono.Data.Sqlite;
using System;

namespace Directiva10.A_General
{
    class TModeloPrincipal
    {
        private TConexionLocal ConexionLocal;

        public TModeloPrincipal()
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

        public SqliteDataReader ObtenerParametrosdeSesion()
        {
            SqliteDataReader SqliteDataReaderRespuesta = null;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;
                SqliteCommandComando.CommandText = @"SELECT * FROM Sesion WHERE PARAMETRO = 'Distribucion' OR PARAMETRO = 'SistemaReal' OR PARAMETRO = 'Usuario'
                                                                             OR PARAMETRO = 'IdEntidad' OR PARAMETRO = 'Nombre' OR PARAMETRO = 'Apellidos' 
                                                                             OR PARAMETRO = 'TieneAcceso'";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                SqliteDataReaderRespuesta = SqliteCommandComando.ExecuteReader();
            }
            return SqliteDataReaderRespuesta;
        }

    }
}
