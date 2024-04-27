using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Proyecto1.Models;
using System.Collections;

namespace Proyecto1.Interfaces
{
    public class ParqueoRepository : IParqueoRepository
    {
        public ParqueoRepository()
        {
            using (var context = new ApiContext())
            {
                if (context.Parqueos.Count() == 0)
                {
                    var parqueos = new List<Parqueo>
                    {
                    new Parqueo
                    (
                        1,
                        "Parqueo Ejemplo",
                        100,
                        "08:00",
                        "18:00",
                        10,
                        5
                    )
                    };
                    context.Parqueos.AddRange(parqueos);
                    context.SaveChanges();
                }

            }
        }
        public List<Parqueo> GetParqueos()
        {
            using (var context = new ApiContext())
            {
                var list = context.Parqueos
                    .ToList();
                return list;
            }
        }

        public Parqueo GetParqueoId(int id) 
        {
            using (var context = new ApiContext())
            {
                List<Parqueo> listaParqueos;
                listaParqueos = GetParqueos();
                var parqueo = listaParqueos
                    .FirstOrDefault(a => a.idParqueo == id);
                return parqueo;
            }

        }

        public void PostParqueo(Parqueo parqueo) 
        {
            using (var context = new ApiContext())
            {
                context.Parqueos.AddRange(parqueo);
                context.SaveChanges();
            }


        }

        public void EditParqueo(int id, Parqueo parqueo) 
        {
            using (var context = new ApiContext())
            {
                Parqueo parqueoOriginal = GetParqueoId(id);

                if (parqueoOriginal != null)
                {
                    // Actualizar las propiedades del parqueo original con las del parqueo recibido
                    context.Entry(parqueoOriginal).CurrentValues.SetValues(parqueo);
                    context.Entry(parqueoOriginal).State = EntityState.Modified;
                    // Guardar los cambios en la base de datos
                    context.SaveChanges();
                }

            }
        }

        public void DeleteParqueo(Parqueo parqueo) 
        {
            using (var context = new ApiContext()) 
            {

                context.Parqueos.Remove(parqueo);

                context.SaveChanges();
            
            }

        }

    }
}

