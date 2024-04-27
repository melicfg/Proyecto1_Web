using Proyecto1.Models;

namespace Proyecto1.Interfaces
{
    public interface IParqueoRepository
    {
        public List<Parqueo> GetParqueos();
        public Parqueo GetParqueoId(int id);
        public void PostParqueo(Parqueo parqueo);
        public void EditParqueo(int id, Parqueo parqueo);
        public void DeleteParqueo(Parqueo parqueo);
    }
}
