# Meet-e-Kat

Meetups ASP .Net Core WebAPI 


## How to Run

### Install all required Dependencies

To build, run and use this project You will need to install:
1. [.Net 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. [PostgreSQL 14](https://www.postgresql.org)

### Clone and Prepare the project

Now You should clone the project:
```
git clone https://github.com/satma0745/Meet-e-Kat.git
```

Open the project root directory (`Meet-e-Kat` by default) and install all NuGet dependencies:
```
dotnet restore
dotnet tool restore
```

Check application configuration in the `Meetekat.WebApi/appsettings.json` file.
Probably some of the configuration parameters wouldn't satisfy You (i.e. `"Persistence:Password"`).
You can override them by copying this file, renaming a copy into a `appsettings.Development.json` and changing whichever configuration parameter You want.
You can also delete all not-overridden parameters (including whole sections like `"Auth"` or `"Swagger"`).

Now You can safely migrate database and run the application:
```
dotnet ef database update --project ./Meetekat.WebApi
dotnet run --project ./Meetekat.WebApi
```

Application is up and running. Now You can go to the `"http://localhost:5201/api/swagger"` URL (by default) to explore all endpoints in Swagger Documentation.
