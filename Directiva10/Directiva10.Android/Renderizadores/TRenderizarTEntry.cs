using Android.Content;
using Android.Graphics.Drawables;
using Directiva10.Droid.Renderizadores;
using Directiva10.Interfaces;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TEntry), typeof(TRenderizarTEntry))]
namespace Directiva10.Droid.Renderizadores
{
    /// <summary>
    /// Versión 2.0
    /// Modificación 16/12/2020
    /// </summary>

    public class TRenderizarTEntry : EntryRenderer
    {
        public TRenderizarTEntry(Context context) : base(context)
        {

        }

        public TEntry EntryModificado => Element as TEntry;

        protected override FormsEditText CreateNativeControl()
        {
            FormsEditText FormsEditTextControl = base.CreateNativeControl();
            ActualizarFondo(FormsEditTextControl);
            return FormsEditTextControl;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TEntry.BindablePropertyBorderColor.PropertyName)
            {
                ActualizarFondo();
            }
            else if (e.PropertyName == TEntry.BindablePropertyBorderRadius.PropertyName)
            {
                ActualizarFondo();
            }
            else if (e.PropertyName == TEntry.BindablePropertyBorderWidth.PropertyName)
            {
                ActualizarFondo();
            }
            base.OnElementPropertyChanged(sender, e);
        }

        protected override void UpdateBackgroundColor()
        {
            ActualizarFondo();
        }

        protected void ActualizarFondo()
        {
            ActualizarFondo(Control);
        }

        protected void ActualizarFondo(FormsEditText FormsEditTextControl)
        {
            // Este método hace que los Entry sean blancos con bordes redondeados
            if (FormsEditTextControl == null) return;

            GradientDrawable GradientDrawableDibujo = new GradientDrawable();
            // Fondo del Entry
            GradientDrawableDibujo.SetColor(Element.BackgroundColor.ToAndroid());
            // Que tan redondo el borde
            GradientDrawableDibujo.SetCornerRadius(Context.ToPixels(EntryModificado.BorderRadius));
            // Ancho y color del contorno del borde
            GradientDrawableDibujo.SetStroke((int)Context.ToPixels(EntryModificado.BorderWidth), EntryModificado.BorderColor.ToAndroid());
            FormsEditTextControl.SetBackground(GradientDrawableDibujo);

            // Padding del contenido
            int paddingSuperior = (int)Context.ToPixels(EntryModificado.Padding.Top);
            int paddingInferior = (int)Context.ToPixels(EntryModificado.Padding.Bottom);
            int paddingIzquierda = (int)Context.ToPixels(EntryModificado.Padding.Left);
            int paddingDerecha = (int)Context.ToPixels(EntryModificado.Padding.Right);

            FormsEditTextControl.SetPadding(paddingIzquierda, paddingSuperior, paddingDerecha, paddingInferior);
        }
    }
}