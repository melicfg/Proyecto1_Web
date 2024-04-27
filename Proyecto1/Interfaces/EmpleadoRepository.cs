using Microsoft.EntityFrameworkCore;
using Proyecto1.Models;

namespace Proyecto1.Interfaces
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        public EmpleadoRepository()
        {
            using (var context = new ApiContext())
            {
                if (context.Empleados.Count() == 0)
                {
                    var empleado = new List<Empleado>
                    {
                        new Empleado
                        (
                            1,
                            DateTime.Now, 
                            "John",
                            "Doe",
                            new DateTime(1990, 5, 15), 
                            123456789,
                            "123 Main St", 
                            "john@example.com", 
                            "555-123-4567", 
                            "Emergency Contact" 
                        )
                    };
                    context.Empleados.AddRange(empleado);
                    context.SaveChanges();
                }

            }
        }
        public List<Empleado> GetEmpleados()
        {
            using (var context = new ApiContext())
            {
                var list = context.Empleados
                    .ToList();
                return list;
            }
        }

        public Empleado GetEmpleadoId(int id)
        {
            using (var context = new ApiContext())
            {
                List<Empleado> listaEmpleado;
                listaEmpleado = GetEmpleados();
                var empleado = listaEmpleado
                    .FirstOrDefault(a => a.numeroEmpleado == id);
                return empleado;
            }

        }

        public void PostEmpleado(Empleado empleado)
        {
            using (var context = new ApiContext())
            {
                context.Empleados.AddRange(empleado);
                context.SaveChanges();
            }


        }

        public void EditEmpleado(int id, Empleado empleado)
        {
            using (var context = new ApiContext())
            {
                Empleado empleadoOriginal = GetEmpleadoId(id);

                if (empleadoOriginal != null)
                {
                    // Actualizar las propiedades del empleado original con las del empleado recibido
                    context.Entry(empleadoOriginal).CurrentValues.SetValues(empleado);
                    context.Entry(empleadoOriginal).State = EntityState.Modified;
                    // Guardar los cambios en la base de datos
                    context.SaveChanges();
                }

            }
        }

        public void DeleteEmpleado(Empleado empleado)
        {
            using (var context = new ApiContext())
            {

                context.Empleados.Remove(empleado);

                context.SaveChanges();

            }

        }

    }
}
