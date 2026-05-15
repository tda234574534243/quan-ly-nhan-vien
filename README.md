# QUANLYNHANVIEN

Hệ thống quản lý nhân viên (QuanLyNhanVien) — một ứng dụng WPF (.NET Framework 4.8) để quản lý tài khoản, chấm công, bảng lương, phòng ban và báo cáo.

## Tổng quan
- Kiến trúc: 3 tầng (DAL, BUS, DTO) + giao diện WPF trong project `QuanLyNhanVien`.
- Công nghệ: .NET Framework 4.8, WPF, SQL Server, ADO.NET.
- Các project trong workspace:
  - `DAL` — Data Access Layer (kết nối DB, truy vấn SQL).
  - `BUS` — Business logic.
  - `DTO` — Các đối tượng truyền dữ liệu.
  - `QuanLyNhanVien` — Giao diện người dùng WPF.

## Tính năng chính
- Quản lý tài khoản người dùng
- Đăng nhập/đổi mật khẩu
- Quản lý nhân viên, phòng ban
- Chấm công, tính lương
- Khen thưởng/kỷ luật, báo cáo và xuất Excel

## Yêu cầu
- Windows
- Visual Studio 2019 hoặc 2022 (hỗ trợ .NET Framework 4.8)
- SQL Server (có thể là LocalDB hoặc named instance)
- Quyền truy cập cơ sở dữ liệu `QUANLYNHANVIEN` (schema dự kiến theo project)

## Cài đặt và cấu hình
1. Mở giải pháp trong Visual Studio: `QuanLyNhanVien.sln` (mở thư mục gốc chứa các project).

2. Cấu hình chuỗi kết nối SQL Server:
   - Tạm thời chuỗi kết nối nằm trong `DAL\KetNoi.cs` ở biến `connection`.
   - Ví dụ (named instance):
     ```csharp
     public SqlConnection connection = new SqlConnection(@"Server=DESKTOP-E4P638H\TRANDUCANH;Database=QUANLYNHANVIEN;Integrated Security=True;");
     ```
   - Lưu ý: nếu dùng verbatim string (`@"..."`) chỉ cần một dấu backslash cho separator instance: `@"Server=HOST\\INSTANCE;..."` là sai — phải `@"Server=HOST\INSTANCE;..."`.
   - Đề xuất: chuyển chuỗi kết nối vào `App.config` và đọc bằng `ConfigurationManager.ConnectionStrings` để dễ cấu hình.

3. Tạo hoặc nhập cơ sở dữ liệu `QUANLYNHANVIEN` trên SQL Server. Nếu repository không kèm file schema, cần cung cấp script tạo bảng/tên cột theo mong đợi trong `DAL`.

4. Build giải pháp (Debug/Release) trong Visual Studio. Đảm bảo các project tham chiếu đúng nhau.

5. Chạy project `QuanLyNhanVien` (Set as Startup Project) — ứng dụng WPF sẽ mở form đăng nhập.

## Ghi chú vận hành và khắc phục lỗi phổ biến
- Lỗi `InvalidOperationException: Instance failure.` thường do chuỗi kết nối sai tên máy/instance; kiểm tra `DAL\KetNoi.cs` hoặc cấu hình trong `App.config`.
- Lỗi `FormatException: String was not recognized as a valid DateTime.` thường do định dạng ngày không khớp khi gọi `DateTime.ParseExact` — kiểm tra mã nơi sử dụng `ParseExact` và dùng `TryParse`, hoặc tạo `DateTime` từ tháng/năm số nguyên.
- Nên sử dụng `using` cho `SqlConnection`, `SqlCommand`, `SqlDataReader` và truy vấn tham số (`SqlParameter`) để tránh SQL injection và rò rỉ kết nối.

## Tổ chức mã nguồn
- Thêm/điều chỉnh cấu hình chuỗi kết nối: `DAL\KetNoi.cs` (tạm thời) — khuyến nghị di chuyển sang `App.config`.
- Các lớp xử lý DB: `DAL\DAL_TAIKHOAN.cs`, `DAL\...`.
- View/ViewModel: trong `QuanLyNhanVien\MVVM`.

## Đóng góp
- Mở issue hoặc tạo pull request cho các sửa đổi.
- Trước khi gửi PR, chạy build toàn bộ solution và test tính năng chính (đăng nhập, CRUD nhân viên, xuất Excel).

## License
- Chưa chỉ định. Thêm file `LICENSE` nếu cần.
