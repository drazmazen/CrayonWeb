# CrayonWeb

This is a simple ASP.NET Core Web API to handle automated software purchase from the (imaginary) Cloud Computing Provider(CCP). For persisting, it uses Microsoft SQL Server.

## Running the project
The solution is created in Visual Studio 2022. Two simplest ways of running it are: 
 - using orchestration with docker-compose and in this case it will run both the API and the database in docker containers
 - using Visual Studio IIS Express. If this is the case, the database needs to be started using `docker-compose up db`
   
_Important note:_ When running and debugging using orchestration, the connestion string should reference the db server in appsettings.json as `server=db;`.
However, if the API is started in IIS Express, in the connection string db server should be `server=localhost,5434;`

## Testing using Swagger
If the project is started in development environment, Swagger support will be injected and the API page will be opened in the browser.

## Database seeding
When running the project in development environment, on each start, the DB will be migrated and seeded with some data if the tables are empty.
Seeded data is:

### Customers

| Id | Name|
|-----|-------|
|1|Cowabunga Inc.|
|2|Watchmen Inc.|

### Accounts
|Id|Name|CustomerId|
|--|----|----------|
|1|Leonardo|1|
|2|Raphael|1|
|3|Donatello|1|
|4|Michelangelo|1|
|5|Doctor Manhattan|2|
|6|Rorschach|2|
|7|Silk Spectre|2|
|8|Nite Owl|2|

## CCP client
All configurations regarding CCP client can be found inside appsettings.json, in the **Ccp** section:
```
"Ccp": {
    "UseCcpClientProd": false,
    "BaseAddress": "",
    "CancelSoftwareEndpoint": "",
    "ChangeQuantityEndpoint": "",
    "ExtendLicenseEndpoint": "",
    "GetAvailableSoftwareEndpoint": "",
    "OrderEndpoint": ""
  }
```
If `UseCcpClientProd` setting is true, a production version client will be injected at startup. Otherwise, the dev client will be injected which just returns hardcoded data.
