using Microsoft.EntityFrameworkCore;
using Proyecto1.Models;
using System.Security.Cryptography;

namespace Proyecto1.Interfaces
{
    public class FacturaRepository : IFacturaRepository
    {
        public FacturaRepository()
        {

        }

        private static int _id = 0;

        public List<Factura> GetFacturas()
        {
            using (var context = new ApiContext())
            {
                var list = context.Facturas
                    .ToList();
                return list;
            }
        }

        public Factura GetFacturaId(int id)
        {
            using (var context = new ApiContext())
            {
                List<Factura> listaFacturas;
                listaFacturas = GetFacturas();
                var factura = listaFacturas
                    .FirstOrDefault(a => a.idFactura == id);
                return factura;
            }

        }

        public void PostFactura(Tiquete tiquete)
        {

            using (var context = new ApiContext())
            {
                List<Factura> listaFacturas;
                double valor = calcularValor(tiquete);
                listaFacturas = context.Facturas.ToList();
                Factura nuevaFactura = new Factura(
                    _id,
                    tiquete.id,
                    tiquete.idParqueo,
                    valor,
                    tiquete.salida ?? DateTime.Now);

                context.Facturas.AddRange(nuevaFactura);
                context.SaveChanges();

                _id += 1;
            }


        }

        public void EditFactura(Tiquete tiquete, Factura facturaOriginal)
        {
            using (var context = new ApiContext())
            {
                double valor = calcularValor(tiquete);
                if (facturaOriginal != null)
                {
                    if (tiquete.salida != null)
                    {
                        context.Entry(facturaOriginal).Property(nameof(facturaOriginal.fecha)).CurrentValue = tiquete.salida;
                    }

                    context.Entry(facturaOriginal).Property(nameof(facturaOriginal.valor)).CurrentValue = valor;
                    context.Entry(facturaOriginal).State = EntityState.Modified;

                    context.SaveChanges();
                }

            }
        }

        public void DeleteFactura(int id)
        {
            using (var context = new ApiContext())
            {

                List<Factura> listaFacturas = GetFacturas();
                var factura = listaFacturas
                    .FirstOrDefault(a => a.idFactura == id); ;

                context.Facturas.Remove(factura);

                context.SaveChanges();

            }

        }

        public double calcularValor(Tiquete tiquete)
        {
            using (var context = new ApiContext())
            {
                List<Parqueo> listaParqueos = context.Parqueos.ToList();
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
            }

        }
}
