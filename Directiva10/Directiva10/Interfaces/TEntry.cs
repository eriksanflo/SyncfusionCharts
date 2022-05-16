using Xamarin.Forms;

namespace Directiva10.Interfaces
{
    public sealed class TEntry : Entry
    {
        public static BindableProperty BindablePropertyBorderColor = BindableProperty.Create("BorderColor", typeof(Color), typeof(TEntry), Color.Transparent);
        public static BindableProperty BindablePropertyBorderRadius = BindableProperty.Create("BorderRadius", typeof(int), typeof(TEntry), 0);
        public static BindableProperty BindablePropertyBorderWidth = BindableProperty.Create("BorderWidth", typeof(int), typeof(TEntry), 0);
        public static BindableProperty BindablePropertyPadding = BindableProperty.Create("Padding", typeof(Thickness), typeof(TEntry), new Thickness(5));

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
        public Thickness Padding
        {
            get => (Thickness)GetValue(BindablePropertyPadding);
            set => SetValue(BindablePropertyPadding, value);
        }
    }
}
