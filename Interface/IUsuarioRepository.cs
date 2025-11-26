using Models;
namespace Interface;

public interface IUsuarioRepository
{
    Usuario GetUser(string username, string password);
}