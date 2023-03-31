using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SISPruebaTecnica.DataAccess.Dacs;
using SISPruebaTecnica.DataAccess.Interfaces;
using SISPruebaTecnica.Entities.Models;
using SISPruebaTecnica.Entities.ModelsConfiguration;
using SISPruebaTecnica.Services.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SISPruebaTecnica.Services.Services
{
    public class UsuariosService : IUsuariosService
    {
        #region CONSTRUCTOR AND DEPENDENCY INYECTION
        public JwtModel Jwt { get; set; }
        public IUsuarioDac IUsuarioDac { get; set; }
        public UsuariosService(IConfiguration Configuration,
            IUsuarioDac IUsuarioDac)
        {
            this.IUsuarioDac = IUsuarioDac;

            var settings = Configuration.GetSection("Jwt");

            if (settings == null)
            {
                this.Jwt = new();
                return;
            }

            var modelSecurity = settings.Get<JwtModel>();

            if (modelSecurity == null)
            {
                this.Jwt = new();
                return;
            }

            this.Jwt = modelSecurity;
        }
        #endregion

        #region METHODS
        private string GetTokenLogin(string user)
        {
            DateTime dateExpired = DateTime.UtcNow.AddHours(+12);
            var tokenHandler = new JwtSecurityTokenHandler();
            var llave = Encoding.UTF8.GetBytes(this.Jwt.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Name, user),
                        }
                    ),
                Expires = dateExpired,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public RestResponseModel ProcessLogin(LoginModel loginModel)
        {
            RestResponseModel response = new();
            try
            {
                if (string.IsNullOrEmpty(loginModel.Usuario))
                    throw new Exception("Verify Usuario");

                if (string.IsNullOrEmpty(loginModel.Password))
                    throw new Exception("Verify Password");

                if (loginModel.Usuario.Equals("evertecusertest"))
                {
                    if (loginModel.Password.Equals("test"))
                    {
                        string jwtToken =
                            this.GetTokenLogin(loginModel.Usuario);

                        response.IsSucess = true;
                        response.Response = JsonConvert.SerializeObject(
                            new
                            {
                                Mensaje = $"Inicio de sesión correcto, use el AccessToken " +
                                $"para hacer llamados a otros endpoints. Vencimiento {loginModel.FechaLogin.AddDays(7)}",
                                AccessToken = jwtToken,
                                loginModel.Usuario,
                            });
                        return response;
                    }
                }

                response.IsSucess = false;
                response.Response = JsonConvert.SerializeObject(
                    new
                    {
                        Mensaje = "No se pudo iniciar sesión, verifique las credenciales",
                        Usuario = loginModel.Usuario,
                    });
                return response;
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Response = ex.Message;
            }
            return response;
        }
        public RestResponseModel InsertarUsuario(Usuarios usuario)
        {
            RestResponseModel response = new();
            try
            {
                if (string.IsNullOrEmpty(usuario.Nombres))
                    throw new Exception("Verify Nombres");

                if (string.IsNullOrEmpty(usuario.Apellidos))
                    throw new Exception("Verify Apellidos");

                string rpta = this.IUsuarioDac.InsertarUsuario(usuario).Result;

                if (!rpta.Equals("OK"))
                {
                    response.IsSucess = false;
                    response.Response = JsonConvert.SerializeObject(
                        new
                        {
                            Mensaje = $"No se pudo crear el usuario"
                        });
                    return response;
                }
                else
                {
                    response.IsSucess = true;
                    response.Response = JsonConvert.SerializeObject(
                        new
                        {
                            Mensaje = $"Usuario creado con éxito",
                            Usuario = usuario.Id_usuario
                        });
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Response = ex.Message;
            }
            return response;
        }
        public RestResponseModel BuscarUsuarios(SearchBindingModel search)
        {
            RestResponseModel response = new();
            try
            {
                if (string.IsNullOrEmpty(search.Type_search))
                    throw new Exception("Verify Type_search");

                string rpta =
                       this.IUsuarioDac.SearchUsuarios(search.Type_search,
                       search.Value_search, out DataTable dtUsers);

                List<Usuarios> usuarios = new();

                if (dtUsers == null)
                {
                    if (rpta.Equals("OK"))
                    {
                        usuarios.Add(new Usuarios()
                        {
                            Nombres = "Find empty"
                        });

                        response.IsSucess = true;
                        response.Response = JsonConvert.SerializeObject(usuarios);
                        return response;
                    }
                    else
                        throw new Exception($"Error | {rpta}");
                }

                usuarios = (from DataRow row in dtUsers.Rows
                           select new Usuarios(row)).ToList();

                response.IsSucess = true;
                response.Response = JsonConvert.SerializeObject(usuarios);
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.Response = ex.Message;
            }
            return response;
        }
        #endregion
    }
}
