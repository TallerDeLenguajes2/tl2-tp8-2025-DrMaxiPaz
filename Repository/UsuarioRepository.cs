using Models;
using Interface;
using Microsoft.Data.Sqlite;
namespace Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string cadenaConeccion;

    public UsuarioRepository()
    {
        cadenaConeccion = "Data Source=db/Tienda.db";
    }
    // Lógica para conectar con la DB y buscar por user/pass.
    public Usuario GetUser(string usuario, string contrasena)
    {
        Usuario user = null;
        //Consulta SQL que busca por Usuario Y Contrasena
        const string sql = @"SELECT Id, Nombre, User, Pass, Rol FROM Usuarios
        WHERE User = @Usuario AND Pass = @Contrasena";
        using var conexion = new SqliteConnection(cadenaConeccion);
        conexion.Open();
        using var comando = new SqliteCommand(sql, conexion);

        // Se usan parámetros para prevenir inyección 
        comando.Parameters.AddWithValue("@Usuario", usuario);
        comando.Parameters.AddWithValue("@Contrasena", contrasena);
        using var reader = comando.ExecuteReader();
        if (reader.Read())
        {
            // Si el lector encuentra una fila, el usuario existe y las credenciales son correctas
            user = new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };
        }
        return user;
    }
}