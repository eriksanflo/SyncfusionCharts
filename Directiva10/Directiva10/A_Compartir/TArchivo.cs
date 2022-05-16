using Directiva10.Interfaces;
using System;
using System.IO;
using Xamarin.Forms;

namespace Directiva10.A_Compartir
{
    class TArchivo
    {
        public static bool BorrarCarpetaLocal(string DIRECTORIO_LOCAL)
        {
            bool respuesta = true;
            try
            {
                if (!string.IsNullOrEmpty(DIRECTORIO_LOCAL))
                {
                    bool todoslosArchivosEliminados = true;
                    foreach (string archivo in Directory.GetFiles(DIRECTORIO_LOCAL, "*.*"))
                    {
                        if (File.Exists(archivo))
                        {
                            File.SetAttributes(archivo, FileAttributes.Normal);
                            File.Delete(archivo);
                            if (File.Exists(archivo))
                                todoslosArchivosEliminados = false;
                        }
                    }
                    respuesta = todoslosArchivosEliminados;
                }
                else
                    respuesta = true;
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringTitulodeMensajedeError"], e.ToString(), "Aceptar"); });
            };
            return respuesta;
        }

        public static bool BorrarArchivoLocal(string DIRECTORIO_LOCAL)
        {
            bool respuesta = false;
            try
            {
                if (!string.IsNullOrEmpty(DIRECTORIO_LOCAL))
                {
                    if (File.Exists(DIRECTORIO_LOCAL))
                    {
                        File.Delete(DIRECTORIO_LOCAL);
                        respuesta = !File.Exists(DIRECTORIO_LOCAL);
                    }
                    else
                        respuesta = true;
                }
                else
                    respuesta = true;
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert((string)Application.Current.Resources["StringTitulodeMensajedeError"], e.ToString(), "Aceptar"); });
            };
            return respuesta;
        }

        public static string GenerarNombredelArchivo(string EXTENSION_DE_ARCHIVO)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + "." + EXTENSION_DE_ARCHIVO;
        }

        public static bool CopiarArchivoalaCarpeta(string ARCHIVO_CON_DIRECTORIO_ORIGEN, string NOMBRE_DEL_ALBUM)
        {
            bool respuesta = false;
            try
            {
                string directoriodelAlbum = DependencyService.Get<IDirectorioLocal>().ObtenerRutaAlbumMultimedia(NOMBRE_DEL_ALBUM);
                if (!string.IsNullOrEmpty(directoriodelAlbum))
                {
                    if (File.Exists(ARCHIVO_CON_DIRECTORIO_ORIGEN))
                    {
                        Directory.CreateDirectory(directoriodelAlbum);
                        File.Copy(ARCHIVO_CON_DIRECTORIO_ORIGEN, directoriodelAlbum, true);
                        respuesta = true;
                    }
                }
            }
            catch (Exception)
            {
                respuesta = false;
            }
            return respuesta;
        }

        public static string CopiaryRenombraArchivoalaCarpeta(string ARCHIVO_CON_DIRECTORIO_ORIGEN, string NOMBRE_DEL_ALBUM_DIRECTORIO_DESTINO, string NUEVO_NOMBRE)
        {
            string respuesta = "";
            try
            {
                if (!string.IsNullOrEmpty(NOMBRE_DEL_ALBUM_DIRECTORIO_DESTINO))
                {
                    string nuevoNombre = NOMBRE_DEL_ALBUM_DIRECTORIO_DESTINO + "/" + NUEVO_NOMBRE.Trim();
                    if (File.Exists(ARCHIVO_CON_DIRECTORIO_ORIGEN))
                    {
                        Directory.CreateDirectory(NOMBRE_DEL_ALBUM_DIRECTORIO_DESTINO);
                        string archivoRenombrado = NOMBRE_DEL_ALBUM_DIRECTORIO_DESTINO + "/" + NUEVO_NOMBRE;
                        File.Copy(ARCHIVO_CON_DIRECTORIO_ORIGEN, archivoRenombrado, true);
                        if (File.Exists(archivoRenombrado))
                            respuesta = archivoRenombrado;
                    }
                }
            }
            catch (Exception e)
            { }
            return respuesta;
        }

        public static string GuardarArchivoalaCarpeta(Stream StreamOrigen, string NOMBRE_DEL_ALBUM_DIRECTORIO_DESTINO, string NUEVO_NOMBRE)
        {
            string respuesta = "";
            try
            {
                if (StreamOrigen != null)
                {
                    if (StreamOrigen.Length > 0 && StreamOrigen.CanRead)
                    {
                        BinaryReader BinaryReaderBinario = new BinaryReader(StreamOrigen);
                        BinaryReaderBinario.BaseStream.Position = 0;
                        Byte[] ArreglodeBytes = BinaryReaderBinario.ReadBytes((int)StreamOrigen.Length);
                        byte[] arreglodeByte = (byte[])ArreglodeBytes;

                        Directory.CreateDirectory(NOMBRE_DEL_ALBUM_DIRECTORIO_DESTINO);
                        string archivoRenombrado = NOMBRE_DEL_ALBUM_DIRECTORIO_DESTINO + "/" + NUEVO_NOMBRE;
                        File.WriteAllBytes(archivoRenombrado, arreglodeByte);
                        if (File.Exists(archivoRenombrado))
                            respuesta = archivoRenombrado;
                    }
                }
            }
            catch (Exception e)
            { }
            return respuesta;
        }
    }
}
