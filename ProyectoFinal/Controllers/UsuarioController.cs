using ProyectoFinal.Models;
using ProyectoFinal.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public UsuarioRepository _usuarioRepository;

        public UsuarioController()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        
        /// <summary>
        /// Traer Usuario 
        /// Debe recibir un nombre del usuario, buscarlo en la base de datos y devolver todos sus datos 
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <returns></returns>
        /// 
        [HttpGet("{nombreUsuario}")]
        public ActionResult TraerUsuarioPorNombre(string nombreUsuario)
        {
            var result = _usuarioRepository.TraerUsuarioPorNombre(nombreUsuario);

            return Ok(result);
        }

        /// <summary>
        /// Inicio de sesión 
        /// Se le pasa como parámetro el nombre del usuario y la contraseña, 
        /// buscar en la base de datos si el usuario existe y si coincide con la contraseña lo devuelve,
        /// caso contrario devuelve error.
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <param name="contraseña"></param>
        /// <returns></returns>

        [HttpGet("{nombreUsuario}/{contraseña}")]
        public ActionResult LoginUsuario(string nombreUsuario, string contraseña)
        {
            var result = _usuarioRepository.LoginUsuario(nombreUsuario, contraseña);

            return Ok(result);
            
        }

        /// <summary>
        /// Crear usuario
        /// Recibe como parámetro un JSON con todos los datos cargados 
        /// y debe dar un alta inmediata del usuario con los mismos validando que todos los datos obligatorios
        /// estén cargados, por el contrario, devolverá error (No se puede repetir el nombre de usuario.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult CrearUsuario(Usuario usuario)
        {
            var result = _usuarioRepository.CrearUsuario(usuario);

            return Ok(result);
        }

        /// <summary>
        /// Modificar usuario 
        /// Se recibirán todos los datos del usuario por un JSON 
        /// y se deberá modificar el mismo con los datos nuevos.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>

        [HttpPut]

        public ActionResult ModificarUsuario(Usuario usuario)
        {
            var result = _usuarioRepository.ModificarUsuario(usuario);

            return Ok(result);
        }

        /// <summary>
        /// Eliminar Usuario
        /// Recibe el ID del usuario a eliminar y lo deberá eliminar de la base de datos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete("{idUsuario}")]

        public ActionResult EliminarUsuario(long idUsuario)
        {
            var result = _usuarioRepository.EliminarUsuario(idUsuario);

            return Ok(result);
        }
    }
}
