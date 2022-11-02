using System.Data.SqlClient;
using System.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class ProductoVendidoRepository
    {

        public ProductoRepository _productoRepository;

        public ProductoVendidoRepository()
        {
            _productoRepository = new ProductoRepository();
        }

        public List<ProductoVendido> TraerProductosVendidos(long idUsuario)
        {
            var listaProductosVendidos = new List<ProductoVendido>();
            try
            {
                List<Producto> listaProductosUsaurio = new List<Producto>();
                
                DataSql db = new DataSql();

                if (db.ConectarSQL())
                {
                    listaProductosUsaurio = _productoRepository.TraerProductos();
                    foreach (Producto producto in listaProductosUsaurio)
                    {
                        if (producto.IdUsuario == idUsuario)
                        {
                            SqlCommand cmd = db.Connection.CreateCommand();

                            cmd.CommandText = "SELECT Id,Stock,IdProducto,IdVenta " +
                                "FROM ProductoVendido " +
                                "WHERE IdProducto=@IdPro";
                            var paramIdPro = new SqlParameter("IdPro", SqlDbType.BigInt);
                            paramIdPro.Value=producto.Id;
                            cmd.Parameters.Add(paramIdPro);
                            using var reader = cmd.ExecuteReader();
                            {
                                while (reader.Read())
                                {
                                    var productoVendido = new ProductoVendido();
                                    productoVendido.Id = Convert.ToInt64(reader.GetValue(0));
                                    productoVendido.Stock = Convert.ToInt32(reader.GetValue(1));
                                    productoVendido.IdProducto = Convert.ToInt64(reader.GetValue(2));
                                    productoVendido.IdVenta = Convert.ToInt64(reader.GetValue(3));
                                    listaProductosVendidos.Add(productoVendido);
                                }
                                reader.Close();
                            }
                        }
                    }
                    db.DesconectarSQL();
                }
                
            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR TraerProductosVendidos  " + error);
            }
            return listaProductosVendidos;
        }

        public bool CrearProductoVendido(int stock, long idproducto, long idventa)

        {
            bool respuesta = false;
            try
            {
                    DataSql db = new DataSql();
                    if (db.ConectarSQL())
                    {
                        var queryInsert = @"Insert Into ProductoVendido (Stock, IdProducto
                                           ,idventa) 
                                          values (@Stock,@IdPro,@idventa); select @@IDENTITY";
                        var paramStocknuevo = new SqlParameter("Stock", SqlDbType.Int);
                        paramStocknuevo.Value = stock;
                        var paramCostoNuevo = new SqlParameter("IdPro", SqlDbType.BigInt);
                        paramCostoNuevo.Value = idproducto;
                        var paramIdVenNuevo = new SqlParameter("idventa", SqlDbType.BigInt);
                        paramIdVenNuevo.Value = idventa;

                        SqlCommand commandoInsert = new SqlCommand(queryInsert, db.Connection);
                        commandoInsert.Parameters.Add(paramStocknuevo);
                        commandoInsert.Parameters.Add(paramCostoNuevo);
                        commandoInsert.Parameters.Add(paramIdVenNuevo);
                        
                        double idNuevo = Convert.ToInt64(commandoInsert.ExecuteScalar());
                        if (idNuevo > 0)
                        {
                            respuesta = true;
                        }
                        else
                        {
                            respuesta = false;
                        }
                    }
                    else
                    {
                        respuesta = false;
                    }
                
            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR CrearProducto  " + error);
                respuesta = false;
            }
            return respuesta;
        }

        public bool ModificarProductoVendido(long id, int stock, long idproducto, long idventa)

        {
            try
            {
                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                    var queryUpdate = @"UPDATE ProductoVendido set 
                                     Stock=@Stock
                                     ,IdProducto=@IdProd
                                     ,idventa =@IdVenta
                                    WHERE Id = @Id";


                    var paramStock = new SqlParameter("Stock", SqlDbType.Int);
                    paramStock.Value = stock;
                    var paramCosto = new SqlParameter("IdProd", SqlDbType.BigInt);
                    paramCosto.Value = idproducto;
                    var paramIdVen = new SqlParameter("idventa", SqlDbType.BigInt);
                    paramIdVen.Value = idventa;
                    var paramId = new SqlParameter("id", SqlDbType.BigInt);
                    paramId.Value = id;

                    SqlCommand commandoUpdate = new SqlCommand(queryUpdate, db.Connection);
                    commandoUpdate.Parameters.Add(paramStock);
                    commandoUpdate.Parameters.Add(paramCosto);
                    commandoUpdate.Parameters.Add(paramIdVen);
                    commandoUpdate.Parameters.Add(paramId);
                    int recordsAffected = commandoUpdate.ExecuteNonQuery();

                    db.DesconectarSQL();

                    if (recordsAffected == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR ModificarProducto  " + error);
                return false;
            }
        }


        public bool EliminarProductoVendido(long id)

        {
            try
            {

                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                   var queryDelete = @" DELETE
                                    ProductoVendido
                                    WHERE 
                                    IdVenta = @ID
                                    ";
                    var paramId = new SqlParameter("id", SqlDbType.BigInt);
                    paramId.Value = id;
                    SqlCommand commandoDelete = new SqlCommand(queryDelete, db.Connection);
                    commandoDelete.Parameters.Add(paramId);
                    int recordsAffected = commandoDelete.ExecuteNonQuery();
                    db.DesconectarSQL();

                    if (recordsAffected == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }


                }
                else
                {
                    return false;
                }
            }

            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR EliminarProducto  " + error);
                return false;
            }
        }
        public static bool EliminarProductoVendidoPorProducto(long id)

        {
            try
            {

                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                    var queryDelete = @" DELETE
                                       ProductoVendido
                                       WHERE 
                                       IdProducto = @ID
                                       ";
                    var paramId = new SqlParameter("id", SqlDbType.BigInt);
                    paramId.Value = id;
                    SqlCommand commandoDelete = new SqlCommand(queryDelete, db.Connection);
                    commandoDelete.Parameters.Add(paramId);
                    int recordsAffected = commandoDelete.ExecuteNonQuery();
                    db.DesconectarSQL();

                    if (recordsAffected == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR EliminarProducto  " + error);
                return false;
            }
        }

        public bool EliminarProductoVendidoPorIdVenta(long idVenta)

        {
            try
            {
                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                    SqlCommand cmd = db.Connection.CreateCommand();
                                       
                    //recorro tabla Producto vendido para el IdVenta a eliminar
                    cmd.CommandText = "SELECT Id,Stock,IdProducto " +
                                      "FROM ProductoVendido " +
                                      "WHERE IdVenta=@IdVenta";
                    var paramIdVen = new SqlParameter("IdVenta", SqlDbType.BigInt);
                    paramIdVen.Value = idVenta;
                    cmd.Parameters.Add(paramIdVen);
                    using var reader = cmd.ExecuteReader();
                    {
                        
                        while (reader.Read())
                        {
                            long id = Convert.ToInt64(reader.GetValue(0));
                            int stock = Convert.ToInt32(reader.GetValue(1)) * -1;
                            long idProducto = Convert.ToInt64(reader.GetValue(2));
                            // sumo al stock en tabla producto del registro a eliminar
                            bool updateStockOk = _productoRepository.ActualizarStock(idProducto, stock);
                        }
                        reader.Close();
                    }
                                                            
                    db.DesconectarSQL();

                    bool delOk = EliminarProductoVendido(idVenta);
                    if (delOk)
                    {                                         
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR EliminarProductoVendidoPorIdVenta  " + error);
                return false;
            }
        }
    }
}
