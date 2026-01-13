# 🛋️ Furniture E-commerce Website (Capstone Project)

 Đồ án tốt nghiệp: Xây dựng website kinh doanh nội thất tích hợp gợi ý sản phẩm bằng AI.
 **Fullstack Project using ASP.NET Core API & Vue.js**

## 📖 Introduction
Dự án là một hệ thống thương mại điện tử hoàn chỉnh phục vụ việc kinh doanh nội thất. Hệ thống bao gồm trang dành cho khách hàng (Customer) để mua sắm và trang quản trị (Admin) để quản lý sản phẩm, đơn hàng. Đặc biệt, hệ thống tích hợp module AI để gợi ý sản phẩm cá nhân hóa cho người dùng.

## 🚀 Tech Stack
Dự án sử dụng các công nghệ hiện đại theo kiến trúc Microservices/Monolithic (tuỳ bạn chọn từ):

* **Backend:** ASP.NET Core Web API 6.0/8.0 (C#)
* **Frontend:** Vue.js (Vue 2/3), Vuex/Pinia, Bootstrap/Tailwind
* **Database:** SQL Server, Entity Framework Core
* **Tools:** Visual Studio, VS Code, Postman, Git

## ✨ Key Features
### 🛒 Customer Site
* **Authentication:** Đăng ký, Đăng nhập, Quên mật khẩu.
* **Product:** Xem danh sách, tìm kiếm, lọc sản phẩm, xem chi tiết.
* **Order:** Thêm vào giỏ hàng, Thanh toán (Checkout), Xem lịch sử đơn hàng.
* **AI Recommendation:** Gợi ý sản phẩm tương tự hoặc sản phẩm người dùng có thể thích.

### 🛠️ Admin Portal
* **Dashboard:** Thống kê doanh thu, số lượng đơn hàng.
* **Management:** CRUD Sản phẩm, Danh mục, User.
* **Order Processing:** Duyệt đơn, cập nhật trạng thái giao hàng.

## 📸 Screenshots


### Home Page
<img width="1869" height="813" alt="image" src="https://github.com/user-attachments/assets/9575a850-2d39-4c62-9f06-dbdde5440db3" />



### Admin Dashboard
<img width="460" height="209" alt="image" src="https://github.com/user-attachments/assets/2ae4d9c6-95b4-48a5-84e2-c628f9c3eb32" />



## 🔧 Installation & Setup

### 1. Backend (API)
1.  Clone repo: `git clone https://github.com/trung110103/ProjectFinal.git`
2.  Mở `appsettings.json` trong folder `api` và cấu hình lại **Connection String** tới SQL Server của bạn.
3.  Chạy lệnh Update Database (Migration):
    ```bash
    dotnet ef database update
    ```
4.  Run Project: `dotnet run`

### 2. Frontend (Vue.js)
1.  Di chuyển vào folder frontend: `cd FrontEnd`
2.  Cài đặt packages: `npm install`
3.  Chạy dự án: `npm run serve`

## 👨‍💻 Author
**Dương Đình Trung**
* Student at Hanoi University of Industry (HaUI)
* Major: Software Engineering
* Contact: duongtrung110103@gmail.ccom
