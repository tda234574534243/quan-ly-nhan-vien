# QUANLYNHANVIEN

H? th?ng qu?n lý nhân viên (QuanLyNhanVien) — m?t ?ng d?ng WPF (.NET Framework 4.8) ?? qu?n lý tài kho?n, ch?m công, b?ng l??ng, phòng ban và báo cáo.

## T?ng quan
- Ki?n trúc: 3 t?ng (DAL, BUS, DTO) + giao di?n WPF trong project `QuanLyNhanVien`.
- Công ngh?: .NET Framework 4.8, WPF, SQL Server, ADO.NET.
- Các project trong workspace:
  - `DAL` — Data Access Layer (k?t n?i DB, truy v?n SQL).
  - `BUS` — Business logic.
  - `DTO` — Các ??i t??ng truy?n d? li?u.
  - `QuanLyNhanVien` — Giao di?n ng??i dùng WPF.

## Tính n?ng chính
- Qu?n lý tài kho?n ng??i dùng
- ??ng nh?p/??i m?t kh?u
- Qu?n lý nhân viên, phòng ban
- Ch?m công, tính l??ng
- Khen th??ng/k? lu?t, báo cáo và xu?t Excel

## Yêu c?u
- Windows
- Visual Studio 2019 ho?c 2022 (h? tr? .NET Framework 4.8)
- SQL Server (có th? là LocalDB ho?c named instance)
- Quy?n truy c?p c? s? d? li?u `QUANLYNHANVIEN` (schema d? ki?n theo project)

## Cài ??t và c?u hình
1. M? gi?i pháp trong Visual Studio: `QuanLyNhanVien.sln` (m? th? m?c g?c ch?a các project).

2. C?u hình chu?i k?t n?i SQL Server:
   - T?m th?i chu?i k?t n?i n?m trong `DAL\KetNoi.cs` ? bi?n `connection`.
   - Ví d? (named instance):
     ```csharp
     public SqlConnection connection = new SqlConnection(@"Server=DESKTOP-E4P638H\\TRANDUCANH;Database=QUANLYNHANVIEN;Integrated Security=True;");
     ```
   - L?u ý: n?u dùng verbatim string (`@"..."`) ch? c?n m?t d?u backslash cho separator instance: `@"Server=HOST\\INSTANCE;..."` là sai — ph?i `@"Server=HOST\INSTANCE;..."`.
   - ?? xu?t: chuy?n chu?i k?t n?i vào `App.config` và ??c b?ng `ConfigurationManager.ConnectionStrings` ?? d? c?u hình.

3. T?o ho?c nh?p c? s? d? li?u `QUANLYNHANVIEN` trên SQL Server. N?u repository không kèm file schema, c?n cung c?p script t?o b?ng/tên c?t theo mong ??i trong `DAL`.

4. Build gi?i pháp (Debug/Release) trong Visual Studio. ??m b?o các project tham chi?u ?úng nhau.

5. Ch?y project `QuanLyNhanVien` (Set as Startup Project) — ?ng d?ng WPF s? m? form ??ng nh?p.

## Ghi chú v?n hành và kh?c ph?c l?i ph? bi?n
- L?i `InvalidOperationException: Instance failure.` th??ng do chu?i k?t n?i sai tên máy/instance; ki?m tra `DAL\KetNoi.cs` ho?c c?u hình trong `App.config`.
- L?i `FormatException: String was not recognized as a valid DateTime.` th??ng do ??nh d?ng ngày không kh?p khi g?i `DateTime.ParseExact` — ki?m tra mã n?i s? d?ng `ParseExact` và dùng `TryParse`, ho?c t?o `DateTime` t? tháng/n?m s? nguyên.
- Nên s? d?ng `using` cho `SqlConnection`, `SqlCommand`, `SqlDataReader` và truy v?n tham s? (`SqlParameter`) ?? tránh SQL injection và rò r? k?t n?i.

## T? ch?c mã ngu?n
- Thêm/?i?u ch?nh c?u hình chu?i k?t n?i: `DAL\KetNoi.cs` (t?m th?i) ? khuy?n ngh? di chuy?n sang `App.config`.
- Các l?p x? lý DB: `DAL\DAL_TAIKHOAN.cs`, `DAL\...`.
- View/ViewModel: trong `QuanLyNhanVien\MVVM`.

## ?óng góp
- M? issue ho?c t?o pull request cho các s?a ??i.
- Tr??c khi g?i PR, ch?y build toàn b? solution và test tính n?ng chính (??ng nh?p, CRUD nhân viên, xu?t Excel).

## License
- Ch?a ch? ??nh. Thêm file `LICENSE` n?u c?n.

