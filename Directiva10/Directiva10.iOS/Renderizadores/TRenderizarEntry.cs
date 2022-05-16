using CoreAnimation;
using CoreGraphics;
using Directiva10.iOS.Renderizadores;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(TRenderizarEntry))]
namespace Directiva10.iOS.Renderizadores
{
    /// <summary>
    /// Versión 2.0
    /// Modificación 08/12/2020
    /// </summary>
    class TRenderizarEntry : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            // Necesita conectarse al evento Sizechanged porque la primera vez que se ejecuta el renderizado el Entry no tiene tamaño (-1).
            // El siguiente método hace que aparezca una línea gris inferior en el Entry y usa 
            //  using CoreAnimation;
            //  using CoreGraphics;
            //  using UIKit;
            if (e.NewElement != null)
            {
                e.NewElement.SizeChanged += (OBJETO, ARGUMENTOS) =>
                {
                    var vistaEntry = OBJETO as Entry;
                    if (vistaEntry == null)
                        return;

                    // Obtenemos el control nativo (UITextField)
                    UITextField UITextFieldEntryNativo = this.Control;

                    // Creamos los bordes, solo de la parte inferior
                    CALayer CALayerBorde = new CALayer();
                    float ancho = 1.0f;
                    // Borde color gris
                    CALayerBorde.BorderColor = new CoreGraphics.CGColor(0.73f, 0.7451f, 0.7647f);
                    CALayerBorde.Frame = new CGRect(x: 0, y: vistaEntry.Height - ancho, width: vistaEntry.Width, height: 1.0f);
                    CALayerBorde.BorderWidth = ancho;
                    CALayerBorde.BackgroundColor = new CoreGraphics.CGColor(0, 0, 0, 0);

                    UITextFieldEntryNativo.Layer.AddSublayer(CALayerBorde);
                    UITextFieldEntryNativo.Layer.MasksToBounds = true;
                    UITextFieldEntryNativo.BorderStyle = UITextBorderStyle.None;
                    // Fondo blanco 
                    //UITextFieldEntryNativo.BackgroundColor = new UIColor(1, 1, 1, 1);

                    // Fondo transparente
                    UITextFieldEntryNativo.BackgroundColor = new UIColor(0, 0, 0, 0);
                };
            }
        }
    }
}