using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Proyecto1.Models
{
    public class Empleado
    {
        public Empleado() { }
        public Empleado(int numeroEmpleado, DateTime ingreso, string nombre, string apellidos, DateTime nacimiento, int cedula, string direccion, string email, string telefono, string contacto) 
        {
            this.numeroEmpleado = numeroEmpleado;
            this.ingreso = ingreso;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.nacimiento = nacimiento;
            this.cedula = cedula;
            this.direccion = direccion;
            this.email = email;
            this.telefono = telefono; 
            this.contacto = contacto;
  
        }
        [Key]
        public int numeroEmpleado { get; set; }

        public string nombre { get; set; }

        public string apellidos { get; set; }

        public DateTime ingreso { get; set; }

        public DateTime nacimiento { get; set; }

        public int cedula { get; set; }

        public string direccion { get; set; }

        public string email { get; set; }

        public string telefono { get; set; }

        public string contacto { get; set; }

    }
}
