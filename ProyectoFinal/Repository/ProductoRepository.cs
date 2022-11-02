using ProyectoFinal.Models;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace ProyectoFinal.Repository
{
      
    public class ProductoRepository
    {
        public List<Producto> TraerProductos()
        {
            var listaProductos = new List<Producto>();
            try
            {
                DataSql db = new DataSql();

                if (db.ConectarSQL())
                {
                    SqlCommand cmd = db.Connection.CreateCommand();
                    cmd.CommandText = "SELECT Id,Descripciones,Costo,PrecioVenta,Stock,IdUsuario " +
                                      "FROM Producto";
                    using var reader = cmd.ExecuteReader();
                    {
                        while (reader.Read())
                        {
                            var producto = new Producto();
                            producto.Id = Convert.ToInt64(reader.GetValue(0));
                            producto.Descripciones = reader.GetValue(1).ToString();
                            producto.Costo = Convert.ToDouble(reader.GetValue(2));
                            producto.PrecioVenta = Convert.ToDouble(reader.GetValue(3));
                            producto.Stock = Convert.ToInt32(reader.GetValue(4));
                            producto.IdUsuario = Convert.ToInt32(reader.GetValue(5));
                            listaProductos.Add(producto);
                        }
                        reader.Close();
                        db.DesconectarSQL();
                    }
                }
                return listaProductos;
            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR TraerProductos  " + error);
                return listaProductos;
            }
        }

        public bool CrearProducto(Producto producto)

        {
            bool respuesta = false;
            try
            {
                if (producto.Id == 0) //Valido que el id que viene sea 0 
                {
                    double costo;
                    double precioventa;
                    int stock;
                    long idusuario;

                    // Valido los datos recibidos sean los correctos
                    bool costoOk = double.TryParse(producto.Costo.ToString(), out costo);
                    bool precioVentaOk = double.TryParse(producto.PrecioVenta.ToString(), out precioventa);
                    bool stockOK = int.TryParse(producto.Stock.ToString(), out stock);
                    bool idusuarioOK = long.TryParse(producto.IdUsuario.ToString(), out idusuario);

                    //if (!String.IsNullOrEmpty(producto.Descripciones) ||
                    //    !costoOk || !precioVentaOk || !stockOK || !idusuarioOK)
                    if (!String.IsNullOrEmpty(producto.Descripciones) && producto.Costo >0
                        && producto.PrecioVenta>0 && producto.Stock>0 && producto.IdUsuario>0 )
                    {


                        //Verifico si el nombre del producto ya se encuentra ingresado
                        bool chkproductoIngresado = TraerProductoPorNombre(producto.Descripciones);
                        if (!chkproductoIngresado) // Si el Producto no existe lo agrego
                        {
                            DataSql db = new DataSql();
                            if (db.ConectarSQL())
                            {
                                var queryInsert = @"Insert Into Producto (Descripciones, Costo
                                                   ,PrecioVenta,Stock, IdUsuario) 
                                                  values (@Descripciones, @Costo,@PrecioVenta,@Stock, @IdUsuario); select @@IDENTITY";
                                var paramDesNuevo = new SqlParameter("Descripciones", SqlDbType.VarChar);
                                paramDesNuevo.Value = producto.Descripciones;

                                var paramCostoNuevo = new SqlParameter("Costo", SqlDbType.Money);
                                paramCostoNuevo.Value = producto.Costo;

                                var paramPVNuevo = new SqlParameter("PrecioVenta", SqlDbType.Money);
                                paramPVNuevo.Value = producto.PrecioVenta;

                                var paramStockNuevo = new SqlParameter("Stock", SqlDbType.Int);
                                paramStockNuevo.Value = producto.Stock;

                                var paramIdUsuNuevo = new SqlParameter("IdUsuario", SqlDbType.BigInt);
                                paramIdUsuNuevo.Value = producto.IdUsuario;

                                SqlCommand commandoInsert = new SqlCommand(queryInsert, db.Connection);
                                commandoInsert.Parameters.Add(paramDesNuevo);
                                commandoInsert.Parameters.Add(paramCostoNuevo);
                                commandoInsert.Parameters.Add(paramPVNuevo);
                                commandoInsert.Parameters.Add(paramStockNuevo);
                                commandoInsert.Parameters.Add(paramIdUsuNuevo);

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
                    }
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

        public bool TraerProductoPorNombre(string descripciones)
        {
            bool prouctoExiste = false;
            try
            {
                DataSql db = new DataSql();

                if (db.ConectarSQL())
                {
                    SqlCommand cmd = db.Connection.CreateCommand();
                    cmd.CommandText = "SELECT Id FROM producto WHERE Descripciones=@DesProd";
                    var paramNomProd = new SqlParameter("DesProd", SqlDbType.VarChar);
                    paramNomProd.Value = descripciones;
                    cmd.Parameters.Add(paramNomProd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            prouctoExiste = true;
                        }
                    }
                    db.DesconectarSQL();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR TraerProductoPorNombre  " + error);
                prouctoExiste = true;
            }
            return prouctoExiste;
        }

        public bool ModificarProducto(Producto producto)

        {
            try
            {
                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                    var queryUpdate = @"UPDATE Producto set 
                                     Descripciones =@Descripciones
                                    ,costo=@Costo
                                    ,Precioventa=@PV
                                    ,Stock = @Stock
                                    ,Idusuario=@IdUsu 
                                    WHERE Id = @Id";

                    var paramDes = new SqlParameter("Descripciones", SqlDbType.VarChar);
                    paramDes.Value = producto.Descripciones;
                    var paramCosto = new SqlParameter("Costo", SqlDbType.Money);
                    paramCosto.Value = producto.Costo;
                    var paramPV = new SqlParameter("PV", SqlDbType.Money);
                    paramPV.Value = producto.PrecioVenta;
                    var paramStock = new SqlParameter("Stock", SqlDbType.Int);
                    paramStock.Value = producto.Stock;
                    var paramIdUsu = new SqlParameter("IdUsu", SqlDbType.BigInt);
                    paramIdUsu.Value = producto.IdUsuario;
                    var paramId = new SqlParameter("Id", SqlDbType.BigInt);
                    paramId.Value = producto.Id;
                    SqlCommand commandoUpdate = new SqlCommand(queryUpdate, db.Connection);
                    commandoUpdate.Parameters.Add(paramDes);
                    commandoUpdate.Parameters.Add(paramCosto);
                    commandoUpdate.Parameters.Add(paramPV);
                    commandoUpdate.Parameters.Add(paramStock);
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
                Console.WriteLine("\nERROR ModificarProducto  " + error);
                return false;
            }

        }

        public bool EliminarProducto(long id)

        {
            try
            {

                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                    // si el el id existe en la tabla productovendido lo elimino
                    //para evitar error cuando elimino de la tabla producto
                    ProductoVendidoRepository.EliminarProductoVendidoPorProducto(id); 
                    var queryDelete = @" DELETE
                                    Producto
                                    WHERE 
                                    Id = @ID
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
        public bool ActualizarStock(long id,int descontarStock)
        {
            try
            {
                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                    var queryUpdate = @"UPDATE Producto set 
                                     Stock = Stock - @Stock
                                     WHERE Id = @Id";

                    var paramStock = new SqlParameter("Stock", SqlDbType.Int);
                    paramStock.Value = descontarStock;
                    var paramId = new SqlParameter("Id", SqlDbType.BigInt);
                    paramId.Value = id;
                    SqlCommand commandoUpdate = new SqlCommand(queryUpdate, db.Connection);
                    commandoUpdate.Parameters.Add(paramStock);
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
                Console.WriteLine("\nERROR ActualizarStock  " + error);
                return false;
            }
        }
    }

}
