namespace Directiva10.A_General
{
    class TSesionPrincipal
    {
        private static string _FotodePerfil;

        public static int IdEntidad { get; set; }
        public static string Usuario { get; set; }
        public static string Nombre { get; set; }
        public static string Apellidos { get; set; }
        public static bool TieneAccesoalSistema { get; set; }
        public static string FotoPerfil
        {
            get
            {
                return string.IsNullOrWhiteSpace(_FotodePerfil) ? "sinFotodePerfil.png" : _FotodePerfil;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Contains("no-perfil.jpg"))
                    _FotodePerfil = "";
                else
                    _FotodePerfil = "https://s3.us-west-2.amazonaws.com/" + value;
            }
        }
    }
}
