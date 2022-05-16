
namespace ElementosComunes.Clases
{
    public class deFormatos
    {
        //---------------------------------------------------------------------------
        //---------          FORMATO PARA DIRECCION COMPLETA            -----------//
        //---------------------------------------------------------------------------
        public static string generaDireccionCompleta(string CALLE, string NUMERO_EXTERIOR, string NUMERO_INTERIOR, string COLONIA, string LOCALIDAD, string MUNICIPIO, string ESTADO, string PAIS, int CODIGO_POSTAL)
        {
            string direccionCompleta = "";

            if (!string.IsNullOrEmpty(CALLE))
                direccionCompleta += CALLE;

            if (!string.IsNullOrEmpty(NUMERO_EXTERIOR))
                direccionCompleta += " # " + NUMERO_EXTERIOR;

            if (!string.IsNullOrEmpty(NUMERO_INTERIOR))
                direccionCompleta += " INT. " + NUMERO_INTERIOR;

            if (!string.IsNullOrEmpty(COLONIA))
                direccionCompleta += ", COL. " + COLONIA;

            if (!string.IsNullOrEmpty(LOCALIDAD))
                direccionCompleta += ", " + LOCALIDAD;

            if (!string.IsNullOrEmpty(MUNICIPIO))
                direccionCompleta += ", " + MUNICIPIO;

            if (!string.IsNullOrEmpty(ESTADO))
                direccionCompleta += ", " + ESTADO;

            if (!string.IsNullOrEmpty(PAIS))
                direccionCompleta += ", " + PAIS;

            if (CODIGO_POSTAL > 0 && !string.IsNullOrEmpty(CODIGO_POSTAL.ToString()))
                direccionCompleta += ", CP. " + (CODIGO_POSTAL.ToString());

            return direccionCompleta;
        }
        //---------------------------------------------------------------------------





        //---------------------------------------------------------------------------
        //--------       FORMATO PARA NÚMEROS DE CONTRATO COMPLETO        ---------//
        //---------------------------------------------------------------------------
        public static string getNumerodeContratoCompleto(string SERIE_DE_CONTRATO, int NUMERO_DE_CONTRATO)
        {
            string respuesta;

            string NUM_CONTRATO_STRING = NUMERO_DE_CONTRATO.ToString();
            while (NUM_CONTRATO_STRING.Length < 5)
                NUM_CONTRATO_STRING = "0" + NUM_CONTRATO_STRING;

            if (SERIE_DE_CONTRATO != "SS")
                respuesta = (SERIE_DE_CONTRATO + NUM_CONTRATO_STRING);
            else
                respuesta = NUM_CONTRATO_STRING;

            return respuesta;
        }
        //---------------------------------------------------------------------------
    }
}
