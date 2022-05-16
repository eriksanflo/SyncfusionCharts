using ElementosComunes.Conexiones;
using Mono.Data.Sqlite;
using System;

namespace Directiva10.A_General
{
    class TModeloBasedeDatos
    {
        private TConexionLocal ConexionLocal;

        public TModeloBasedeDatos()
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

        public bool CrearBasedeDatos()
        {
            bool respuesta = false;
            try
            {
                if (ConexionLocal.ConexionAbierta)
                {
                    ConexionLocal.IniciarTransaccion();

                    SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;

                    /* Tabla Sesion */
                    SqliteCommandComando.CommandText = @"
                    CREATE TABLE Sesion(
                        Parametro VARCHAR(60) NOT NULL,
                        Valor VARCHAR(150) NOT NULL,
                        CONSTRAINT PkSesion PRIMARY KEY(Parametro)
                    )";
                    SqliteCommandComando.ExecuteNonQuery();

                    SqliteCommandComando.CommandText = "INSERT INTO Sesion (Parametro, Valor) VALUES ('Usuario' , 'USUARIO')";
                    SqliteCommandComando.ExecuteNonQuery();
                    SqliteCommandComando.CommandText = "INSERT INTO Sesion (Parametro, Valor) VALUES ('NombreEmpresa' , 'Innovacion Tecnologica Grupmyl')";
                    SqliteCommandComando.ExecuteNonQuery();
                    SqliteCommandComando.CommandText = "INSERT INTO Sesion (Parametro, Valor) VALUES ('Contrasena' , '1')";
                    SqliteCommandComando.ExecuteNonQuery();
                    SqliteCommandComando.CommandText = "INSERT INTO Sesion (Parametro, Valor) VALUES ('SistemaReal' , 'Procap 3.5')";
                    SqliteCommandComando.ExecuteNonQuery();
                    SqliteCommandComando.CommandText = "INSERT INTO Sesion (Parametro, Valor) VALUES ('Distribucion' , 'CDIGITAL PROCAP')";
                    SqliteCommandComando.ExecuteNonQuery();
                    SqliteCommandComando.CommandText = "INSERT INTO Sesion (Parametro, Valor) VALUES ('TieneAcceso' , 'SI')";
                    SqliteCommandComando.ExecuteNonQuery();                                       

                    /* Tabla Permisos */
                    SqliteCommandComando.CommandText = @"
                    CREATE TABLE Permisos(
			            IdPermiso INTEGER NOT NULL,
                        Valor INTEGER NOT NULL,
			            PRIMARY KEY (IdPermiso)
                    )";
                    SqliteCommandComando.ExecuteNonQuery();

                    /* Tabla Sucursales */
                    SqliteCommandComando.CommandText = @"
                    CREATE TABLE Sucursales(
			            IdSucursal INTEGER NOT NULL,
                        Clave VARCHAR(50) NOT NULL,
			            PRIMARY KEY (IdSucursal)
                    )";
                    SqliteCommandComando.ExecuteNonQuery();

                    /* Tabla Balance */
                    SqliteCommandComando.CommandText = @"
                    CREATE TABLE Balance(
                        FechaUltimaSincronizacion VARCHAR(50)NOT NULL,
                        HoraUltimaSincronizacion VARCHAR(50)NOT NULL,
                        IngresoAcumuladodeHoy DOUBLE PRECISION NOT NULL,
                        IngresoAcumuladoSemanal DOUBLE PRECISION NOT NULL,
                        IngresoAcumuladoMensual DOUBLE PRECISION NOT NULL,
                        EgresoAcumuladodeHoy DOUBLE PRECISION NOT NULL,
                        EgresoAcumuladoSemanal DOUBLE PRECISION NOT NULL,
                        EgresoAcumuladoMensual DOUBLE PRECISION NOT NULL
                    )";
                    SqliteCommandComando.ExecuteNonQuery();

                    /* Tabla Balance de Usuarios */
                    SqliteCommandComando.CommandText = @"
                    CREATE TABLE BalancedeUsuarios(
                        UsuariodeRegistro VARCHAR(50) NOT NULL,
			            IdSucursal INTEGER NOT NULL,
                        TotaldeIngresos DOUBLE PRECISION NOT NULL,
                        TotaldeRecibos DOUBLE PRECISION NOT NULL,
                        TotaldeRecibosEfectivo DOUBLE PRECISION NOT NULL,
                        TotaldeEgresos DOUBLE PRECISION NOT NULL,
                        FOREIGN KEY(IdSucursal) REFERENCES Sucursales (IdSucursal) ON DELETE NO ACTION ON UPDATE CASCADE
                    )";
                    SqliteCommandComando.ExecuteNonQuery();

                    /* Tabla de Prueba */
                    SqliteCommandComando.CommandText = @"
                    CREATE TABLE Pruebas(
                        IdPersona INTEGER NOT NULL,
                        Nombre VARCHAR(100) NOT NULL
                    )";
                    SqliteCommandComando.ExecuteNonQuery();

                    /* Tabla Configuración App */
                    SqliteCommandComando.CommandText = @"
                    CREATE TABLE Configuracion (
                        Parametro VARCHAR(60) NOT NULL, 
                        Valor VARCHAR(100) NOT NULL, 
                        CONSTRAINT PkSesion PRIMARY KEY (Parametro)
                    )";
                    SqliteCommandComando.ExecuteNonQuery();

                    SqliteCommandComando.CommandText = "INSERT INTO Configuracion (Parametro, Valor) VALUES ('Pin' , '0000')";
                    SqliteCommandComando.ExecuteNonQuery();
                    SqliteCommandComando.CommandText = "INSERT INTO Configuracion (Parametro, Valor) VALUES ('PinBloqueo' , 'SI')";
                    SqliteCommandComando.ExecuteNonQuery();
                    SqliteCommandComando.CommandText = "INSERT INTO Configuracion (Parametro, Valor) VALUES ('VersionBDInstalada' , '1')";
                    SqliteCommandComando.ExecuteNonQuery();

                    ConexionLocal.TerminarTransaccion();
                    respuesta = true;
                }
            }
            catch (Exception e)
            {
                ConexionLocal.CancelarTransaccion();
                throw e;
            }
            return respuesta;
        }

        public string ObtenerVersionBDInstalada()
        {
            string respuesta = "";
            if (ConexionLocal.ConexionAbierta)
            {
                SqliteCommand SqliteCommandComando = ConexionLocal.SqliteCommandComando;
                SqliteCommandComando.CommandText = "SELECT Valor FROM Configuracion WHERE Parametro = 'VersionBDInstalada'";
                SqliteCommandComando.CommandType = System.Data.CommandType.Text;
                respuesta = SqliteCommandComando.ExecuteScalar().ToString();
            }
            return respuesta;
        }

        public bool EjecutarScriptBDVersion2()
        {
            bool respuesta = true;
            try
            {
                if (ConexionLocal.ConexionAbierta)
                {
                    //respuesta = MetodosComunes.ActualizarVersionBDInstalada(ConexionLocal, 2, SqliteCommandComando);
                }
                if (respuesta)
                    ConexionLocal.TerminarTransaccion();
                else
                    ConexionLocal.CancelarTransaccion();
            }
            catch (Exception e)
            {
                ConexionLocal.CancelarTransaccion();
                throw e;
            }
            return respuesta;
        }
    }
}
