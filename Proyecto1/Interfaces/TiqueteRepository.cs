using Microsoft.EntityFrameworkCore;
using Proyecto1.Models;

namespace Proyecto1.Interfaces
{
    public class TiqueteRepository : ITiqueteRepository
    {

        public TiqueteRepository()
        {
            using (var context = new ApiContext())
            {

            }
        }
        public List<Tiquete> GetTiquetes()
        {
            using (var context = new ApiContext())
            {
                var list = context.Tiquetes
                    .ToList();
                return list;
            }
        }

        public Tiquete GetTiqueteId(int id)
        {
            using (var context = new ApiContext())
            {
                List<Tiquete> listaTiquete;
                listaTiquete = GetTiquetes();
                var tiquete = listaTiquete
                    .FirstOrDefault(a => a.id == id);
                return tiquete;
            }

        }

        public void PostTiquete(Tiquete tiquete)
        {
            using (var context = new ApiContext())
            {
                context.Tiquetes.AddRange(tiquete);
                context.SaveChanges();
            }


        }

        public void EditTiquete(int id, Tiquete tiquete)
        {
            using (var context = new ApiContext())
            {
                Tiquete tiqueteOriginal = GetTiqueteId(id);

                if (tiqueteOriginal != null)
                {
                    // Actualizar las propiedades del tiquete original con las del tiquete recibido
                    context.Entry(tiqueteOriginal).CurrentValues.SetValues(tiquete);
                    context.Entry(tiqueteOriginal).State = EntityState.Modified;
                    // Guardar los cambios en la base de datos
                    context.SaveChanges();
                }

            }
        }

        public void DeleteTiquete(Tiquete tiquete)
        {
            using (var context = new ApiContext())
            {

                context.Tiquetes.Remove(tiquete);

                context.SaveChanges();

            }

        }


    }
}
