using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationApiArmonii.Models;

namespace WebApplicationApiArmonii.Controllers
{
    public class EventoController : ApiController
    {
        private ArmoniiEntities db = new ArmoniiEntities();

        // GET: api/Eventoes
        public IHttpActionResult GetEventos()
        {
            // Deshabilitar la carga perezosa
            db.Configuration.LazyLoadingEnabled = false;

            // Proyección para evitar anidamientos excesivos y ciclos de referencia
            var eventos = db.Evento
                .Select(e => new
                {
                    e.id,
                    e.nombre,
                    e.fecha,
                    e.descripcion,
                    e.estado,
                    e.duracion,
                    e.idLocal,
                    e.idMusico
                })
                .ToList();

            return Ok(eventos);
        }

        // GET: api/Eventoes/5
        [ResponseType(typeof(Evento))]
        public async Task<IHttpActionResult> GetEvento(int id)
        {
            // Deshabilitar la carga perezosa
            db.Configuration.LazyLoadingEnabled = false;

            // Proyección para evitar anidamientos excesivos y ciclos de referencia
            var evento = await db.Evento
                .Where(e => e.id == id)
                .Select(e => new
                {
                    e.id,
                    e.nombre,
                    e.fecha,
                    e.descripcion,
                    e.estado,
                    e.duracion,
                    Local = new { e.Local.id},
                    Musico = new { e.Musico.id}
                })
                .FirstOrDefaultAsync();

            if (evento == null)
            {
                return NotFound();
            }

            return Ok(evento);
        }

        // POST: api/Eventoes
        [ResponseType(typeof(Evento))]
        public async Task<IHttpActionResult> PostEvento(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Evento.Add(evento);
            await db.SaveChangesAsync();

            // Devolver una respuesta simplificada
            var response = new
            {
                evento.id,
                evento.nombre,
                evento.fecha,
                evento.descripcion,
                evento.estado,
                evento.duracion,
                Local = new { evento.Local.id,},
                Musico = new { evento.Musico.id}
            };

            return CreatedAtRoute("DefaultApi", new { id = evento.id }, response);
        }

        // PUT: api/Eventoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEvento(int id, Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != evento.id)
            {
                return BadRequest();
            }

            db.Entry(evento).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
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

        // DELETE: api/Eventoes/5
        [ResponseType(typeof(Evento))]
        public async Task<IHttpActionResult> DeleteEvento(int id)
        {
            Evento evento = await db.Evento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            db.Evento.Remove(evento);
            await db.SaveChangesAsync();

            // Devolver una respuesta simplificada
            var response = new
            {
                evento.id,
                evento.nombre,
                evento.fecha,
                evento.descripcion,
                evento.estado,
                evento.duracion,
                Local = new { evento.Local.id },
                Musico = new { evento.Musico.id }
            };

            return Ok(response);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventoExists(int id)
        {
            return db.Evento.Count(e => e.id == id) > 0;
        }
    }
}