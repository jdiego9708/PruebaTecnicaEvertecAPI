namespace SISPruebaTecnica.Entities.ModelsConfiguration
{
    public class SearchBindingModel
    {
        public SearchBindingModel()
        {
            this.Type_search = string.Empty;
            this.Value_search = string.Empty;
        }
        public string Type_search { get; set; }
        public string Value_search { get; set; }
    }
}
