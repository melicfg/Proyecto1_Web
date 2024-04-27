using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Proyecto1.Interfaces;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class TiqueteController : Controller
    {
        private int duracionMinutos;

        readonly ITiqueteRepository _tiqueteRepository;
        readonly IFacturaRepository _facturaRepository;
        readonly IParqueoRepository _parqueoRepository;


        public TiqueteController(ITiqueteRepository tiqueteRepository, IFacturaRepository facturaRepository, IParqueoRepository parqueoRepository)
        {
            _tiqueteRepository = tiqueteRepository;
            _facturaRepository = facturaRepository;
            _parqueoRepository = parqueoRepository;
        }


        // GET: TiqueteController
        public ActionResult Index(int? id)
        {
            List<Parqueo> listaParqueos = _parqueoRepository.GetParqueos();
            ViewBag.parqueos = listaParqueos;
            List<Tiquete> listaTiquetes;
            listaTiquetes = _tiqueteRepository.GetTiquetes();

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
            Tiquete tiquete = _tiqueteRepository.GetTiqueteId(id);
            return View(tiquete);
        }

        // GET: TiqueteController/Create
        public ActionResult Create()
        {
            List<Parqueo> listaParqueos = _parqueoRepository.GetParqueos();
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
                if (_tiqueteRepository.GetTiquetes().Any(p => p.id.Equals(nuevoTiquete.id)))
                {
                    ModelState.AddModelError("id", "El ID ya se encuentra en uso");
                    List<Parqueo> listaParqueos = _parqueoRepository.GetParqueos();
                    ViewBag.idParqueo = new SelectList(listaParqueos, "idParqueo", "nombre");
                    return View(nuevoTiquete);
                }
                else
                {
                    _tiqueteRepository.PostTiquete(nuevoTiquete);
                    List<Parqueo> listaParqueos = _parqueoRepository.GetParqueos();
                    ViewBag.idParqueo = new SelectList(listaParqueos, "idParqueo", "nombre");
                    return RedirectToAction(nameof(Index));
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: TiqueteController/Edit/5
        public ActionResult Edit(int id)
        {

            Tiquete tiquete = _tiqueteRepository.GetTiqueteId(id);
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
                    List<Parqueo> listaParqueos = _parqueoRepository.GetParqueos();
                    ViewBag.idParqueo = new SelectList(listaParqueos, "idParqueo", "nombre");

                    return View(nuevoTiquete); // Devolver la vista con el mensaje de error
                }
                else
                {
                    List<Factura> listaFacturas = _facturaRepository.GetFacturas();
                    _tiqueteRepository.EditTiquete(id, nuevoTiquete);

                    // Verifica si la factura existe en facturas


                    int facturaIndice = listaFacturas.FindIndex(f => f.idTiquete == id);

                    if (facturaIndice >= 0)
                    {
                        Factura factura = listaFacturas.FirstOrDefault(f => f.idTiquete == id);
                        _facturaRepository.EditFactura(nuevoTiquete, factura);
                    }
                    else
                    {
                        _facturaRepository.PostFactura(nuevoTiquete);
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
            Tiquete tiquete = _tiqueteRepository.GetTiqueteId(id);
            return View(tiquete);
        }

        // POST: TiqueteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tiquete tiquete)
        {
            try
            {
                _tiqueteRepository.DeleteTiquete(tiquete);
                _facturaRepository.DeleteFactura(id);
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
