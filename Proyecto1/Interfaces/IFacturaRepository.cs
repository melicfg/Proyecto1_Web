using Proyecto1.Models;

namespace Proyecto1.Interfaces
{
    public interface IFacturaRepository
    {
        public List<Factura> GetFacturas();
        public Factura GetFacturaId(int id);
        public void PostFactura(Tiquete tiquete);
        public void EditFactura(Tiquete tiquete, Factura facturaOriginal);
        public void DeleteFactura(int id);
    }
}
