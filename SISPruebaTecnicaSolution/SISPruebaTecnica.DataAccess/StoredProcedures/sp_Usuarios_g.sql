CREATE OR ALTER PROC sp_Usuarios_g
@Tipo_busqueda varchar(50),
@Texto_busqueda varchar(50)
AS
BEGIN
	IF (@Tipo_busqueda = 'ALL')
	BEGIN
		SELECT *
		FROM Usuarios
	END
	ELSE IF (@Tipo_busqueda = 'ID USUARIO')
	BEGIN
		SELECT *
		FROM Usuarios us
		WHERE us.Id_usuario = CONVERT(int, @Texto_busqueda)
	END
END