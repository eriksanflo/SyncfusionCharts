using Android.Content;
using Android.Graphics.Drawables;
using Android.Widget;
using Directiva10.Droid.Renderizadores;
using Directiva10.Interfaces;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TDatePicker), typeof(TRenderizarTDatePicker))]
namespace Directiva10.Droid.Renderizadores
{
    public class TRenderizarTDatePicker : DatePickerRenderer
    {
        public TDatePicker DatePickerModificado => Element as TDatePicker;

        public TRenderizarTDatePicker(Context ContextBase) : base(ContextBase)
        {

        }

        protected override EditText CreateNativeControl()
        {
            EditText EditTextControl = base.CreateNativeControl();
            ActualizarFondo(EditTextControl);
            return EditTextControl;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TDatePicker.BindablePropertyBorderColor.PropertyName)
            {
                ActualizarFondo();
            }
            else if (e.PropertyName == TDatePicker.BindablePropertyBorderRadius.PropertyName)
            {
                ActualizarFondo();
            }
            else if (e.PropertyName == TDatePicker.BindablePropertyBorderWidth.PropertyName)
            {
                ActualizarFondo();
            }
            base.OnElementPropertyChanged(sender, e);
        }

        protected void ActualizarFondo()
        {
            ActualizarFondo(Control);
        }

        protected void ActualizarFondo(EditText FormsEditTextControl)
        {
            // Este método hace que los DatePicker sean blancos con bordes redondeados
            if (FormsEditTextControl == null) return;

            GradientDrawable GradientDrawableDibujo = new GradientDrawable();
            // Color de fondo del contenido
            GradientDrawableDibujo.SetColor(DatePickerModificado.BackgroundColor.ToAndroid());
            // Que tan redondo el borde
            GradientDrawableDibujo.SetCornerRadius(Context.ToPixels(DatePickerModificado.BorderRadius));
            GradientDrawableDibujo.SetGradientRadius(Context.ToPixels(DatePickerModificado.BorderRadius));
            // Ancho y color del contorno del borde
            GradientDrawableDibujo.SetStroke((int)Context.ToPixels(DatePickerModificado.BorderWidth), DatePickerModificado.BorderColor.ToAndroid());
            FormsEditTextControl.SetBackground(GradientDrawableDibujo);

            // Padding del contenido
            int paddingSuperior = (int)Context.ToPixels(DatePickerModificado.PaddingAndroid.Top);
            int paddingInferior = (int)Context.ToPixels(DatePickerModificado.PaddingAndroid.Bottom);
            int paddingIzquierda = (int)Context.ToPixels(DatePickerModificado.PaddingAndroid.Left);
            int paddingDerecha = (int)Context.ToPixels(DatePickerModificado.PaddingAndroid.Right);

            FormsEditTextControl.SetPadding(paddingIzquierda, paddingSuperior, paddingDerecha, paddingInferior);
            FormsEditTextControl.SetClipToOutline(true);

        }
    }
}