# QUANLYNHANVIEN

A personnel management desktop application (targeting .NET Framework 4.8) that demonstrates user/role management and database security practices.

## Prerequisites

- Visual Studio (2019/2022/2026) with .NET Framework 4.8 workload
- SQL Server (LocalDB / Express / Developer / full instance) and SSMS
- PowerShell (powershell.exe)

## Installation & Database setup

1. Clone the repository:
   ```powershell
   git clone https://github.com/tda234574534243/quan-ly-nhan-vien.git
   cd quan-ly-nhan-vien
   ```

2. Build the solution
   - Open the solution in Visual Studio and build (Debug configuration recommended for testing).

3. Create and prepare the database
   - Create a database named `QUANLYNHANVIEN` (or modify the connection string to a different database).
   - In SSMS, open and execute `DBScripts/init_security.sql` to create core tables and initial parameters.
   - Deploy stored procedures: run each `.sql` file in `DBScripts/StoredProcedures/` so they exist as separate objects in the target DB.

4. Verify stored procedure output
   - Run in SSMS:
     ```sql
     EXEC app.usp_User_GetByUsername @TENDANGNHAP = 'ADMIN';
     ```
   - Confirm the returned columns are in this order:
     `MATK, MALOAITK, TENCHUTAIKHOAN, TENDANGNHAP, MATKHAU, FailedLoginCount, LockoutUntil`.

5. Configure the application connection
   - Update the connection string in the application configuration (App.config) to point to your SQL Server instance. Prefer Windows Authentication in production.

6. Create or repair an admin account
   - Use the app UI to create users, or set a password directly from PowerShell:
     ```powershell
     powershell.exe -NoProfile -ExecutionPolicy Bypass -File tools/generate_password_hash.ps1 -password "1" -username "ADMIN"
     ```
     - Copy the printed `UPDATE` SQL and execute it in SSMS to set the ADMIN password.
     - If the account is locked, reset counters in SSMS:
       ```sql
       UPDATE TAIKHOAN SET FailedLoginCount = 0, LockoutUntil = NULL WHERE TENDANGNHAP = 'ADMIN';
       ```

## Implemented security features

- PBKDF2 password hashing (DAL/Hash256.cs) with format: `pbkdf2$<iterations>$<saltB64>$<hashB64>`
- Legacy-hash migration (plaintext, hex-SHA256, base64-SHA256) to PBKDF2 on successful login
- Account lockout and failure counters (`FailedLoginCount`, `LockoutUntil`, configurable via `THAMSO`)
- Audit logging (dbo.AuditLog and dbo.usp_AuditLog_Add)
- Parameterized SQL usage in DAL to reduce SQL injection risk

## Recommended hardening

- Use least-privileged SQL accounts: create a DB user that only has `EXECUTE` on stored procedures used by the app.
- Explicitly GRANT/REVOKE permissions for stored procedures and deny direct table access.
- Ensure `MATKHAU` column can store full PBKDF2 strings (e.g., `NVARCHAR(512)` or `VARCHAR(MAX)`).
- Protect configuration files and use encrypted connection strings (ProtectedConfiguration).
- Consider SQL Server features: Always Encrypted, Transparent Data Encryption (TDE), SQL Audit, and Row-Level Security.
- Secure logging and avoid storing sensitive data in plain text logs.

## Troubleshooting

- If login fails:
  - Inspect `MATKHAU` in DB for the account and ensure it matches `pbkdf2$...` format without extra parentheses.
  - Verify `app.usp_User_GetByUsername` columns are returned in the expected order.
  - Reset `FailedLoginCount` and `LockoutUntil` for testing if the account is locked.
  - Check `QuanLyNhanVien/bin/Debug/logs/auth.log` for verification traces.

## Important files

- `DAL/Hash256.cs` — PBKDF2 implementation and legacy verification helpers
- `DAL/DAL_TAIKHOAN.cs` — Authentication, migration, lockout, audit logic
- `DBScripts/init_security.sql` — core DDL and parameters
- `DBScripts/StoredProcedures/*` — stored procedure files
- `tools/generate_password_hash.ps1` — helper to generate pbkdf2 strings for manual DB updates

If you want a Vietnamese version of this README or a shorter student-facing README, tell me and I will produce it.
