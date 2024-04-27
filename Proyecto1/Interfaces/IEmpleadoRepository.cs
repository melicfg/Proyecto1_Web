using Proyecto1.Models;

namespace Proyecto1.Interfaces
{
    public interface IEmpleadoRepository
    {
        public List<Empleado> GetEmpleados();
        public Empleado GetEmpleadoId(int id);
        public void PostEmpleado(Empleado empleado);
        public void EditEmpleado(int id, Empleado empleado);
        public void DeleteEmpleado(Empleado empleado);
    }
}
