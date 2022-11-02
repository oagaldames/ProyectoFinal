using ProyectoFinal.Models;
using ProyectoFinal.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoVendidoController : ControllerBase
    {
        public ProductoVendidoRepository _productoVendidoRepository;

        public ProductoVendidoController()
        {
            _productoVendidoRepository = new ProductoVendidoRepository();
        }



        /// <summary>
        /// Traer Todos los productos vendidos de un Usuario, cuya información está en su producto 
        /// (Utilizar dentro de esta función el "Traer Productos" 
        /// anteriormente hecho para saber que productosVendidos ir a buscar).
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>        

        [HttpGet("{idUsuario}")]
        public ActionResult TraerProductosVendidos(long idUsuario)
        {
            var result = _productoVendidoRepository.TraerProductosVendidos(idUsuario);
            return Ok(result);

        }

       
    }
}

