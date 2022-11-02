using ProyectoFinal.Models;
using ProyectoFinal.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        public ProductoRepository _productoRepository;

        public ProductoController()
        {
            _productoRepository = new ProductoRepository();
        }

        /// <summary>
        /// Traer Productos
        /// Debe traer todos los productos cargados en la base.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TraerProductos()
        {
            var result = _productoRepository.TraerProductos();
            return Ok(result);

        }

        /// <summary>
        /// Crear producto
        /// Recibe una lista de tareas por JSON, 
        /// número de Id 0, Descripción, costo, precio venta y stock.
        /// </summary>
        /// <param name="productos"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CrearProducto(Producto productos)
        {
            var result = _productoRepository.CrearProducto(productos);

            return Ok(result);
        }

        /// <summary>
        /// Modificar producto
        /// Recibe un producto con su número de Id, debe modificarlo con la nueva información.
        /// el id del producto viene en la lista
        /// </summary>
        /// <param name="productos"></param>
        /// <returns></returns>

        [HttpPut]
       
        public ActionResult ModificarProducto(Producto productos)
        {
            var result = _productoRepository.ModificarProducto(productos); 
             
            return Ok(result);
        }

        /// <summary>
        /// Eliminar producto
        /// Recibe el número de Id de un producto a eliminar y debe eliminarlo de la base de datos. 
        /// primero elimino de la tabla producto vendido el registro que ecorresponde a ese producto
        /// </summary>
        /// <param name="idProducto"></param>
        /// <returns></returns>

        [HttpDelete("{idProducto}")]

        public ActionResult EliminarProducto(long idProducto)
        {
            var result = _productoRepository.EliminarProducto(idProducto);

            return Ok(result);
        }
    }
}
