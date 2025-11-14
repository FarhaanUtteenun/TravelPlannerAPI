# 🚀 Travel Planner API

A comprehensive microservice-based travel route planning system built with .NET 8, designed to aggregate and optimize travel routes across multiple transportation modes.

[![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![API Gateway](https://img.shields.io/badge/API%20Gateway-Ocelot-orange)](https://github.com/ThreeMammals/Ocelot)

## 📋 Overview

Travel Planner API is a lightweight, microservice-based system built with .NET 8.
It demonstrates how separate transport services (Train, Bus, Flight) can be unified under a single API Gateway (Ocelot).

This project simulates a “multi-modal travel search engine” using mock data, allowing clients to request routes from one endpoint:

### Key Highlights

- **Unified API Gateway**: Single entry point using Ocelot for all transportation services
- **Microservices Architecture**: Independent, scalable services for each transport mode
- **Blazor Frontend**: Modern, interactive web interface
- **Secure**: JWT-based authentication and authorization
- **Well-Documented**: Comprehensive Swagger/OpenAPI documentation
- **Clean Architecture**: SOLID principles and separation of concerns

---

## ✨ Features

- 🔍 **Multi-Modal Route Search**: Search across trains, buses, and flights simultaneously
- 🔐 **JWT Authentication**: Secure API access with token-based authentication
- 📊 **Route Aggregation**: Intelligent combination of routes from multiple sources
- 🚆 **Train Service**: Dedicated microservice for railway routes
- 🚌 **Bus Service**: Dedicated microservice for bus routes
- ✈️ **Flight Service**: Dedicated microservice for flight routes
- 📝 **Swagger Documentation**: Interactive API documentation for all endpoints
- 💾 **SQL Server Integration**: Persistent storage for routes and search caching
- 🎨 **Blazor UI**: Responsive web interface for end users
- 🔄 **API Gateway**: Centralized routing, load balancing, and request aggregation

---

## 🏗️ High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                         Blazor Frontend                          │
│                    (User Interface Layer)                        │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             │ HTTPS
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Ocelot API Gateway                            │
│                  (Port: 5000 / 5001)                            │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  • JWT Authentication                                     │  │
│  │  • Request Routing                                        │  │
│  │  • Load Balancing                                         │  │
│  │  • Rate Limiting                                          │  │
│  └──────────────────────────────────────────────────────────┘  │
└───────┬────────────────────┬────────────────────┬───────────────┘
        │                    │                    │
        │ HTTP               │ HTTP               │ HTTP
        ▼                    ▼                    ▼
┌──────────────┐    ┌──────────────┐    ┌──────────────┐
│   Train      │    │     Bus      │    │   Flight     │
│ Microservice │    │ Microservice │    │ Microservice │
│ (Port: 5100) │    │ (Port: 5200) │    │ (Port: 5300) │
└──────┬───────┘    └──────┬───────┘    └──────┬───────┘
       │                   │                    │
       └───────────────────┴────────────────────┘
                           │
                           ▼
                 ┌───────────────────┐
                 │   SQL Server      │
                 │   Database        │
                 │  • Routes Data    │
                 │  • Cache          │
                 │  • User Info      │
                 └───────────────────┘
```

---

## 🛠️ Tech Stack

### Backend

- **Framework**: .NET 8.0
- **API Gateway**: Ocelot
- **Authentication**: JWT (JSON Web Tokens)
- **Database**: SQL Server
- **ORM**: Entity Framework Core 8.0
- **Documentation**: Swashbuckle (Swagger/OpenAPI)

### Frontend

- **Framework**: Blazor Server/WebAssembly
- **UI Components**: Bootstrap 5 / MudBlazor

### DevOps & Tools

- **Version Control**: Git
- **API Testing**: Swagger UI, Postman
- **Dependency Injection**: Built-in .NET DI Container
- **Logging**: Serilog / Microsoft.Extensions.Logging

---

## 📁 Project Structure

```
TravelPlannerAPI/
│
├── src/
│   ├── ApiGateway/
│   │   ├── TravelPlanner.Gateway/
│   │   │   ├── ocelot.json              # Ocelot configuration
│   │   │   ├── Program.cs
│   │   │   └── appsettings.json
│   │
│   ├── Services/
│   │   ├── TravelPlanner.TrainService/
│   │   │   ├── Controllers/
│   │   │   ├── Models/
│   │   │   ├── Services/
│   │   │   ├── Data/
│   │   │   └── Program.cs
│   │   │
│   │   ├── TravelPlanner.BusService/
│   │   │   ├── Controllers/
│   │   │   ├── Models/
│   │   │   ├── Services/
│   │   │   └── Program.cs
│   │   │
│   │   └── TravelPlanner.FlightService/
│   │       ├── Controllers/
│   │       ├── Models/
│   │       ├── Services/
│   │       └── Program.cs
│   │
│   ├── Frontend/
│   │   └── TravelPlanner.BlazorApp/
│   │       ├── Pages/
│   │       ├── Shared/
│   │       ├── Services/
│   │       └── Program.cs
│   │
│   └── Shared/
│       └── TravelPlanner.Shared/
│           ├── DTOs/
│           ├── Interfaces/
│           └── Constants/
│
├── tests/
│   ├── TravelPlanner.TrainService.Tests/
│   ├── TravelPlanner.BusService.Tests/
│   └── TravelPlanner.FlightService.Tests/
│
├── docs/
│   ├── architecture.md
│   └── api-specs.md
│
├── .gitignore
├── README.md
└── TravelPlannerAPI.sln
```

---

## 🚀 Installation & Setup

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Step 1: Clone the Repository

```bash
git clone https://github.com/yourusername/travel-planner-api.git
cd travel-planner-api
```

### Step 2: Configure Connection Strings

Update the `appsettings.json` in each microservice with your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TravelPlannerDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Step 3: Run Database Migrations

Navigate to each microservice directory and run migrations:

```bash
# Train Service
cd src/Services/TravelPlanner.TrainService
dotnet ef database update

# Bus Service
cd ../TravelPlanner.BusService
dotnet ef database update

# Flight Service
cd ../TravelPlanner.FlightService
dotnet ef database update
```

**Alternative: Run all migrations from solution root**

```bash
dotnet ef database update --project src/Services/TravelPlanner.TrainService
dotnet ef database update --project src/Services/TravelPlanner.BusService
dotnet ef database update --project src/Services/TravelPlanner.FlightService
```

### Step 4: Run Microservices

Open multiple terminal windows and start each service:

**Terminal 1 - Train Service**

```bash
cd src/Services/TravelPlanner.TrainService
dotnet run
```

Default URL: `https://localhost:5100`

**Terminal 2 - Bus Service**

```bash
cd src/Services/TravelPlanner.BusService
dotnet run
```

Default URL: `https://localhost:5200`

**Terminal 3 - Flight Service**

```bash
cd src/Services/TravelPlanner.FlightService
dotnet run
```

Default URL: `https://localhost:5300`

### Step 5: Run Ocelot API Gateway

**Terminal 4 - API Gateway**

```bash
cd src/ApiGateway/TravelPlanner.Gateway
dotnet run
```

Default URL: `https://localhost:5000`

### Step 6: Run Blazor Frontend

**Terminal 5 - Blazor App**

```bash
cd src/Frontend/TravelPlanner.BlazorApp
dotnet run
```

Default URL: `https://localhost:7000`

### Step 7: Access the Application

- **Frontend**: https://localhost:7000
- **API Gateway**: https://localhost:5000/swagger
- **Train Service**: https://localhost:5100/swagger
- **Bus Service**: https://localhost:5200/swagger
- **Flight Service**: https://localhost:5300/swagger

---

## 📡 API Endpoints

### API Gateway Endpoints

| Method | Endpoint                                     | Description                             | Authentication |
| ------ | -------------------------------------------- | --------------------------------------- | -------------- |
| `GET`  | `/api/routes`                                | Get all available routes (aggregated)   | Required       |
| `GET`  | `/api/routes?from={origin}&to={destination}` | Search routes by origin and destination | Required       |
| `POST` | `/api/auth/login`                            | Authenticate user and get JWT token     | Not Required   |
| `POST` | `/api/auth/register`                         | Register new user                       | Not Required   |

### Train Service Endpoints

| Method | Endpoint                                       | Description                    |
| ------ | ---------------------------------------------- | ------------------------------ |
| `GET`  | `/train/routes`                                | Get all train routes           |
| `GET`  | `/train/routes?from={origin}&to={destination}` | Search train routes            |
| `GET`  | `/train/routes/{id}`                           | Get specific train route by ID |

### Bus Service Endpoints

| Method | Endpoint                                     | Description                  |
| ------ | -------------------------------------------- | ---------------------------- |
| `GET`  | `/bus/routes`                                | Get all bus routes           |
| `GET`  | `/bus/routes?from={origin}&to={destination}` | Search bus routes            |
| `GET`  | `/bus/routes/{id}`                           | Get specific bus route by ID |

### Flight Service Endpoints

| Method | Endpoint                                        | Description                     |
| ------ | ----------------------------------------------- | ------------------------------- |
| `GET`  | `/flight/routes`                                | Get all flight routes           |
| `GET`  | `/flight/routes?from={origin}&to={destination}` | Search flight routes            |
| `GET`  | `/flight/routes/{id}`                           | Get specific flight route by ID |

---

## 📝 Sample Requests & Responses

### 1. User Authentication

**Request:**

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "john.doe@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-11-14T10:30:00Z",
  "username": "john.doe@example.com"
}
```

### 2. Search Routes (Aggregated)

**Request:**

```http
GET /api/routes?from=London&to=Paris
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response:**

```json
{
  "searchCriteria": {
    "from": "London",
    "to": "Paris",
    "timestamp": "2025-11-13T14:30:00Z"
  },
  "routes": [
    {
      "id": "TR-001",
      "type": "Train",
      "from": "London St Pancras",
      "to": "Paris Gare du Nord",
      "departureTime": "2025-11-14T09:00:00Z",
      "arrivalTime": "2025-11-14T11:30:00Z",
      "duration": "2h 30m",
      "price": 120.5,
      "currency": "EUR",
      "provider": "Eurostar",
      "availableSeats": 45
    },
    {
      "id": "FL-042",
      "type": "Flight",
      "from": "London Heathrow (LHR)",
      "to": "Paris Charles de Gaulle (CDG)",
      "departureTime": "2025-11-14T10:15:00Z",
      "arrivalTime": "2025-11-14T12:30:00Z",
      "duration": "1h 15m",
      "price": 89.99,
      "currency": "EUR",
      "provider": "British Airways",
      "availableSeats": 120
    },
    {
      "id": "BUS-315",
      "type": "Bus",
      "from": "London Victoria Coach Station",
      "to": "Paris Gallieni",
      "departureTime": "2025-11-14T08:00:00Z",
      "arrivalTime": "2025-11-14T16:30:00Z",
      "duration": "8h 30m",
      "price": 35.0,
      "currency": "EUR",
      "provider": "FlixBus",
      "availableSeats": 28
    }
  ],
  "totalResults": 3,
  "bestRoute": {
    "byPrice": "BUS-315",
    "byDuration": "FL-042",
    "recommended": "TR-001"
  }
}
```

### 3. Get Train Routes

**Request:**

```http
GET /train/routes?from=Manchester&to=Edinburgh
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response:**

```json
{
  "routes": [
    {
      "id": "TR-156",
      "from": "Manchester Piccadilly",
      "to": "Edinburgh Waverley",
      "departureTime": "2025-11-14T07:30:00Z",
      "arrivalTime": "2025-11-14T11:15:00Z",
      "duration": "3h 45m",
      "price": 85.0,
      "currency": "GBP",
      "provider": "Avanti West Coast",
      "availableSeats": 67,
      "class": "Standard",
      "amenities": ["WiFi", "Power Outlets", "Refreshments"]
    }
  ]
}
```

---

## 📸 Screenshots

### Main Dashboard

> _[Placeholder: Add screenshot of Blazor frontend showing route search interface]_

![Dashboard](docs/images/dashboard.png)

### Route Search Results

> _[Placeholder: Add screenshot showing aggregated route results from multiple services]_

![Search Results](docs/images/search-results.png)

### Swagger API Documentation

> _[Placeholder: Add screenshot of Swagger UI showing API endpoints]_

![Swagger UI](docs/images/swagger-ui.png)

### Architecture Flow

> _[Placeholder: Add animated GIF showing request flow through API Gateway to microservices]_

![Architecture Flow](docs/images/architecture-flow.gif)

---

## 🧪 Testing

### Run Unit Tests

```bash
# Run all tests
dotnet test

# Run tests for specific service
dotnet test tests/TravelPlanner.TrainService.Tests

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=opencover
```

### API Testing with Postman

Import the Postman collection from `docs/postman/TravelPlannerAPI.postman_collection.json` to test all endpoints.

---

## 🔒 Authentication

The API uses JWT Bearer token authentication. To access protected endpoints:

1. Obtain a token via `/api/auth/login`
2. Include the token in the Authorization header:
   ```
   Authorization: Bearer YOUR_TOKEN_HERE
   ```

Token expiration is set to 24 hours by default (configurable in `appsettings.json`).

---

## 🌟 Future Enhancements

- [ ] **Real-time Updates**: WebSocket integration for live route updates
- [ ] **Price Comparison**: Historical price tracking and price alerts
- [ ] **Multi-leg Journeys**: Support for complex routes with multiple transfers
- [ ] **User Preferences**: Save favorite routes and travel preferences
- [ ] **Payment Integration**: Direct booking and payment through the platform
- [ ] **Mobile App**: Native iOS and Android applications
- [ ] **AI Route Optimization**: Machine learning for best route recommendations
- [ ] **Carbon Footprint**: Calculate and display environmental impact per route
- [ ] **Notifications**: Email/SMS alerts for price drops and delays
- [ ] **Containerization**: Docker support for easier deployment
- [ ] **Kubernetes**: Orchestration for production-grade scalability
- [ ] **GraphQL API**: Alternative API interface alongside REST
- [ ] **Caching Layer**: Redis integration for improved performance
- [ ] **Analytics Dashboard**: Admin panel with usage statistics and insights

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

Please ensure your code follows the existing style and includes appropriate tests.

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

```
MIT License

Copyright (c) 2025 Travel Planner API

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

---

## 👥 Authors

- **Your Name** - _Initial work_ - [YourGitHub](https://github.com/yourusername)

---

## 🙏 Acknowledgments

- [Ocelot](https://github.com/ThreeMammals/Ocelot) - API Gateway framework
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - ORM
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) - Swagger integration
- Microsoft for the excellent .NET ecosystem

---

## 📞 Support

For support, email support@travelplannerapi.com or open an issue in the GitHub repository.

---

<div align="center">

**⭐ If you find this project useful, please consider giving it a star! ⭐**

Made with ❤️ using .NET 8

</div>
