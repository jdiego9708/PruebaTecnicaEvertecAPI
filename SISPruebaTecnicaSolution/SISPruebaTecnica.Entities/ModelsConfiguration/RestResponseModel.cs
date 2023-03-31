namespace SISPruebaTecnica.Entities.ModelsConfiguration
{
    public class RestResponseModel
    {
        public RestResponseModel()
        {
            this.Response = string.Empty;
        }
        public bool IsSucess { get; set; }
        public string Response { get; set; }
    }
}
