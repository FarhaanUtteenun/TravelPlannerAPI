# Travel Planner API - API Specifications

## Base URLs

- **API Gateway**: `https://localhost:5000`
- **Train Service**: `https://localhost:5100`
- **Bus Service**: `https://localhost:5200`
- **Flight Service**: `https://localhost:5300`

## Authentication

All endpoints (except auth endpoints) require a valid JWT Bearer token.

### Headers
```
Authorization: Bearer {your_jwt_token}
Content-Type: application/json
```

## Endpoints

### Authentication Endpoints

#### POST /api/auth/login
Authenticate a user and receive a JWT token.

**Request Body:**
```json
{
  "username": "user@example.com",
  "password": "Password123!"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-11-15T10:30:00Z",
  "username": "user@example.com"
}
```

**Error Responses:**
- `401 Unauthorized`: Invalid credentials
- `400 Bad Request`: Missing or invalid request data

---

### Route Endpoints

#### GET /api/routes
Get all routes (aggregated from all services).

**Query Parameters:**
- `from` (optional): Origin location
- `to` (optional): Destination location

**Example Request:**
```
GET /api/routes?from=London&to=Paris
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "routeId": "TR-001",
    "from": "London St Pancras",
    "to": "Paris Gare du Nord",
    "departureTime": "2025-11-15T09:00:00Z",
    "arrivalTime": "2025-11-15T11:30:00Z",
    "duration": "2h 30m",
    "price": 120.50,
    "currency": "EUR",
    "provider": "Eurostar",
    "availableSeats": 45,
    "type": "Train"
  },
  {
    "id": 1,
    "routeId": "FL-042",
    "from": "London Heathrow (LHR)",
    "to": "Paris Charles de Gaulle (CDG)",
    "departureTime": "2025-11-15T10:15:00Z",
    "arrivalTime": "2025-11-15T12:30:00Z",
    "duration": "1h 15m",
    "price": 89.99,
    "currency": "EUR",
    "provider": "British Airways",
    "availableSeats": 120,
    "type": "Flight"
  }
]
```

**Error Responses:**
- `401 Unauthorized`: Missing or invalid token
- `500 Internal Server Error`: Server error

---

#### GET /api/train/routes
Get train routes only.

**Query Parameters:**
- `from` (optional): Origin station
- `to` (optional): Destination station

**Example Request:**
```
GET /api/train/routes?from=Manchester&to=Edinburgh
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "id": 2,
    "routeId": "TR-156",
    "from": "Manchester Piccadilly",
    "to": "Edinburgh Waverley",
    "departureTime": "2025-11-15T07:30:00Z",
    "arrivalTime": "2025-11-15T11:15:00Z",
    "duration": "3h 45m",
    "price": 85.00,
    "currency": "GBP",
    "provider": "Avanti West Coast",
    "availableSeats": 67,
    "class": "Standard",
    "amenities": "WiFi,Power Outlets,Refreshments"
  }
]
```

---

#### GET /api/bus/routes
Get bus routes only.

**Query Parameters:**
- `from` (optional): Origin station
- `to` (optional): Destination station

**Response Structure:** Same as train routes

---

#### GET /api/flight/routes
Get flight routes only.

**Query Parameters:**
- `from` (optional): Origin airport
- `to` (optional): Destination airport

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "routeId": "FL-042",
    "from": "London Heathrow (LHR)",
    "to": "Paris Charles de Gaulle (CDG)",
    "departureTime": "2025-11-15T10:15:00Z",
    "arrivalTime": "2025-11-15T12:30:00Z",
    "duration": "1h 15m",
    "price": 89.99,
    "currency": "EUR",
    "provider": "British Airways",
    "availableSeats": 120,
    "class": "Economy",
    "flightNumber": "BA308",
    "amenities": "In-flight Entertainment,Meals,WiFi"
  }
]
```

---

#### GET /api/train/routes/{id}
Get a specific train route by ID.

**Path Parameters:**
- `id`: Route ID (integer)

**Response (200 OK):** Single route object

**Error Responses:**
- `404 Not Found`: Route not found

---

#### POST /api/train/routes
Create a new train route (Admin only).

**Request Body:**
```json
{
  "routeId": "TR-999",
  "from": "Boston",
  "to": "New York",
  "departureTime": "2025-11-16T08:00:00Z",
  "arrivalTime": "2025-11-16T12:30:00Z",
  "duration": "4h 30m",
  "price": 95.00,
  "currency": "USD",
  "provider": "Amtrak",
  "availableSeats": 100,
  "class": "Business",
  "amenities": "WiFi,Power Outlets,Cafe Car"
}
```

**Response (201 Created):**
```json
{
  "id": 10,
  "routeId": "TR-999",
  "from": "Boston",
  "to": "New York",
  ...
}
```

---

#### PUT /api/train/routes/{id}
Update an existing train route.

**Path Parameters:**
- `id`: Route ID (integer)

**Request Body:** Complete route object with updated fields

**Response (204 No Content)**

**Error Responses:**
- `400 Bad Request`: ID mismatch
- `404 Not Found`: Route not found

---

#### DELETE /api/train/routes/{id}
Delete a train route.

**Path Parameters:**
- `id`: Route ID (integer)

**Response (204 No Content)**

**Error Responses:**
- `404 Not Found`: Route not found

---

## Error Handling

All errors return a consistent format:

```json
{
  "message": "Error description",
  "statusCode": 400,
  "timestamp": "2025-11-14T10:30:00Z"
}
```

## Rate Limiting

- **Limit**: 100 requests per minute per client
- **Response**: `429 Too Many Requests`

## Versioning

Current API version: v1

Future versions will use URL versioning: `/api/v2/routes`

## Data Models

### RouteDto
```typescript
{
  id: number;
  routeId: string;
  from: string;
  to: string;
  departureTime: DateTime;
  arrivalTime: DateTime;
  duration: string;
  price: decimal;
  currency: string;
  provider: string;
  availableSeats: number;
  type?: string; // "Train" | "Bus" | "Flight"
}
```

### LoginRequest
```typescript
{
  username: string;
  password: string;
}
```

### LoginResponse
```typescript
{
  token: string;
  expiration: DateTime;
  username: string;
}
```
