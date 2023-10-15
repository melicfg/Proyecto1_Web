namespace Proyecto1.Models
{
    public class Factura
    {

        public Factura(int idFactura, int idTiquete, int idParqueo, Double valor, DateTime? fecha) 
        { 
            this.idFactura = idFactura;
            this.idTiquete = idTiquete;
            this.idParqueo = idParqueo;
            this.valor = valor;
            this.fecha = fecha.HasValue ? fecha.Value : DateTime.MinValue;

        }
        
        public int idFactura { get; set; }

        public int idTiquete { get; set; }

        public int idParqueo { get; set; }

        public double valor { get; set; }

        public DateTime fecha { get; set; }

    }
}
