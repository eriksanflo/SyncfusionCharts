using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Directiva10.Usuarios
{
    internal class TUsuario : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechadeConexion { get; set; }
        public TimeSpan HoradeConexion { get; set; }
        public string Sistema { get; set; }
        public string VersionInstalada { get; set; }
        public bool EstaOnline { get; set; }
        public bool EstaSeleccionado { get; set; }

        public double RotacionIcono
        {
            get
            {
                double respuesta = 0;
                if (EstaOnline == true)
                    respuesta = 90;
                return respuesta;
            }
        }

        public string Icono
        {
            get
            {
                string respuesta = "iconoUsuarioOffline.png";
                if (EstaOnline == true)
                    respuesta = "iconoUsuario.png";
                return respuesta;
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
