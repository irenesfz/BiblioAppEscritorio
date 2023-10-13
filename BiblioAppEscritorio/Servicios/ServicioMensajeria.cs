using BiblioAppEscritorio.Clases;
using BiblioAppEscritorio.ExternalApi;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace BiblioAppEscritorio.Servicios
{
    /// <summary>
    /// Clase de mensajería para enviar un socio seleccionado de una ventana a otra.
    /// </summary>
    class SocioSeleccionadoMessage : RequestMessage<Socio> { }
    /// <summary>
    /// Clase de mensajería para enviar un libro seleccionado de una ventana a otra.
    /// </summary>
    class LibroSeleccionadoMessage : RequestMessage<Libros> { }
    /// <summary>
    /// Clase de mensajería para enviar un préstamo seleccionado de una ventana a otra.
    /// </summary>
    class PrestamoSeleccionadoMessage : RequestMessage<Prestamo> { }
    /// <summary>
    /// Clase de mensajería para enviar un préstamo seleccionado de una ventana a otra.
    /// </summary>
    class PrestamoGridPrestamosSeleccionadoMessage : RequestMessage<GridPrestamos> { }
    /// <summary>
    /// Clase de mensajería para enviar una reserva seleccionada de una ventana a otra.
    /// </summary>
    class ReservaSeleccionadaMessage : RequestMessage<Reserva> { }
    /// <summary>
    /// Clase de mensajería para enviar una reserva seleccionada de una ventana a otra.
    /// </summary>
    class GridReservasSeleccionadaMessage : RequestMessage<GridReservas> { }
    /// <summary>
    /// Clase de mensajería para enviar el evento seleccionado de una ventana a otra.
    /// </summary>
    class EventoSeleccionadoMessage : RequestMessage<Evento> { }
    /// <summary>
    /// Clase de mensajería para enviar préstamo interbibliotecario seleccionado de una ventana a otra.
    /// </summary>
    class PrestamoInterbibliotecarioMessage : RequestMessage<Item> { }
    /// <summary>
    /// Clase de mensajería para enviar el registro del préstamo interbibliotecario seleccionado de una ventana a otra.
    /// </summary>
    class PrestamoInterbibliotecarioRegistroMessage : RequestMessage<PrestamoInterbibliotecario> { }

    /// <summary>
    /// Clase de mensajería para actualizar la lista de unidades de libros.
    /// </summary>
    class ActualizarLibroSeleccionadoMessage : ValueChangedMessage<Libros>
    {
        public ActualizarLibroSeleccionadoMessage(Libros valor) : base(valor) { }
    }

    /// <summary>
    /// Clase de mensajería para actualizar la lista de préstamos interbibliotecarios.
    /// </summary>
    class ActualizarPrestamoInterbibliotecarioMessage : ValueChangedMessage<PrestamoInterbibliotecario>
    {
        public ActualizarPrestamoInterbibliotecarioMessage(PrestamoInterbibliotecario valor) : base(valor) { }
    }

    /// <summary>
    /// Clase de mensajería para actualizar la lista de socios.
    /// </summary>
    class ActualizarGridSociosMessage : ValueChangedMessage<Socio>
    {
        public ActualizarGridSociosMessage(Socio valor) : base(valor) { }
    }

    /// <summary>
    /// Clase de mensajería para actualizar la lista de libros.
    /// </summary>
    class ActualizarGridLibrosMessage : ValueChangedMessage<Libros>
    {
        public ActualizarGridLibrosMessage(Libros valor) : base(valor) { }
    }

    /// <summary>
    /// Clase de mensajería para actualizar la lista de eventos.
    /// </summary>
    class ActualizarGridEventoMessage : ValueChangedMessage<Evento>
    {
        public ActualizarGridEventoMessage(Evento valor) : base(valor) { }
    }

    /// <summary>
    /// Clase de mensajería para actualizar la lista de reservas.
    /// </summary>
    class ActualizarGridReservasMessage : ValueChangedMessage<GridReservas>
    {
        public ActualizarGridReservasMessage(GridReservas valor) : base(valor) { }
    }

    /// <summary>
    /// Clase de mensajería para actualizar la lista de préstamos.
    /// </summary>
    class ActualizarGridPrestamosMessage : ValueChangedMessage<GridPrestamos>
    {
        public ActualizarGridPrestamosMessage(GridPrestamos valor) : base(valor) { }
    }
}
