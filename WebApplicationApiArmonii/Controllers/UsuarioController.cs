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
    public class UsuarioController : ApiController
    {
        private ArmoniiEntities db = new ArmoniiEntities();

        // GET: api/Usuario
        public IQueryable<Usuario> GetUsuario()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.Usuario;
        }

        // GET: api/Usuario/5
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> GetUsuario(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            Usuario usuario = await db.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }


        // GET: api/Usuario?correo={correo}
        [HttpGet]
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> GetUsuarioPorCorreo([FromUri] string correo)
        {
            db.Configuration.LazyLoadingEnabled = false;

            Usuario usuario = await db.Usuario
                .Where(u => u.correo == correo)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }






        // PUT: api/Usuario/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.id)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuario
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuario.Add(usuario);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = usuario.id }, usuario);
        }

        // DELETE: api/Usuario/5
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> DeleteUsuario(int id)
        {
            Usuario usuario = await db.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuario.Remove(usuario);
            await db.SaveChangesAsync();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuario.Count(e => e.id == id) > 0;
        }
    }
}