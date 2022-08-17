using Intelitrader.Models;

namespace Intelitrader.Repository
{
    public interface IUsuarioRepository
    {
        Task <IEnumerable<Usuario>> BuscaUsuarios();
        Task<Usuario> BuscaUsuario(string id);
        void AdicionaUsuario(Usuario usuario);
        void AtualizaUsuario(Usuario usuario);
        void DeleteUsuario(Usuario usuario);

        Task<bool> SaveChangesAsync();
        
    }
}
