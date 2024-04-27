using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Proyecto1.Interfaces;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class FacturaController : Controller
    {
        private static int _id = 0;
        private static double _total = 0;


        readonly IFacturaRepository _facturaRepository;
        readonly IParqueoRepository _parqueoRepository;
        public FacturaController(IFacturaRepository facturaRepository, IParqueoRepository parqueoRepository)
        {
            _facturaRepository = facturaRepository;
            _parqueoRepository = parqueoRepository;
        }

        public List<Factura> ObtenerFacturas()
        {
            List<Factura> listaFacturas;

            listaFacturas = _facturaRepository.GetFacturas();

            return listaFacturas;
        }

        public Factura ObtenerFacturaId(int id)
        {
            Factura factura = _facturaRepository.GetFacturaId(id);

            return factura;


        }

        public void Guardar(Tiquete tiquete)
        {
            _facturaRepository.PostFactura(tiquete);

        }

        public void Editar(Tiquete tiquete, int indice)
        {

            List<Factura> listaFacturas;
            listaFacturas = ObtenerFacturas();
            Factura factura = listaFacturas[indice];
            _facturaRepository.EditFactura(tiquete, factura);


        }


        // GET: FacturaController
        public ActionResult Index(int? idParqueo)
        {
            List<Parqueo> listaParqueos = _parqueoRepository.GetParqueos();
            ViewBag.parqueos = listaParqueos;
            List<Factura> listaFacturas;
            listaFacturas = _facturaRepository.GetFacturas();
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
            List<Parqueo> listaParqueos = _parqueoRepository.GetParqueos();

            Factura factura = _facturaRepository.GetFacturaId(id);
            Parqueo parqueo = listaParqueos.FirstOrDefault(p => p.idParqueo == factura.idParqueo);
            ViewBag.parqueo = parqueo;
            return View(factura);
        }

        // GET: FacturaController/Create

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
