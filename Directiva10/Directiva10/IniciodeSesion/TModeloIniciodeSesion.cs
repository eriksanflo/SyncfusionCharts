using ElementosComunes.Conexiones;
using Mono.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;

namespace Directiva10.IniciodeSesion
{
    class TModeloIniciodeSesion
    {
        private TConexionLocal ConexionLocal;

        public TModeloIniciodeSesion()
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

        public bool SesionIniciada()
        {
            bool respuesta = false;
            try
            {
                if (ConexionLocal.ConexionAbierta)
                {
                    SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;
                    SqliteCommandComando.CommandText = "SELECT * FROM Sesion";
                    SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                    SqliteDataReader SqliteDataReaderRespuesta = SqliteCommandComando.ExecuteReader();
                    if (SqliteDataReaderRespuesta.HasRows)
                        respuesta = true;
                }
            }
            catch (Exception e)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public bool InsertarConfiguracionInicial(string NOMBRE_EMPRESA, string NOMBRE_DE_USUARIO, string CONTRASENA, string SISTEMA_REAL, string DISTRIBUCION, JObject JObjectInformacionUsuario, JObject JObjectPermisosUsuario, int VERSION_BD)
        {
            bool respuesta = false;
            try
            {
                if (ConexionLocal.ConexionAbierta)
                {
                    ConexionLocal.IniciarTransaccion();
                    SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;

                    respuesta = InsertarSesion("Usuario", NOMBRE_DE_USUARIO, SqliteCommandComando);
                    if (respuesta)
                    {
                        if (NOMBRE_DE_USUARIO.ToLower() == "admin")
                        {
                            if (respuesta) respuesta = InsertarSesion("IdEntidad", "0", SqliteCommandComando);
                            if (respuesta) respuesta = InsertarSesion("Nombre", "Administrador", SqliteCommandComando);
                            if (respuesta) respuesta = InsertarSesion("Apellidos", "", SqliteCommandComando);
                        }
                        else
                        {
                            foreach (var informacion in JObjectInformacionUsuario)
                            {
                                if (respuesta)
                                {
                                    if (respuesta) respuesta = InsertarSesion("IdEntidad", informacion.Value.Value<string>("ID_ENTIDAD"), SqliteCommandComando);
                                    if (respuesta) respuesta = InsertarSesion("Nombre", informacion.Value.Value<string>("NOMBRE"), SqliteCommandComando);
                                    if (respuesta) respuesta = InsertarSesion("Apellidos", informacion.Value.Value<string>("APELLIDOS"), SqliteCommandComando);
                                }
                                else
                                    break;
                            }
                            if (respuesta) respuesta = InsertarPermisos(JObjectPermisosUsuario, SqliteCommandComando);
                        }
                    }
                    if (respuesta) respuesta = InsertarSesion("NombreEmpresa", NOMBRE_EMPRESA, SqliteCommandComando);
                    if (respuesta) respuesta = InsertarSesion("Contrasena", CONTRASENA, SqliteCommandComando);
                    if (respuesta) respuesta = InsertarSesion("SistemaReal", SISTEMA_REAL, SqliteCommandComando);
                    if (respuesta) respuesta = InsertarSesion("Distribucion", DISTRIBUCION, SqliteCommandComando);
                    if (respuesta) respuesta = InsertarSesion("TieneAcceso", "SI", SqliteCommandComando);
                    if (respuesta) respuesta = InsertarConfiguracion("Pin", "0000", SqliteCommandComando);
                    if (respuesta) respuesta = InsertarConfiguracion("PinBloqueo", "SI", SqliteCommandComando);
                    if (respuesta) respuesta = InsertarConfiguracion("VersionBDInstalada", VERSION_BD.ToString(), SqliteCommandComando);
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

        private bool InsertarSesion(string PARAMETRO, string VALOR, SqliteCommand SqliteCommandComando)
        {
            bool respuesta = false;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommandComando.CommandText = "INSERT INTO Sesion (Parametro, Valor) VALUES (@Parametro , @Valor)";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@Parametro", PARAMETRO));
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@Valor", VALOR));
                respuesta = SqliteCommandComando.ExecuteNonQuery() == 1;
            }
            return respuesta;
        }

        private bool InsertarPermisos(JObject JObjectPermisosUsuario, SqliteCommand SqliteCommandComando)
        {
            bool respuesta = true;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommandComando.CommandText = "INSERT INTO Permisos (IdPermiso, Valor) VALUES (@IdPermiso , @Valor)";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                foreach (var permiso in JObjectPermisosUsuario)
                {
                    if (respuesta)
                    {
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@IdPermiso", permiso.Value.Value<int>("ID_MODULO")));
                        SqliteCommandComando.Parameters.Add(new SqliteParameter("@Valor", permiso.Value.Value<int>("PERMISO")));
                        respuesta = SqliteCommandComando.ExecuteNonQuery() == 1;
                    }
                    else
                        break;
                }
            }
            return respuesta;
        }

        private bool InsertarConfiguracion(string PARAMETRO, string VALOR, SqliteCommand SqliteCommandComando)
        {
            bool respuesta = false;
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommandComando.CommandText = "INSERT INTO Configuracion (Parametro, Valor) VALUES (@Parametro , @Valor)";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@Parametro", PARAMETRO));
                SqliteCommandComando.Parameters.Add(new SqliteParameter("@Valor", VALOR));
                respuesta = SqliteCommandComando.ExecuteNonQuery() == 1;
            }
            return respuesta;
        }
    }
}
