using Proyecto1.Models;

namespace Proyecto1.Interfaces
{
    public interface ITiqueteRepository
    {
        public List<Tiquete> GetTiquetes();
        public Tiquete GetTiqueteId(int id);
        public void PostTiquete(Tiquete tiquete);
        public void EditTiquete(int id, Tiquete tiquete);
        public void DeleteTiquete(Tiquete tiquete);
    }
}
