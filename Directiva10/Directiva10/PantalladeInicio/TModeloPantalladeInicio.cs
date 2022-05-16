using ElementosComunes.Conexiones;
using Mono.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;

namespace Directiva10.PantalladeInicio
{
    internal class TModeloPantalladeInicio
    {
        private TConexionLocal ConexionLocal;

        public TModeloPantalladeInicio()
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

        // Métodos públicos

        public SqliteDataReader ObtenerPersonas(int ID_PERSONA)
        {
            SqliteDataReader SqliteDataReaderRespuesta = null;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;
                SqliteCommandComando.CommandText = @"SELECT * 
                                                       FROM Pruebas";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                //SqliteCommandComando.Parameters.Add(new SqliteParameter("@Nombre", ID_PERSONA));
                SqliteDataReaderRespuesta = SqliteCommandComando.ExecuteReader();
            }
            return SqliteDataReaderRespuesta;
        }


        public bool InsertarTodaslasPersonas(JObject JObjectPersonas)
        {
            bool respuesta = true;
            try
            {
                if (ConexionLocal.ConexionAbierta)
                {
                    ConexionLocal.IniciarTransaccion();
                    SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;

                    respuesta = BorrarTodoslosRegistros(ConexionLocal, "Pruebas", SqliteCommandComando);
                    if (respuesta) respuesta = InsertarPersonas(JObjectPersonas, SqliteCommandComando);
                    
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

        // Métodos privados

        private bool BorrarTodoslosRegistros(TConexionLocal ConexionLocal, string NOMBRE_TABLA, SqliteCommand SqliteCommandComando)
        {
            bool respuesta = false;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommandComando.CommandText = @"DELETE FROM " + NOMBRE_TABLA;
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                SqliteCommandComando.ExecuteNonQuery();
                respuesta = true;
            }
            return respuesta;
        }

        private bool InsertarPersonas(JObject JObjectPersonas, SqliteCommand SqliteCommandComando)
        {
            bool respuesta = true;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommandComando.CommandText = @"INSERT INTO Pruebas (IdPersona, Nombre) 
                                                          VALUES (@IdPersona , @Nombre)";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                foreach (var persona in JObjectPersonas)
                {
                    if (respuesta)
                    {
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@IdPersona", persona.Value.Value<int>("ID_ENTIDAD")));
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@Nombre", persona.Value.Value<string>("NOMBRE_COMPLETO")));
                        respuesta = SqliteCommandComando.ExecuteNonQuery() == 1;
                    }
                    else
                        break;
                }
            }
            return respuesta;
        }

    }
}
