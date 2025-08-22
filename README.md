# 🏆 ITEC Management WebApp  

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)  
![.NET](https://img.shields.io/badge/.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)  
![MySQL](https://img.shields.io/badge/MySQL-005C84?style=for-the-badge&logo=mysql&logoColor=white)  
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)  
![Bootstrap](https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)  

A **web-based management system** for the **Information Technology Exhibition and Competition (ITEC)** at **UET Lahore**.  
This system helps in **event organization, participant registration, role assignments, finances, venue allocation, and reporting**.  

---

## ✨ Features  

- 👥 **Participants Management** – Register and manage participants with event & role mapping  
- 📅 **Events & Committees** – Create events, assign committees, and manage duties  
- 💰 **Finance Tracking** – Record payments, sponsorships, and dues  
- 🏛️ **Venue Allocation** – Allocate halls, labs, and sessions  
- 📊 **Reports & Dashboards** – Generate financial summaries, participant reports, and event details  
- 🔐 **Authentication** – User login with **ASP.NET Core Identity & Cookie Authentication**  

---

## 🛠️ Tech Stack  

- **Frontend:** ASP.NET Core MVC, Razor Pages, HTML, CSS, Bootstrap, AdminLTE  
- **Backend:** ASP.NET Core (C#)  
- **Database:** MySQL  
- **Authentication:** ASP.NET Core Identity  
- **Tools:** Visual Studio, MySQL Workbench, Git  

---

⚙️ Installation & Setup
git clone https://github.com/YOUR-USERNAME/itecwebapp.git
cd itecwebapp
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=itecwebapp;user=root;password=yourpassword;"
}
dotnet run

## 📂 Project Structure  

```bash
itecwebapp/
│
├── Controllers/      # MVC Controllers
├── Models/           # Entity Models
├── Interfaces/       # Business Layer Interfaces
├── BL/               # Business Logic Layer
├── DAL/              # Data Access Layer
├── Helpers/          # Utility Classes
├── Views/            # Razor Views (UI)
├── wwwroot/          # Static files (CSS, JS, Images)
└── appsettings.json  # Database connection & config
...
