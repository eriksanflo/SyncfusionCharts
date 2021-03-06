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
                JObject JObjectServidor = await TConexion.RealizarPeticionGetWebData("TSM_TablerosdeControl", "getKPIs_ADesarrollo", JObjectParametros, CancellationTokenCancelar);//Linea de consumo api
                if (JObjectServidor.Value<string>("Respuesta") == "true")
                    ListRespuesta = JObjectToGraficas(JObjectServidor.Value<JObject>("INDICADORES"));//Le mandas la respuesta al servidor
                else
                    Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeTitulo"], TMensaje.ObtenerMensajeDefault(JObjectServidor.Value<string>("Respuesta")), (string)Application.Current.Resources["StringMensajeBoton"]); });
                //ListRespuesta = JObjectToGraficas(new JObject());//Le mandas la respuesta al servidor

            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringMensajeErrorTitulo"], e.ToString(), (string)Application.Current.Resources["StringMensajeBoton"]); });
            }
            return ListRespuesta;
        }

        private List<TIndicador> JObjectToGraficas(JObject JObjectIndicadores)
        {
            List<TIndicador> ListRespuesta = new List<TIndicador>();
            TIndicador Indicador = new TIndicador();

            foreach (var indicador in JObjectIndicadores)
            {
                Indicador = new TIndicador();
                Indicador.Id = indicador.Value.Value<int>("ID_INDICADOR");
                Indicador.Tipo = indicador.Value.Value<string>("TIPO").ToUpper();
                Indicador.Titulo = indicador.Value.Value<string>("NOMBRE_GRAFICA");
                Indicador.EsFavorita = indicador.Value.Value<string>("FAVORITA") == "1";
                Indicador.Medicion = indicador.Value.Value<string>("MEDICION");
                if (Indicador.Medicion == "PESOS")
                    Indicador.FormatodeMedicion = "$ ##,###.##";
                JObject JObjectEtiquetas = indicador.Value.Value<JObject>("ETIQUETAS");
                if (!string.IsNullOrWhiteSpace(Indicador.Tipo))
                {
                    if (Indicador.Tipo == "DOUGHNUT" || Indicador.Tipo == "CIRCULARPROGRESSBAR" || Indicador.Tipo == "CRECIMIENTO")
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
                    #region Add Filters selected
                    JObject JObjectFiltrosAplicados = indicador.Value.Value<JObject>("FILTROS_APLICADOS");
                    if (JObjectFiltrosAplicados != null)
                    {
                        Indicador.FiltrosAplicados = new List<TFiltroAplicado>();
                        TFiltroAplicado FiltroAplicado = null;
                        foreach (var filtroAplicado in JObjectFiltrosAplicados)
                        {
                            FiltroAplicado = new TFiltroAplicado();
                            FiltroAplicado.TipoFiltro = filtroAplicado.Value.Value<string>("TIPO_DE_FILTRO");
                            FiltroAplicado.Valor = filtroAplicado.Value.Value<string>("VALOR");
                            Indicador.FiltrosAplicados.Add(FiltroAplicado);
                        }
                    }
                    #endregion

                    #region Add Filters
                    JObject JObjectListaFiltros = indicador.Value.Value<JObject>("FILTROS");
                    if (JObjectListaFiltros != null)
                    {
                        TFiltro Filtro = null;
                        Indicador.Filtros = new List<TFiltro>();
                        foreach (var filtro in JObjectListaFiltros)
                        {
                            Filtro = new TFiltro();
                            Filtro.ValoresFiltro = new List<TFiltroValor>();
                            Filtro.TipoFiltro = filtro.Value.Value<string>("TIPO");
                            foreach (var valor in filtro.Value.Value<JObject>("VALORES"))
                            {
                                TFiltroValor FiltroValor = new TFiltroValor();
                                FiltroValor.FiltroValor = valor.Value.Value<string>("VALOR");
                                FiltroValor.Nombre = valor.Value.Value<string>("NOMBRE");
                                JObject JObjectvaloresNivel2Arreglo = valor.Value.Value<JObject>("VALORES");
                                if (JObjectvaloresNivel2Arreglo != null)
                                {
                                    FiltroValor.Valores = new List<TFiltroValor>();
                                    var indexNivel2 = 1;
                                    foreach (var nivel2 in JObjectvaloresNivel2Arreglo)
                                    {
                                        FiltroValor.Valores.Add(new TFiltroValor
                                        {
                                            IdFiltroValor = indexNivel2++,
                                            FiltroValor = nivel2.Value.Value<string>("VALOR"),
                                        });
                                    }
                                }
                                Filtro.ValoresFiltro.Add(FiltroValor);
                            }
                            Indicador.Filtros.Add(Filtro);
                        }
                    }
                    #endregion
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
            Indicador.Tipo = "CIRCULARPROGRESSBAR";
            Indicador.ListSeries = new List<TSerie>
            {
                new TSerie
                {
                    Nombre = "PORCENTAJE_DE_AVANCE",
                    ObservableCollectionPuntos = new ObservableCollection<TPunto>
                    {
                        new TPunto{ Nombre = "PORCENTAJE_DE_AVANCE", Valor = -12}
                    },
                }
            };
            ListRespuesta.Add(Indicador);

            Indicador = new TIndicador();
            Indicador.Titulo = "Margen de Beneficio Bruto";
            Indicador.Tipo = "CIRCULARPROGRESSBAR";
            Indicador.ListSeries = new List<TSerie>
            {
                new TSerie
                {
                    Nombre = "PORCENTAJE_DE_AVANCE",
                    ObservableCollectionPuntos = new ObservableCollection<TPunto>
                    {
                        new TPunto{ Nombre = "PORCENTAJE_DE_AVANCE", Valor = 34 }
                    },
                }
            };
            ListRespuesta.Add(Indicador);

            Indicador = new TIndicador();
            Indicador.Id = 13;
            Indicador.Titulo = "Crecimiento de Ingresos";
            Indicador.Tipo = "crecimiento";
            Indicador.ListSeries = new List<TSerie>
            {
                new TSerie
                {
                    Nombre = "PORCENTAJE_DE_AVANCE",
                    ObservableCollectionPuntos = new ObservableCollection<TPunto>
                    {
                        new TPunto{ Nombre = "PORCENTAJE_DE_AVANCE", Valor = -93.325678245115 }  
                    },
                }
            };
            Indicador.Filtros = new List<TFiltro>()
            {
                new TFiltro
                {
                    TipoFiltro = "PERIODO",
                    ValoresFiltro = new List<TFiltroValor>
                    {
                        new TFiltroValor { IdFiltroValor = 1, FiltroValor = "Mensual" },
                        new TFiltroValor { IdFiltroValor = 2, FiltroValor = "Trimestral" },
                        new TFiltroValor { IdFiltroValor = 3, FiltroValor = "Semestral" },
                        new TFiltroValor { IdFiltroValor = 4, FiltroValor = "Anual" },
                    }
                },
                new TFiltro
                {
                    TipoFiltro = "AÑOS",
                    ValoresFiltro = GetAniosFromRange(1990, 2022),
                },
                new TFiltro
                {
                    TipoFiltro = "ESPECIFICOS",
                    ValoresFiltro = new List<TFiltroValor>
                    {
                        new TFiltroValor
                        {
                            Nombre = "PERIODOS_MENSUAL",
                            Valores = GetMonthsOfYear(),
                        },
                        new TFiltroValor
                        {
                            Nombre = "PERIODOS_TRIMESTRAL",
                            Valores = GetTrimesterOfYear(),
                        },
                        new TFiltroValor
                        {
                            Nombre = "PERIODOS_SEMESTRE",
                            Valores = GetSemesterOfYear(),
                        }
                    }
                }
            };
            Indicador.FiltrosAplicados = new List<TFiltroAplicado>()
            {
                //new TFiltroAplicado()
                //{
                //    TipoFiltro = "PERIODO",
                //    Valor = "Mensual",
                //},
                //new TFiltroAplicado()
                //{
                //    TipoFiltro = "AÑO_INICIAL",
                //    Valor = "2021",
                //},
                //new TFiltroAplicado()
                //{
                //    TipoFiltro = "ESPECIFICO_INICIAL",
                //    Valor = "5",
                //},
                //new TFiltroAplicado()
                //{
                //    TipoFiltro = "AÑO_FINAL",
                //    Valor = "2021",
                //},
                //new TFiltroAplicado()
                //{
                //    TipoFiltro = "ESPECIFICO_FINAL",
                //    Valor = "7",
                //},
            };
            ListRespuesta.Add(Indicador);
            
            return ListRespuesta;
        }

        private List<TFiltroValor> GetAniosFromRange(int start, int end)
        {
            var response = new List<TFiltroValor>();
            for (int i = start; i <= end; i++)
            {
                response.Add(new TFiltroValor { IdFiltroValor = i, FiltroValor = i.ToString() });
            }
            return response;
        }
        private List<TFiltroValor> GetMonthsOfYear()
        {
            var response = new List<TFiltroValor>();
            response.Add(new TFiltroValor { IdFiltroValor = 1, FiltroValor = "Enero1" });
            response.Add(new TFiltroValor { IdFiltroValor = 2, FiltroValor = "Febrero" });
            response.Add(new TFiltroValor { IdFiltroValor = 3, FiltroValor = "Marzo" });
            response.Add(new TFiltroValor { IdFiltroValor = 4, FiltroValor = "Abril" });
            response.Add(new TFiltroValor { IdFiltroValor = 5, FiltroValor = "Mayo1" });
            response.Add(new TFiltroValor { IdFiltroValor = 6, FiltroValor = "Junio" });
            response.Add(new TFiltroValor { IdFiltroValor = 7, FiltroValor = "Julio" });
            response.Add(new TFiltroValor { IdFiltroValor = 8, FiltroValor = "Agosto" });
            response.Add(new TFiltroValor { IdFiltroValor = 9, FiltroValor = "Septiembre" });
            response.Add(new TFiltroValor { IdFiltroValor = 10, FiltroValor = "Octubre" });
            response.Add(new TFiltroValor { IdFiltroValor = 11, FiltroValor = "Noviembre" });
            response.Add(new TFiltroValor { IdFiltroValor = 12, FiltroValor = "Diciembre" });
            return response;
        }
        private List<TFiltroValor> GetTrimesterOfYear()
        {
            var response = new List<TFiltroValor>();
            response.Add(new TFiltroValor { IdFiltroValor = 1, FiltroValor = "1er Trimestre" });
            response.Add(new TFiltroValor { IdFiltroValor = 2, FiltroValor = "2do Trimestre" });
            response.Add(new TFiltroValor { IdFiltroValor = 3, FiltroValor = "3er Trimestre" });
            response.Add(new TFiltroValor { IdFiltroValor = 4, FiltroValor = "4to Trimestre" });
            return response;
        }
        private List<TFiltroValor> GetSemesterOfYear()
        {
            var response = new List<TFiltroValor>();
            response.Add(new TFiltroValor { IdFiltroValor = 1, FiltroValor = "1er Semestre" });
            response.Add(new TFiltroValor { IdFiltroValor = 2, FiltroValor = "2do Semestre" });
            return response;
        }
    }
}
