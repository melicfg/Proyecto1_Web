using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class FacturaController : Controller
    {
        private static int _id = 0;
        private static double _total = 0;

        private readonly IMemoryCache _cache;
        public FacturaController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public List<Factura> ObtenerFacturas()
        {
            List<Factura> listaFacturas;

            if (_cache.Get("ListaFacturas") is null)
            {
                listaFacturas = new List<Factura>();
                _cache.Set("ListaFacturas", listaFacturas);
            }
            else
            {

                listaFacturas = (List<Factura>)_cache.Get("ListaFacturas");
            }

            return listaFacturas;
        }

        public Factura ObtenerFacturaId(int id)
        {

            List<Factura> listaFacturas;

            listaFacturas = ObtenerFacturas();
            foreach (var factura in listaFacturas)
            {
                if (factura.idTiquete == id)
                    return factura;
            }

            return null;
        }

        public void Guardar(Tiquete tiquete)
        {
            List<Factura> listaFacturas;
            double valor = calcularValor(tiquete);
            listaFacturas = ObtenerFacturas();
            Factura nuevaFactura = new Factura(
                _id, 
                tiquete.id, 
                tiquete.idParqueo, 
                valor,
                tiquete.salida?? DateTime.Now);

            listaFacturas.Add(nuevaFactura);

            _id++;
        }

        public void Editar(Tiquete tiquete, int indice)
        {

            List<Factura> listaFacturas;
            listaFacturas = ObtenerFacturas();

            double nuevoValor = calcularValor(tiquete);
            listaFacturas[indice].fecha = tiquete.salida ?? listaFacturas[indice].fecha;
            listaFacturas[indice].valor = nuevoValor;

            _cache.Set("ListaFacturas", listaFacturas);
        }

        public void Eliminar(int idTiquete)
        {
            List<Factura> listaFacturas;

            listaFacturas = ObtenerFacturas();

            Factura factura = ObtenerFacturaId(idTiquete);

            listaFacturas.Remove(factura);
        }

        // GET: FacturaController
        public ActionResult Index(int? idParqueo)
        {
            List<Parqueo> listaParqueos = _cache.Get<List<Parqueo>>("ListaParqueos");
            ViewBag.parqueos = listaParqueos;
            List<Factura> listaFacturas;
            listaFacturas = ObtenerFacturas();
            Parqueo parqueoSeleccionado = idParqueo.HasValue ? listaParqueos.FirstOrDefault(p => p.idParqueo == idParqueo) : null;

            // Obtener la lista de facturas para el parqueo seleccionado (o todas las facturas si parqueoId es null)
            List<Factura> listaFacturasSelec = parqueoSeleccionado != null
                ? listaFacturas.Where(f => f.idParqueo == parqueoSeleccionado.idParqueo).ToList()
                : listaFacturas;

            ViewBag.facturas = listaFacturasSelec;
            // Calcular el total de las facturas
            _total = listaFacturasSelec.Sum(f => f.valor);

            ViewBag.Total = _total;
            ViewBag.parqueo = parqueoSeleccionado;
            return View(listaFacturas);
        }

        // GET: FacturaController/Details/5
        public ActionResult Details(int id)
        {
            List<Parqueo> listaParqueos = _cache.Get<List<Parqueo>>("ListaParqueos");

            Factura factura = ObtenerFacturaId(id);
            Parqueo parqueo = listaParqueos.FirstOrDefault(p => p.idParqueo == factura.idParqueo);
            ViewBag.parqueo = parqueo;
            return View(factura);
        }

        // GET: FacturaController/Create

        public double calcularValor(Tiquete tiquete)
        {
            List<Parqueo> listaParqueos = _cache.Get<List<Parqueo>>("ListaParqueos");
            Parqueo parqueo = listaParqueos.FirstOrDefault(p => p.idParqueo == tiquete.idParqueo);
            TimeSpan duracion = (TimeSpan)(tiquete.salida - tiquete.ingreso);
            double minutosTotales = duracion.TotalMinutes;

            int horasCompletas = (int)(minutosTotales / 60);
            int minutosRestantes = (int)(minutosTotales % 60);

            double costoHorasCompletas = horasCompletas * parqueo.tarifaHora;
            double costoMinutosRestantes = minutosRestantes <= 30 ? minutosRestantes * (parqueo.tarifaMediaHora / 30.0) : minutosRestantes * (parqueo.tarifaHora / 60.0);

            double total = costoHorasCompletas + costoMinutosRestantes;

            return total;
        }

        // POST: FacturaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}
