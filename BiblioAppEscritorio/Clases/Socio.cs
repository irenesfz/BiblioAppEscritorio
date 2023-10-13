using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioAppEscritorio.Clases
{
    class Socio
    {
        public string apellidos { get; set; }
        public string categoriasInteres { get; set; }
        public string contrasenya { get; set; }
        public string correo { get; set; }
        public string direccion { get; set; }
        public string dni { get; set; }
        public int idSocio { get; set; }
        public string imagen { get; set; }
        public string nombre { get; set; }
        public string rol { get; set; }
        public string fechaNacimiento { get; set; }
        public int telefono { get; set; }

        public Socio()
        {

        }

        public Socio(string apellidos, string categoriasInteres, 
            string contrasenya, string correo, string direccion, 
            string dni, int idSocio, string imagen, string nombre, 
            string rol, string fechaNacimiento, int telefono)
        {
            this.apellidos = apellidos;
            this.categoriasInteres = categoriasInteres;
            this.contrasenya = contrasenya;
            this.correo = correo;
            this.direccion = direccion;
            this.dni = dni;
            this.idSocio = idSocio;
            this.imagen = imagen;
            this.nombre = nombre;
            this.rol = rol;
            this.fechaNacimiento = fechaNacimiento;
            this.telefono = telefono;
        }
        public Socio(string apellidos, 
        string contrasenya, string correo, string direccion,
        string dni, int idSocio, string imagen, string nombre,
        string rol, string fechaNacimiento, int telefono)
            {
                this.apellidos = apellidos;
                this.contrasenya = contrasenya;
                this.correo = correo;
                this.direccion = direccion;
                this.dni = dni;
                this.idSocio = idSocio;
                this.imagen = imagen;
                this.nombre = nombre;
                this.rol = rol;
                this.fechaNacimiento = fechaNacimiento;
                this.telefono = telefono;
            }
    }
}
