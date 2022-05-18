using ElementosComunes.Clases;
using ElementosComunes.Conexiones;
using Newtonsoft.Json.Linq;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Directiva10.KPIs
{
    internal class TControlKPIs
    {
        private JObject JObjectParametros;

        public TControlKPIs()
        {
            JObjectParametros = TVariables.ObtenerParesComunes();
        }

        // Métodos públicos

        public async Task<List<TIndicador>> ObtenerDatosdeGrafica(CancellationToken CancellationTokenCancelar)
        {
            List<TIndicador> ListRespuesta = new List<TIndicador>();
            try
            {
                JObjectParametros["ID_INDICADOR"] = "0";
                TConexion.CalcularPuertoAleatorio();
                //JObject JObjectServidor = await TConexion.RealizarPeticionGetWebData("TSM_TablerosdeControl", "getKPIs_ADesarrollo", JObjectParametros, CancellationTokenCancelar);//Linea de consumo api
                //if (JObjectServidor.Value<string>("Respuesta") == "true")
                //    ListRespuesta = JObjectToGraficas(JObjectServidor.Value<JObject>("INDICADORES"));//Le mandas la respuesta al servidor
                //else
                //    Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeTitulo"], TMensaje.ObtenerMensajeDefault(JObjectServidor.Value<string>("Respuesta")), (string)Application.Current.Resources["StringMensajeBoton"]); });
                ListRespuesta = JObjectToGraficas(new JObject());//Le mandas la respuesta al servidor

            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], e.ToString(), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            return ListRespuesta;
        }

        // Métodos privados

        private List<TIndicador> JObjectToGraficas(JObject JObjectIndicadores)
        {
            List<TIndicador> ListRespuesta = new List<TIndicador>();
            TIndicador Indicador = new TIndicador();

            foreach (var indicador in JObjectIndicadores)
            {
                Indicador = new TIndicador();
                Indicador.Id = indicador.Value.Value<int>("ID_INDICADOR");
                if (Indicador.Id.Equals(12))
                {
                    break;
                }
                Indicador.Tipo = indicador.Value.Value<string>("TIPO");
                Indicador.Titulo = indicador.Value.Value<string>("NOMBRE_GRAFICA");
                Indicador.EsFavorita = indicador.Value.Value<string>("FAVORITA") == "1";
                Indicador.Medicion = indicador.Value.Value<string>("MEDICION");
                if (Indicador.Medicion == "PESOS") // Solicitar a la Ing.
                    Indicador.FormatodeMedicion = "$ ##,###.##";
                JObject JObjectEtiquetas = indicador.Value.Value<JObject>("ETIQUETAS");
                if (!string.IsNullOrWhiteSpace(Indicador.Tipo))
                {
                    Indicador.Tipo = Indicador.Tipo.ToUpper();
                    if (Indicador.Tipo.ToUpper() == "DOUGHNUT")
                    {
                        // La grafica DOUGHNUT o VERTICALBAR solo recibe una serie
                        List<Color> ListPaletadeColores = new List<Color>();
                        ListPaletadeColores.Add(Color.FromHex("e2a223"));
                        ListPaletadeColores.Add(Color.FromHex("193c54"));
                        ListPaletadeColores.Add(Color.FromHex("a59d9d"));
                        ListPaletadeColores.Add(Color.FromHex("f35a0f"));
                        ListPaletadeColores.Add(Color.FromHex("3d8fa8"));
                        ListPaletadeColores.Add(Color.FromHex("1f7a4e"));
                        ListPaletadeColores.Add(Color.FromHex("455b73"));
                        ListPaletadeColores.Add(Color.FromHex("9A58B6"));
                        ListPaletadeColores.Add(Color.FromHex("BFC4C7"));
                        ListPaletadeColores.Add(Color.FromHex("ED4331"));

                        Indicador.MostrarLeyenda = true;
                        Indicador.MostrarLeyendaSecundaria = true;
                        Indicador.FormatoPorcentaje = "##.#";
                        Indicador.LegendPlacementPosicionLeyenda = LegendPlacement.Right;

                        int indiceSerie = 0;
                        foreach (var serie in indicador.Value.Value<JObject>("SERIES"))
                        {
                            TSerie Serie = new TSerie();
                            Serie.Nombre = serie.Value.Value<string>("NOMBRE_SERIE");
                            Serie.ListColoresdePuntos = ListPaletadeColores;
                            JArray JArrayValoresdeSerie = serie.Value.Value<JArray>("VALORES_SERIE");

                            int indicePunto = 0;
                            foreach (double valor in JArrayValoresdeSerie)
                            {
                                TPunto Punto = new TPunto();
                                Punto.Nombre = JObjectEtiquetas.Value<string>(indicePunto.ToString());
                                Punto.Valor = valor;
                                Serie.ObservableCollectionPuntos.Add(Punto);
                                indicePunto = indicePunto + 1;
                            }
                            Indicador.ListSeries.Add(Serie);
                            indiceSerie = indiceSerie + 1;
                        }
                        foreach (var detalle in indicador.Value.Value<JObject>("DETALLES"))
                        {
                            TEtiqueta EtiquetaDetalle = new TEtiqueta();
                            EtiquetaDetalle.Titulo = detalle.Value.Value<string>("DESCRIPCION");
                            EtiquetaDetalle.Mensaje = detalle.Value.Value<string>("VALOR");
                            Indicador.ListDetalles.Add(EtiquetaDetalle);
                        }
                    }
                    ListRespuesta.Add(Indicador);
                }
            }




            //Grafica DOUGHNUT ejemplo de llenado Hard-code
            Indicador = new TIndicador();
            Indicador.Titulo = "Contratos del Periodo";
            Indicador.Tipo = "DOUGHNUT";
            Indicador.EsFavorita = false;
            Indicador.Medicion = "NUMERO";
            Indicador.FormatodeMedicion = "";
            Indicador.MostrarLeyendaSecundaria = true;
            Indicador.FormatoPorcentaje = "##";
            Indicador.MostrarLeyenda = true;
            Indicador.LegendPlacementPosicionLeyenda = LegendPlacement.Left;
            Indicador.ListSeries.Add(
                new TSerie
                {
                    Nombre = "TOTAL_CONTRATOS",
                    ObservableCollectionPuntos = new ObservableCollection<TPunto>()
                    {
                        new TPunto { Nombre = "SF - UI", Valor = 83 },
                        new TPunto { Nombre = "SF - UF", Valor = 85 },
                        new TPunto { Nombre = "COBERTURA", Valor = 19 },
                        new TPunto { Nombre = "NICHO", Valor = 33 },
                        new TPunto { Nombre = "CEMENTERIO", Valor = 44 },
                    },
                    ListColoresdePuntos = new List<Color>()
                    {
                        Color.FromHex("2C99DE"),
                        Color.FromHex("24ba9b"),
                        Color.FromHex("9A58B6"),
                        Color.FromHex("BFC4C7"),
                        Color.FromHex("ED4331"),
                    }
                }
            );
            ListRespuesta.Add(Indicador);
            //llenado de las graficas

            Indicador = new TIndicador();
            Indicador.Titulo = "Margen de Beneficio Bruto";
            Indicador.Tipo = "circularProgressBar";
            Indicador.Porcentaje = -12;
            ListRespuesta.Add(Indicador);

            Indicador = new TIndicador();
            Indicador.Titulo = "Margen de Beneficio Bruto";
            Indicador.Tipo = "circularProgressBar";
            Indicador.Porcentaje = 34;
            ListRespuesta.Add(Indicador);

            Indicador = new TIndicador();
            Indicador.Titulo = "Crecimiento de Ingresos";
            Indicador.Tipo = "crecimiento";
            Indicador.Porcentaje = -3.5;
            ListRespuesta.Add(Indicador);

            return ListRespuesta;
        }
    }
}
