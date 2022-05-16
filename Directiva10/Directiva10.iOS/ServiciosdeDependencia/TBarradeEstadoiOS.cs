using Foundation;
using Directiva10.Interfaces;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Directiva10.iOS.ServiciosdeDependencia;

[assembly: Dependency(typeof(TBarradeEstadoiOS))]
namespace Directiva10.iOS.ServiciosdeDependencia
{
    class TBarradeEstadoiOS : IBarradeEstado
    {
        public void AplicarColoraBarradeEstado(System.Drawing.Color ColorBarradeEstado, bool PINTAR_OSCURO_BARRA_DE_ESTADO)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                var barradeEstado = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame);
                barradeEstado.BackgroundColor = ColorBarradeEstado.ToPlatformColor();
                UIApplication.SharedApplication.KeyWindow.AddSubview(barradeEstado);
            }
            else
            {
                var barradeEstado = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
                if (barradeEstado.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                {
                    barradeEstado.BackgroundColor = ColorBarradeEstado.ToPlatformColor();
                }
            }
            var estilos = PINTAR_OSCURO_BARRA_DE_ESTADO ? UIStatusBarStyle.DarkContent : UIStatusBarStyle.LightContent;
            UIApplication.SharedApplication.SetStatusBarStyle(estilos, false);
            Xamarin.Essentials.Platform.GetCurrentUIViewController()?.SetNeedsStatusBarAppearanceUpdate();
        }
    }
}