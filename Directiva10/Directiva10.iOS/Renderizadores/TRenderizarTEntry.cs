using CoreGraphics;
using Foundation;
using Directiva10.Interfaces;
using System.ComponentModel;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Directiva10.iOS.Renderizadores;

[assembly: ExportRenderer(typeof(TEntry), typeof(TRenderizarTEntry))]
namespace Directiva10.iOS.Renderizadores
{
    /// <summary>
    /// TEntry se puede hacer formas circulares
    /// Versión 2.0
    /// Modificación 16/12/2020
    /// </summary>
    public class TRenderizarTEntry : EntryRenderer
    {
        public TEntry EntryModificado => Element as TEntry;
        public UITextFieldPadding UITextFieldPaddingControlV2 => Control as UITextFieldPadding;

        protected override UITextField CreateNativeControl()
        {
            var control = new UITextFieldPadding(RectangleF.Empty)
            {
                Padding = EntryModificado.Padding,
                BorderStyle = UITextBorderStyle.RoundedRect,
                ClipsToBounds = true
            };

            UpdateBackground(control);

            return control;
        }

        protected void UpdateBackground(UITextField control)
        {
            if (control == null) return;
            // Que tan redondo el borde
            control.Layer.CornerRadius = EntryModificado.BorderRadius;
            // Ancho y color del contorno del borde
            control.Layer.BorderWidth = EntryModificado.BorderWidth;
            control.Layer.BorderColor = EntryModificado.BorderColor.ToCGColor();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TEntry.BindablePropertyPadding.PropertyName)
            {
                ActualizarPadding();
            }

            base.OnElementPropertyChanged(sender, e);
        }

        protected void ActualizarPadding()
        {
            if (Control == null)
                return;
            // Padding del contenido
            UITextFieldPaddingControlV2.Padding = EntryModificado.Padding;
        }


        public class UITextFieldPadding : UITextField
        {
            private Thickness ThicknessPadding = new Thickness(5);

            public Thickness Padding
            {
                get => ThicknessPadding;
                set
                {
                    if (ThicknessPadding != value)
                    {
                        ThicknessPadding = value;
                    }
                }
            }

            public UITextFieldPadding()
            {
            }
            public UITextFieldPadding(NSCoder NSCoderBase) : base(NSCoderBase)
            {
            }

            public UITextFieldPadding(CGRect CGRectBase) : base(CGRectBase)
            {
            }

            public override CGRect TextRect(CGRect CGRectLimites)
            {
                var insets = new UIEdgeInsets((float)Padding.Top, (float)Padding.Left, (float)Padding.Bottom, (float)Padding.Right);
                return insets.InsetRect(CGRectLimites);
            }

            public override CGRect PlaceholderRect(CGRect CGRectLimites)
            {
                var insets = new UIEdgeInsets((float)Padding.Top, (float)Padding.Left, (float)Padding.Bottom, (float)Padding.Right);
                return insets.InsetRect(CGRectLimites);
            }

            public override CGRect EditingRect(CGRect CGRectLimites)
            {
                var insets = new UIEdgeInsets((float)Padding.Top, (float)Padding.Left, (float)Padding.Bottom, (float)Padding.Right);
                return insets.InsetRect(CGRectLimites);
            }
        }
    }
}