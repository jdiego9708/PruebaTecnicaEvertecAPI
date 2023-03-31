CREATE OR ALTER PROC sp_Usuarios_i
@Id_usuario int output,
@Nombres varchar(50),
@Apellidos varchar(50),
@Fecha_nacimiento date,
@Foto_usuario varchar(50),
@Estado_civil varchar(50),
@Hermanos bit
AS
BEGIN
	INSERT INTO Usuarios
	VALUES (@Nombres, @Apellidos, @Fecha_nacimiento, 
	@Foto_usuario, @Estado_civil, @Hermanos)

	SET @Id_usuario = SCOPE_IDENTITY()
END