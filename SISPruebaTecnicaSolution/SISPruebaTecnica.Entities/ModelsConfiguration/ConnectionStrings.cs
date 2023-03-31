namespace SISPruebaTecnica.Entities.ModelsConfiguration
{
    public class ConnectionStrings
    {
        public ConnectionStrings()
        {
            this.ConexionBDPredeterminada = string.Empty;
            this.ConexionBDAzure = string.Empty;
            this.ConexionBDLocal = string.Empty;
        }
        public string ConexionBDPredeterminada { get; set; }
        public string ConexionBDAzure { get; set; }
        public string ConexionBDLocal { get; set; }
    }
}
