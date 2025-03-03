# 📅 Appointment Booking System

## 🚀 Overview
This is a **backend API** for an **Appointment Booking System**, allowing customers to book appointments with sales managers based on language, product, and customer rating criteria.

### **Key Features**
- 📅 Customers can book 1-hour appointment slots.
- 🔍 API returns available appointment slots based on given filters.
- 🗄️ Uses **PostgreSQL** for database storage.
- 🔧 Built with **.NET 8** using **C#**.
- 📏 Follows **SOLID Principles** and **Repository Pattern**.
- ✅ Includes **Unit Tests** and **Integration Tests**.

---
## 🏗️ Architecture
The system follows **Clean Code principles** with **separation of concerns**:
- **Controllers** – Handle API requests and responses.
- **Services** – Business logic processing.
- **Repositories** – Data access layer, interacting with the database.
- **Entities/Models** – Define the data structures.
- **Database** – PostgreSQL using **EF Core (Code First Approach)**.

---
## 🛠️ Tech Stack
- **Backend**: .NET 8, C# 12.0
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Testing**: xUnit, Moq (for Unit Tests), WebApplicationFactory (for Integration Tests)
- **Dependency Injection**: Built-in .NET DI

---
## Project Structure

- **Appointment.API**: Contains the API controllers and endpoints.
- **Appointment.Core**: Contains the core business logic and service interfaces.
- **Appointment.Domain**: Contains the domain entities.
- **Appointment.Infrastructure**: Contains the data access layer and repository implementations.
- **Appointment.Common**: Contains common DTOs and utility classes.
- **Appointment.Tests**: Contains unit tests for the application.

---
## 🚀 Setup & Installation
### **1️⃣ Clone the Repository**
```sh
git clone https://github.com/your-repo/appointment-booking.git
cd appointment-booking
```

### **2️⃣ Configure Database**
Ensure **PostgreSQL** is running and update `appsettings.json` with your connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=AppointmentDB;Username=youruser;Password=yourpassword"
}
```

### **3️⃣ Apply Migrations & Seed Database**
```sh
dotnet ef database update
```

### **4️⃣ Run the API**
```sh
dotnet run
```
The API will start at **http://localhost:3000**

---
## 📌 API Endpoints
### **Get Available Appointment Slots**
📍 **POST** `/calendar/query`
#### **Request Body**
```json
{
  "date": "2024-05-03",
  "products": ["SolarPanels", "Heatpumps"],
  "language": "German",
  "rating": "Gold"
}
```
#### **Response**
```json
[
  {
    "available_count": 1,
    "start_date": "2024-05-03T10:30:00.00Z"
  },
  {
    "available_count": 2,
    "start_date": "2024-05-03T12:00:00.00Z"
  }
]
```

---
## 🧪 Running Tests
### **Unit Tests**
```sh
dotnet test
```
### **Integration Tests**
```sh
dotnet test --filter Category=Integration
```

---
## 🏆 Best Practices Followed
✅ **SOLID Principles** (Separation of Concerns)  
✅ **Repository Pattern** (For maintainability)  
✅ **Unit Testing with Stubs**  
✅ **Integration Testing with In-Memory DB**  
✅ **Dependency Injection** for better testability  

---
## 📜 License
MIT License. Feel free to use and modify this project.

---
## 👨‍💻 Author
**Your Name**  
GitHub: [yourgithub](https://github.com/yourgithub)
