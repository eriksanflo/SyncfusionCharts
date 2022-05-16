using Directiva10.Interfaces;
using Directiva10.iOS.Renderizadores;
using System;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TDatePicker), typeof(TRenderizarTDatePicker))]
namespace Directiva10.iOS.Renderizadores
{
    public class TRenderizarTDatePicker : DatePickerRenderer
    {
        public TDatePicker DatePickerModificado => Element as TDatePicker;

        protected override UITextField CreateNativeControl()
        {
            UITextField UITextFieldControl = new UITextField(RectangleF.Empty);
            UITextFieldControl.Layer.MasksToBounds = true;
            UITextFieldControl.ClipsToBounds = true;
            UITextFieldControl.BorderStyle = UITextBorderStyle.RoundedRect;
            ActualizarBorde(UITextFieldControl);
            return UITextFieldControl;
        }

        protected void ActualizarBorde(UITextField Control)
        {
            if (Control == null) return;
            Control.Layer.CornerRadius = (nfloat)DatePickerModificado.BorderRadius;
            Control.Layer.BorderColor = DatePickerModificado.BorderColor.ToCGColor();
            Control.Layer.BorderWidth = (nfloat)DatePickerModificado.BorderWidth;
            Control.Layer.BackgroundColor = DatePickerModificado.BackgroundColor.ToCGColor();
        }
    }
}