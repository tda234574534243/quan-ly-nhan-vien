CREATE PROCEDURE dbo.usp_AuditLog_Add
	@EventType NVARCHAR(100),
	@Username NVARCHAR(200),
	@Target NVARCHAR(200),
	@Details NVARCHAR(MAX),
	@UserId INT = NULL,
	@Status NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.AuditLog(EventType, Username, Target, Details, UserId, Status)
	VALUES(@EventType, @Username, @Target, @Details, @UserId, @Status);
END
