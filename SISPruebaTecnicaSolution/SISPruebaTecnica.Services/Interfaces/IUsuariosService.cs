using SISPruebaTecnica.Entities.Models;
using SISPruebaTecnica.Entities.ModelsConfiguration;

namespace SISPruebaTecnica.Services.Interfaces
{
    public interface IUsuariosService
    {
        RestResponseModel ProcessLogin(LoginModel loginModel);
        RestResponseModel InsertarUsuario(Usuarios usuario);
        RestResponseModel BuscarUsuarios(SearchBindingModel search);
    }
}
