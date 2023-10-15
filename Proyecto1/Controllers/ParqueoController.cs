using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class ParqueoController : Controller
    {

        private readonly IMemoryCache _cache;

        public ParqueoController(IMemoryCache cache)
        {
            _cache = cache;

            // Crear un parqueo predeterminado y guardarlo en la caché si no existe
            if (_cache.Get("ListaParqueos") is null)
            {
                var listaParqueos = new List<Parqueo>
                {
                    new Parqueo
                    (
                        1,
                        "Parqueo Ejemplo",
                        100,
                        "08:00",
                        "18:00",
                        10,
                        5
                    )
                };

                _cache.Set("ListaParqueos", listaParqueos);
            }
        }

        private List<Parqueo> ObtenerParqueos()
        {
            List<Parqueo> listaParqueos;

            if (_cache.Get("ListaParqueos") is null)
            {
                listaParqueos = new List<Parqueo>();
                _cache.Set("ListaParqueos", listaParqueos);
            }
            else
            {

                listaParqueos = (List<Parqueo>)_cache.Get("ListaParqueos");
            }

            return listaParqueos;
        }

        private Parqueo ObtenerParqueoId(int id)
        {

            List<Parqueo> listaParqueos;

            listaParqueos = ObtenerParqueos();
            foreach (var parqueo in listaParqueos)
            {
                if (parqueo.idParqueo == id)
                    return parqueo;
            }

            return null;
        }

        private void Guardar(Parqueo parqueo)
        {
            List<Parqueo> listaParqueos;

            listaParqueos = ObtenerParqueos();

            listaParqueos.Add(parqueo);
        }

        private void Editar(Parqueo parqueo)
        {
            Parqueo parqueoOriginal = ObtenerParqueoId(parqueo.idParqueo);
            List<Parqueo> listaParqueos;
            listaParqueos = ObtenerParqueos();
            

            int indice = listaParqueos.IndexOf(parqueoOriginal);

            listaParqueos[indice] = parqueo;

            _cache.Set("ListaParqueos", listaParqueos);
        }

        private void Eliminar(Parqueo parqueo)
        {
            List<Parqueo> listaParqueos;

            listaParqueos = ObtenerParqueos();

            listaParqueos.Remove(parqueo);
        }

        // GET: ParqueoController
        public ActionResult Index(int? id)
        {
            List<Parqueo> listaParqueos;

            listaParqueos = ObtenerParqueos();

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
            Parqueo parqueo = ObtenerParqueoId(id);

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
            try
            {
                Guardar(parqueo);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ParqueoController/Edit/5
        public ActionResult Edit(int id)
        {
            Parqueo parqueo = ObtenerParqueoId(id);
            return View(parqueo);
        }

        // POST: ParqueoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Parqueo parqueo)
        {
            try
            {
                Editar(parqueo);
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
            Parqueo parqueo = ObtenerParqueoId(id);
            return View(parqueo);
        }

        // POST: ParqueoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            

            try
            {
                Parqueo parqueo = ObtenerParqueoId(id);
                Eliminar(parqueo);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
