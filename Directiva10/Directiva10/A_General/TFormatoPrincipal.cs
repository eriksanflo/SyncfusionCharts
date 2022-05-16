using ElementosComunes.Clases;
using System;
using System.Text;

namespace Directiva10.A_General
{
    class TFormatoPrincipal : TFormato
    {
        public static string FormatoHoraparaServidor(string HORA)
        {
            return DateTime.Parse(HORA).ToString(@"HH\:mm\:ss");
        }

        public static string FormatoFechaparaServidor(DateTime FECHA)
        {
            return FECHA.ToString(@"dd/MM/yyyy");
        }

        public static string Decodificar(string CADENA_CODIFICADA)
        {
            string respuesta = "";
            try
            {
                if (!string.IsNullOrEmpty(CADENA_CODIFICADA))
                {
                    byte[] ArreglodeBytes = Convert.FromBase64String(CADENA_CODIFICADA);
                    respuesta = Encoding.UTF8.GetString(ArreglodeBytes);
                }
            }
            catch (Exception e)
            {
                respuesta = "";
            }
            return respuesta;
        }
    }
}
