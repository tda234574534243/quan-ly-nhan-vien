Project: QUANLYNHANVIEN — Installation & Database Security Notes

Overview

This repository implements a personnel-management application with user/role management and several database security mechanisms. The codebase includes a .NET Framework 4.8 WinForms/desktop app (DAL layer under DAL/), database deployment scripts (DBScripts/), stored procedures (DBScripts/StoredProcedures/), and small tooling (tools/generate_password_hash.ps1).

Prerequisites

- Microsoft Visual Studio (recommended) for .NET Framework 4.8 projects
- SQL Server instance accessible from the machine (SSMS recommended for manual script runs)
- .NET Framework 4.8 runtime
- PowerShell (built-in powershell.exe is sufficient)

Quick install / setup

1. Build the app
   - Open the solution in Visual Studio and build.

2. Prepare the database
   - Create a database named QUANLYNHANVIEN (or use an existing DB and update the connection string accordingly).
   - In SSMS, run DBScripts/init_security.sql to create core tables (AuditLog, PHANLOAITAIKHOAN, TAIKHOAN alterations, THAMSO entries, etc.).
   - Deploy stored procedures: open each .sql file under DBScripts/StoredProcedures/ and run them in the target database. The project keeps procedures split into one object per file for SSDT compatibility.
   - Verify app.usp_User_GetByUsername exists and returns columns in this order: MATK, MALOAITK, TENCHUTAIKHOAN, TENDANGNHAP, MATKHAU, FailedLoginCount, LockoutUntil

3. Configure connection string
   - Update the app configuration to point to your SQL Server instance (use integrated security or a least-privileged SQL user). Prefer Windows Authentication where possible.

4. Create test data / admin user
   - You can create users using the app UI or by inserting test rows.
   - If the ADMIN account is locked or password needs replacement, run tools/generate_password_hash.ps1 to generate a PBKDF2 value and then run the printed UPDATE in SSMS. Example:
	 - PowerShell: powershell.exe -NoProfile -ExecutionPolicy Bypass -File tools/generate_password_hash.ps1 -password "1" -username "ADMIN"
	 - Run the generated UPDATE in SSMS and reset FailedLoginCount and LockoutUntil if needed.

Where the code implements security

- Password hashing: DAL/Hash256.cs implements PBKDF2 (Rfc2898DeriveBytes) and stores hashes in the format: pbkdf2$<iterations>$<saltB64>$<hashB64>. DAL/DAL_TAIKHOAN.cs verifies and migrates legacy stored formats (plaintext, unsalted hex SHA256, base64 SHA256) to PBKDF2 on successful login.
- Authentication policies: DAL increments FailedLoginCount and sets LockoutUntil when threshold is reached (THAMSO values TS06/TS07 configurable).
- Audit logging: dbo.AuditLog table + stored proc dbo.usp_AuditLog_Add; DAL writes audit events for login success/failure, migrations, and user admin actions.
- Parameterized queries: DAL uses SqlCommand with parameters (reduces SQL injection risk).
- Stored procedures: User/role management is implemented as stored procedures under DBScripts/StoredProcedures.
- Runtime tracing: DAL writes an auth log (bin/Debug/logs/auth.log) to assist debugging — protect this file in production.

Recommendations & additional DB security measures (to reach best-practice)

- Least privilege accounts: create a SQL login used by the app with EXECUTE permission only on stored procedures, and deny direct SELECT/UPDATE/DELETE on tables from that login.
- GRANT/REVOKE: explicitly grant EXEC on required procs and deny table access; maintain separate roles for admin vs regular operations.
- Column sizing & types: ensure MATKHAU column can store full PBKDF2 strings (use NVARCHAR(512) or VARCHAR(MAX) as appropriate). Check collation and trimming issues.
- Always Encrypted / TDE: consider Always Encrypted for client-side sensitive columns or Transparent Data Encryption for data-at-rest.
- Auditing: enable SQL Server Audit or Extended Events for sensitive actions (logins, privilege changes, DDL changes).
- Password policies: enforce complexity, rotation, and reuse rules; consider implementing password strength checks at the UI layer.
- Connection string protection: encrypt connection strings in config (ProtectedConfiguration) and restrict file system access.
- Secure logging: protect auth.log and other files — store logs centrally and restrict access; avoid logging plaintext passwords.
- Use Windows Authentication when possible and follow secure service account practices.
- Monitor and backup: monitor failed login events and lockouts; ensure regular encrypted backups and secure backup storage.

Troubleshooting tips

- If login fails, verify:
  - The stored MATKHAU value format (it should be pbkdf2$iterations$salt$hash without extra parentheses).
  - The stored procedure app.usp_User_GetByUsername returns the expected columns in the expected order.
  - MATKHAU column length is sufficient and not truncated.
  - FailedLoginCount and LockoutUntil values (reset if needed for testing).
- Use the provided tools/generate_password_hash.ps1 to create a valid PBKDF2 string if you need to set an account password directly from SSMS.

Files of interest

- DAL/Hash256.cs — PBKDF2 implementation and legacy hash helpers
- DAL/DAL_TAIKHOAN.cs — Authentication logic, migration, lockout, audit calls
- DBScripts/init_security.sql — core DDL for tables and parameters
- DBScripts/StoredProcedures/* — stored procedure implementations (user/role/audit)
- tools/generate_password_hash.ps1 — helper to create pbkdf2$ strings for manual DB updates

If you want, I can produce a compact README.md version suitable for the project root and/or generate sample SSMS commands to deploy the DB objects automatically.