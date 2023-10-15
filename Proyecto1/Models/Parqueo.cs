using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class Parqueo
    {
        public Parqueo() { }

        public Parqueo(int idParqueo, string nombre, int cantidadMax, string horaCierre, string horaApertura, int tarifaHora, int tarifaMediaHora) 
        {

            this.idParqueo = idParqueo;
            this.nombre = nombre;
            this.cantidadMax = cantidadMax;
            this.horaCierre = horaCierre;
            this.horaApertura = horaApertura;
            this.tarifaHora = tarifaHora;
            this.tarifaMediaHora = tarifaMediaHora;

        
        }

        public int idParqueo { get; set; }
        public string nombre { get; set; }

        public int cantidadMax { get; set; }

        public string horaCierre { get; set; }

        public string horaApertura { get; set; }

        public int tarifaHora { get; set; }

        public int tarifaMediaHora { get; set; }

    }
}
