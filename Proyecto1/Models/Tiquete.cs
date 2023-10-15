namespace Proyecto1.Models
{
    public class Tiquete
    {
        public Tiquete()
        {
            // Constructor sin parámetros requerido para el model binding
        }

        public Tiquete(int id, DateTime ingreso, DateTime salida, string placa, int idParqueo)
        {
            this.id = id;
            this.ingreso = ingreso;
            this.salida = salida;
            this.placa = placa;
            this.idParqueo = idParqueo;
        }

        public int id { get; set; }
        public DateTime ingreso { get; set; }

        public DateTime? salida { get; set; }

        public string placa { get; set; }

        public int idParqueo { get; set; }

    }
}
