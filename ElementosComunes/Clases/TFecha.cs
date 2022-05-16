
using System;

namespace ElementosComunes.Clases
{
    public class TFecha
    {
        /// <summary>
        /// Regresa el nombre del mes de la fecha ingresada
        /// </summary>
        /// <param name="DateTimeFecha"></param>
        /// <returns></returns>
        public static string getMesString(DateTime DateTimeFecha)
        {
            string[] ArrayMeses = new string[]
            {
                "",
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            };

            return ArrayMeses[DateTimeFecha.Month];
        }
    }
}
