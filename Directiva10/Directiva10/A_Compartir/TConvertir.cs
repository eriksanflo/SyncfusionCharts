using System;
using System.Globalization;
using Xamarin.Forms;

namespace Directiva10.A_Compartir
{
    /// <summary>
    /// Versión 2.0
    /// Modificación 04/08/2020
    /// </summary>

    public class TInvertirValorBool : IValueConverter
    {
        public object Convert(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            bool respuesta = false;
            if (VALOR != null && bool.TryParse(VALOR.ToString(), out bool valorBool))
                respuesta = !valorBool;
            return respuesta;
        }

        public object ConvertBack(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            bool respuesta = false;
            if (VALOR != null && bool.TryParse(VALOR.ToString(), out bool valorBool))
                respuesta = !valorBool;
            return respuesta;
        }

        public object ProvideValue(IServiceProvider ServiceProviderServicio)
        {
            return this;
        }
    }

    public class TDoubleToFormatoMoneda : IValueConverter
    {
        public object Convert(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && double.TryParse(VALOR.ToString(), out double valorDouble))
                respuesta = A_General.TFormatoPrincipal.FormatearAMonedadelSistema(valorDouble);
            return respuesta;
        }

        public object ConvertBack(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && double.TryParse(VALOR.ToString(), out double valorDouble))
                respuesta = A_General.TFormatoPrincipal.FormatearAMonedadelSistema(valorDouble);
            return respuesta;
        }

        public object ProvideValue(IServiceProvider ServiceProviderServicio)
        {
            return this;
        }
    }

    public class TDoubleToFormatoDouble : IValueConverter
    {
        public object Convert(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && double.TryParse(VALOR.ToString(), out double valorDouble))
                respuesta = A_General.TFormatoPrincipal.FormatearADouble(valorDouble);
            return respuesta;
        }

        public object ConvertBack(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && double.TryParse(VALOR.ToString(), out double valorDouble))
                respuesta = A_General.TFormatoPrincipal.FormatearADouble(valorDouble);
            return respuesta;
        }

        public object ProvideValue(IServiceProvider ServiceProviderServicio)
        {
            return this;
        }
    }

    public class TDateTimeToFormatoFecha : IValueConverter
    {
        public object Convert(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && DateTime.TryParse(VALOR.ToString(), out DateTime valorDateTime))
            {
                if (!valorDateTime.Equals(new DateTime()))
                    respuesta = A_General.TFormatoPrincipal.FormatearAFecha(valorDateTime);
            }
            return respuesta;
        }

        public object ConvertBack(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && DateTime.TryParse(VALOR.ToString(), out DateTime valorDateTime))
            {
                if (!valorDateTime.Equals(new DateTime()))
                    respuesta = A_General.TFormatoPrincipal.FormatearAFecha(valorDateTime);
            }
            return respuesta;
        }

        public object ProvideValue(IServiceProvider ServiceProviderServicio)
        {
            return this;
        }
    }

    public class TDateTimeToFormatoFechaLarga : IValueConverter
    {
        public object Convert(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && DateTime.TryParse(VALOR.ToString(), out DateTime valorDateTime))
            {
                if (!valorDateTime.Equals(new DateTime()))
                    respuesta = A_General.TFormatoPrincipal.FormatearAFechaLarga(valorDateTime);
            }
            return respuesta;
        }

        public object ConvertBack(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && DateTime.TryParse(VALOR.ToString(), out DateTime valorDateTime))
            {
                if (!valorDateTime.Equals(new DateTime()))
                    respuesta = A_General.TFormatoPrincipal.FormatearAFechaLarga(valorDateTime);
            }
            return respuesta;
        }

        public object ProvideValue(IServiceProvider ServiceProviderServicio)
        {
            return this;
        }
    }

    public class TDateTimeToFormatoDiaMesLargo : IValueConverter
    {
        public object Convert(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && DateTime.TryParse(VALOR.ToString(), out DateTime valorDateTime))
            {
                if (!valorDateTime.Equals(new DateTime()))
                    respuesta = valorDateTime.ToString("MMMM dd").ToUpper();
            }
            return respuesta;
        }

        public object ConvertBack(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && DateTime.TryParse(VALOR.ToString(), out DateTime valorDateTime))
            {
                if (!valorDateTime.Equals(new DateTime()))
                    respuesta = valorDateTime.ToString("MMMM dd").ToUpper();
            }
            return respuesta;
        }

        public object ProvideValue(IServiceProvider ServiceProviderServicio)
        {
            return this;
        }
    }

    public class TTimeSpanToFormatoHora : IValueConverter
    {
        public object Convert(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && TimeSpan.TryParse(VALOR.ToString(), out TimeSpan valorTimeSpan))
            {
                if (!valorTimeSpan.Equals(new TimeSpan()))
                    respuesta = A_General.TFormatoPrincipal.FormatoHora(valorTimeSpan);
            }
            return respuesta;
        }

        public object ConvertBack(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            string respuesta = "";
            if (VALOR != null && TimeSpan.TryParse(VALOR.ToString(), out TimeSpan valorTimeSpan))
            {
                if (!valorTimeSpan.Equals(new DateTime()))
                    respuesta = A_General.TFormatoPrincipal.FormatoHora(valorTimeSpan);
            }
            return respuesta;
        }

        public object ProvideValue(IServiceProvider ServiceProviderServicio)
        {
            return this;
        }
    }

    public class TStringTieneValor : IValueConverter
    {
        public object Convert(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            bool respuesta = false;
            if (VALOR != null && !string.IsNullOrWhiteSpace(VALOR.ToString()))
                respuesta = true;
            return respuesta;
        }

        public object ConvertBack(object VALOR, Type TypeTipodeObjeto, object PARAMETRO, CultureInfo CultureInfoInformacionGeneral)
        {
            bool respuesta = false;
            if (VALOR != null && !string.IsNullOrWhiteSpace(VALOR.ToString()))
                respuesta = true;
            return respuesta;
        }

        public object ProvideValue(IServiceProvider ServiceProviderServicio)
        {
            return this;
        }
    }
}
