## Project Structure

```
asp-net-core-api/
├── api/                              # Main API project
│   ├── src/
│   │   ├── Domain/
│   │   │   ├── Entities/
│   │   │   │   └── Investment.cs     # Investment entity with TVPI logic
│   │   │   ├── ObjectValues/
│   │   │   │   └── ErrorResponse.cs  # Error response model
│   │   │   └── Repositories/
│   │   │       └── InvestmentRepository.cs  # Repository interface
│   │   └── Infrastructure/
│   │       ├── Middleware/
│   │       │   └── GlobalExceptionHandlingMiddleware.cs
│   │       └── Repositories/
│   │           └── InMemoryDB/
│   │               └── InvestmentRepository.cs  # Implementation
│   ├── Program.cs                    # Application entry point
│   ├── api.csproj                   # Project file
│   └── Properties/
│       └── launchSettings.json     # Launch configuration
└── api.Tests/                       # Test project
    ├── src/Domain/Entities/
    │   └── InvestmentTest.cs        # Unit tests for Investment entity
    ├── UnitTest1.cs
    └── api.Tests.csproj
```

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Any IDE that supports .NET (Visual Studio, VS Code, JetBrains Rider)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd asp-net-core-api
```

### 2. Restore Dependencies

```bash
cd api
dotnet restore
```

### 3. Build the Project

```bash
dotnet build
```

### 4. Run the Application

```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### 5. Run Tests

```bash
cd ../api.Tests
dotnet test
```

## API Documentation
### Endpoints

#### 1. Health Check
```http
GET /
```
**Response**: `"Hello World!"`

#### 2. Get All Investments
```http
GET /api/investments
```
**Response**:
```json
[
  {
    "id": 1,
    "name": "Tech Startup Fund",
    "committedCapital": 1000000.0,
    "distributedCapital": 200000.0,
    "currentNetAssetValue": 1500000.0
  }
]
```

#### 3. Get Investment by ID
```http
GET /api/investments/{id}
```
**Parameters**:
- `id` (int): Investment ID

**Response**:
```json
{
  "id": 1,
  "name": "Tech Startup Fund",
  "committedCapital": 1000000.0,
  "distributedCapital": 200000.0,
  "currentNetAssetValue": 1500000.0
}
```

#### 4. Get TVPI for Investment
```http
GET /api/investments/{id}/tvpi
```
**Parameters**:
- `id` (int): Investment ID

**Response**:
```json
{
  "investmentId": 1,
  "tvpi": 1.7
}
```

#### 5. Create Investment
```http
POST /api/investments
Content-Type: application/json
```
**Request Body**:
```json
{
  "name": "New Fund",
  "committedCapital": 500000.0,
  "distributedCapital": 0.0,
  "currentNetAssetValue": 600000.0
}
```

**Response** (201 Created):
```json
{
  "id": 2,
  "name": "New Fund",
  "committedCapital": 500000.0,
  "distributedCapital": 0.0,
  "currentNetAssetValue": 600000.0
}
```

#### 6. Delete Investment
```http
DELETE /api/investments/{id}
```
**Parameters**:
- `id` (int): Investment ID

**Response**: 204 No Content

### Error Responses

All errors follow this format:
```json
{
  "message": "Error description",
  "statusCode": 400,
  "details": "Additional error details",
  "timestamp": "2024-01-01T00:00:00.000Z"
}
```

**Common Status Codes**:
- `400 Bad Request`: Invalid input data
- `404 Not Found`: Investment not found
- `500 Internal Server Error`: Server errors

## Example Usage

### Using curl

#### Create an investment:
```bash
curl -X POST http://localhost:5000/api/investments \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Real Estate Fund",
    "committedCapital": 2000000,
    "distributedCapital": 300000,
    "currentNetAssetValue": 2500000
  }'
```

#### Get TVPI:
```bash
curl http://localhost:5000/api/investments/1/tvpi
```

### Using PowerShell

#### Create an investment:
```powershell
$body = @{
    name = "Real Estate Fund"
    committedCapital = 2000000
    distributedCapital = 300000
    currentNetAssetValue = 2500000
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/investments" -Method Post -Body $body -ContentType "application/json"
```

#### Get all investments:
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/investments" -Method Get
```

## Testing

The project includes comprehensive unit tests:

```bash
cd api.Tests
dotnet test --verbosity normal
```