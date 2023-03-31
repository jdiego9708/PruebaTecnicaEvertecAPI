using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SISPruebaTecnica.DataAccess.Dacs;
using SISPruebaTecnica.DataAccess.Interfaces;
using SISPruebaTecnica.Entities.ModelsConfiguration;
using SISPruebaTecnica.Services.Interfaces;
using SISPruebaTecnica.Services.Services;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
         .AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Assembly GetAssemblyByName(string name)
{
    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().
           SingleOrDefault(assembly => assembly.GetName().Name == name);

    if (assembly == null)
        return null;

    return assembly;
}

var a = GetAssemblyByName("SISPruebaTecnica.API");

using var stream = a.GetManifestResourceStream("SISPruebaTecnica.API.appsettings.json");

var config = new ConfigurationBuilder()
    .AddJsonStream(stream)
    .Build();

#region SECURITY IN JWT

var settings = builder.Configuration.GetSection("Jwt");
JwtModel jwtSecurity = settings.Get<JwtModel>();

if (jwtSecurity == null)
    return;

var llave = Encoding.UTF8.GetBytes(jwtSecurity.Secreto);

builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(llave)
        };
    });

builder.Services.AddCors();

#endregion

#region INYECCION DE DEPENDENCIAS
builder.Services.AddTransient<IConnectionDac, ConnectionDac>();
builder.Services.AddTransient<IUsuariosService, UsuariosService>();
builder.Services.AddTransient<IUsuarioDac, UsuariosDac>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithOrigins("http://localhost:4200");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
