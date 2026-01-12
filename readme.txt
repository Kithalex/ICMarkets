---------------------------------
1. OVERVIEW
---------------------------------
This project is a .NET Web API that retrieves blockchain data from the BlockCypher APIs for:

- Ethereum mainnet (https://api.blockcypher.com/v1/eth/main)
- Dash mainnet (https://api.blockcypher.com/v1/dash/main)
- Bitcoin mainnet (https://api.blockcypher.com/v1/btc/main)
- Bitcoin testnet3 (https://api.blockcypher.com/v1/btc/test3)
- Litecoin mainnet (https://api.blockcypher.com/v1/ltc/main)

Each call to BlockCypher is stored in a SQLite database as the original JSON response plus a CreatedAt timestamp.

The API exposes endpoints to:

- Trigger data fetch and persistence per blockchain
- Fetch all blockchains in parallel
- Retrieve historical data, ordered by CreatedAt descending

---------------------------------
2. ARCHITECTURE
---------------------------------

The solution uses a clean, layered architecture:

- Api : ASP.NET Core Web API (controllers, Swagger, health, CORS)
- Application : CQRS (MediatR), use cases, abstractions (interfaces)
- Domain : Entities (BlockchainSnapshot), enums (BlockchainType)
- Infrastructure: EF Core (SQLite), repositories, Unit of Work, HttpClient services

Key patterns:

- Repository pattern
- Unit of Work pattern
- CQRS with MediatR
- Async + parallel operations (Task.WhenAll for “fetch-all”)

---------------------------------
3.TECHNOLOGIES
---------------------------------

- .NET 8 Web API (compatible with .NET 6+ requirement)
- Entity Framework Core + SQLite
- HttpClientFactory + Polly (retry logic)
- Swagger / OpenAPI
- ASP.NET Core HealthChecks '/health' endpoint
- CORS (basic, allow-all)
- xUnit for Unit and Integration tests
- Docker (Linux container) support

---------------------------------
4. PREREQUISITES
---------------------------------

- .NET 8 SDK installed
- dotnet-ef tool installed globally: dotnet tool install --global dotnet-ef
- (Optional, for Docker) Docker Desktop running and using Linux containers

---------------------------------
5. BUILD AND RUN LOCALLY
---------------------------------

From the repository root:

- Restore and build

	dotnet restore
	dotnet build

- Apply database migrations (creates blockchain.db)

	cd src/Infrastructure
	dotnet ef database update -s ../Api/Api.csproj -c BlockchainDbContext

- Run the API

	cd ../Api
	dotnet run

The console will show something like:

   	Now listening on: http://localhost:5140

Use the HTTP URL and port shown in your console.

---------------------------------
6. API ENDPOINTS
---------------------------------

Base URL (example):

   	http://localhost:5140/swagger/index.html

Note: Adjust port to whatever dotnet run prints.

- Fetch latest snapshot for a single blockchain

   	POST /api/blockchain/{type}/fetch

   	Path parameter type (case-insensitive enum):

	- Eth
	- Dash
	- BtcMain
	- BtcTest3
	- Ltc

	Behavior:

	- Calls the corresponding BlockCypher endpoint.
	- Stores the full JSON response + CreatedAt timestamp in BlockchainSnapshot.
	- Returns 202 Accepted on success.

- Fetch latest snapshots for all blockchains

	POST /api/blockchain/fetch-all

	Behavior:

	- Sequentially triggers fetch for:
		Eth, Dash, BtcMain, BtcTest3, Ltc.
	- Each response is persisted as a separate BlockchainSnapshot.
	- Returns 202 Accepted when all fetches complete.

- Get historical snapshots

	GET /api/blockchain/{type}/history?pageNumber=1&pageSize=50

	Query parameters:

		pageNumber – default 1
		pageSize – default 50

	Behavior:

	- Returns stored BlockchainSnapshot records for the given blockchain.
	- Ordered by CreatedAt descending (newest first).
	- Includes:
		Id
		BlockchainType
		RawJson
		CreatedAt

- Health check

	GET /health

	Returns a basic health status for the API.

---------------------------------
7. Testing
---------------------------------

Run all tests from the repo root:

	dotnet test

Test types

- UnitTests:
	Validate application logic.

- IntegrationTests:
	Example:
		POST /api/blockchain/eth/fetch
		GET /api/blockchain/eth/history
		Assert history contains at least one snapshot.

---------------------------------
8. Docker (Linux) Runtime
---------------------------------

A Linux Dockerfile is provided at the repository root.

- Build the image
	From the repository root (where Dockerfile is):

		docker build -t icmarkets-blockchain . --progress=plain

- Run the container
	Map host port 8080 to container port 8080:

		docker run -p 8080:8080 icmarkets-blockchain

- Accessing the API in Docker

	From the host:

		- http://localhost:8080/health
		- http://localhost:8080/swagger/index.html