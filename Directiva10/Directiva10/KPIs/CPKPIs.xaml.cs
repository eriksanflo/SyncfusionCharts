using Directiva10.A_Compartir;
using Directiva10.Plantilla;
using ElementosComunes.Conexiones;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
								DibujarGraficaProgressBar(CrearObjetoEncabezadodeGrafica(Indicador.Id, Indicador.Titulo), Indicador.ListSeries);
								break;

							case "crecimiento":
								DibujarGraficaCrecimiento(CrearObjetoEncabezadodeGrafica(Indicador.Id, Indicador.Titulo), Indicador.ListSeries, Indicador.Filtros, Indicador.FiltrosAplicados, "Marzo 2022", 1025600.45m);
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

		private void DibujarGraficaProgressBar(Grid GridEncabezado, List<TSerie> listSeries)
		{
			ChartSeriesCollection ChartSeriesCollectionSeries = new ChartSeriesCollection();
			ChartSeriesCollection PieChartSeriesCollectionSeries = new ChartSeriesCollection();

			var porcentaje = listSeries.FirstOrDefault().ObservableCollectionPuntos.ToList().FirstOrDefault().Valor;

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
				Source = porcentaje < 0 ? "AD_Icono_KPIS02.png" : "AD_Icono_KPIS01.png",
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

		private void DibujarGraficaCrecimiento(Grid GridEncabezado, List<TSerie> listSeries, List<TFiltro> listFilters, List<TFiltroAplicado> filtrosAplicados, string month, decimal total)
		{
			var porcentaje = listSeries.FirstOrDefault().ObservableCollectionPuntos.ToList().FirstOrDefault().Valor;

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
				Source = porcentaje < 0 ? "AD_Icono_KPIS05.png" : "AD_Icono_KPIS04.png",
				WidthRequest = (int)ViewModelKPIs?.AltodeGraficaPrincipal,
				HeightRequest = (int)ViewModelKPIs?.AltodeGraficaPrincipal,
			};

			Label labelPorcentaje = new Label()
			{
				Text = $"{porcentaje}%",
				FontSize = 60,
				FontAttributes = FontAttributes.Bold,
				TextColor = Color.Black,
				BackgroundColor = Color.White,
				HorizontalTextAlignment = TextAlignment.Center,
			};

			Grid gridFilters = CreateGridFilter(listFilters, filtrosAplicados);

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
					gridFilters,
					labelTitle,
					labelTotal,
					imageDescription,
					labelPorcentaje,
				}
			};
			XamlStackLayoutContenedordeGraficas.Children.Add(StackLayoutTarjetaGrafica);
		}

        #region Chart Crecimiento
		private Grid CreateGridFilter(List<TFiltro> listFilters, List<TFiltroAplicado> filtrosAplicados)
        {
			var _idGridSpecifics = Guid.NewGuid();
			Grid gridFilters = new Grid()
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition(),
					new RowDefinition(),
					new RowDefinition(),
					new RowDefinition(),
					new RowDefinition(),
				},
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					new ColumnDefinition(),
				}
			};
            #region Periodo
            Grid periodo = new Grid()
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition(){ Height = GridLength.Auto },
				},
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					new ColumnDefinition(){ Width = GridLength.Auto },
					new ColumnDefinition(),
				}
			};
			Label label = new Label()
			{
				Text = "PERIODO:",
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.End,
				FontSize = 20,
			};
			Picker values = new Picker()
			{
				ItemsSource = listFilters.FirstOrDefault(x => x.TipoFiltro == "PERIODO").ValoresFiltro.Select(x => x.FiltroValor).ToList(),
			};
			var _periodoSleccionado = filtrosAplicados.Where(x => x.TipoFiltro == "PERIODO").ToList();
			if (_periodoSleccionado.Any())
            {
				values.SelectedItem = _periodoSleccionado.FirstOrDefault().Valor;
				if (_periodoSleccionado.FirstOrDefault().Valor != "anual")
                {
					var _selectedSpecific1 = filtrosAplicados.Where(x => x.TipoFiltro == "ESPECIFICO_INICIAL").ToList();
					var _selectedSpecific2 = filtrosAplicados.Where(x => x.TipoFiltro == "ESPECIFICO_FINAL").ToList();
					string _specificIdSelected1 = string.Empty, _specificIdSelected2 = string.Empty;
					if (_selectedSpecific1.Any())
                    {
						_specificIdSelected1 = _selectedSpecific1.FirstOrDefault().Valor;
					}
					if (_selectedSpecific2.Any())
					{
						_specificIdSelected2 = _selectedSpecific2.FirstOrDefault().Valor;
					}
					Specifics(_periodoSleccionado.FirstOrDefault().Valor.ToLower(), _specificIdSelected1, _specificIdSelected2);
				}
            }
			values.SelectedIndexChanged += (sender, args) =>
			{
				var picker = (Picker)sender;
				int selectedIndex = picker.SelectedIndex;

				if (selectedIndex != -1)
				{
					var selected = picker.Items[selectedIndex].ToLower();
					if(selected != "anual")
                    {
						Specifics(selected, string.Empty, string.Empty);
					}
                    else
                    {
						RemoveSpecific();
					}
				}
			};
            #region Specifics
			void Specifics(string typeFilter, string _selectedValue1, string _selectedValue2)
            {
				var specifics = listFilters.FirstOrDefault(x => x.TipoFiltro == "ESPECIFICOS").ValoresFiltro;
				RemoveSpecific();
				switch (typeFilter)
				{
					case "mensual":
						var mensuales = specifics.FirstOrDefault(x => x.Nombre == "PERIODOS_MENSUAL").Valores;
						CreateSpecifics(mensuales, _selectedValue1, _selectedValue2);
						break;

					case "trimestral":
						var trimestrales = specifics.FirstOrDefault(x => x.Nombre == "PERIODOS_TRIMESTRAL").Valores;
						CreateSpecifics(trimestrales, _selectedValue1, _selectedValue2);
						break;

					case "semestral":
						var semestrales = specifics.FirstOrDefault(x => x.Nombre == "PERIODOS_SEMESTRE").Valores;
						CreateSpecifics(semestrales, _selectedValue1, _selectedValue2);
						break;
				}
			}
            void CreateSpecifics(List<TFiltroValor> Valores, string _selectedValue1, string _selectedValue2)
            {
				Grid gridSpecifics = new Grid()
				{
					RowDefinitions = new RowDefinitionCollection
					{
						new RowDefinition(),
					},
					ColumnDefinitions = new ColumnDefinitionCollection
					{
						new ColumnDefinition(){ Width = GridLength.Auto },
						new ColumnDefinition(),
						new ColumnDefinition(){ Width = GridLength.Auto },
						new ColumnDefinition(),
					} 					
				};
				_idGridSpecifics = gridSpecifics.Id;
				Label Specifics1 = new Label()
				{
					Text = "ESPECÍFICO:",
					FontAttributes = FontAttributes.Bold,
					VerticalTextAlignment = TextAlignment.Start,
					FontSize = 20,
				};
				Label Specifics2 = new Label()
				{
					Text = "ESPECÍFICO:",
					FontAttributes = FontAttributes.Bold,
					VerticalTextAlignment = TextAlignment.Start,
					FontSize = 20,
				};
				Picker valuesSpecifics1 = new Picker()
				{
					ItemsSource = Valores.Select(x => x.FiltroValor).ToList(),
				};
				if (!string.IsNullOrEmpty(_selectedValue1))
                {
					var matched1 = Valores.Where(x => x.IdFiltroValor.ToString() == _selectedValue1).ToList();
					if (matched1.Any())
                    {
						valuesSpecifics1.SelectedItem = matched1.FirstOrDefault().FiltroValor;
					}
				}
				Picker valuesSpecifics2 = new Picker()
				{
					ItemsSource = Valores.Select(x => x.FiltroValor).ToList(),
				};
				if (!string.IsNullOrEmpty(_selectedValue2))
				{
					var matched2 = Valores.Where(x => x.IdFiltroValor.ToString() == _selectedValue2).ToList();
					if (matched2.Any())
					{
						valuesSpecifics2.SelectedItem = matched2.FirstOrDefault().FiltroValor;
					}
				}
				gridSpecifics.Children.Add(Specifics1, 0, 0);
				gridSpecifics.Children.Add(valuesSpecifics1, 1, 0);
				gridSpecifics.Children.Add(Specifics2, 2, 0);
				gridSpecifics.Children.Add(valuesSpecifics2, 3, 0);
				gridFilters.Children.Add(gridSpecifics, 0, 3);
			}
			void RemoveSpecific()
            {
				var index = 0;
                foreach (var _child in gridFilters.Children.ToList())
                {
					if (_child.Id == _idGridSpecifics)
                    {
						gridFilters.Children.Remove(_child);
					}
					index++;
                }
			}
            #endregion
			periodo.Children.Add(label, 0, 0);
			periodo.Children.Add(values, 1, 0);
			gridFilters.Children.Add(periodo, 0, 0);
			#endregion
			#region Rango Title
			Grid rangos = new Grid()
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition(),
				},
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					new ColumnDefinition(),
					new ColumnDefinition(),
				}
			};
			Label rango1 = new Label()
			{
				Text = "RANGO 1",
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 20,
			};
			Label rango2 = new Label()
			{
				Text = "RANGO 2",
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 20,
			};
			rangos.Children.Add(rango1, 0, 0);
			rangos.Children.Add(rango2, 1, 0);
			gridFilters.Children.Add(rangos, 0, 1);
			#endregion
			#region Years
			Grid years = new Grid()
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition(),
				},
				ColumnDefinitions = new ColumnDefinitionCollection
				{
					new ColumnDefinition(){ Width = GridLength.Auto },
					new ColumnDefinition(),
					new ColumnDefinition(){ Width = GridLength.Auto },
					new ColumnDefinition(),
				}
			};
			Label years1 = new Label()
			{
				Text = "AÑO:",
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Start,
				FontSize = 20,
			};
			Label years2 = new Label()
			{
				Text = "AÑO:",
				FontAttributes = FontAttributes.Bold,
				VerticalTextAlignment = TextAlignment.Start,
				FontSize = 20,
			};
			Picker valuesyears1 = new Picker()
			{
				ItemsSource = listFilters.FirstOrDefault(x => x.TipoFiltro == "AÑOS").ValoresFiltro.Select(x => x.FiltroValor).ToList(),
			};
			Picker valuesyears2 = new Picker()
			{
				ItemsSource = listFilters.FirstOrDefault(x => x.TipoFiltro == "AÑOS").ValoresFiltro.Select(x => x.FiltroValor).ToList(),
				//ItemsSource = new List<string>() { DateTime.Now.ToString("yyyy") },
			};
			var _yearOneSelected = filtrosAplicados.Where(x => x.TipoFiltro == "AÑO_INICIAL").ToList();
			var _yearTwoSelected = filtrosAplicados.Where(x => x.TipoFiltro == "AÑO_FINAL").ToList();
			if (_yearOneSelected.Any())
			{
				valuesyears1.SelectedItem = _yearOneSelected.FirstOrDefault().Valor;
			}
			if (_yearTwoSelected.Any())
			{
				valuesyears2.SelectedItem = _yearTwoSelected.FirstOrDefault().Valor;
			}
			years.Children.Add(years1, 0, 0);
			years.Children.Add(valuesyears1, 1, 0);
			years.Children.Add(years2, 2, 0);
			years.Children.Add(valuesyears2, 3, 0);
			gridFilters.Children.Add(years, 0, 2);
			#endregion
			#region Button Refresh
			Button btnRefresh = new Button()
			{
				Text = "Update Data",
				BackgroundColor = Color.Black,
				TextColor = Color.White,
			};
			gridFilters.Children.Add(btnRefresh, 0, 4);
			#endregion
			return gridFilters;
		}
		#endregion

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