using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class TiqueteController : Controller
    {
        public static List<Tiquete> tiquetes = new List<Tiquete>();
        private int duracionMinutos;

        private readonly IMemoryCache _cache;
        private readonly FacturaController _facturaController;
        public TiqueteController(IMemoryCache cache, FacturaController facturaController)
        {
            _cache = cache;
            _facturaController = facturaController;
        }

        private List<Tiquete> ObtenerTiquetes()
        {
            List<Tiquete> listaTiquetes;

            if (_cache.Get("ListaTiquetes") is null)
            {
                listaTiquetes = new List<Tiquete>();
                _cache.Set("ListaTiquetes", listaTiquetes);
            }
            else
            {

                listaTiquetes = (List<Tiquete>)_cache.Get("ListaTiquetes");
            }

            return listaTiquetes;
        }

        private Tiquete ObtenerTiqueteId(int id)
        {

            List<Tiquete> listaTiquetes;

            listaTiquetes = ObtenerTiquetes();
            foreach (var tiquete in listaTiquetes)
            {
                if (tiquete.id == id)
                    return tiquete;
            }

            return null;
        }

        private void Guardar(Tiquete tiquete)
        {
            List<Tiquete> ListaTiquetes;

            ListaTiquetes = ObtenerTiquetes();

            ListaTiquetes.Add(tiquete);
        }

        private void Editar(Tiquete tiquete)
        {
            Tiquete tiqueteOriginal = ObtenerTiqueteId(tiquete.id);
            List<Tiquete> ListaTiquetes;
            ListaTiquetes = ObtenerTiquetes();


            int indice = ListaTiquetes.IndexOf(tiqueteOriginal);

            ListaTiquetes[indice] = tiquete;

            _cache.Set("ListaTiquetes", ListaTiquetes);
        }

        private void Eliminar(Tiquete tiquete)
        {
            List<Tiquete> ListaTiquetes;

            ListaTiquetes = ObtenerTiquetes();

            ListaTiquetes.Remove(tiquete);
        }

        // GET: TiqueteController
        public ActionResult Index(int? id)
        {
            List<Parqueo> listaParqueos = _cache.Get<List<Parqueo>>("ListaParqueos");
            ViewBag.parqueos = listaParqueos;
            List<Tiquete> listaTiquetes;
            listaTiquetes = ObtenerTiquetes();

            // Filtrar los tiquetes si se proporciona un ID en la consulta
            if (id.HasValue)
            {
                listaTiquetes = listaTiquetes.Where(t => t.id == id.Value).ToList();
            }

            
            return View(listaTiquetes);
        }

        // GET: TiqueteController/Details/5
        public ActionResult Details(int id)
        {
            Tiquete tiquete = ObtenerTiqueteId(id);
            return View(tiquete);
        }

        // GET: TiqueteController/Create
        public ActionResult Create()
        {
            List<Parqueo> listaParqueos = _cache.Get<List<Parqueo>>("ListaParqueos");
            if (listaParqueos == null)
            {
                // Si listaParqueos es nula, asignar una lista vacía para evitar el error
                listaParqueos = new List<Parqueo>();
            }
            ViewBag.idParqueo = new SelectList(listaParqueos, "idParqueo", "nombre");

            return View();
        }

        // POST: TiqueteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tiquete nuevoTiquete)
        {
            try
            {
                Guardar(nuevoTiquete);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TiqueteController/Edit/5
        public ActionResult Edit(int id)
        {

            Tiquete tiquete = ObtenerTiqueteId(id);
            return View(tiquete);
        }


        // POST: TiqueteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tiquete nuevoTiquete)
        {

            try
            {

                if (nuevoTiquete.salida <= nuevoTiquete.ingreso)
                {
                    // Agregar un error de modelo para mostrar un mensaje en la vista
                    ModelState.AddModelError("salida", "La hora de salida debe ser después de la hora de entrada.");
                    List<Parqueo> listaParqueos = _cache.Get<List<Parqueo>>("ListaParqueos");
                    ViewBag.idParqueo = new SelectList(listaParqueos, "idParqueo", "nombre");

                    return View(nuevoTiquete); // Devolver la vista con el mensaje de error
                }
                else
                {
                    List<Factura> listaFacturas = _facturaController.ObtenerFacturas();
                    Editar(nuevoTiquete);

                    // Verifica si la factura existe en facturas


                    int facturaIndice = listaFacturas.FindIndex(f => f.idTiquete == id);
                    if (facturaIndice >= 0)
                    {
                        _facturaController.Editar(nuevoTiquete, facturaIndice);
                    }
                    else
                    {
                        _facturaController.Guardar(nuevoTiquete);
                    }

                    return RedirectToAction(nameof(Index));

                }

            }
            catch
            {
                return View();
            }
        }

        // GET: TiqueteController/Delete/5
        public ActionResult Delete(int id)
        {
            Tiquete tiquete = ObtenerTiqueteId(id);
            return View(tiquete);
        }

        // POST: TiqueteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tiquete tiquete)
        {
            try
            {
                Eliminar(tiquete);
                _facturaController.Eliminar(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //public ActionResult TiquetesPorParqueo(int idParqueo)
        //{

        //    var tiquetesPorParqueo = tiquetes.Where(t => t.idParqueo == idParqueo).ToList();
        //    ViewBag.Parqueos = new SelectList(ParqueoController.parqueos, "idParqueo", "nombre");

        //    return View(tiquetesPorParqueo);
        //}
    }
}
