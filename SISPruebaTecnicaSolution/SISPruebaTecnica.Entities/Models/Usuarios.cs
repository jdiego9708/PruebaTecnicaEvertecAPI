using Newtonsoft.Json;
using SISPruebaTecnica.Entities.Helpers;
using System.Data;

namespace SISPruebaTecnica.Entities.Models
{
    public class Usuarios
    {
        public Usuarios()
        {
            this.Nombres = string.Empty;
            this.Apellidos = string.Empty;
            this.Foto_usuario = string.Empty;
            this.Estado_civil = string.Empty;
        }
        public Usuarios(DataRow row)
        {
            this.Id_usuario = ConvertValueHelper.ConvertIntValue(row["Id_usuario"]);
            this.Nombres = ConvertValueHelper.ConvertStringValue(row["Nombres"]);
            this.Apellidos = ConvertValueHelper.ConvertStringValue(row["Apellidos"]);
            this.Fecha_nacimiento = ConvertValueHelper.ConvertDateValue(row["Fecha_nacimiento"]);
            this.Foto_usuario = ConvertValueHelper.ConvertStringValue(row["Foto_usuario"]);
            this.Estado_civil = ConvertValueHelper.ConvertStringValue(row["Estado_civil"]);
            this.Hermanos = ConvertValueHelper.ConvertBoolean(row["Hermanos"]);
        }

        public int Id_usuario { get; set; }
        [JsonProperty("nombre")]
        public string Nombres { get; set; }
        [JsonProperty("apellido")]
        public string Apellidos { get; set; }
        [JsonProperty("fechaNacimiento")]
        public DateTime Fecha_nacimiento { get; set; }
        [JsonProperty("fotoUsuario")]
        public string Foto_usuario { get; set; }
        [JsonProperty("estadoCivil")]
        public string Estado_civil { get; set; }
        public bool Hermanos { get; set; }
    }
}
