using Directiva10.A_Compartir;
using Directiva10.Plantilla;
using ElementosComunes.Conexiones;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace Directiva10.KPIs
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CPKPIs : ContentPage
	{
		private object ObjectBloqueo;
		private bool BotonOcupado, MostrarAnimacion;
		private double Ancho, Alto, DuraciondeAnimacion, AltoIconoLeyendas;
		private Color ColorEncabezado, ColorMalla, ColorDescripcion, ColorLeyendaSecundaria;

		TViewModelKPIs ViewModelKPIs;
		public CPKPIs()
		{
			ObjectBloqueo = new object();
			BotonOcupado = false;
			MostrarAnimacion = true;
			DuraciondeAnimacion = 0.8;
			AltoIconoLeyendas = 15;
			ColorEncabezado = Color.FromHex("142b48");
			ColorMalla = Color.FromHex("d3dae1");
			ColorDescripcion = Color.FromHex("f6f6f6");
			ColorLeyendaSecundaria = Color.FromHex("999999");
			ViewModelKPIs = new TViewModelKPIs();
			InitializeComponent();
			BindingContext = ViewModelKPIs;
			IniciarVista();
		}

		protected async override void OnSizeAllocated(double width, double height)
		{
			await Task.Delay(300);
			base.OnSizeAllocated(width, height);
			if (width > 0 && height > 0)
			{
				if (Ancho != width && Alto != height)
				{
					Ancho = width;
					Alto = height;
					if (Ancho > Alto)
						ViewModelKPIs.MostrarVistaVertical(false, Alto);
					else
						ViewModelKPIs.MostrarVistaVertical(true, Alto);
				}
			}
		}

		public async void IniciarVista()
		{
			await InicializarTodaslasGraficas();
		}

		public async Task InicializarTodaslasGraficas()//En este metodo se consume la api y se manda los parametros a mostrar en la grafica
		{
			if (ViewModelKPIs != null)
			{
				if (TConexion.TieneConectividadaInternet())
				{
					Navigation.PushModalAsync(new PPLoading(ViewModelKPIs.ObtenerNuevoTokendeCancelacion()));
					TControlKPIs ControlKPIs = new TControlKPIs();
					List<TIndicador> ListaGraficas = await ControlKPIs.ObtenerDatosdeGrafica(ViewModelKPIs.CancellationTokenCancelar);
					XamlStackLayoutContenedordeGraficas.Children.Clear();
					foreach (TIndicador Indicador in ListaGraficas)
					{
						switch (Indicador.Tipo)
						{
							case "DOUGHNUT":
								DibujarGraficaDona(CrearObjetoEncabezadodeGrafica(Indicador.Id, Indicador.Titulo), CrearObjetoPiedeGrafica(Indicador.ListDetalles), Indicador.Medicion, Indicador.MostrarLeyenda, Indicador.LegendPlacementPosicionLeyenda, Indicador.MostrarLeyendaSecundaria, Indicador.FormatoPorcentaje, Indicador.ListSeries);
								break;

							case "circularProgressBar":
								DibujarGraficaProgressBar(CrearObjetoEncabezadodeGrafica(Indicador.Id, Indicador.Titulo), Indicador.Porcentaje);
								break;

							case "crecimiento":
								DibujarGraficaCrecimiento(CrearObjetoEncabezadodeGrafica(Indicador.Id, Indicador.Titulo), Indicador.Porcentaje, "Marzo 2022", 1025600.45m);
								break;
						}
					}
					if (Navigation.ModalStack.Count > 0)
						await Navigation.PopModalAsync();
				}
				else
				{
					await Navigation.PushModalAsync(new CPSinConexionaInternet(true));
				}
			}
		}

		private void DibujarGraficaDona(Grid GridEncabezado, StackLayout StackLayoutPie, string MEDICION, bool MOSTRAR_LEYENDAS, LegendPlacement LegendPlacementPosicionLeyenda, bool MOSTRAR_LEYENDA_SECUNDARIA, string FORMATO_PORCENTAJE, List<TSerie> ListSeries)
		{
			// La grafica DOUGHNUT solo recibe una serie
			LabelContent LabelContentMostrarPorcentaje = string.IsNullOrWhiteSpace(FORMATO_PORCENTAJE) ? LabelContent.YValue : LabelContent.Percentage;
			ChartSeriesCollection ChartSeriesCollectionSeries = new ChartSeriesCollection();
			foreach (TSerie Serie in ListSeries)
			{
				DoughnutSeries DoughnutSeriesSerie = new DoughnutSeries
				{
					EnableAnimation = true,
					AnimationDuration = DuraciondeAnimacion,
					Spacing = 0,
					DoughnutCoefficient = 0.6,
					Label = Serie.Nombre,
					ItemsSource = Serie.ObservableCollectionPuntos,
					XBindingPath = "Nombre",
					YBindingPath = "Valor",
					EnableTooltip = true,
					ColorModel = new ChartColorModel
					{
						Palette = ChartColorPalette.Custom,
						CustomBrushes = Serie.ListColoresdePuntos
					},
					DataMarker = new ChartDataMarker()
					{
						LabelContent = LabelContentMostrarPorcentaje,
						LabelStyle = new DataMarkerLabelStyle
						{
							BackgroundColor = Color.Transparent,
							LabelFormat = FORMATO_PORCENTAJE,
							TextColor = Color.White
						},
					},
					TooltipTemplate = new DataTemplate(() =>
					{
						Label LabelPuntoNombre = new Label()
						{
							TextColor = Color.White
						};
						Label LabelPuntoValor = new Label()
						{
							TextColor = Color.White
						};
						LabelPuntoNombre.SetBinding(Label.TextProperty, "Nombre", BindingMode.OneWay);
						if (MEDICION == "PESOS")
							LabelPuntoValor.SetBinding(Label.TextProperty, "Valor", BindingMode.OneWay, converter: new TDoubleToFormatoMoneda());
						else
							LabelPuntoValor.SetBinding(Label.TextProperty, "Valor", BindingMode.OneWay);
						StackLayout StackLayoutTooltip = new StackLayout
						{
							Spacing = 0,
							Children =
							{
								LabelPuntoNombre,
								LabelPuntoValor
							}
						};
						return new ViewCell { View = StackLayoutTooltip };
					})
				};
				ChartSeriesCollectionSeries.Add(DoughnutSeriesSerie);
			}

			SfChart SfChartGrafica = new SfChart
			{
				Legend = new ChartLegend
				{
					IsVisible = MOSTRAR_LEYENDAS,
					CornerRadius = new ChartCornerRadius(5),
					BackgroundColor = Color.White,
					Margin = new Thickness(5),
					DockPosition = LegendPlacementPosicionLeyenda,
					ItemMargin = new Thickness(0, 0, 60, 0),
					Orientation = ChartOrientation.Vertical,
					LabelStyle = new ChartLegendLabelStyle
					{
						FontSize = 14,
						FontFamily = "Monserrat-Regular",
						TextColor = Color.FromHex("828282"),
					},
					ItemTemplate = new DataTemplate(() =>
					{
						Ellipse EllipseColor = new Ellipse
						{
							BackgroundColor = Color.White,
							StrokeThickness = 4,
							HeightRequest = AltoIconoLeyendas,
							WidthRequest = AltoIconoLeyendas
						};
						Label LabelPuntoNombre = new Label
						{
							FontSize = 10,
							LineBreakMode = LineBreakMode.TailTruncation
						};
						Label LabelPuntoValor = new Label
						{
							TextColor = ColorLeyendaSecundaria,
							FontSize = 10,
						};
						//  DataPoint y IconColor es palabra reservada
						EllipseColor.SetBinding(Ellipse.StrokeProperty, "IconColor", BindingMode.OneWay);
						LabelPuntoNombre.SetBinding(Label.TextProperty, "DataPoint.Nombre", BindingMode.OneWay);
						if (MEDICION == "PESOS")
							LabelPuntoValor.SetBinding(Label.TextProperty, "DataPoint.Valor", BindingMode.OneWay, converter: new TDoubleToFormatoMoneda());
						else
							LabelPuntoValor.SetBinding(Label.TextProperty, "DataPoint.Valor", BindingMode.OneWay);
						Grid GridEtiqueta = new Grid
						{
							ColumnDefinitions = new ColumnDefinitionCollection
							{
								new ColumnDefinition
								{
									Width = AltoIconoLeyendas,
								},
								new ColumnDefinition
								{
									Width = new GridLength(5, GridUnitType.Auto),
								}
							},
							RowDefinitions = new RowDefinitionCollection
							{
								new RowDefinition
								{
									Height = GridLength.Auto
								},
								new RowDefinition
								{
									Height = GridLength.Auto
								}
							},
							RowSpacing = 0
						};
						GridEtiqueta.Children.Add(EllipseColor, 0, 0);
						GridEtiqueta.Children.Add(LabelPuntoNombre, 1, 0);
						if (MOSTRAR_LEYENDA_SECUNDARIA)
							GridEtiqueta.Children.Add(LabelPuntoValor, 1, 1);
						StackLayout StackLayoutDataTemplate = new StackLayout
						{
							Padding = new Thickness(0, 3, 0, 3),
							Children =
							{
								GridEtiqueta
							}
						};
						return StackLayoutDataTemplate;
					})
				},
				Series = ChartSeriesCollectionSeries
			};
			SfChartGrafica.SetBinding<TViewModelKPIs>(SfChart.HeightRequestProperty, ViewModel => ViewModel.AltodeGraficaSecundaria, mode: BindingMode.OneWay);
			StackLayout StackLayoutGrafica = new StackLayout
			{
				BackgroundColor = Color.White,
				Padding = new Thickness(15, 15, 15, 5),
				Children =
				{
					new StackLayout
					{
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Children =
						{
							SfChartGrafica
						}
					},
				}
			};
			StackLayoutGrafica.SetBinding<TViewModelKPIs>(StackLayout.OrientationProperty, ViewModel => ViewModel.StackOrientationGrafica, mode: BindingMode.OneWay);
			StackLayout StackLayoutTarjetaGrafica = new StackLayout
			{
				Spacing = 0,
				Children = {
					new StackLayout
					{
						Children =
						{
							GridEncabezado
						}
					},
					StackLayoutGrafica,
					StackLayoutPie
				}
			};
			XamlStackLayoutContenedordeGraficas.Children.Add(StackLayoutTarjetaGrafica);
		}

		private void DibujarGraficaProgressBar(Grid GridEncabezado, double porcentaje)
		{
			ChartSeriesCollection ChartSeriesCollectionSeries = new ChartSeriesCollection();
			ChartSeriesCollection PieChartSeriesCollectionSeries = new ChartSeriesCollection();

			DoughnutSeries DoughnutSeriesSerie = new DoughnutSeries
			{
				ItemsSource = new ObservableCollection<TPunto>()
				{
					//Lineas ocupadas
					new TPunto { Nombre = "", Valor = 0 },
					new TPunto { Nombre = "", Valor = 0 },
					new TPunto { Nombre = "", Valor = 0 },
					new TPunto { Nombre = "", Valor = porcentaje < 0 ? porcentaje * -1 : porcentaje },
				},
				XBindingPath = "Nombre",
				YBindingPath = "Valor",
				IsStackedDoughnut = true,
				CapStyle = DoughnutCapStyle.BothCurve,
				Spacing = 0.8,
				MaximumValue = 100,

				EnableAnimation = true,
				AnimationDuration = DuraciondeAnimacion,
				ColorModel = new ChartColorModel
				{
					Palette = ChartColorPalette.Custom,
					CustomBrushes = new List<Color>()
					{
						porcentaje < 0 ? Color.Red : Color.Green,
					}
				},
			};
			ChartSeriesCollectionSeries.Add(DoughnutSeriesSerie);

			PieSeries PieSeriesSerie = new PieSeries
			{
				ItemsSource = new ObservableCollection<TPunto>()
				{
					new TPunto { Nombre = "", Valor = 100 },
				},
				XBindingPath = "Nombre",
				YBindingPath = "Valor",
				EnableAnimation = true,
				AnimationDuration = DuraciondeAnimacion,
				ColorModel = new ChartColorModel
				{
					Palette = ChartColorPalette.Custom,
					CustomBrushes = new List<Color>()
					{
						Color.Orange
					}
				},
			};
			PieChartSeriesCollectionSeries.Add(PieSeriesSerie);
			SfChart PieChart = new SfChart
			{
				BackgroundColor = Color.Transparent,
				Series = PieChartSeriesCollectionSeries,
				Margin = new Thickness(20, 20, 20, 20),
			};
			PieChart.SetBinding<TViewModelKPIs>(SfChart.HeightRequestProperty, ViewModel => ViewModel.AltodeGraficaSecundaria, mode: BindingMode.TwoWay);

			SfChart SfChartGrafica = new SfChart
			{
				BackgroundColor = Color.Transparent,
				Series = ChartSeriesCollectionSeries,
				Margin = new Thickness(-30, -30, -30, -30),
				Rotation = porcentaje < 0 ? ClculateRotation(porcentaje) : 90, //Rotacion del color
			};
			SfChartGrafica.SetBinding<TViewModelKPIs>(SfChart.HeightRequestProperty, ViewModel => ViewModel.AltodeGraficaPrincipal, mode: BindingMode.TwoWay);

			Label labelTitle = new Label
			{
				Text = $"Mayo 2022",
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = ColorEncabezado,
				BackgroundColor = Color.White,
				FontSize = 32,
				FontAttributes = FontAttributes.Bold,
			};

			Label labelCenter = new Label();
			labelCenter.Text = $"{porcentaje}%";
			labelCenter.FontSize = 80;
			labelCenter.FontAttributes = FontAttributes.Bold;
			labelCenter.TextColor = Color.White;
			labelCenter.HorizontalOptions = LayoutOptions.Center;
			labelCenter.VerticalOptions = LayoutOptions.Center;

			Image imageButtom = new Image
			{
				Source = porcentaje < 0 ? "desc_red.png" : "asc_green.png",
				WidthRequest = 80,
				HeightRequest = 80,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Margin = new Thickness(0, (int)ViewModelKPIs?.AltodeGraficaPrincipal, 0, 0),
			};

			Grid gridGrafica = new Grid();
			gridGrafica.BackgroundColor = Color.WhiteSmoke;
			gridGrafica.RowDefinitions.Add(new RowDefinition());
			gridGrafica.ColumnDefinitions.Add(new ColumnDefinition());
			gridGrafica.Children.Add(SfChartGrafica, 0, 0);
			gridGrafica.Children.Add(PieChart, 0, 0);
			gridGrafica.Children.Add(labelCenter, 0, 0);
			gridGrafica.Children.Add(imageButtom, 0, 0);

			StackLayout StackLayoutTarjetaGrafica = new StackLayout
			{
				Spacing = 0,
				Children = {
					new StackLayout
					{
						Children =
						{
							GridEncabezado
						}
					},
					labelTitle,
					gridGrafica,
				}
			};
			XamlStackLayoutContenedordeGraficas.Children.Add(StackLayoutTarjetaGrafica);
		}

		private void DibujarGraficaCrecimiento(Grid GridEncabezado, double porcentaje, string month, decimal total)
		{
			Label labelTitle = new Label
			{
				Text = month,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.Gray,
				BackgroundColor = Color.White,
				FontSize = 32,
				FontAttributes = FontAttributes.Bold,
			};

			Label labelTotal = new Label()
			{
				Text = $"Total: {total.ToString("$ #,#.00")}",
				FontSize = 24,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.Gray,
				BackgroundColor = Color.White,
			};

			Image imageDescription = new Image
			{
				Source = porcentaje < 0 ? "desc_red.png" : "asc_green.png",
				WidthRequest = (int)ViewModelKPIs?.AltodeGraficaPrincipal,
				HeightRequest = (int)ViewModelKPIs?.AltodeGraficaPrincipal,
			};

			Label labelPorcentaje = new Label()
			{
				Text = $"{porcentaje}%",
				FontSize = 80,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.Black,
				BackgroundColor = Color.White,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			StackLayout StackLayoutTarjetaGrafica = new StackLayout
			{
				Spacing = 0,
				BackgroundColor = Color.White,
				Children = {
					new StackLayout
					{
						Children =
						{
							GridEncabezado
						}
					},
					labelTitle,
					labelTotal,
					imageDescription,
					labelPorcentaje,
				}
			};
			XamlStackLayoutContenedordeGraficas.Children.Add(StackLayoutTarjetaGrafica);
		}

		private Grid CrearObjetoEncabezadodeGrafica(int ID_INDICADOR, string TITULO)
		{
			Grid GridRespuesta = new Grid
			{
				ColumnDefinitions =
				{
					new ColumnDefinition
					{
						Width = 60
					},
					new ColumnDefinition
					{
						Width = new GridLength(1, GridUnitType.Star)
					},
					new ColumnDefinition
					{
						Width = 60
					}
				}
			};
			GridRespuesta.Children.Add(new BoxView
			{
				CornerRadius = new CornerRadius(15, 15, 0, 0),
				BackgroundColor = ColorEncabezado
			}, 0, 3, 0, 1);
			GridRespuesta.Children.Add(new StackLayout
			{
				Padding = new Thickness(0, 20, 0, 20),
				Children =
				{
					new Label
					{
						Text = TITULO,
						TextColor = Color.White,
						FontSize = 16,
						FontAttributes = FontAttributes.Bold,
						HorizontalTextAlignment = TextAlignment.Center,
					}
				}
			}, 1, 0);
			StackLayout StackLayoutIconoInformativo = new StackLayout
			{
				Children =
				{
					new Image
					{
						Source = "menuInferiorCajas.png",
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.Center,
					}
				}
			};
			GridRespuesta.Children.Add(new StackLayout
			{
				Padding = new Thickness(0, 20, 0, 20),
				Children =
				{
					StackLayoutIconoInformativo
				}
			}, 2, 0);
			TapGestureRecognizer TapGestureRecognizerMostrarDescripcion = new TapGestureRecognizer();
			TapGestureRecognizerMostrarDescripcion.Tapped += TapGestureRecognizerMostrarDescripcionTapped;
			TapGestureRecognizerMostrarDescripcion.CommandParameter = ID_INDICADOR;
			GridRespuesta.GestureRecognizers.Add(TapGestureRecognizerMostrarDescripcion);
			return GridRespuesta;
		}

		private async void TapGestureRecognizerMostrarDescripcionTapped(object sender, EventArgs e)
		{
			lock (ObjectBloqueo)
			{
				if (BotonOcupado)
					return;
				BotonOcupado = true;
			}
			try
			{
				TappedEventArgs TappedEventArgsEvento = (TappedEventArgs)e;
				if (TappedEventArgsEvento != null)
				{
					int idIndicador = (int)TappedEventArgsEvento.Parameter;
					if (idIndicador > 0)
					{
					}
				}
			}
			finally
			{
				lock (ObjectBloqueo)
				{
					BotonOcupado = false;
				}
			}
		}

		private StackLayout CrearObjetoPiedeGrafica(List<TEtiqueta> ListDetalles)
		{
			StackLayout StackLayoutRespuesta = new StackLayout()
			{
				Spacing = 0
			};
			if (ListDetalles.Count > 0)
			{
				Grid GridDetalles = new Grid()
				{
					Padding = new Thickness(0, 15, 0, 0),
					BackgroundColor = ColorDescripcion,
					ColumnDefinitions = new ColumnDefinitionCollection
					{
						new ColumnDefinition
						{
							Width = new GridLength(1, GridUnitType.Star)
						},
						new ColumnDefinition
						{
							Width = new GridLength(1, GridUnitType.Star)
						}
					},
					ColumnSpacing = 10,
					RowSpacing = 0
				};

				int renglon = 0;
				foreach (TEtiqueta Etiqueta in ListDetalles)
				{
					GridDetalles.RowDefinitions.Add(new RowDefinition
					{
						Height = GridLength.Auto
					});
					GridDetalles.Children.Add(new Label
					{
						Text = Etiqueta.Titulo,
						HorizontalTextAlignment = TextAlignment.End
					}, 0, renglon);
					GridDetalles.Children.Add(new Label
					{
						Text = Etiqueta.Mensaje,
						FontAttributes = FontAttributes.Bold,
						HorizontalTextAlignment = TextAlignment.Start
					}, 1, renglon);
					renglon = renglon + 1;
				}
				StackLayoutRespuesta.Children.Add(GridDetalles);
				StackLayoutRespuesta.Children.Add(new BoxView
				{
					CornerRadius = new CornerRadius(0, 0, 15, 15),
					BackgroundColor = ColorDescripcion,
					HeightRequest = AltoIconoLeyendas
				});
			}
			else
			{
				StackLayoutRespuesta.Children.Add(new BoxView
				{
					CornerRadius = new CornerRadius(0, 0, 15, 15),
					BackgroundColor = Color.White,
					HeightRequest = AltoIconoLeyendas
				});

			}
			return StackLayoutRespuesta;
		}

		private int ClculateRotation(double porcentaje)
		{
			var porcentPositive = porcentaje < 0 ? porcentaje * -1 : porcentaje;
			if (porcentPositive > 25)
			{
				return (int)((porcentPositive * 3.6) - 90) * -1;
			}
			else
			{
				return (int)((25 - porcentPositive) * 3.6);
			}
		}
	}

	public class TViewModelKPIs : INotifyPropertyChanged
	{
		private string EsVistaVertical;
		private CancellationTokenSource CancellationTokenSourcePeticion;
		private string Fuente;

		public CancellationToken CancellationTokenCancelar;

		public StackOrientation StackOrientationGrafica { get; set; }
		public double AltodeGraficaPrincipal { get; set; }
		public double AltodeGraficaSecundaria { get; set; }
		public bool MostrarEtiquetasMontoVentaCanceladasPromedio { get; set; }
		public bool MostrarContadoresenCuadriculaVertical { get; set; }

		public TViewModelKPIs()
		{
			EsVistaVertical = "";
			CancellationTokenSourcePeticion = new CancellationTokenSource();
			CancellationTokenCancelar = CancellationTokenSourcePeticion.Token;
			StackOrientationGrafica = StackOrientation.Vertical;
			AltodeGraficaPrincipal = 300;
			AltodeGraficaSecundaria = 150;
		}

		public CancellationTokenSource ObtenerNuevoTokendeCancelacion()
		{
			CancellationTokenSourcePeticion = new CancellationTokenSource();
			CancellationTokenCancelar = CancellationTokenSourcePeticion.Token;
			return CancellationTokenSourcePeticion;
		}

		public void MostrarVistaVertical(bool ES_VERTICAL, double ALTO_DE_PANTALLA)
		{
			if (EsVistaVertical != ES_VERTICAL.ToString())
			{
				if (ES_VERTICAL)
				{
					//El primer número 60 es el menu superior en iOs en adroid es 50  y el segundo número es el menú inferior, se calculo al azar
					if (Device.Idiom == TargetIdiom.Tablet)
					{
						StackOrientationGrafica = StackOrientation.Vertical;
						MostrarContadoresenCuadriculaVertical = true;
						AltodeGraficaPrincipal = Device.RuntimePlatform == Device.iOS ? Math.Round(ALTO_DE_PANTALLA - 60 - 50) : Math.Round(ALTO_DE_PANTALLA - 50 - 50);
						AltodeGraficaSecundaria = Device.RuntimePlatform == Device.iOS ? Math.Round((ALTO_DE_PANTALLA / 2) - 60 - 50) : Math.Round((ALTO_DE_PANTALLA / 2) - 50 - 50);
					}
					else
					{
						StackOrientationGrafica = StackOrientation.Vertical;
						MostrarContadoresenCuadriculaVertical = true;
						AltodeGraficaPrincipal = Device.RuntimePlatform == Device.iOS ? Math.Round(ALTO_DE_PANTALLA - 60 - 50) : Math.Round(ALTO_DE_PANTALLA - 50 - 50);
						AltodeGraficaSecundaria = Device.RuntimePlatform == Device.iOS ? Math.Round(((ALTO_DE_PANTALLA / 3) * 2) - 60 - 50) : Math.Round(((ALTO_DE_PANTALLA / 3) * 2) - 50 - 50);
					}
					MostrarEtiquetasMontoVentaCanceladasPromedio = false;
					EsVistaVertical = "true";
				}
				else
				{
					//El primer número 60 es el menu superior en iOs en adroid es 50  y el segundo número es el menú inferior, se calculo al azar
					if (Device.Idiom == TargetIdiom.Tablet)
					{
						StackOrientationGrafica = StackOrientation.Horizontal;
						MostrarContadoresenCuadriculaVertical = false;
						AltodeGraficaPrincipal = Device.RuntimePlatform == Device.iOS ? Math.Round(ALTO_DE_PANTALLA - 60 - 50) : Math.Round(ALTO_DE_PANTALLA - 50 - 50);
						AltodeGraficaSecundaria = Device.RuntimePlatform == Device.iOS ? Math.Round(ALTO_DE_PANTALLA - 60 - 50) : Math.Round(ALTO_DE_PANTALLA - 50 - 50);
					}
					else
					{
						StackOrientationGrafica = StackOrientation.Vertical;
						MostrarContadoresenCuadriculaVertical = true;
						AltodeGraficaPrincipal = Device.RuntimePlatform == Device.iOS ? Math.Round(ALTO_DE_PANTALLA - 60 - 50) : Math.Round(ALTO_DE_PANTALLA - 50 - 50);
						AltodeGraficaSecundaria = Device.RuntimePlatform == Device.iOS ? Math.Round(ALTO_DE_PANTALLA - 60 - 50) : Math.Round(ALTO_DE_PANTALLA - 50 - 50);
					}
					MostrarEtiquetasMontoVentaCanceladasPromedio = true;
					EsVistaVertical = "false";
				}
			}
			OnPropertyChanged(nameof(StackOrientationGrafica));
			OnPropertyChanged(nameof(MostrarContadoresenCuadriculaVertical));
			OnPropertyChanged(nameof(AltodeGraficaPrincipal));
			OnPropertyChanged(nameof(AltodeGraficaSecundaria));
			OnPropertyChanged(nameof(MostrarEtiquetasMontoVentaCanceladasPromedio));
		}

		#region INotifyPropertyChanged Implementation
		public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}