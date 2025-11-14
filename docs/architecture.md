# Travel Planner API Architecture

## Overview

The Travel Planner API is built using a microservices architecture pattern, where each transport mode (Train, Bus, Flight) is implemented as an independent service. An API Gateway (Ocelot) provides a unified entry point for all client applications.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                         Client Layer                             │
│                    (Blazor WebAssembly)                          │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             │ HTTPS/JWT
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Ocelot API Gateway                            │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  • Authentication & Authorization (JWT)                   │  │
│  │  • Request Routing & Aggregation                          │  │
│  │  • Rate Limiting                                          │  │
│  │  • Load Balancing                                         │  │
│  │  • Service Discovery                                      │  │
│  └──────────────────────────────────────────────────────────┘  │
└───────┬────────────────────┬────────────────────┬───────────────┘
        │                    │                    │
        ▼                    ▼                    ▼
┌──────────────┐    ┌──────────────┐    ┌──────────────┐
│   Train      │    │     Bus      │    │   Flight     │
│ Microservice │    │ Microservice │    │ Microservice │
│              │    │              │    │              │
│ Port: 5100   │    │ Port: 5200   │    │ Port: 5300   │
│              │    │              │    │              │
│ ┌──────────┐ │    │ ┌──────────┐ │    │ ┌──────────┐ │
│ │Controller│ │    │ │Controller│ │    │ │Controller│ │
│ └────┬─────┘ │    │ └────┬─────┘ │    │ └────┬─────┘ │
│      ▼       │    │      ▼       │    │      ▼       │
│ ┌──────────┐ │    │ ┌──────────┐ │    │ ┌──────────┐ │
│ │ Service  │ │    │ │ Service  │ │    │ │ Service  │ │
│ └────┬─────┘ │    │ └────┬─────┘ │    │ └────┬─────┘ │
│      ▼       │    │      ▼       │    │      ▼       │
│ ┌──────────┐ │    │ ┌──────────┐ │    │ ┌──────────┐ │
│ │EF Core   │ │    │ │EF Core   │ │    │ │EF Core   │ │
│ │DbContext │ │    │ │DbContext │ │    │ │DbContext │ │
│ └────┬─────┘ │    │ └────┬─────┘ │    │ └────┬─────┘ │
└──────┼───────┘    └──────┼───────┘    └──────┼───────┘
       │                   │                    │
       └───────────────────┴────────────────────┘
                           │
                           ▼
                 ┌───────────────────┐
                 │   SQL Server      │
                 │   Database        │
                 │                   │
                 │  • TrainDB        │
                 │  • BusDB          │
                 │  • FlightDB       │
                 └───────────────────┘
```

## Key Components

### 1. API Gateway (Ocelot)
- **Responsibility**: Single entry point for all client requests
- **Features**:
  - Request routing to appropriate microservices
  - JWT token validation
  - Rate limiting and throttling
  - Response aggregation
  - CORS policy management
- **Technology**: Ocelot on ASP.NET Core 8

### 2. Microservices

#### Train Service
- **Port**: 5100
- **Database**: TravelPlannerTrainDB
- **Endpoints**: `/api/routes`
- **Functionality**: Manages train route data

#### Bus Service
- **Port**: 5200
- **Database**: TravelPlannerBusDB
- **Endpoints**: `/api/routes`
- **Functionality**: Manages bus route data

#### Flight Service
- **Port**: 5300
- **Database**: TravelPlannerFlightDB
- **Endpoints**: `/api/routes`
- **Functionality**: Manages flight route data

### 3. Frontend (Blazor WebAssembly)
- **Port**: 7000
- **Technology**: Blazor WebAssembly
- **Features**:
  - Interactive route search interface
  - Real-time route comparison
  - Responsive design with Bootstrap 5

### 4. Shared Library
- **Purpose**: Common DTOs, interfaces, and constants
- **Used by**: All microservices and frontend

## Design Patterns

### 1. Microservices Pattern
Each transport mode is an independent service with its own:
- Database
- Business logic
- API endpoints
- Deployment lifecycle

### 2. API Gateway Pattern
Ocelot acts as the gateway providing:
- Single entry point
- Request routing
- Authentication enforcement
- Response aggregation

### 3. Repository Pattern
Each microservice uses Entity Framework Core as a repository abstraction.

### 4. Clean Architecture
- **Controllers**: Handle HTTP requests
- **Services**: Business logic (future implementation)
- **Data Access**: Entity Framework Core DbContext
- **Models**: Domain entities

## Communication Flow

### Search Routes Flow
1. Client sends GET request to Gateway: `/api/routes?from=London&to=Paris`
2. Gateway validates JWT token
3. Gateway routes request to all three microservices in parallel
4. Each microservice queries its database
5. Gateway aggregates responses
6. Gateway returns unified result to client

### Authentication Flow
1. Client sends credentials to `/api/auth/login`
2. Gateway validates credentials
3. Gateway generates JWT token
4. Client stores token
5. Client includes token in all subsequent requests

## Security

- **Authentication**: JWT Bearer tokens
- **Authorization**: Role-based access control (future)
- **HTTPS**: All communication encrypted
- **CORS**: Configured to allow specific origins

## Scalability Considerations

- Each microservice can be scaled independently
- Database per service allows for isolated scaling
- API Gateway can be load-balanced
- Stateless services enable horizontal scaling

## Future Enhancements

- Service discovery with Consul
- Distributed caching with Redis
- Message queue for async communication (RabbitMQ/Azure Service Bus)
- Circuit breaker pattern (Polly)
- Distributed tracing (OpenTelemetry)
- Containerization with Docker
- Orchestration with Kubernetes
