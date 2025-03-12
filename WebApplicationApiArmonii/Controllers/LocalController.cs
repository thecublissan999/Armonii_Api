using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationApiArmonii.Models;

namespace WebApplicationApiArmonii.Controllers
{
    public class LocalController : ApiController
    {
        private ArmoniiEntities db = new ArmoniiEntities();

        // GET: api/Local
        public IQueryable<object> GetLocal()
        {
            return db.Local
                     .Include(l => l.Usuario) // Cargar los datos del usuario asociado
                     .Select(l => new {
                         l.id,
                         l.direccion,
                         l.tipo_local,
                         l.horarioApertura,
                         l.horarioCierre,
                         Usuario = new
                         {
                             l.Usuario.nombre,
                             l.Usuario.correo,
                             l.Usuario.contrasenya,
                             l.Usuario.telefono,
                             l.Usuario.latitud,
                             l.Usuario.longitud,
                             l.Usuario.fechaRegistro,
                             l.Usuario.estado,
                             l.Usuario.valoracion,
                             l.Usuario.tipo
                         }
                     });
        }


        // GET: api/Local/5
        [ResponseType(typeof(Local))]
        public async Task<IHttpActionResult> GetLocal(int id)
        {
            Local local = await db.Local.FindAsync(id);
            if (local == null)
            {
                return NotFound();
            }

            return Ok(local);
        }


        // GET: api/Local/correo/{correo}
        [HttpGet]
        [Route("api/Local/id/{id}")]
        public async Task<IHttpActionResult> FindByCorreo(int id)
        {
            IHttpActionResult result;
            db.Configuration.LazyLoadingEnabled = false;

            try
            {
                var locales = db.Local
                    .Include(l => l.Usuario) // Incluye la información del Usuario relacionado con el Local
                    .Where(l => l.Usuario.id == id) // Filtra por el correo del Usuario
                    .ToList(); // Ejecuta la consulta y convierte el resultado en una lista



                // Si no se encuentra el local, devolvemos un 404
                if (locales == null || locales.Count == 0)
                {
                    result = NotFound();
                }
                else
                {
                    result = Ok(locales);
                }
            }
            catch (DbUpdateException ex)
            {
                SqlException sqlException = (SqlException)ex.InnerException.InnerException;
                string missatge = Clases.Utilitat.MensajeError(sqlException);
                result = BadRequest(missatge);
            }
            catch (Exception ex)
            {
                result = InternalServerError(new Exception("Error al obtener la información del local.", ex));
            }

            return result;
        }



        // PUT: api/Local/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLocal(int id, Local local)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != local.id)
            {
                return BadRequest();
            }

            db.Entry(local).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalExists(id))
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

        // POST: api/Local
        [ResponseType(typeof(Local))]
        public async Task<IHttpActionResult> PostLocal(Local local)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Local.Add(local);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LocalExists(local.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = local.id }, local);
        }

        // DELETE: api/Local/5
        [ResponseType(typeof(Local))]
        public async Task<IHttpActionResult> DeleteLocal(int id)
        {
            Local local = await db.Local.FindAsync(id);
            if (local == null)
            {
                return NotFound();
            }

            db.Local.Remove(local);
            await db.SaveChangesAsync();

            return Ok(local);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocalExists(int id)
        {
            return db.Local.Count(e => e.id == id) > 0;
        }
    }
}