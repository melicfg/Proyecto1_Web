using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Proyecto1.Interfaces;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: EmpleadoController

        readonly IEmpleadoRepository _empleadoRepository;
        public EmpleadoController(IEmpleadoRepository empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        public ActionResult Index(int? id)
        {
            List<Empleado> listaEmpleados;
            listaEmpleados = _empleadoRepository.GetEmpleados();
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
            Empleado empleado = _empleadoRepository.GetEmpleadoId(id);
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
                _empleadoRepository.PostEmpleado(nuevoEmpleado);
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
            Empleado empleado = _empleadoRepository.GetEmpleadoId(id);
            return View(empleado);
        }

        // POST: EmpleadoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Empleado nuevoEmpleado)
        {
            try
            {

                _empleadoRepository.EditEmpleado(id, nuevoEmpleado);
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
            Empleado empleado = _empleadoRepository.GetEmpleadoId(id);
            
            return View(empleado);
        }

        // POST: EmpleadoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Empleado empleado = _empleadoRepository.GetEmpleadoId(id);
                _empleadoRepository.DeleteEmpleado(empleado);
                    
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
