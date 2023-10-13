using BiblioAppEscritorio.ViewModel;
using BiblioAppEscritorio.Vistas;
using BiblioAppEscritorio.Vistas.Eventos;
using BiblioAppEscritorio.Vistas.Libros;
using BiblioAppEscritorio.Vistas.Socio;
using BiblioAppEscritorio.Vistas.VentanaLibros;
using BiblioAppEscritorio.Vistas.VentanaPrestamoInterbibliotecario;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BiblioAppEscritorio.Servicios
{
    /// <summary>
    /// El servicio ServicioDialogos nos permite usar los diálogos mas necesitados en nuestro programación.
    /// </summary>
    class ServicioDialogos
    {
        /// <summary>
        /// La función AbrirArchivoDialogo nos ayudará a saber que archivo elige el usuario.
        /// </summary>
        /// <param name="filtro">Este parámetro lo utilizamos para indicarle que tipos de archivos puede elegir el usuario.</param>
        /// <returns>Ruta del archivo elegido por el usuario.</returns>
        public string AbrirArchivoDialogo(string filtro)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = filtro
            };
            openFileDialog.ShowDialog();

            return openFileDialog.FileName;
        }

        /// <summary>
        /// Esta función nos ayudará a indicarle al usuario de una forma simple un error.
        /// </summary>
        /// <param name="tituloMessageBox">Título tendrá la ventana emergente.</param>
        /// <param name="mensajeError">Error a indicar.</param>
        public void MensajeError(string tituloMessageBox, string mensajeError)
        {
            MessageBox.Show(mensajeError, tituloMessageBox, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Esta función nos ayudará a indicarle al usuario de una forma simple un mensaje.
        /// </summary>
        /// <param name="tituloMessageBox">Título tendrá la ventana emergente.</param>
        /// <param name="mensajeError">Mensaje a indicar.</param>
        public void MensajeInformativo(string tituloMessageBox, string mensajeInformativo)
        {
            MessageBox.Show(mensajeInformativo, tituloMessageBox, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// La función AbrirDialogoEliminarSocio es un diálogo personalizado para eliminar socios.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoEliminarSocio()
        {
            DialogoEliminarSocio dialogo = new DialogoEliminarSocio();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoEliminarLibro es un diálogo personalizado para eliminar libros.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoEliminarLibro()
        {
            DialogoEliminarLibro dialogo = new DialogoEliminarLibro();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoFinalizaPrestamo es un diálogo personalizado para finalizar préstamo.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoFinalizaPrestamo()
        {
            DialogoFinalizarPrestamo dialogo = new DialogoFinalizarPrestamo();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoFinalizaReserva es un diálogo personalizado para finalizar reserva.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoFinalizaReserva()
        {
            DialogoFinalizarReserva dialogo = new DialogoFinalizarReserva();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoEliminarPrestamo es un diálogo personalizado para eliminar préstamo.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoEliminarPrestamo()
        {
            DialogoEliminarPrestamo dialogo = new DialogoEliminarPrestamo();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoEliminarReserva es un diálogo personalizado para eliminar reserva.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoEliminarReserva()
        {
            DialogoEliminarReserva dialogo = new DialogoEliminarReserva();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoEliminarEvento es un diálogo personalizado para eliminar evento.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoEliminarEvento()
        {
            DialogoEliminarEvento dialogo = new DialogoEliminarEvento();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoEditarEvento es un diálogo personalizado para editar evento.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoEditarEvento()
        {
            DialogoEditarEvento dialogo = new DialogoEditarEvento();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoAddPrestamo es un diálogo personalizado para añadir préstamo.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoAddPrestamo()
        {
            DialogoCreaPrestamo dialogo = new DialogoCreaPrestamo();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoAddReserva es un diálogo personalizado para añadir reserva.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoAddReserva()
        {
            DialogoCreaReserva dialogo = new DialogoCreaReserva();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoRestablecePass es un diálogo personalizado para reestablecer contraseña.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoRestablecePass()
        {
            DialogoRestablecerPass dialogo = new DialogoRestablecerPass();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoAddLibro es un diálogo personalizado para añadir libro.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoAddLibro()
        {
            DialogoCreaLibro dialogo = new DialogoCreaLibro();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoEditarLibro es un diálogo personalizado para editar libro.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoEditarLibro()
        {
            DialogoEditaLibro dialogo = new DialogoEditaLibro();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoEliminarPrestamoInter es un diálogo personalizado para eliminar préstamo interbibliotecario.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoEliminarPrestamoInter()
        {
            DialogoEliminarPrestamoInterbibliotecario dialogo = new DialogoEliminarPrestamoInterbibliotecario();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoFinalizarPrestamoInter es un diálogo personalizado para finalizar préstamo interbibliotecario.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoFinalizarPrestamoInter()
        {
            DialogoFinalizarPrestamoInterbibliotecario dialogo = new DialogoFinalizarPrestamoInterbibliotecario();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función AbrirDialogoDialogoSolicitarLibro es un diálogo personalizado para solicitar libro.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? AbrirDialogoDialogoSolicitarLibro()
        {
            DialogoSolicitarLibro dialogo = new DialogoSolicitarLibro();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función CargarDialogoAddSocio es un diálogo personalizado para finalizar añadir socio.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? CargarDialogoAddSocio()
        {
            DialogoAddSocio dialogo = new DialogoAddSocio();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función CargarDialogoEditaSocio es un diálogo personalizado para finalizar editar socio.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? CargarDialogoEditaSocio()
        {
            EditarSocioWindow dialogo = new EditarSocioWindow();
            return dialogo.ShowDialog();
        }

        /// <summary>
        /// La función CargarVentanaAddEvento es un diálogo personalizado para finalizar añadir evento.
        /// </summary>
        /// <returns>Boolean según le ha dado a aceptar o cancelar.</returns>
        public bool? CargarVentanaAddEvento()
        {
            AddEventoWindow dialogo = new AddEventoWindow();
            return dialogo.ShowDialog();
        }
    }
}
