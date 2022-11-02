using ProyectoFinal.Models;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace ProyectoFinal.Repository
{
    public class UsuarioRepository
    {
        public List<Usuario> TraerUsuarios()
        {
            var listaUsuarios = new List<Usuario>();
            try
            {
                DataSql db = new DataSql();

                if (db.ConectarSQL())
                {
                    SqlCommand cmd = db.Connection.CreateCommand();
                    cmd.CommandText = "SELECT Id,Nombre,Apellido,NombreUsuario,Contraseña,Mail FROM Usuario ";

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var usuario = new Usuario();
                                usuario.Id = Convert.ToInt32(reader.GetValue(0));
                                usuario.Nombre = reader.GetValue(1).ToString();
                                usuario.Apellido = reader.GetValue(2).ToString();
                                usuario.NombreUsuario = reader.GetValue(3).ToString();
                                usuario.Contraseña = reader.GetValue(4).ToString();
                                usuario.Mail = reader.GetValue(5).ToString();
                                listaUsuarios.Add(usuario);
                            }
                        }
                    }
                    db.DesconectarSQL();
                }
                return listaUsuarios;
            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR TraerUsuarios  " + error);
                return listaUsuarios;
            }

        }
        public Usuario TraerUsuarioPorNombre(string Nombre)
        {
            var usuarioNom = new Usuario();
            try
            {
                DataSql db = new DataSql();

                if (db.ConectarSQL())
                {
                    SqlCommand cmd = db.Connection.CreateCommand();
                    cmd.CommandText = "SELECT Id,Nombre,Apellido,NombreUsuario,Contraseña,Mail FROM Usuario WHERE NombreUsuario=@NomUsu";
                    var paramNomUsu = new SqlParameter("NomUsu", SqlDbType.VarChar);
                    paramNomUsu.Value = Nombre;
                    cmd.Parameters.Add(paramNomUsu);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                usuarioNom.Id = Convert.ToInt32(reader.GetValue(0));
                                usuarioNom.Nombre = reader.GetValue(1).ToString();
                                usuarioNom.Apellido = reader.GetValue(2).ToString();
                                usuarioNom.NombreUsuario = reader.GetValue(3).ToString();
                                usuarioNom.Contraseña = reader.GetValue(4).ToString();
                                usuarioNom.Mail = reader.GetValue(5).ToString();
                            }
                        }
                        else
                        {
                            usuarioNom.Id = 0;
                            usuarioNom.Nombre = String.Empty;
                            usuarioNom.Apellido = String.Empty;
                            usuarioNom.Contraseña = String.Empty;
                            usuarioNom.Mail = String.Empty;

                        }
                        //listaUsuarios.Add(usuarioNom);

                    }

                    db.DesconectarSQL();
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR TraerUsuarioPorNombre  " + error);

            }
            return usuarioNom;
        }
        public bool TraerUsuarioPorNombreUsuario(string NombreUsuario)
        {
            bool usuarioExiste = false;
            try
            {
                DataSql db = new DataSql();

                if (db.ConectarSQL())
                {
                    SqlCommand cmd = db.Connection.CreateCommand();
                    cmd.CommandText = "SELECT Id FROM Usuario WHERE NombreUsuario=@NomUsu";
                    var paramNomUsu = new SqlParameter("NomUsu", SqlDbType.VarChar);
                    paramNomUsu.Value = NombreUsuario;
                    cmd.Parameters.Add(paramNomUsu);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            usuarioExiste = true;
                        }
                    }
                    db.DesconectarSQL();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR TraerUsuarioPorNombreUsuario  " + error);
                usuarioExiste = true;
            }
            return usuarioExiste;
        }
        public Usuario LoginUsuario(string NombreUsuario, string Pass)
        {
            var listaUsuarios = new List<Usuario>();
            try
            {
                DataSql db = new DataSql();

                if (db.ConectarSQL())
                {
                    SqlCommand cmd = db.Connection.CreateCommand();
                    cmd.CommandText = "SELECT Id,Nombre,Apellido,NombreUsuario,Contraseña,Mail FROM Usuario WHERE NombreUsuario LIKE @NomUsu AND Contraseña=@passUsu";

                    var paramNomUsu = new SqlParameter("NomUsu", SqlDbType.VarChar);
                    paramNomUsu.Value = NombreUsuario;

                    var paramPass = new SqlParameter("passUsu", SqlDbType.VarChar);
                    paramPass.Value = Pass;

                    cmd.Parameters.Add(paramNomUsu);
                    cmd.Parameters.Add(paramPass);

                    using (var reader = cmd.ExecuteReader())
                    {

                        var usuario = new Usuario();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                usuario.Id = Convert.ToInt32(reader.GetValue(0));
                                usuario.Nombre = reader.GetValue(1).ToString();
                                usuario.Apellido = reader.GetValue(2).ToString();
                                usuario.NombreUsuario = reader.GetValue(3).ToString();
                                usuario.Contraseña = reader.GetValue(4).ToString();
                                usuario.Mail = reader.GetValue(5).ToString();
                            }
                        }
                        else
                        {
                            usuario.Id = 0;
                            usuario.Nombre = String.Empty;
                            usuario.Apellido = String.Empty;
                            usuario.Contraseña = String.Empty;
                            usuario.Mail = String.Empty;

                        }
                        listaUsuarios.Add(usuario);
                    }
                    db.DesconectarSQL();
                }

            }
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR LoginUsuario  " + error);

            }
            var usuariologin = new Usuario();
            usuariologin = listaUsuarios[0];
            return usuariologin;

        }

        public bool CrearUsuario(Usuario usuario)

        {
            bool respuesta = false;
            try
            {
                // Valido que los datos ingresados no esten vacios
                if (!string.IsNullOrEmpty(usuario.Nombre) && !string.IsNullOrEmpty(usuario.Apellido) &&
                    !string.IsNullOrEmpty(usuario.NombreUsuario) && !string.IsNullOrEmpty(usuario.Contraseña) &&
                    !string.IsNullOrEmpty(usuario.Mail))
                {
                    //Verifico si el nombre de usuario ya se encuentra ingresado
                    bool chkUsuarioIngresado = TraerUsuarioPorNombreUsuario(usuario.NombreUsuario);
                    if (!chkUsuarioIngresado) // Si el nombre de usuario no existe lo agrego
                    {
                        DataSql db = new DataSql();
                        if (db.ConectarSQL())
                        {
                            var queryInsert = @"Insert Into Usuario (Nombre, Apellido,NombreUsuario,Contraseña, Mail) 
                                              values (@Nombre, @Apellido,@NombreUsuario,@Contraseña, @Mail);select @@IDENTITY";
                            var paramNomNuevo = new SqlParameter("Nombre", SqlDbType.VarChar);
                            paramNomNuevo.Value = usuario.Nombre;

                            var paramApeNuevo = new SqlParameter("Apellido", SqlDbType.VarChar);
                            paramApeNuevo.Value = usuario.Apellido;

                            var paramUsuNuevo = new SqlParameter("NombreUsuario", SqlDbType.VarChar);
                            paramUsuNuevo.Value = usuario.NombreUsuario;

                            var paramConNuevo = new SqlParameter("Contraseña", SqlDbType.VarChar);
                            paramConNuevo.Value = usuario.Contraseña;

                            var paramMailNuevo = new SqlParameter("Mail", SqlDbType.VarChar);
                            paramMailNuevo.Value = usuario.Mail;

                            SqlCommand commandoInsert = new SqlCommand(queryInsert, db.Connection);
                            commandoInsert.Parameters.Add(paramNomNuevo);
                            commandoInsert.Parameters.Add(paramApeNuevo);
                            commandoInsert.Parameters.Add(paramUsuNuevo);
                            commandoInsert.Parameters.Add(paramConNuevo);
                            commandoInsert.Parameters.Add(paramMailNuevo);

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
            catch (Exception err)
            {
                string error = err.Message;
                Console.WriteLine("\nERROR CrearUsuario  " + error);
                respuesta = false;
            }
            return respuesta;
        }
        public bool ModificarUsuario(Usuario usuario)

        {
            try
            {
                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                    var queryUpdate = @"UPDATE Usuario set 
                                     Nombre =@Nombre
                                    ,Apellido=@Apellido
                                    ,NombreUsuario=@NombreUsuario
                                    ,Contraseña = @Contraseña
                                    ,Mail=@Mail 
                                    WHERE Id = @id";

                    var paramId = new SqlParameter("id", SqlDbType.BigInt);
                    paramId.Value = usuario.Id;
                    var paramNom = new SqlParameter("Nombre", SqlDbType.VarChar);
                    paramNom.Value = usuario.Nombre;
                    var paramApe = new SqlParameter("Apellido", SqlDbType.VarChar);
                    paramApe.Value = usuario.Apellido;
                    var paramUsu = new SqlParameter("NombreUsuario", SqlDbType.VarChar);
                    paramUsu.Value = usuario.NombreUsuario;
                    var paramCon = new SqlParameter("Contraseña", SqlDbType.VarChar);
                    paramCon.Value = usuario.Contraseña;
                    var paramMail = new SqlParameter("Mail", SqlDbType.VarChar);
                    paramMail.Value = usuario.Mail;

                    SqlCommand commandoUpdate = new SqlCommand(queryUpdate, db.Connection);
                    commandoUpdate.Parameters.Add(paramId);
                    commandoUpdate.Parameters.Add(paramNom);
                    commandoUpdate.Parameters.Add(paramApe);
                    commandoUpdate.Parameters.Add(paramUsu);
                    commandoUpdate.Parameters.Add(paramCon);
                    commandoUpdate.Parameters.Add(paramMail);

                    int recordsAffected = commandoUpdate.ExecuteNonQuery(); //Se ejecuta realmente UPDATE

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
                Console.WriteLine("\nERROR ModificarUsuario  " + error);
                return false;
            }

        }
        public bool EliminarUsuario(long id)

        {
            try
            {
                
                DataSql db = new DataSql();
                if (db.ConectarSQL())
                {
                    var queryDelete = @" DELETE
                                       Usuario
                                       WHERE 
                                       Id = @ID
                                       ";
                    var paramId = new SqlParameter("id", SqlDbType.BigInt);
                    paramId.Value = id;
                    SqlCommand commandoDelete = new SqlCommand(queryDelete, db.Connection);
                    commandoDelete.Parameters.Add(paramId);
                    int recordsAffected = commandoDelete.ExecuteNonQuery(); //Se ejecuta realmente el DELETE
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
                Console.WriteLine("\nERROR EliminarUsuario  " + error);
                return false;
            }
        }
    }
}