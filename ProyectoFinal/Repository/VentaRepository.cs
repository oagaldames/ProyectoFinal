using ProyectoFinal.Models;
using System.Data.SqlClient;
using System.Data;
using System;
using System.ComponentModel;

namespace ProyectoFinal.Repository
{
    public class VentaRepository
    {
        public ProductoVendidoRepository _productoVendidoRepository;

        public ProductoRepository _productoRepository;

        public VentaRepository()
        {
            _productoVendidoRepository = new ProductoVendidoRepository();
            _productoRepository = new ProductoRepository();
        }
        public bool CargarVenta(List<Producto> productos, int idUsuario)
        {
            bool procesoOk = false;
            int cantidad = productos.Count();
            try
            {
                //Agrego nueva venta en tabla Venta
                long idVentaNueva = CrearVenta("", idUsuario);
                    
                if (idVentaNueva > 0) // Verifico que se agrego la venta y traigo el Id generado
                {
                    foreach (Producto producto in productos)
                    {
                        //Agrego en tabla ProductoVendido el producto de la lista
                        bool insertProductovendido = _productoVendidoRepository.CrearProductoVendido(producto.Stock, producto.Id, idVentaNueva);
                        if (insertProductovendido)  //Verifico si se creo el nuevo registro en Tabla ProductoVendido
                        {
                            // Actualizo Stock en tabla productos
                            bool updateStockOk =_productoRepository.ActualizarStock(producto.Id, producto.Stock);
                            if (updateStockOk)
                            {
                                cantidad--;
                            }
                        }
                    }

                    if (cantidad == 0)
                    {
                        procesoOk = true;
                    }
                }
                
            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR CargarVenta  " + error);
            }
            return procesoOk;
        }

        public List<VentaConProducto> TraerVentas()
        {
            var listaVenta = new List<VentaConProducto>();
            try
            {
                DataSql db = new DataSql();

                if (db.ConectarSQL())
                {
                    SqlCommand cmd = db.Connection.CreateCommand();
                    cmd.CommandText = "SELECT v.id,p.Descripciones,pv.Stock," +
                        "v.Comentarios,v.IdUsuario " +
                        "FROM venta v INNER JOIN ProductoVendido pv " +
                        "ON v.id=pv.Idventa " +
                        "INNER JOIN producto p " +
                        "ON pv.IdProducto=p.id ";
                    using var reader = cmd.ExecuteReader();
                    {
                        while (reader.Read())
                        {
                            var venta = new VentaConProducto();
                            venta.Id = Convert.ToInt32(reader.GetValue(0).ToString());
                            venta.Descripcion = reader.GetValue(1).ToString();
                            venta.Cantidad = Convert.ToInt32(reader.GetValue(2).ToString());
                            venta.Comentarios = reader.GetValue(3).ToString();
                            venta.IdUsuario = Convert.ToInt32(reader.GetValue(4));

                            listaVenta.Add(venta);

                        }
                        reader.Close();
                    }
                    db.DesconectarSQL();
                }
                return listaVenta;
            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR TraerVentas" + error);
                return listaVenta;
            }
        }
        public long CrearVenta(string comentarios, int idUsuario)

        {
            long idNuevo = 0;
            try
            {
                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                    var queryInsert = @"Insert Into Venta (Comentarios, IdUsuario) 
                                          values (@comen,@IdUsu); select @@IDENTITY";
                    var paramComen = new SqlParameter("comen", SqlDbType.VarChar);
                    paramComen.Value = comentarios;
                    var paramIdUsu = new SqlParameter("IdUsu", SqlDbType.BigInt);
                    paramIdUsu.Value = idUsuario;
                    
                    SqlCommand commandoInsert = new SqlCommand(queryInsert, db.Connection);
                    commandoInsert.Parameters.Add(paramComen);
                    commandoInsert.Parameters.Add(paramIdUsu);
                    
                    idNuevo = Convert.ToInt64(commandoInsert.ExecuteScalar());
                    
                }
               
            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR CrearVenta  " + error);
                
            }
            return idNuevo;
        }
        public bool ModificarVenta(long id,string comentarios, int idUsuario)

        {
            try
            {
                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                    var queryUpdate = @"UPDATE Venta set 
                                      Comentarios = @Comen
                                      ,IdUsuario = @IdUsu
                                      WHERE Id = @Id";


                    var paramComen = new SqlParameter("comen", SqlDbType.VarChar);
                    paramComen.Value = comentarios;
                    var paramIdUsu = new SqlParameter("IdUsu", SqlDbType.BigInt);
                    paramIdUsu.Value = idUsuario;
                    var paramId = new SqlParameter("id", SqlDbType.BigInt);
                    paramId.Value = id;

                    SqlCommand commandoUpdate = new SqlCommand(queryUpdate, db.Connection);
                    commandoUpdate.Parameters.Add(paramComen);
                    commandoUpdate.Parameters.Add(paramIdUsu);
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
                Console.WriteLine("\nERROR ModificarVenta  " + error);
                return false;
            }
        }
        public bool EliminarVenta(long id)

        {
            try
            {
                // Eliminar en tabla ProductoVendido los asociados a IdVenta
                if (_productoVendidoRepository.EliminarProductoVendidoPorIdVenta(id))
                {
                    DataSql db = new DataSql();
                    if (db.ConectarSQL())
                    {
                        var queryDelete = @"DELETE Venta WHERE Id = @id
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
                else
                {
                    return false;
                }
                
            }

            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR EliminarVenta  " + error);
                return false;
            }
        }

    }

    
}
