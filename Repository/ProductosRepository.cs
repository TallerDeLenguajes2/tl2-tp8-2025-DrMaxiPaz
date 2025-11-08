using Microsoft.Data.Sqlite;
using Models;
using Interface;

namespace Repository;

class ProductosRepository : IProductos
{
    string cadenaConeccion = "Data Source=db/Tienda.db";
    public void CrearProducto(Productos producto)
    {
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        string sql = "INSERT INTO Productos (Descripcion , Precio) VALUES(@Descripcion , @Precio)";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
        comando.ExecuteNonQuery();
        conexion.Close();
    }

    public void ActualizarProducto(Productos producto)
    {
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        string sql = "UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE IdProducto = @IdProducto";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
        comando.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
        comando.Parameters.AddWithValue("@Precio", producto.Precio);
        comando.ExecuteNonQuery();

        conexion.Close();
    }

    public List<Productos> ListarProductos()
    {
        var productos = new List<Productos>();
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        string sql = "SELECT IdProducto, Descripcion, Precio FROM Productos";
        using var comando = new SqliteCommand(sql, conexion);
        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            var p = new Productos
            {
                IdProducto = Convert.ToInt32(lector["IdProducto"]),
                Descripcion = lector["Descripcion"].ToString(),
                Precio = Convert.ToInt32(lector["Precio"])
            };
            productos.Add(p);
        }
        conexion.Close();
        return productos;
    }

    public Productos ObtenerProductoPorId(int id)
    {
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        string sql = "SELECT IdProducto, Descripcion, Precio FROM Productos WHERE IdProducto = @IdProducto";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@IdProducto", id));
        using var lector = comando.ExecuteReader();
        if (lector.Read()) // si encontró un registro
        {
            var producto = new Productos
            {
                IdProducto = Convert.ToInt32(lector["IdProducto"]),
                Descripcion = lector["Descripcion"].ToString(),
                Precio = Convert.ToInt32(lector["Precio"])
            };
            conexion.Close();
            return producto;
        }
        return null;
    }

    public void EliminarProducto(int id)
    {
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        string sql = "DELETE FROM Productos WHERE IdProducto = @IdProducto";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@IdProducto", id));
        comando.ExecuteNonQuery();
        conexion.Close();
    }

}
