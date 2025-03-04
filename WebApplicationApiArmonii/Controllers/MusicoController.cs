using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationApiArmonii.Models;

namespace WebApplicationApiArmonii.Controllers
{
    public class MusicoController : ApiController
    {
        private ArmoniiEntities db = new ArmoniiEntities();

        // GET: api/Musico
        public IQueryable<object> GetMusico()
        {
            return db.Musico
                     .Include(m => m.Usuario) // Cargar los datos del usuario asociado
                     .Select(m => new {
                         m.id,
                         m.nombreArtistico,
                         m.genero,
                         m.biografia,
                         Usuario = new
                         {
                             m.Usuario.nombre,
                             m.Usuario.correo,
                             m.Usuario.contrasenya,
                             m.Usuario.telefono,
                             m.Usuario.latitud,
                             m.Usuario.longitud,
                             m.Usuario.fechaRegistro,
                             m.Usuario.estado,
                             m.Usuario.edad,
                             m.Usuario.valoracion,
                             m.Usuario.tipo
                         }
                     });
        }


        // GET: api/Musico/5
        [ResponseType(typeof(Musico))]
        public async Task<IHttpActionResult> GetMusico(int id)
        {
            Musico musico = await db.Musico.FindAsync(id);
            if (musico == null)
            {
                return NotFound();
            }

            return Ok(musico);
        }

        // PUT: api/Musico/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMusico(int id, Musico musico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != musico.id)
            {
                return BadRequest();
            }

            db.Entry(musico).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;                
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Musico
        [ResponseType(typeof(Musico))]
        public async Task<IHttpActionResult> PostMusico(Musico musico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Musico.Add(musico);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MusicoExists(musico.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = musico.id }, musico);
        }

        // DELETE: api/Musico/5
        [ResponseType(typeof(Musico))]
        public async Task<IHttpActionResult> DeleteMusico(int id)
        {
            Musico musico = await db.Musico.FindAsync(id);
            if (musico == null)
            {
                return NotFound();
            }

            db.Musico.Remove(musico);
            await db.SaveChangesAsync();

            return Ok(musico);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MusicoExists(int id)
        {
            return db.Musico.Count(e => e.id == id) > 0;
        }
    }
}