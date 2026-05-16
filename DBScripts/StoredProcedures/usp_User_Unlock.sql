CREATE PROCEDURE dbo.usp_User_Unlock
	@MATK INT
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE TAIKHOAN SET FailedLoginCount = 0, LockoutUntil = NULL WHERE MATK = @MATK;
	-- use named parameters to avoid positional parsing issues
	DECLARE @target NVARCHAR(50);
	SET @target = CAST(@MATK AS NVARCHAR(50));
	EXEC dbo.usp_AuditLog_Add
		@EventType = 'UserUnlock',
		@Username = NULL,
		@Target = @target,
		@Details = 'Account unlocked',
		@UserId = @MATK,
		@Status = 'Success';
END
