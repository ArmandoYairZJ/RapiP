using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace RapiPirata.DB
{
    class OpMySQL
    {

        //Conexion a la base de datos
        public string ipDB = "192.168.137.37";
        public string db = "RapiPirata";
        public MySqlConnection conexDb(string Correo, string Contrasena)
        {
            string conxstring = $"Server={ipDB};Database={db};User={Correo};Password={Contrasena};SslMode=None;";
            return new MySqlConnection(conxstring);
        }

        //verificaicones de usaurios 

        public bool InSesion(string Correo, string Contrasena)
        {

            using (MySqlConnection conex = conexDb(Correo, Contrasena))
            {
                try
                {
                    if (VeriCliente(Correo, Contrasena))
                    {
                        conex.Open();
                        return true;
                    }
                    else if (VeriUsuario(Correo, Contrasena))
                    {
                        conex.Open();
                        return true;
                    }
                    else
                    {
                        Console.Write("Error en la conexion");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al conectar con la base de datos: " + ex.Message);
                    return false;
                }

            }


        }

        public bool VeriCliente(string Correo, string Contrasena)
        {
            using (MySqlConnection conex = conexDb(Correo, Contrasena))
            {

                try
                {
                    conex.Open();
                    string cd = "SELECT COUNT(*) FROM Vendedor WHERE correo = @correo AND contrasena = @Contrasena";

                    using (var comando = new MySqlCommand(cd, conex))
                    {
                        comando.Parameters.AddWithValue("@correo", Correo);
                        comando.Parameters.AddWithValue("@Contrasena", Contrasena);

                        int resultado = Convert.ToInt32(comando.ExecuteScalar());

                        return resultado > 0;
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error al conectar con la base de datos: " + ex.Message);
                    return false;
                }
            }

        }

        public bool VeriUsuario(string Correo, string Contrasena)
        {
            using (MySqlConnection conex = conexDb(Correo, Contrasena))
            {

                try
                {
                    conex.Open();
                    string cd = "SELECT COUNT(*) FROM Cliente WHERE correo = @correo AND Contrasena = @contrasena";

                    using (var comando = new MySqlCommand(cd, conex))
                    {
                        comando.Parameters.AddWithValue("@correo", Correo);
                        comando.Parameters.AddWithValue("@Contrasena", Contrasena);

                        int resultado = Convert.ToInt32(comando.ExecuteScalar());

                        return resultado > 0;
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error al conectar con la base de datos: " + ex.Message);
                    return false;
                }
            }

        }

        //Agreagar Productos

        public bool RegistrarProductos(string correo, string Contrasena, string ProductoID, string Descripcion, decimal Pventa, float saldo, byte[] ImagenBytes)
        {

            MySqlConnection conexion = conexDb(correo, Contrasena);
            conexion.Open();

            try
            {
                string cd = "Insert into Producto(ProductoID, Descripcion, PVenta, Saldo, Imagen) " +
                    "Values(@ProductoID, @Descripcion, @PVenta, @Saldo, @Imagen)";
                MySqlCommand com;
                com = new MySqlCommand(cd, conexion);
                com.Parameters.AddWithValue("@ProductoID", ProductoID);
                com.Parameters.AddWithValue("@Descripcion", Descripcion);
                com.Parameters.AddWithValue("@PVenta", Pventa);
                com.Parameters.AddWithValue("@Saldo", saldo);
                com.Parameters.AddWithValue("@Imagen", ImagenBytes);

                com.ExecuteNonQuery();
                conexion.Close();
                return true;
            }

            catch (Exception ex)
            {

                Console.WriteLine("Se produjo un error" + ex.Message);
                return false;
            }
        }


        //Consulta de Productos 

        public DataTable MostrarProductos(string correo, string Contrasena)
        {

            DataTable productosTable = new DataTable();

            using (MySqlConnection conexion = conexDb(correo, Contrasena))
            {
                conexion.Open();
                try
                {
                    string query = "SELECT * FROM Producto";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            productosTable.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Se produjo un error: " + ex.Message);
                    throw; 
                }
                finally
                {
                    conexion.Close();
                }
            }

            return productosTable;
        }

        public byte[] ObtenerImagen(string correo, string contrasena, int idProducto)
        {
            byte[] imagenBytes = null;

            using (MySqlConnection conexion = conexDb(correo, contrasena))
            {
                conexion.Open();

                string query = "SELECT imagen FROM Producto WHERE id = @ProductoID";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                imagenBytes = (byte[])reader["imagen"];
                            }
                        }
                    }
                }
            }

            return imagenBytes;
        }

    }
}
