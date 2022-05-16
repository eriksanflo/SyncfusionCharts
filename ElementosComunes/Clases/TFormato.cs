using System;
using System.Globalization;

namespace ElementosComunes.Clases
{
    public class TFormato
    {
        /// <summary>
        /// Función que regresa un string con el formato $#,###.##
        /// </summary>
        /// <param name="CANTIDAD"></param>
        /// <returns></returns>
        public static string FormatearAMoneda(double CANTIDAD)
        {
            return Convert.ToDecimal(CANTIDAD).ToString("C");
        }

        /// <summary>
        /// Función que regresa un string con el formato de moneda del sistema del móvil
        /// </summary>
        /// <param name="CANTIDAD"></param>
        /// <returns></returns>
        public static string FormatearAMonedadelSistema(double CANTIDAD)
        {
            return Convert.ToDecimal(CANTIDAD).ToString("C", CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Función que regresa un string con el formato ####.## de punto fijo
        /// </summary>
        /// <param name="CANTIDAD"></param>
        /// <returns></returns>
        public static string FormatearADouble(double CANTIDAD)
        {
            return CANTIDAD.ToString("F");
        }

        /// <summary>
        /// Función que regresa un string de fecha con el formato ##/##/####
        /// </summary>
        /// <param name="FECHA"></param>
        /// <returns></returns>
        public static string FormatearAFecha(DateTime FECHA)
        {
            //return FECHA.ToString("dd/MM/yyyy");
            return FECHA.ToString("dd'/'MM'/'yyyy");
        }

        /// <summary>
        /// Función que regresa un string de hora con el formato en 12 hrs
        /// </summary>
        /// <param name="HORA"></param>
        /// <returns></returns>
        public static string FormatoHora(TimeSpan HORA)
        {
            return DateTime.Today.Add(HORA).ToString("hh:mm tt");
        }

        /// <summary>
        /// Función que regresa un string de tiempo transcurrido
        /// </summary>
        /// <param name="TIEMPO"></param>
        /// <returns></returns>
        public static string FormatoTiempoTranscurrido(TimeSpan TIEMPO)
        {
            return DateTime.Today.Add(TIEMPO).ToString("mm:ss");
        }

        /// <summary>
        /// Función que regresa un string de fecha con el formato "DIA ## de MES de ####"
        /// </summary>
        /// <param name="FECHA"></param>
        /// <returns></returns>
        public static string FormatearAFechaLarga(DateTime FECHA)
        {
            return Capitalizar(FECHA.ToString("dddd dd 'de' ")) + Capitalizar(FECHA.ToString("MMMM 'de' yyyy"));
        }

        /// <summary>
        /// Función que regresa un string de fecha sin año con el formato "DIA ## de MES"
        /// </summary>
        /// <param name="FECHA"></param>
        /// <returns></returns>
        public static string FormatoFechaSinAño(DateTime FECHA)
        {
            return Capitalizar(FECHA.ToString("dddd dd 'de' ")) + Capitalizar(FECHA.ToString("MMMM"));
        }

        /// <summary>
        /// Función que retorna un string con la primera letra capitalizada
        /// </summary>
        /// <param name="CADENA"></param>
        /// <returns></returns>
        public static string Capitalizar(string CADENA)
        {
            string respuesta = "";

            respuesta = CADENA.Substring(0, 1).ToUpper() + CADENA.Substring(1, CADENA.Length - 1).ToLower();

            return respuesta;
        }

        /// <summary>
        /// Función que retorna un string con un espacio entre las letras
        /// </summary>
        /// <param name="TEXTO"></param>
        /// <returns></returns>
        public static string SepararTexto(string TEXTO)
        {
            char[] texto = TEXTO.ToCharArray();
            string respuesta = "";
            foreach (char letra in texto)
            {
                respuesta += letra;
                respuesta += " ";
            }

            return respuesta;
        }

        /// <summary>
        /// Función que retorna un string quitando espacios entre texto o letras
        /// </summary>
        /// <param name="TEXTO"></param>
        /// <returns></returns>
        public static string JuntarTexto(string TEXTO)
        {
            char[] texto = TEXTO.ToCharArray();
            string respuesta = "";
            foreach (char letra in texto)
            {   if(letra != ' ')
                respuesta += letra;
            }

            return respuesta;
        }

        /// <summary>
        /// Convierte un string que tiene la fecha dd/MM/yyy a un DateTime
        /// </summary>
        /// <param name="FECHA">dd/MM/yyyy</param>
        /// <returns></returns>
        public static DateTime StringDDMMYYYYToDateTime(string FECHA)
        {
            DateTime respuesta = new DateTime();
            if (!string.IsNullOrEmpty(FECHA))
            {
                string[] Temporal = FECHA.Split('/');
                if (Temporal.Length == 3)
                {
                    respuesta = new DateTime(int.Parse(Temporal[2]), int.Parse(Temporal[1]), int.Parse(Temporal[0]));
                }
            }
            return respuesta;
        }

        /// <summary>
        /// Convierte un numero double que esta en string a un valor string pero en formato del idioma Español - México
        /// </summary>
        /// <param name="CANTIDAD"></param>
        /// <returns></returns>
        public static string FormatearANumeroDoubleServidor(string CANTIDAD)
        {
            string respuesta = "";
            if (!string.IsNullOrEmpty(CANTIDAD))
            {
                double valor = double.Parse(CANTIDAD);
                respuesta = string.Format(new CultureInfo("es-MX"), "{0:g}", valor);
            }
            return respuesta;
        }
    }
}
