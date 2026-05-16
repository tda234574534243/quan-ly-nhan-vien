CREATE PROCEDURE dbo.usp_UserRole_Assign
	@MATK INT,
	@MALOAITK INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS(SELECT 1 FROM USER_ROLE WHERE MATK = @MATK AND MALOAITK = @MALOAITK)
		INSERT INTO USER_ROLE(MATK, MALOAITK) VALUES(@MATK, @MALOAITK);
	-- prepare string casts and use named parameters to avoid parser issues
	DECLARE @sMatk NVARCHAR(50) = CAST(@MATK AS NVARCHAR(50));
	DECLARE @sMaloaitk NVARCHAR(50) = CAST(@MALOAITK AS NVARCHAR(50));
	EXEC dbo.usp_AuditLog_Add
		@EventType = 'UserRoleAssign',
		@Username = NULL,
		@Target = @sMatk,
		@Details = @sMaloaitk,
		@UserId = @MATK,
		@Status = 'Success';
END
