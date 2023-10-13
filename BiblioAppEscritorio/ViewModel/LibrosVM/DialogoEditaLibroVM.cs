using BiblioAppEscritorio.Api;
using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.Servicios;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.ViewModel.LibrosVM
{
    class DialogoEditaLibroVM : ObservableObject
    {
        //VARIABLES
        private readonly ApiService api;
        private readonly ServicioAzure servicioAzure;
        private readonly ServicioDialogos servicioDialogos;
        private Libros libroAEditar;
        private static readonly string filtrosRuta = "JPG (*.jpg)|*.jpg|PNG (*.png*)|*.png*";

        public Libros LibroAEditar
        {
            get { return libroAEditar; }
            set { SetProperty(ref libroAEditar, value); }
        }
        private string imagen;

        public string Imagen
        {
            get { return imagen; }
            set { SetProperty(ref imagen, value); }
        }

        public DialogoEditaLibroVM()
        {
            servicioDialogos = new ServicioDialogos();
            servicioAzure = new ServicioAzure();
            api = new ApiService();
            LibroAEditar = WeakReferenceMessenger.Default.Send<LibroSeleccionadoMessage>();
            Imagen = LibroAEditar.imagen;
        }

        public void EditaLibro()
        {
            if (CamposCorrectos())
            {
                LibroAEditar.imagen = Imagen;
                string mensaje = api.EditaLibro(api.GetElementoSeguridad(), LibroAEditar);
                if (mensaje != "Libro actualizado")
                {
                    servicioDialogos.MensajeError("ERROR", mensaje);
                }
                else
                {
                    servicioDialogos.MensajeInformativo("OK", "Libro editado");
                    WeakReferenceMessenger.Default.Send(new ActualizarGridLibrosMessage(LibroAEditar));
                }
            }
            else
            {
                servicioDialogos.MensajeError("ERROR", "Campos incorrectos");
            }
        }
        public void ExaminarImagen()
        {
            try
            {
                string rutaImagen = servicioDialogos.AbrirArchivoDialogo(filtrosRuta);
                string rutaAzure = servicioAzure.GuardarImagen(rutaImagen);
                LibroAEditar.imagen = rutaAzure;
                Imagen = rutaAzure;
            }//System.AggregateException System.ArgumentOutOfRangeException
            catch (AggregateException)
            {
                servicioDialogos.MensajeError("ERROR", "Error al guardar la imagen");
            }
            catch (ArgumentException) { Console.WriteLine("Ha cerrado dialogo"); }
            catch (Azure.RequestFailedException)
            {
                servicioDialogos.MensajeError("ERROR", "Error con la imagen");
            }
            catch (Exception)
            {
                servicioDialogos.MensajeError("ERROR", "Error desconocido");
            }
        }

        private bool CamposCorrectos()
        {
            return LibroAEditar.autores != string.Empty &&
                LibroAEditar.descripcion != string.Empty &&
                LibroAEditar.anyoPublicacion != 0 &&
                LibroAEditar.paginas != 0 &&
                LibroAEditar.titulo != string.Empty &&
                LibroAEditar.idioma != string.Empty &&
                Imagen != string.Empty &&
                Imagen != null &&
                LibroAEditar.isbn != string.Empty &&
                LibroAEditar.categoria != string.Empty;
        }
    }
}