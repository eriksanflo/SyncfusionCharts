using Android.Content;
using Android.OS;
using Android.Views.InputMethods;
using Directiva10.Droid.Renderizadores;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ListView), typeof(TRenderizarListView))]
namespace Directiva10.Droid.Renderizadores
{
    /// <summary>
    /// Versión 2.0
    /// Modificación 18/07/2020
    /// </summary>
    class TRenderizarListView : ListViewRenderer
    {
        public TRenderizarListView(Context context) : base(context)
        {
        }

        /* Ocultar el teclado al hacer scroll en la lista */
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.ScrollStateChanged += (sender, evt) =>
                {
                    //Ocultamos teclado de Android
                    new Handler().Post(delegate
                    {
                        var manejador = (InputMethodManager)Control.Context.GetSystemService(Android.Content.Context.InputMethodService);
                        var resultado = manejador.HideSoftInputFromWindow(Control.WindowToken, 0);
                    });
                };

                Control.Drag += (sender, evt) =>
                {
                    //Ocultamos teclado de Android
                    new Handler().Post(delegate
                    {
                        var manejador = (InputMethodManager)Control.Context.GetSystemService(Android.Content.Context.InputMethodService);
                        var resultado = manejador.HideSoftInputFromWindow(Control.WindowToken, 0);
                    });
                };
            }
        }
    }
}