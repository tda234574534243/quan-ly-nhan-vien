# QUANLYNHANVIEN

Ứng dụng quản lý nhân viên (desktop) — target .NET Framework 4.8. Ứng dụng có quản lý người dùng, vai trò và nhiều tính năng bảo mật cơ sở dữ liệu.

## Yêu cầu trước khi cài

- Visual Studio (có workload .NET Framework 4.8)
- SQL Server (LocalDB/Express/Developer/full) và SQL Server Management Studio (SSMS)
- PowerShell (powershell.exe)

## Hướng dẫn cài đặt & chuẩn bị cơ sở dữ liệu

1. Clone repository:
   ```powershell
   git clone https://github.com/tda234574534243/quan-ly-nhan-vien.git
   cd quan-ly-nhan-vien
   ```

2. Build project
   - Mở solution trong Visual Studio và build (chế độ Debug để test).

3. Tạo và chuẩn bị database
   - Tạo database `QUANLYNHANVIEN` (hoặc dùng DB khác và cập nhật connection string).
   - Trong SSMS, mở và chạy `DBScripts/init_security.sql` để tạo bảng và tham số ban đầu.
   - Triển khai stored procedure: chạy từng file `.sql` trong `DBScripts/StoredProcedures/` để tạo từng đối tượng riêng lẻ trong DB.

4. Kiểm tra kết quả stored procedure
   - Chạy trong SSMS:
     ```sql
     EXEC app.usp_User_GetByUsername @TENDANGNHAP = 'ADMIN';
     ```
   - Xác nhận các cột trả về theo thứ tự: `MATK, MALOAITK, TENCHUTAIKHOAN, TENDANGNHAP, MATKHAU, FailedLoginCount, LockoutUntil`.

5. Cấu hình kết nối cho ứng dụng
   - Cập nhật connection string trong App.config để trỏ tới SQL Server. Nên dùng Windows Authentication cho môi trường production.

6. Tạo hoặc sửa tài khoản ADMIN
   - Dùng UI ứng dụng để tạo user, hoặc tạo mật khẩu bằng PowerShell:
     ```powershell
     powershell.exe -NoProfile -ExecutionPolicy Bypass -File tools/generate_password_hash.ps1 -password "1" -username "ADMIN"
     ```
     - Sao chép lệnh `UPDATE` được in ra và chạy trong SSMS để cập nhật mật khẩu ADMIN.
     - Nếu tài khoản bị khoá, reset counters:
       ```sql
       UPDATE TAIKHOAN SET FailedLoginCount = 0, LockoutUntil = NULL WHERE TENDANGNHAP = 'ADMIN';
       ```

## Các tính năng bảo mật đã triển khai

- Băm mật khẩu PBKDF2 (DAL/Hash256.cs) với định dạng: `pbkdf2$<iterations>$<saltB64>$<hashB64>`
- Hỗ trợ di chuyển (migrate) từ dạng băm cũ (plaintext, SHA256 hex, SHA256 base64) sang PBKDF2 khi đăng nhập thành công
- Cơ chế khoá tài khoản dựa trên `FailedLoginCount` và `LockoutUntil` (tham số có thể cấu hình trong bảng `THAMSO`)
- Ghi nhật ký audit vào bảng `AuditLog` bằng stored procedure
- Sử dụng parameterized SQL trong DAL để giảm rủi ro SQL injection

## Khuyến nghị bảo mật

- Sử dụng tài khoản SQL có quyền ít nhất: chỉ cấp `EXECUTE` cho stored procedures cần thiết, cấm truy cập trực tiếp vào bảng.
- Cấp quyền GRANT/REVOKE rõ ràng cho procedures và từ chối quyền truy cập bảng.
- Đảm bảo cột `MATKHAU` đủ dài để lưu chuỗi PBKDF2 đầy đủ (ví dụ `NVARCHAR(512)` hoặc `VARCHAR(MAX)`).
- Bảo vệ file cấu hình (mã hoá connection string), bảo mật file log.
- Cân nhắc sử dụng tính năng SQL Server: Always Encrypted, TDE, SQL Audit, Row-Level Security.

## Xử lý sự cố

- Nếu không đăng nhập được:
  - Kiểm tra giá trị `MATKHAU` trong DB (định dạng `pbkdf2$...` và không có ký tự thừa).
  - Kiểm tra thứ tự cột trả về từ `app.usp_User_GetByUsername`.
  - Reset `FailedLoginCount` và `LockoutUntil` nếu tài khoản bị khoá.
  - Kiểm tra file log: `QuanLyNhanVien/bin/Debug/logs/auth.log` để xem trace xác thực.

## Các file quan trọng

- `DAL/Hash256.cs` — PBKDF2 và helper kiểm tra băm cũ
- `DAL/DAL_TAIKHOAN.cs` — logic xác thực, migrate, khoá, audit
- `DBScripts/init_security.sql` — DDL cơ bản và tham số
- `DBScripts/StoredProcedures/*` — các stored procedure
- `tools/generate_password_hash.ps1` — helper tạo chuỗi pbkdf2 để cập nhật DB


