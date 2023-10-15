using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: EmpleadoController
        public static List<Empleado> empleados = new List<Empleado>();

        private readonly IMemoryCache _cache;
        public EmpleadoController(IMemoryCache cache)
        {
            _cache = cache;
        }

        private List<Empleado> ObtenerEmpleado()
        {
            List<Empleado> listaEmpleados;

            if (_cache.Get("ListaEmpleados") is null)
            {
                listaEmpleados = new List<Empleado>();
                _cache.Set("ListaEmpleados", listaEmpleados);
            }
            else
            {

                listaEmpleados = (List<Empleado>)_cache.Get("ListaEmpleados");
            }

            return listaEmpleados;
        }

        private Empleado ObtenerEmpleadoId(int id)
        {

            List<Empleado> listaEmpleados;

            listaEmpleados = ObtenerEmpleado();
            foreach (var empleado in listaEmpleados)
            {
                if (empleado.numeroEmpleado == id)
                    return empleado;
            }

            return null;
        }

        private void Guardar(Empleado empleado)
        {
            List<Empleado> ListaEmpleados;

            ListaEmpleados = ObtenerEmpleado();

            ListaEmpleados.Add(empleado);
        }

        private void Editar(Empleado empleado)
        {
            Empleado empleadoOriginal = ObtenerEmpleadoId(empleado.numeroEmpleado);
            List<Empleado>listaEmpleados;
            listaEmpleados = ObtenerEmpleado();


            int indice = listaEmpleados.IndexOf(empleadoOriginal);

            listaEmpleados[indice] = empleado;

            _cache.Set("ListaEmpleados", listaEmpleados);
        }

        private void Eliminar(Empleado empleado)
        {
            List<Empleado> listaEmpleado;

            listaEmpleado = ObtenerEmpleado();

            listaEmpleado.Remove(empleado);
        }
        public ActionResult Index(int? id)
        {
            List<Empleado> listaEmpleados;
            listaEmpleados = ObtenerEmpleado();
            // Filtrar los tiquetes si se proporciona un ID en la consulta
            if (id.HasValue)
            {
                listaEmpleados = listaEmpleados.Where(t => t.numeroEmpleado == id.Value).ToList();
            }
            return View(listaEmpleados);
        }

        // GET: EmpleadoController/Details/5
        public ActionResult Details(int id)
        {
            Empleado empleado = ObtenerEmpleadoId(id);
            return View(empleado);
        }

        // GET: EmpleadoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpleadoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empleado nuevoEmpleado)
        {
            try
            {
                Guardar(nuevoEmpleado);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmpleadoController/Edit/5
        public ActionResult Edit(int id)
        {
            Empleado empleado = ObtenerEmpleadoId(id);
            return View(empleado);
        }

        // POST: EmpleadoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Empleado nuevoEmpleado)
        {
            try
            {

                Editar(nuevoEmpleado);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmpleadoController/Delete/5
        public ActionResult Delete(int id)
        {
            Empleado empleado = ObtenerEmpleadoId(id);
            
            return View(empleado);
        }

        // POST: EmpleadoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Empleado empleado = ObtenerEmpleadoId(id);
                Eliminar(empleado);
                    
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
