using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Proyecto1.Interfaces;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class ParqueoController : Controller
    {
        readonly IParqueoRepository _parqueoRepository;

        public ParqueoController(IParqueoRepository parqueoRepository)
        {
            _parqueoRepository = parqueoRepository;
        }


        // GET: ParqueoController
        public ActionResult Index(int? id)
        {
            List<Parqueo> listaParqueos;

            listaParqueos = _parqueoRepository.GetParqueos();

            // Filtrar los tiquetes si se proporciona un ID en la consulta
            if (id.HasValue)
            {
                listaParqueos = listaParqueos.Where(t => t.idParqueo == id.Value).ToList();
            }

            return View(listaParqueos);
        }

        // GET: ParqueoController/Details/5
        public ActionResult Details(int id)
        {
            Parqueo parqueo = _parqueoRepository.GetParqueoId(id);

            return View(parqueo);
        }

        // GET: ParqueoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParqueoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Parqueo parqueo)
        {
            if (_parqueoRepository.GetParqueos().Any(p => p.idParqueo.Equals(parqueo.idParqueo)))
            {
                ModelState.AddModelError("idParqueo", "El ID ya se encuentra en uso");
                return View(parqueo);
            }
            else {
                _parqueoRepository.PostParqueo(parqueo);
                return RedirectToAction("Create");
            }
        }

        // GET: ParqueoController/Edit/5
        public ActionResult Edit(int id)
        {
            Parqueo parqueo = _parqueoRepository.GetParqueoId(id);
            return View(parqueo);
        }

        // POST: ParqueoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Parqueo parqueo)
        {
            try
            {
                _parqueoRepository.EditParqueo(id, parqueo);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ParqueoController/Delete/5
        public ActionResult Delete(int id)
        {
            Parqueo parqueo = _parqueoRepository.GetParqueoId(id);
            return View(parqueo);
        }

        // POST: ParqueoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            

            try
            {
                Parqueo parqueo = _parqueoRepository.GetParqueoId(id);
                _parqueoRepository.DeleteParqueo(parqueo);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
