CREATE PROCEDURE dbo.usp_UserRole_Revoke
	@MATK INT,
	@MALOAITK INT
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM USER_ROLE WHERE MATK = @MATK AND MALOAITK = @MALOAITK;
	DECLARE @sMatk NVARCHAR(50) = CAST(@MATK AS NVARCHAR(50));
	DECLARE @sMaloaitk NVARCHAR(50) = CAST(@MALOAITK AS NVARCHAR(50));
	EXEC dbo.usp_AuditLog_Add
		@EventType = 'UserRoleRevoke',
		@Username = NULL,
		@Target = @sMatk,
		@Details = @sMaloaitk,
		@UserId = @MATK,
		@Status = 'Success';
END
