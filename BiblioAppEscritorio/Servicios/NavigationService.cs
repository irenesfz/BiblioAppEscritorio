using BiblioAppEscritorio.Vistas;
using BiblioAppEscritorio.Vistas.VentanaPrestamoInterbibliotecario;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BiblioAppEscritorio
{
    /// <summary>
    /// Clase que se encarga de la navegación de ventanas y usercontrol
    /// </summary>
    class NavigationService
    {
        //para no cargar los datos cada vez que llame al user
        static readonly LibrosUserControl listaLibros = new LibrosUserControl();
        static readonly SociosUserControl listaSocios = new SociosUserControl();
        static readonly ReservasUserControl listaReservas = new ReservasUserControl();
        static readonly EventosUserControl listaEventos = new EventosUserControl();
        static readonly PrestamosUserControl listaPrestamos = new PrestamosUserControl();
        static readonly InicioUserControl inicio = new InicioUserControl();
        static readonly PrestamoInterbibliotecarioUserControl interbibliotecario = new PrestamoInterbibliotecarioUserControl();
        static readonly RegistroPrestamoInterbiblitoecario registroInterbibliotecario = new RegistroPrestamoInterbiblitoecario();

        /// <summary>
        /// Abre el UserControl LibrosUserControl
        /// </summary>
        /// <returns>El UserControl a abrir.</returns>
        public UserControl AbrirLibrosUserControl()
        {
            return listaLibros;
        }

        /// <summary>
        /// Abre el UserControl SociosUserControl
        /// </summary>
        /// <returns>El UserControl a abrir.</returns>
        public UserControl AbrirSociosUserControl()
        {
            return listaSocios;
        }

        /// <summary>
        /// Abre el UserControl EventosUserControl
        /// </summary>
        /// <returns>El UserControl a abrir.</returns>
        public UserControl AbrirEventosUserControl()
        {
            return listaEventos;
        }

        /// <summary>
        /// Abre el UserControl ReservasUserControl
        /// </summary>
        /// <returns>El UserControl a abrir.</returns>
        public UserControl AbrirReservasUserControl()
        {
            return listaReservas;
        }

        /// <summary>
        /// Abre el UserControl PrestamosUserControl
        /// </summary>
        /// <returns>El UserControl a abrir.</returns>
        public UserControl AbrirPrestamosUserControl()
        {
            return listaPrestamos;
        }

        /// <summary>
        /// Abre el UserControl InicioUserControl
        /// </summary>
        /// <returns>El UserControl a abrir.</returns>
        public UserControl AbrirInicioUserControl()
        {
            return inicio;
        }

        /// <summary>
        /// Abre el UserControl PrestamoInterbibliotecarioUserControl para buscar libros.
        /// </summary>
        /// <returns>El UserControl a abrir.</returns>
        public UserControl AbrirPrestamoInterbibliotecarioUserControl()
        {
            return interbibliotecario;
        }

        /// <summary>
        /// Abre el UserControl PrestamoInterbiblitoecario con el registro de los préstamos.
        /// </summary>
        /// <returns>El UserControl a abrir.</returns>
        public UserControl AbrirRegistroPrestamoInterbiblitoecario()
        {
            return registroInterbibliotecario;
        }
    }
}
