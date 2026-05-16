# QUANLYNHANVIEN — Overview

This repository contains a personnel-management desktop application (targeting .NET Framework 4.8) with user and role management and several database-side security features.

Key parts of the project:

- Application code: DAL/ contains data-access logic used by the UI.
- Database scripts: DBScripts/ contains core DDL and DBScripts/StoredProcedures/ contains individual stored-procedure files (SSDT-friendly).
- Security utilities: DAL/Hash256.cs implements PBKDF2 hashing and legacy-hash helpers; tools/generate_password_hash.ps1 helps generate pbkdf2 strings for manual DB updates.

## Prerequisites

- Visual Studio (for building the .NET Framework 4.8 project)
- SQL Server instance and SSMS for running deployment scripts
- .NET Framework 4.8 runtime
- PowerShell (powershell.exe)

## Quick setup summary

1. Build the solution in Visual Studio.
2. Create a database (e.g. QUANLYNHANVIEN) and run `DBScripts/init_security.sql` in SSMS to create core tables.
3. Deploy each `.sql` file under `DBScripts/StoredProcedures/` in SSMS so stored procedures exist individually.
4. Confirm `app.usp_User_GetByUsername` returns columns in this order: `MATK, MALOAITK, TENCHUTAIKHOAN, TENDANGNHAP, MATKHAU, FailedLoginCount, LockoutUntil`.
5. Configure the app connection string (prefer Windows Authentication or a least-privileged SQL account).

## Implemented security features

- PBKDF2 password hashing (Hash256.CreateHash / Verify). Stored format: `pbkdf2$<iterations>$<saltB64>$<hashB64>`.
- Legacy-hash migration: DAL migrates plaintext, hex-SHA256, or base64-SHA256 to PBKDF2 on successful login.
- Account lockout: `FailedLoginCount` and `LockoutUntil` control lockout behavior; parameters TS06/TS07 configurable in `THAMSO`.
- Audit logging: `dbo.AuditLog` and `dbo.usp_AuditLog_Add` are used by DAL for important events.
- Parameterized SQL: DAL uses parameterized `SqlCommand` to reduce SQL injection risk.

## Recommendations (to reach best practice)

- Use a least-privileged SQL login and grant `EXECUTE` only on necessary stored procedures; deny direct table access.
- Ensure `MATKHAU` column can store full pbkdf2 strings (use `NVARCHAR(512)` or `VARCHAR(MAX)`).
- Consider SQL Server features: Always Encrypted, Transparent Data Encryption (TDE), SQL Audit, and Row-Level Security where applicable.
- Protect connection strings in config (ProtectedConfiguration) and secure log files.

## Troubleshooting

- If login fails:
  - Check the exact `MATKHAU` value and its length in the database.
  - Ensure the stored procedure `app.usp_User_GetByUsername` returns the expected columns in the expected order.
  - Reset `FailedLoginCount` and `LockoutUntil` for testing if account is locked.
- Use `tools/generate_password_hash.ps1` to create a valid PBKDF2 string and update `TAIKHOAN.MATKHAU` if needed.

## Important files

- `DAL/Hash256.cs` — PBKDF2 functions and legacy verification helpers
- `DAL/DAL_TAIKHOAN.cs` — Authentication, migration, lockout, and audit logic
- `DBScripts/init_security.sql` — core DDL and configuration
- `DBScripts/StoredProcedures/*` — stored procedure files
- `tools/generate_password_hash.ps1` — helper to create pbkdf2 strings

If you want a README.md with installation and step-by-step commands, I can add it to the repository root.
