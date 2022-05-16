using ElementosComunes.Conexiones;
using Mono.Data.Sqlite;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Directiva10.A_General
{
    class TMetodosComunes
    {
        public bool BorrarTodoslosRegistros(TConexionLocal ConexionLocal, string NOMBRE_TABLA, SqliteCommand SqliteCommandComando)
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

        public void AbrirUrl(string URL)
        {
            if (!string.IsNullOrWhiteSpace(URL))
            {
                if (URL.Contains("http"))
                    Device.OpenUri(new Uri(URL));
                else
                    Device.OpenUri(new Uri("http://" + URL));
            }
        }

        public void AbrirRedSocial(string URL)
        {
            if (!string.IsNullOrWhiteSpace(URL))
            {
                Device.OpenUri(new Uri(URL));
            }
        }

        public void LlamarporTelefono(string NUMERO_DE_TELEFONO)
        {
            if (!string.IsNullOrWhiteSpace(NUMERO_DE_TELEFONO))
                Device.OpenUri(new Uri("tel://" + QuitarCaracteresalTelefono(NUMERO_DE_TELEFONO)));
        }

        public void EnviarCorreo(string CORREO_ELECTRONICO)
        {
            if (!string.IsNullOrWhiteSpace(CORREO_ELECTRONICO))
                Device.OpenUri(new Uri("mailto:" + CORREO_ELECTRONICO.Replace(" ", "")));
        }

        public async Task AbrirUbicacionenAppNativa(string LATITUD, string LONGITUD)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                // https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
                float acercamiento = 15; // [2,..,21]
                await Launcher.OpenAsync("maps://maps.apple.com/?sll=" + LATITUD.ToString() + "," + LONGITUD.ToString() + "&z=" + acercamiento.ToString() + "&t=s&q=" + LATITUD.ToString() + "," + LONGITUD.ToString());
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                // https://developer.android.com/guide/components/intents-common#java
                int acercamiento = 17; // [1,..,23]
                await Launcher.OpenAsync("geo:0,0?z=" + acercamiento.ToString() + "&q=" + LATITUD.ToString() + ", " + LONGITUD.ToString());
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                await Launcher.OpenAsync("bingmaps:?where=394 Pacific Ave San Francisco CA");
            }
        }

        public async Task AbrirUbicacionenGoogleMaps(string LATITUD, string LONGITUD)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                float acercamiento = 15; // [2,..,21]
                var soportaURIGoogleMaps = await Launcher.CanOpenAsync("comgooglemaps://");
                if (soportaURIGoogleMaps)
                {
                    // https://xamgirl.com/quick-tip-launching-google-maps-in-xamarin-forms/
                    await Launcher.OpenAsync($"comgooglemaps://?q={LATITUD},{LONGITUD}&z={acercamiento}&t=s&q={LATITUD},{LONGITUD}");
                }
                else
                {
                    // https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
                    await Launcher.OpenAsync($"maps://maps.apple.com/?sll={LATITUD},{LONGITUD}&z={acercamiento}&t=s&q={LATITUD},{LONGITUD}");
                }
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                // https://developer.android.com/guide/components/intents-common#java
                int acercamiento = 17; // [1,..,23]
                await Launcher.OpenAsync($"geo:0,0?z={acercamiento}&q={LATITUD},{LONGITUD}");
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                await Launcher.OpenAsync("bingmaps:?where=394 Pacific Ave San Francisco CA");
            }
        }

        public string QuitarCaracteresalTelefono(string NUMERO_DE_TELEFONO)
        {
            string respuesta = "";
            if (!string.IsNullOrWhiteSpace(NUMERO_DE_TELEFONO))
            {
                string valor = NUMERO_DE_TELEFONO.Replace(" ", "");
                valor = valor.Replace("(", "");
                valor = valor.Replace(")", "");
                valor = valor.Replace("+", "");
                valor = valor.Replace("-", "");
                valor = valor.Replace("#", "");
                valor = valor.Replace("N", "");
                valor = valor.Replace(",", "");
                valor = valor.Replace("*", "");
                valor = valor.Replace("/", "");
                valor = valor.Replace(";", "");
                valor = valor.Replace(".", "");
                if (Int64.TryParse(valor, out long numeroTelefono))
                    respuesta = numeroTelefono.ToString();
            }
            return respuesta;
        }
    }
}
