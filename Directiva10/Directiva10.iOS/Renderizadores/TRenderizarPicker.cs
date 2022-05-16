using CoreAnimation;
using CoreGraphics;
using Directiva10.iOS.Renderizadores;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Picker), typeof(TRenderizarPicker))]
namespace Directiva10.iOS.Renderizadores
{
    /// <summary>
    /// Versión 2.0
    /// Modificación 18/07/2020
    /// </summary>
    class TRenderizarPicker : PickerRenderer
    {
        CALayer CALayerLineaInferior;

        /* Agrega una línea inferior en todos los Picker*/
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (Control != null)
            {
                if (CALayerLineaInferior == null)
                {
                    // Remover le borde que tiene por default
                    Control.BorderStyle = UITextBorderStyle.None;

                    var width = Element.Width;
                    var height = Element.Height;

                    CALayerLineaInferior = new CALayer();
                    CALayerLineaInferior.Frame = new CGRect(0, height - 1, width, 1);
                    CALayerLineaInferior.BackgroundColor = new CoreGraphics.CGColor(0.73f, 0.7451f, 0.7647f);

                    Control.Layer.AddSublayer(CALayerLineaInferior);
                }
            }
        }
    }
}