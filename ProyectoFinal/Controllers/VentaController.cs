using ProyectoFinal.Models;
using ProyectoFinal.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        public VentaRepository _ventaRepository;

        public VentaController()
        {
            _ventaRepository = new VentaRepository();
        }


        /// <summary>
        /// Traer Ventas
        /// Debe traer todas las ventas de la base, 
        /// incluyendo sus Productos, cuya información está en ProductosVendidos.
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        public ActionResult TraerVenta()
        {
            var result = _ventaRepository.TraerVentas();
            return Ok(result);
        }


        /// <summary>
        /// /// <summary>
        /// Cargar Venta
        /// Recibe una lista de productos y el número de IdUsuario de quien la efectuó, 
        /// primero cargar una nueva venta en la base de datos, luego debe cargar los productos recibidos
        /// en la base de ProductosVendidos uno por uno por un lado
        /// , y descontar el stock en la base de productos por el otro.
        /// </summary>
        /// </summary>
        /// <param name="productos"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>

        [HttpPost("{idUsuario}")]
        public ActionResult CargarVenta(List<Producto> productos, int idUsuario)
        {
            var result = _ventaRepository.CargarVenta(productos, idUsuario);
            return Ok(result);
            
        }

        
        /// <summary>
        /// Recibe una venta con su número de Id, 
        /// debe buscar en la base de Productos Vendidos 
        /// cuáles lo tienen eliminándolos, sumar el stock a los productos incluidos, 
        /// y eliminar la venta de la base de datos.
        /// </summary>
        /// <param name="idVenta"></param>
        /// <returns></returns>
        
        [HttpDelete("{idVenta}")]
        public ActionResult EliminarVenta(long idVenta)
        {
            var result = _ventaRepository.EliminarVenta(idVenta);

            return Ok(result);
        }
    }

}
