using SISPruebaTecnica.Entities.Models;
using System.Data;

namespace SISPruebaTecnica.DataAccess.Interfaces
{
    public interface IUsuarioDac
    {
        Task<string> InsertarUsuario(Usuarios usuario);
        string SearchUsuarios(string type_search, string value_search,
            out DataTable dt);
    }
}
