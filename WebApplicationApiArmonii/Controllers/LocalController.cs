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
using WebApplicationApiArmonii.DTO;
using WebApplicationApiArmonii.Models;

namespace WebApplicationApiArmonii.Controllers
{
    public class LocalController : ApiController
    {
        private ArmoniiEntities db = new ArmoniiEntities();

        // GET: api/Local
        public IQueryable<object> GetLocal()
        {
            var locales = from m in db.Local
                          join u in db.Usuario on m.idUsuario equals u.id into usuarioJoin
                          from usuario in usuarioJoin.DefaultIfEmpty() // LEFT JOIN
                          select new
                          {
                              m.id,
                              m.idUsuario,
                              m.direccion,
                              m.descripcion,
                              m.tipo_local,
                              m.horarioApertura,
                              m.horarioCierre,
                              m.imagen,

                              // Propiedades del usuario
                              nombre = usuario != null ? usuario.nombre : null,
                              correo = usuario != null ? usuario.correo : null,
                              contrasenya = usuario != null ? usuario.contrasenya : null,
                              telefono = usuario != null ? usuario.telefono : null,
                              latitud = usuario != null ? usuario.latitud : (double?)null,
                              longitud = usuario != null ? usuario.longitud : (double?)null,
                              fechaRegistro = usuario != null ? usuario.fechaRegistro : (DateTime?)null,
                              estado = usuario != null ? usuario.estado : (bool?)null,
                              valoracion = usuario != null ? usuario.valoracion : (double?)null,
                              tipo = usuario != null ? usuario.tipo : null,
                          };

            return locales;
        }


        // GET: api/Local/5
        [ResponseType(typeof(Local))]
        public async Task<IHttpActionResult> GetLocal(int id)
        {
            var local = from m in db.Local
                         where m.id == id
                         join u in db.Usuario on m.idUsuario equals u.id into usuarioJoin
                         from usuario in usuarioJoin.DefaultIfEmpty() // LEFT JOIN
                         select new
                         {
                             m.id,
                             m.idUsuario,
                             m.direccion,
                             m.descripcion,
                             m.tipo_local,
                             m.imagen,

                             // Propiedades del usuario
                             nombre = usuario != null ? usuario.nombre : null,
                             correo = usuario != null ? usuario.correo : null,
                             contrasenya = usuario != null ? usuario.contrasenya : null,
                             telefono = usuario != null ? usuario.telefono : null,
                             latitud = usuario != null ? usuario.latitud : (double?)null,
                             longitud = usuario != null ? usuario.longitud : (double?)null,
                             fechaRegistro = usuario != null ? usuario.fechaRegistro : (DateTime?)null,
                             estado = usuario != null ? usuario.estado : (bool?)null,
                             valoracion = usuario != null ? usuario.valoracion : (double?)null,
                             tipo = usuario != null ? usuario.tipo : null,
                         };

            return Ok(local);
        }

        // GET: api/Local/correo
        [ResponseType(typeof(DataTransferObjectMusico))]
        public async Task<IHttpActionResult> GetLocalCorreo(string correo)
        {
            var musico = await (from u in db.Usuario
                                where u.correo == correo
                                join m in db.Local on u.id equals m.idUsuario into usuarioJoin
                                from usuario in usuarioJoin.DefaultIfEmpty() // LEFT JOIN
                                select new DataTransferObjectMusico
                                {
                                    id = u.id,
                                    nombre = u.nombre,
                                    correo = u.correo,
                                    contrasenya = u.contrasenya,
                                    telefono = u.telefono,
                                    latitud = u.latitud,
                                    longitud = u.longitud,
                                    fechaRegistro = u.fechaRegistro,
                                    estado = u.estado,
                                    valoracion = u.valoracion,
                                    tipo = u.tipo,

                                    // Local
                                    direccion = usuario != null ? usuario.direccion : null,
                                    tipo_local = usuario != null ? usuario.tipo_local : null,
                                    descripcion = usuario != null ? usuario.descripcion : null,
                                    imagen = usuario != null ? usuario.imagen : null,
                                    idUsuario = usuario != null ? usuario.idUsuario ?? 0 : 0,

                                }).FirstOrDefaultAsync(); // Ejecuta la consulta y obtiene un solo resultado

            if (musico == null)
            {
                return NotFound(); // Retorna 404 si no se encuentra
            }

            return Ok(musico);
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
        [ResponseType(typeof(bool))]
        public Boolean PostLocal(DataTransferObjectMusico localDTO)
        {

            Usuario usuario = new Usuario
            {
                nombre = localDTO.nombre,
                correo = localDTO.correo,
                contrasenya = localDTO.contrasenya,
                telefono = localDTO.telefono,
                latitud = localDTO.latitud,
                longitud = localDTO.longitud,
                fechaRegistro = DateTime.Now,
                estado = localDTO.estado,
                valoracion = localDTO.valoracion,
                tipo = "Local"
            };

            db.Usuario.Add(usuario);
            db.SaveChanges();

            Usuario user = db.Usuario.FirstOrDefault(u => u.correo == usuario.correo);

            // Verifica que el ID del usuario haya sido generado
            if (usuario.id == 0)
            {
                throw new InvalidOperationException("El ID del usuario no fue generado correctamente.");
            }



            Local local = new Local
            {
                direccion = localDTO.direccion,
                tipo_local = localDTO.tipo_local,
                descripcion = localDTO.descripcion,
                imagen = localDTO.imagen,
                idUsuario = user.id
            };

            db.Local.Add(local);
            db.SaveChanges();


            return true;
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