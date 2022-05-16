using Xamarin.Forms;

namespace Directiva10.Interfaces
{
    public class TDatePicker : DatePicker
    {
        public static BindableProperty BindablePropertyBorderColor = BindableProperty.Create("BorderColor", typeof(Color), typeof(TDatePicker), Color.FromHex("#807A79"));
        public static BindableProperty BindablePropertyBorderRadius = BindableProperty.Create("BorderRadius", typeof(int), typeof(TDatePicker), 20);
        public static BindableProperty BindablePropertyBorderWidth = BindableProperty.Create("BorderWidth", typeof(int), typeof(TEntry), 2);
        public static BindableProperty BindablePropertyPaddingAndroid = BindableProperty.Create("PaddingAndroid", typeof(Thickness), typeof(TEntry), new Thickness(10));

        public TDatePicker()
        {
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BindablePropertyBorderColor);
            set => SetValue(BindablePropertyBorderColor, value);
        }

        public int BorderRadius
        {
            get => (int)GetValue(BindablePropertyBorderRadius);
            set => SetValue(BindablePropertyBorderRadius, value);
        }

        public int BorderWidth
        {
            get => (int)GetValue(BindablePropertyBorderWidth);
            set => SetValue(BindablePropertyBorderWidth, value);
        }

        /// <summary>
        /// Esta propiedad no se puede cambiar en iOS.
        /// </summary>
        public Thickness PaddingAndroid
        {
            get => (Thickness)GetValue(BindablePropertyPaddingAndroid);
            set => SetValue(BindablePropertyPaddingAndroid, value);
        }
    }
}
