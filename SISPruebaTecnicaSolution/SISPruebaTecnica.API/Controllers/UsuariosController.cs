using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SISPruebaTecnica.Entities.Models;
using SISPruebaTecnica.Entities.ModelsConfiguration;
using SISPruebaTecnica.Services.Interfaces;

namespace SISPruebaTecnica.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UsuariosController : Controller
    {
        private readonly ILogger<UsuariosController> logger;
        private IUsuariosService IUsuariosService { get; set; }
        public UsuariosController(ILogger<UsuariosController> logger,
            IUsuariosService IUsuariosService)
        {
            this.logger = logger;
            this.IUsuariosService = IUsuariosService;
        }

        [HttpPost]
        [Route("InsertUsuarios")]
        public IActionResult InsertUsuarios(JObject usuarioJson)
        {
            try
            {
                logger.LogInformation("Start InsertUsuarios(");

                if (usuarioJson == null)
                    throw new Exception("InsertUsuarios info empty");

                Usuarios usuarioModel = usuarioJson.ToObject<Usuarios>();

                if (usuarioModel == null)
                {
                    logger.LogInformation("Not Information of usuarioModel");
                    throw new Exception("Not Information of usuarioModel");
                }
                else
                {
                    RestResponseModel rpta = this.IUsuariosService.InsertarUsuario(usuarioModel);
                    if (rpta.IsSucess)
                    {
                        logger.LogInformation($"InsertAuthors successfull");
                        return Ok(rpta.Response);
                    }
                    else
                    {
                        return BadRequest(rpta.Response);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in controller InsertAuthors", ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ProcessLogin")]
        public IActionResult ProcessLogin(JObject loginJson)
        {
            try
            {
                logger.LogInformation("Start ProcessLogin");

                if (loginJson == null)
                    throw new Exception("ProcessLogin info empty");

                LoginModel loginModel = loginJson.ToObject<LoginModel>();

                if (loginModel == null)
                {
                    logger.LogInformation("Not Information of loginModel");
                    throw new Exception("Not Information of loginModel");
                }
                else
                {
                    RestResponseModel rpta = this.IUsuariosService.ProcessLogin(loginModel);
                    if (rpta.IsSucess)
                    {
                        logger.LogInformation($"ProcessLogin successfull");
                        return Ok(rpta.Response);
                    }
                    else
                    {
                        return BadRequest(rpta.Response);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in controller ProcessLogin", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}