using Directiva10.iOS.Renderizadores;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(TRenderizarSearchBar))]
namespace Directiva10.iOS.Renderizadores
{
    class TRenderizarSearchBar : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            // En versión iOS 13 el fondo no se pone blanco, así que se renderiza para esa versión
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0) && Control != null)
            {
                // Fondo blanco el que cubre el searchbar
                //Control.SearchTextField.BackgroundColor = UIColor.FromRGB(255, 255, 255);

                // Fondo transparente el que cubre el searchbar
                //Control.SearchTextField.BackgroundColor = new UIColor(0, 0, 0, 0);
            }
        }
    }
}