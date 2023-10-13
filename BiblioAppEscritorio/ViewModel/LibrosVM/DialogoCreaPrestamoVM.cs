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

namespace BiblioAppEscritorio
{
    class DialogoCreaPrestamoVM : ObservableRecipient
    {
        //VARIABLES
        private readonly ApiService api;
        private readonly ServicioDialogos servicioDialogos;
        private Libros libroSeleccionado;

        public Libros LibroSeleccionado
        {
            get { return libroSeleccionado; }
            set { SetProperty(ref libroSeleccionado, value); }
        }

        private Socio socioSeleccionado;

        public Socio SocioSeleccionado
        {
            get { return socioSeleccionado; }
            set { SetProperty(ref socioSeleccionado, value); }
        }
        private string dni;

        public string Dni
        {
            get { return dni; }
            set { SetProperty(ref dni, value); }
        }

        private string nombreApellidos;

        public string NombreApellidos
        {
            get { return nombreApellidos; }
            set { SetProperty(ref nombreApellidos, value); }
        }


        private Prestamo prestamoACrear;

        public Prestamo PrestamoACrear
        {
            get { return prestamoACrear; }
            set { SetProperty(ref prestamoACrear, value); }
        }

        public DialogoCreaPrestamoVM()
        {
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();
            SocioSeleccionado = null;
            PrestamoACrear = null;
            NombreApellidos = String.Empty;
            dni = String.Empty;
            LibroSeleccionado = WeakReferenceMessenger.Default.Send<LibroSeleccionadoMessage>();
        }
        public void BuscaSocio()
        {
            //45654565T
            if (dni != null && dni != String.Empty)
            {
                SocioSeleccionado = api.GetSocioPorDni(api.GetElementoSeguridad(), dni);
                if (SocioSeleccionado != null)
                    NombreApellidos = SocioSeleccionado.nombre + " " + SocioSeleccionado.apellidos;
            }
            else
            {
                SocioSeleccionado = null;
            }
        }
        public void CreaPrestamo()
        {
            String hoy = DateTime.Today.ToString("yyyy-MM-dd") + "T00:00:00Z[UTC]";
            String fechaTope = DateTime.Now.AddDays(15).ToString("yyyy-MM-dd") + "T00:00:00Z[UTC]";
            if (dni != null && dni != String.Empty)
            {
                Libro libro = api.GetLibroDisponiblePorIsbn(LibroSeleccionado.isbn).First();
                if (libro != null && SocioSeleccionado != null)
                {
                    PrestamoACrear = new Prestamo(hoy, hoy, fechaTope, libro.IdLibro, 0, SocioSeleccionado.idSocio);
                    string mensaje = api.CreaPrestamo(api.CreateSesion(), PrestamoACrear);
                    if (mensaje != "Préstamo insertado")
                    {
                        servicioDialogos.MensajeError("ERROR", "No se ha podido crear el préstamo");
                    }
                    else
                    {
                        api.MarcaUnidadComoNoDisponible(api.CreateSesion(), libro.IdLibro);

                        WeakReferenceMessenger.Default.Send(new ActualizarGridPrestamosMessage(
                            new GridPrestamos(
                                SocioSeleccionado.dni,
                                PrestamoACrear.fechaDevolucion,
                                PrestamoACrear.fechaPrestamo,
                                PrestamoACrear.fechaTope,
                                PrestamoACrear.idLibro,
                                PrestamoACrear.idPrestamo,
                                LibroSeleccionado.imagen,
                                LibroSeleccionado.isbn,
                                SocioSeleccionado.nombre,
                                LibroSeleccionado.titulo)
                            ));
                        //Para actualizar la lista de socios
                        WeakReferenceMessenger.Default.Send(new ActualizarGridSociosMessage(new Socio()));
                        servicioDialogos.MensajeInformativo("OK", mensaje);
                    }
                }
                else
                {
                    servicioDialogos.MensajeError("ERROR", "Error al cargar los datos del libro");
                }
            }
            else
            {
                servicioDialogos.MensajeError("ERROR", "Error al cargar los datos del préstamo");
            }
        }
    }
}
