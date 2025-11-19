using Microsoft.Data.Sqlite;
using Models;
using Interface;

namespace Repository;

class PresupuestosRepository : IPresupuestosRepository
{
    private readonly string cadenaConeccion;

    public PresupuestosRepository()
    {
        cadenaConeccion = "Data Source=db/Tienda.db";
    }
    
    public void CrearPresupuesto(Presupuestos presupuesto)
    {
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        string sql = "INSERT INTO Presupuestos (NombreDestinatario , FechaCreacion) VALUES( @NombreDestinatario , @FechaCreacion);";
        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.AddWithValue("@NombreDestinatario", presupuesto.NombreDestinatario);
        comando.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion);
        comando.ExecuteNonQuery();
        conexion.Close();
    }

    public List<Presupuestos> ListarPresupuestos()
    {
        var presupuestos = new List<Presupuestos>();
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        string sql = "SELECT IdPresupuesto, NombreDestinatario , FechaCreacion FROM Presupuestos";
        using var comando = new SqliteCommand(sql, conexion);
        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            var p = new Presupuestos
            {
                IdPresupuesto = Convert.ToInt32(lector["IdPresupuesto"]),
                NombreDestinatario = lector["NombreDestinatario"].ToString(),
                FechaCreacion = DateOnly.Parse(lector["FechaCreacion"].ToString()),
            };
            presupuestos.Add(p);
        }
        lector.Close();
        foreach (Presupuestos presupuesto in presupuestos)
        {
            presupuesto.Detalle = [];
            string sql1 = "SELECT Cantidad, Descripcion, Precio FROM PresupuestosDetalle INNER JOIN Productos using (idProducto) WHERE idPresupuesto = @IdPresupuesto";
            using var comando1 = new SqliteCommand(sql1, conexion);
            comando1.Parameters.Add(new SqliteParameter("@IdPresupuesto", presupuesto.IdPresupuesto));
            using var lector1 = comando1.ExecuteReader();

            while (lector1.Read())
            {
                var pDetalle = new PresupuestosDetalle
                {
                    Cantidad = Convert.ToInt32(lector1["Cantidad"]),
                    Producto = new Productos
                    {
                        Descripcion = lector1["Descripcion"].ToString(),
                        Precio = Convert.ToInt32(lector1["Precio"])
                    }
                };
                presupuesto.Detalle.Add(pDetalle);
            }
            lector1.Close();
        }
        conexion.Close();
        return presupuestos;
    }

    public void AgregarProductoAPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        string sql = "INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto , Cantidad) VALUES(@IdPresupuesto, @IdProducto , @Cantidad)";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.AddWithValue("@IdPresupuesto", idPresupuesto);
        comando.Parameters.AddWithValue("@IdProducto", idProducto);
        comando.Parameters.AddWithValue("@Cantidad", cantidad);

        comando.ExecuteNonQuery();
        conexion.Close();
    }

    public Presupuestos ObtenerPresupuestoPorId(int id)
    {
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        string sql = "SELECT IdPresupuesto , NombreDestinatario , FechaCreacion  FROM Presupuestos WHERE IdPresupuesto = @IdPresupuesto";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@IdPresupuesto", id));
        using var lector = comando.ExecuteReader();
        if (lector.Read()) // si encontró un registro
        {
            var presupuesto = new Presupuestos
            {
                IdPresupuesto = Convert.ToInt32(lector["IdPresupuesto"]),
                NombreDestinatario = lector["NombreDestinatario"].ToString(),
                FechaCreacion = DateOnly.Parse(lector["FechaCreacion"].ToString()),
            };
            conexion.Close();
            return presupuesto;
        }
        conexion.Close();
        return null;
    }

    public List<PresupuestosDetalle> MostrarDetallePorId(int id){
        ProductosRepository datos = new ProductosRepository();
        string queryString=@"SELECT IdProducto, Cantidad FROM PresupuestosDetalle WHERE IdPresupuesto=@id;";
        List<PresupuestosDetalle> detalles=new List<PresupuestosDetalle>();
        using(SqliteConnection connection=new SqliteConnection(cadenaConeccion)){
            connection.Open();
            SqliteCommand command=new SqliteCommand(queryString, connection);
            command.Parameters.AddWithValue("@id", id);
            using(SqliteDataReader reader=command.ExecuteReader()){
                while(reader.Read()){
                    Productos producto= datos.ObtenerProductoPorId(Convert.ToInt32(reader["IdProducto"]));
                    int cantidad=Convert.ToInt32(reader["Cantidad"]);
                    PresupuestosDetalle detalle= new PresupuestosDetalle(producto, cantidad);
                    detalles.Add(detalle);
                }
            }
            connection.Close();
        }
        return detalles;
    }

    public void EliminarPresupuesto(int id)
    {
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        string sql = "DELETE FROM Presupuestos WHERE IdPresupuesto = @IdPresupuesto";
        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@IdPresupuesto", id));
        comando.ExecuteNonQuery();
    }
}
