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
    class DialogoCreaReservaVM : ObservableRecipient
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

        private Reserva reservaACrear;

        public Reserva ReservaACrear
        {
            get { return reservaACrear; }
            set { SetProperty(ref reservaACrear, value); }
        }
        private string nombreApellidos;

        public string NombreApellidos
        {
            get { return nombreApellidos; }
            set { SetProperty(ref nombreApellidos, value); }
        }


        public DialogoCreaReservaVM()
        {
            servicioDialogos = new ServicioDialogos();
            api = new ApiService();
            SocioSeleccionado = null;
            ReservaACrear = null;
            dni = String.Empty;
            NombreApellidos = String.Empty;
            LibroSeleccionado = WeakReferenceMessenger.Default.Send<LibroSeleccionadoMessage>();
        }
        public void BuscaSocio()
        {
            //45654565T
            if (dni != null && dni != String.Empty)
            {
                SocioSeleccionado = api.GetSocioPorDni(api.GetElementoSeguridad(), dni);
                if(SocioSeleccionado != null)
                    NombreApellidos = SocioSeleccionado.nombre + " " + SocioSeleccionado.apellidos;
            }
            else
            {
                SocioSeleccionado = null;
            }
        }
        public void CreaReserva()
        {
            if (dni != null && dni != String.Empty)
            {
                Libro libro = api.GetLibroDisponibleReservaPorIsbn(LibroSeleccionado.isbn);
                if (libro != null && SocioSeleccionado != null)
                {
                    string mensaje = api.CreaReserva(api.GetElementoSeguridad(), SocioSeleccionado.idSocio, libro.IdLibro);
                    if (mensaje != "Reserva insertada")
                    {
                        servicioDialogos.MensajeError("ERROR", "No se ha podido crear la reserva");
                    }
                    else
                    {
                        api.MarcaUnidadComoNoDisponibleReserva(api.CreateSesion(), libro.IdLibro);
                        //Actualiza grid reservas
                        WeakReferenceMessenger.Default.Send(new ActualizarGridReservasMessage(new GridReservas()));
                        //Actualiza datos socio
                        WeakReferenceMessenger.Default.Send(new ActualizarGridSociosMessage(new Socio()));
                        servicioDialogos.MensajeInformativo("OK", "Reserva creada");
                    }

                }
                else { servicioDialogos.MensajeError("ERROR", "Error al cargar los datos del libro"); }
            }
            else
            {
                servicioDialogos.MensajeError("ERROR", "Error al cargar los datos de la reserva");
            }
        }
    }
}
