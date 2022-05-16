
using Foundation;
using UIKit;

namespace Directiva10.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            /* Inicializando Plugins Inicio */
            Rg.Plugins.Popup.Popup.Init();
            Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();
            /* Inicializando Plugins Fin */

            LoadApplication(new App());

            /* Inicializando Personalizaciones Inicio */
            /* if (UIDevice.CurrentDevice.CheckSystemVersion(15, 0)){} */
            /* Inicializando Personalizaciones Fin */

            return base.FinishedLaunching(app, options);
        }
    }
}
