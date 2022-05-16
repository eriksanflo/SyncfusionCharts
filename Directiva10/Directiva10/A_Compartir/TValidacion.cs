
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Directiva10.A_Compartir
{
    /// <summary>
    /// Versión 2.0
    /// Modificación 08/03/2022
    /// </summary>
    class TValidacion
    {
        public static void ResaltarCamposRequeridos(params object[] ARREGLO_OBJETOS_XAML)
        {
            bool respuesta = true;
            foreach (var elemento in ARREGLO_OBJETOS_XAML)
            {
                Type TypeTipodeElemento = elemento.GetType();
                if (TypeTipodeElemento.Name == "Entry")
                {
                    Entry EntryTemporal = elemento as Entry;
                    string texto = EntryTemporal.Text;
                    if (string.IsNullOrEmpty(texto) || string.IsNullOrWhiteSpace(texto))
                    {
                        if (respuesta)
                            EntryTemporal.Focus();
                        EntryTemporal.Placeholder = "*";
                        EntryTemporal.PlaceholderColor = (Color)Application.Current.Resources["ColorPlaceHolderCampoObligatorio"];
                        respuesta = false;
                    }
                    else
                    {
                        EntryTemporal.Placeholder = "";
                        EntryTemporal.PlaceholderColor = (Color)Application.Current.Resources["ColorPlaceHolder"];
                    }
                }
                else if (TypeTipodeElemento.Name == "TEntry")
                {
                    Entry EntryTemporal = elemento as Entry;
                    string texto = EntryTemporal.Text;
                    if (string.IsNullOrEmpty(texto) || string.IsNullOrWhiteSpace(texto))
                    {
                        if (respuesta)
                            EntryTemporal.Focus();
                        EntryTemporal.PlaceholderColor = (Color)Application.Current.Resources["ColorPlaceHolderCampoObligatorio"];
                        respuesta = false;
                    }
                    else
                    {
                        EntryTemporal.PlaceholderColor = (Color)Application.Current.Resources["ColorPlaceHolder"];
                    }
                }
                else if (TypeTipodeElemento.Name == "Picker")
                {
                    Picker PickerTemporal = elemento as Picker;
                    if (PickerTemporal.SelectedIndex == -1)
                    {
                        if (respuesta)
                            PickerTemporal.Focus();
                        PickerTemporal.Title = "* Seleccione";
                        PickerTemporal.TitleColor = (Color)Application.Current.Resources["ColorPlaceHolderCampoObligatorio"];
                        respuesta = false;
                    }
                    else
                    {
                        PickerTemporal.Title = "Seleccione";
                        PickerTemporal.TitleColor = (Color)Application.Current.Resources["ColorPlaceHolder"];
                    }
                }
                else if (TypeTipodeElemento.Name == "Editor")
                {
                    Editor EditorTemporal = elemento as Editor;
                    string texto = EditorTemporal.Text;
                    if (string.IsNullOrEmpty(texto) || string.IsNullOrWhiteSpace(texto))
                    {
                        if (respuesta)
                            EditorTemporal.Focus();
                        EditorTemporal.Placeholder = "*";
                        EditorTemporal.PlaceholderColor = (Color)Application.Current.Resources["ColorPlaceHolderCampoObligatorio"];
                        respuesta = false;
                    }
                    else
                    {
                        EditorTemporal.Placeholder = "";
                        EditorTemporal.PlaceholderColor = (Color)Application.Current.Resources["ColorPlaceHolder"];
                    }
                }
                else if (TypeTipodeElemento.Name == "DatePicker")
                {
                    DatePicker DatePickerTemporal = elemento as DatePicker;
                    if (DatePickerTemporal.Date == new DateTime() || DatePickerTemporal.Date == DatePickerTemporal.MinimumDate)
                    {
                        if (respuesta)
                            DatePickerTemporal.Focus();
                        DatePickerTemporal.TextColor = (Color)Application.Current.Resources["ColorPlaceHolderCampoObligatorio"];
                        respuesta = false;
                    }
                    else
                    {
                        DatePickerTemporal.TextColor = Color.Default;
                    }
                }
            }
        }

        public static bool FormatoValidoCorreoElectronico(string CORREO_ELECTRONICO)
        {
            bool respuesta = false;
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(CORREO_ELECTRONICO, expresion))
            {
                if (Regex.Replace(CORREO_ELECTRONICO, expresion, string.Empty).Length == 0)
                    respuesta = true;
                else
                    respuesta = false;
            }
            return respuesta;
        }
    }

    class TEntryMayusculas : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += BindableTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= BindableTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void BindableTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                ((Entry)sender).Text = e.NewTextValue.ToUpper();
            }
        }
    }

    class TValidarNumeroEnteroSinSigno : Behavior<Entry>
    {
        /* En el Entry poner la propiedad Keyboard="Numeric" */
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += BindableTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= BindableTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void BindableTextChanged(object sender, TextChangedEventArgs e)
        {
            int numeroEntero;
            var esNumero = int.TryParse(e.NewTextValue, out numeroEntero);

            if (string.IsNullOrEmpty(e.NewTextValue) || esNumero)
                ((Entry)sender).Text = e.NewTextValue;
            else
                ((Entry)sender).Text = e.OldTextValue;
        }
    }

    class TValidarNumeroDecimalSinSigno : Behavior<Entry>
    {
        /* En el Entry poner la propiedad Keyboard="Numeric" */
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += BindableTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= BindableTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void BindableTextChanged(object sender, TextChangedEventArgs e)
        {
            double numeroDecimal;
            var esNumero = double.TryParse(e.NewTextValue, out numeroDecimal);

            if (string.IsNullOrEmpty(e.NewTextValue) || esNumero)
                ((Entry)sender).Text = e.NewTextValue;
            else
                ((Entry)sender).Text = e.OldTextValue;
        }
    }

    class TValidarNumeroEntero : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += BindableTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= BindableTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void BindableTextChanged(object sender, TextChangedEventArgs e)
        {
            int numeroEntero;
            var esNumero = int.TryParse(e.NewTextValue, out numeroEntero);

            if (!string.IsNullOrWhiteSpace(e.NewTextValue) && !esNumero && (e.NewTextValue.Contains(".") || e.NewTextValue.Contains("+") || e.NewTextValue.Contains("-")))
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
            else
            {
                ((Entry)sender).Text = e.NewTextValue.Replace(".", "");
            }
        }
    }

    class TValidarNumeroDecimal : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += BindableTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= BindableTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void BindableTextChanged(object sender, TextChangedEventArgs e)
        {
            float numeroFlotante;
            var esNumero = float.TryParse(e.NewTextValue, out numeroFlotante);

            if (!string.IsNullOrWhiteSpace(e.NewTextValue) && !esNumero && (e.NewTextValue[e.NewTextValue.Length - 1].ToString() == "." || e.NewTextValue.Contains("+") || e.NewTextValue.Contains("-")))
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
            else
            {
                ((Entry)sender).Text = e.NewTextValue;
            }
        }
    }

    class TValidarMaximoLargo : Behavior<Entry>
    {
        public static readonly BindableProperty LARGO_MAXIMO = BindableProperty.Create("LargoMaximo", typeof(int), typeof(TValidarMaximoLargo), 0);

        public int LargoMaximo
        {
            get
            {
                return (int)GetValue(LARGO_MAXIMO);
            }
            set
            {
                SetValue(LARGO_MAXIMO, value);
            }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += BindableTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= BindableTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void BindableTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                if (e.NewTextValue.Length >= LargoMaximo)
                {
                    ((Entry)sender).Text = e.NewTextValue.Substring(0, LargoMaximo);
                }
            }
        }
    }

    class TValidarRequerido : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += BindableTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= BindableTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void BindableTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(((Entry)sender).Text))
            {
                ((Entry)sender).PlaceholderColor = Color.Red;
            }
        }
    }

    class TSwitchSiempreVerdadero : Behavior<Switch>
    {
        /* SiempreVerdadero es el nombre de la propiedad y el true es el valor por default y se puede modificar con new TSwitchSiempreVerdadero() { SiempreVerdadero = false }*/
        public static readonly BindableProperty SIEMPRE_VERDADERO = BindableProperty.Create("SiempreVerdadero", typeof(bool), typeof(TSwitchSiempreVerdadero), true);

        public bool SiempreVerdadero
        {
            get
            {
                return (bool)GetValue(SIEMPRE_VERDADERO);
            }
            set
            {
                SetValue(SIEMPRE_VERDADERO, value);
            }
        }

        protected override void OnAttachedTo(Switch bindable)
        {
            bindable.Toggled += BindableToggled;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Switch bindable)
        {
            bindable.Toggled -= BindableToggled;
            base.OnDetachingFrom(bindable);
        }

        private void BindableToggled(object sender, ToggledEventArgs e)
        {
            if (SiempreVerdadero && (e.Value == false))
                ((Switch)sender).IsToggled = true;
        }
    }
}
