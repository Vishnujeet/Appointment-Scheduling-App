# 📅 Appointment Booking System

## 🚀 Overview

This is a **backend API** for an **Appointment Booking System**, allowing customers to book appointments with sales managers based on language, product, and customer rating criteria.

### **Key Features**

- 📅 **Customers can book 1-hour appointment slots.**
- 🔍 **API returns available appointment slots based on given filters.**
- 🗄️ **Uses PostgreSQL for database storage.**
- 🔧 **Built with .NET 8 using C#.**
- 📏 **Follows SOLID Principles and Repository Pattern.**
- ✅ **Includes Unit Tests and Integration Tests.**
- 🐳 **Production-ready using Docker and Docker Compose.**

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
- **Containerization**: Docker

---

## 📂 Project Structure

- **Build**: Contains `docker-compose.yml` to set up the Docker environment.
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
git clone https://github.com/Vishnujeet/Appointment-Scheduling-App.git
cd AppointmentBookingSystem
```

### **2️⃣ Configure Docker Environment**

Ensure **Docker** is installed and running on your system. The application is designed to run inside Docker containers.

`build/docker-compose.yml`.

---

## 🚀 Running the Application

### **1️⃣ Run in Production or local using Docker Compose**

This application is **production-ready** and can be run using `docker-compose.yml` located inside the `Build` folder.

```sh
docker-compose -f build/docker-compose.yml up --build -d
```

This will start the application and all required dependencies in a containerized environment.

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

#### **Possible HTTP Status Codes**

- `200 OK` – Request successful
- `400 Bad Request` – Invalid input data
- `500 Internal Server Error` – Unexpected server issue

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
✅ **Containerized Deployment using Docker**

---

## 📜 License

MIT License. Feel free to use and modify this project.

---

## 👨‍💻 Author

**Vishnujeet**  
GitHub: [github](https://github.com/Vishnujeet)  
