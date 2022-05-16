using System;

namespace ElementosComunes.Clases
{
    public class TPeriodos
    {
        /// <summary>
        /// Incrementa numero de dias a la fecha ingresada dependiendo del TIPO_DE_PERIODO
        /// TIPO_DE_PERIODO: "SEMANAL" o "CATORCENAL" o "QUINCENAL" o "MENSUAL" o "BIMESTRAL" o "TRIMESTRAL" o "SEMESTRAL" o "ANUAL"
        /// </summary>
        /// <param name="DateTimeFecha"></param>
        /// <param name="TIPO_DE_PERIODO"></param>
        /// <param name="NUMERO_DE_PERIODOS"></param>
        /// <returns></returns>
        public static DateTime IncrementarPeriodos(DateTime DateTimeFecha, string TIPO_DE_PERIODO, int NUMERO_DE_PERIODOS)
        {
            DateTime Respuesta = DateTimeFecha;

            if (NUMERO_DE_PERIODOS != 0)
            {
                if (TIPO_DE_PERIODO == "SEMANAL")
                    Respuesta = Respuesta.AddDays(7 * NUMERO_DE_PERIODOS);
                else
                {
                    if (TIPO_DE_PERIODO == "CATORCENAL")
                        Respuesta = Respuesta.AddDays(14 * NUMERO_DE_PERIODOS);
                    else
                    {
                        if (TIPO_DE_PERIODO == "QUINCENAL")
                            Respuesta = IncrementarQuincenas(Respuesta, NUMERO_DE_PERIODOS);
                        else
                            if (TIPO_DE_PERIODO == "MENSUAL")
                                Respuesta = Respuesta.AddMonths(NUMERO_DE_PERIODOS);
                            else
                                if (TIPO_DE_PERIODO == "BIMESTRAL")
                                    Respuesta = Respuesta.AddMonths(2 * NUMERO_DE_PERIODOS);
                                else
                                    if (TIPO_DE_PERIODO == "TRIMESTRAL")
                                        Respuesta = Respuesta.AddMonths(3 * NUMERO_DE_PERIODOS);
                                    else
                                        if (TIPO_DE_PERIODO == "SEMESTRAL")
                                            Respuesta = Respuesta.AddMonths(6 * NUMERO_DE_PERIODOS);
                                        else
                                            if (TIPO_DE_PERIODO == "ANUAL")
                                                Respuesta = Respuesta.AddMonths(12 * NUMERO_DE_PERIODOS);
                    }
                }
            }

            return Respuesta;
        }

        /// <summary>
        /// No son los Retrasos, son únicamente los periodos, que a la fecha, ya se vencieron
        /// </summary>
        /// <param name="METODO_DE_CALCULO"></param>
        /// <param name="TIPO_DE_PERIODO"></param>
        /// <param name="FECHA_ORIGINAL_DE_PAGO"></param>
        /// <param name="FECHA_DE_CALCULO"></param>
        /// <returns></returns>
        public static int ObtenerPeriodosVencidos(string METODO_DE_CALCULO, string TIPO_DE_PERIODO, DateTime FechaOriginalPago, DateTime FechaCalculo)
        {
            int respuesta = 0;

            if (METODO_DE_CALCULO == "PERIODO COMPLETO")
            {
                DateTime fechaTemporal = FechaOriginalPago;
                while (fechaTemporal < FechaCalculo)
                {
                    respuesta = respuesta + 1;
                    fechaTemporal = IncrementarPeriodos(FechaOriginalPago, TIPO_DE_PERIODO, respuesta);
                }
            }
            else
            {
                if (METODO_DE_CALCULO == "DIARIO")
                {
                    TimeSpan diferencia = FechaCalculo - FechaOriginalPago;
                    respuesta = diferencia.Days;
                }
            }

            return respuesta;
        }

        /// <summary>
        /// Incrementa el numero de quincenas a la fecha ingresada
        /// </summary>
        /// <param name="DateTimeFecha"></param>
        /// <param name="NUMERO_DE_QUINCENAS"></param>
        /// <returns></returns>
        private static DateTime IncrementarQuincenas(DateTime DateTimeFecha, int NUMERO_DE_QUINCENAS)
        {
            DateTime Respuesta = DateTimeFecha;

            if (NUMERO_DE_QUINCENAS == 0)
                return Respuesta;

            int Meses = NUMERO_DE_QUINCENAS / 2;
            Respuesta = Respuesta.AddMonths(Meses);

            if ((Meses * 2) != NUMERO_DE_QUINCENAS)
            {
                if (NUMERO_DE_QUINCENAS > 0)
                {
                    if (Respuesta.Day > 15)
                    {
                        // Por caso especial de enero - febrero
                        if (Respuesta.Month == 1)
                            Respuesta = Respuesta.AddDays(16);
                        else
                            Respuesta = Respuesta.AddMonths(1).AddDays(-15);
                    }
                    else
                    {
                        // Por caso especial de febrero
                        if ((Respuesta.Month == 2) && (Respuesta.Day > 13))
                            if (Respuesta.Day == 14)
                                Respuesta = Respuesta.AddDays(14);
                            else
                                Respuesta = Respuesta.AddDays(13);
                        else
                            Respuesta = Respuesta.AddDays(15);
                    }
                }
                else
                {
                    if (Respuesta.Day > 15)
                    {
                        Respuesta = Respuesta.AddDays(-15);
                        while (Respuesta.Day > 15)
                            Respuesta = Respuesta.AddDays(-1);
                    }
                    else
                    {
                        Respuesta = Respuesta.AddMonths(-1);
                        if ((Respuesta.Month == 2) && (Respuesta.Day > 13))
                        {
                            Respuesta = Respuesta.AddDays(13);
                            while (Respuesta.Month == 2)
                                Respuesta = Respuesta.AddDays(1);

                            if (Respuesta.Month == 3)
                                Respuesta = Respuesta.AddDays(-1);
                        }
                        else
                            Respuesta = Respuesta.AddDays(15);
                    }
                }
            }

            return Respuesta;
        }
    }
}
