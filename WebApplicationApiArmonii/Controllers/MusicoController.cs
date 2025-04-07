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
using WebApplicationApiArmonii.DTO;
using WebApplicationApiArmonii.Models;

namespace WebApplicationApiArmonii.Controllers
{
    public class MusicoController : ApiController
    {
        private ArmoniiEntities db = new ArmoniiEntities();

        // GET: api/Musico
        public IQueryable<object> GetMusico()
        {
            var musicosConGeneros = from m in db.Musico
                                    join u in db.Usuario on m.idUsuario equals u.id into usuarioJoin
                                    from usuario in usuarioJoin.DefaultIfEmpty() // LEFT JOIN
                                    select new
                                    {
                                        m.id,
                                        m.idUsuario,
                                        m.genero,
                                        m.biografia,
                                        m.apellido,
                                        m.apodo,
                                        m.edad,
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

                                        // Géneros musicales asociados con el musico
                                        generosMusicales = db.GenerosMusicos
                                            .Where(gm => gm.idMusico == m.id)
                                            .Join(db.Generos, gm => gm.idGenero, g => g.id, (gm, g) => g.genero)
                                            .ToList() // Obtiene la lista de nombres de géneros musicales asociados con el musico
                                    };

            return musicosConGeneros;
        }




        /*
            
        */

        // GET: api/Musico/5
        [ResponseType(typeof(Musico))]
        public async Task<IHttpActionResult> GetMusico(int id)
        {
            var musico = from m in db.Musico
                            where m.id == id 
                            join u in db.Usuario on m.idUsuario equals u.id into usuarioJoin
                            from usuario in usuarioJoin.DefaultIfEmpty() // LEFT JOIN
                            select new
                            {
                                m.id,
                                m.idUsuario,
                                m.genero,
                                m.biografia,
                                m.apellido,
                                m.apodo,
                                m.edad,
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

                                // Géneros musicales asociados con el musico
                                generosMusicales = db.GenerosMusicos
                                    .Where(gm => gm.idMusico == m.id)
                                    .Join(db.Generos, gm => gm.idGenero, g => g.id, (gm, g) => g.genero)
                                    .ToList() // Obtiene la lista de nombres de géneros musicales asociados con el musico
                            };

            return Ok(musico);
        }

        // GET: api/Musico/correo
        [ResponseType(typeof(DataTransferObjectMusico))]
        public async Task<IHttpActionResult> GetMusicoCorreo(string correo)
        {
            var musico = await (from u in db.Usuario
                                where u.correo == correo
                                join m in db.Musico on u.id equals m.idUsuario into usuarioJoin
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

                                    // Musico
                                    apodo = usuario != null ? usuario.apodo : null,
                                    apellido = usuario != null ? usuario.apellido : null,
                                    genero = usuario != null ? usuario.genero : null,
                                    edad = usuario != null ? usuario.edad : null,
                                    biografia = usuario != null ? usuario.biografia : null,
                                    imagen = usuario != null ? usuario.imagen : null,
                                    idUsuario = usuario != null ? usuario.idUsuario ?? 0 : 0,
                                }).FirstOrDefaultAsync(); // Ejecuta la consulta y obtiene un solo resultado

            if (musico == null)
            {
                return NotFound(); // Retorna 404 si no se encuentra
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
        [ResponseType(typeof(bool))]
        public Boolean PostMusico(DataTransferObjectMusico musicoDTO)
        {
            Usuario usuario = new Usuario
            {
                nombre = musicoDTO.nombre,
                correo = musicoDTO.correo,
                contrasenya = musicoDTO.contrasenya,
                telefono = musicoDTO.telefono,
                latitud = musicoDTO.latitud,
                longitud = musicoDTO.longitud,
                fechaRegistro = DateTime.Now, 
                estado = musicoDTO.estado,
                valoracion = musicoDTO.valoracion,
                tipo = "Musico"
            };

            db.Usuario.Add(usuario);
            db.SaveChanges();

            Usuario user = db.Usuario.FirstOrDefault(u => u.correo == usuario.correo);
            
            // Verifica que el ID del usuario haya sido generado
            if (usuario.id == 0)
            {
                throw new InvalidOperationException("El ID del usuario no fue generado correctamente.");
            }



            Musico musico = new Musico
            {
                apodo = musicoDTO.apodo,
                apellido = musicoDTO.apellido,
                genero = musicoDTO.genero,
                edad = musicoDTO.edad,
                biografia = musicoDTO.biografia,
                imagen = musicoDTO.imagen,
                idUsuario = user.id
                /*generosMusicales = db.Generos // Esto no hace nada, pero funciona, no tocar
                                    .Where(g => musicoDTO.generosMusicales.Contains(g.genero))
                                    .Select(g => g.genero)
                                    .ToList()*/
            };

            db.Musico.Add(musico);
            db.SaveChanges();

            return true;
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