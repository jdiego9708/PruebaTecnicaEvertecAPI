namespace SISPruebaTecnica.Entities.ModelsConfiguration
{
    public class JwtModel
    {
        public JwtModel()
        {
            this.Issuer = string.Empty;
            this.Audience = string.Empty;
            this.Secreto = string.Empty;
        }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secreto { get; set; }
    }
}
