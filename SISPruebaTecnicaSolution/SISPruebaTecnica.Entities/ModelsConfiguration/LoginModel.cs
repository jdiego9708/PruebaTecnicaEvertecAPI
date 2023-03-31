using System;

namespace SISPruebaTecnica.Entities.ModelsConfiguration
{
    public class LoginModel
    {
        public LoginModel()
        {
            this.Usuario = string.Empty;
            this.Password = string.Empty;
        }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public DateTime FechaLogin { get; set; }
    }
}
