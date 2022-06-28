# Grab SDK image, create a name for it - "build"
FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .

# Copy ALL the projects

# App layer
COPY App.BLL/*.csproj ./App.BLL/
COPY App.BLL.DTO/*.csproj ./App.BLL.DTO/
COPY App.Contracts.BLL/*.csproj ./App.Contracts.BLL/
COPY App.Contracts.DAL/*.csproj ./App.Contracts.DAL/
COPY App.DAL.DTO/*.csproj ./App.DAL.DTO/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY App.Domain/*.csproj ./App.Domain/
COPY App.Public.DTO/*.csproj ./App.Public.DTO/
COPY App.Resources/*.csproj ./App.Resources/
COPY WebApp/*.csproj ./WebApp/

# Base layer
COPY Base.BLL/*.csproj ./Base.BLL/
COPY Base.DAL/*.csproj ./Base.DAL/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/
COPY Base.Domain/*.csproj ./Base.Domain/
COPY Base.Extensions/*.csproj ./Base.Extensions/
COPY Base.Resources/*.csproj ./Base.Resources/

# Contracts layer
COPY Base.Contracts.Base/*.csproj ./Base.Contracts.Base/
COPY Base.Contracts.BLL/*.csproj ./Base.Contracts.BLL/
COPY Base.Contracts.DAL/*.csproj ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/*.csproj ./Base.Contracts.Domain/

# Tests
COPY Testing.WebApp/*.csproj ./Testing.WebApp/

# Create the first layer with just NuGet packages installed
RUN dotnet restore

# copy everything else and build app

# App layer
COPY App.BLL/. ./App.BLL/
COPY App.BLL.DTO/. ./App.BLL.DTO/
COPY App.Contracts.BLL/. ./App.Contracts.BLL/
COPY App.Contracts.DAL/. ./App.Contracts.DAL/
COPY App.DAL.DTO/. ./App.DAL.DTO/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY App.Domain/. ./App.Domain/
COPY App.Public.DTO/. ./App.Public.DTO/
COPY App.Resources/. ./App.Resources/
COPY WebApp/. ./WebApp/

# Base layer
COPY Base.BLL/. ./Base.BLL/
COPY Base.DAL/. ./Base.DAL/
COPY Base.DAL.EF/. ./Base.DAL.EF/
COPY Base.Domain/. ./Base.Domain/
COPY Base.Extensions/. ./Base.Extensions/
COPY Base.Resources/. ./Base.Resources/

# Contracts layer
COPY Base.Contracts.Base/. ./Base.Contracts.Base/
COPY Base.Contracts.BLL/. ./Base.Contracts.BLL/
COPY Base.Contracts.DAL/. ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/. ./Base.Contracts.Domain/

# Tests
COPY Testing.WebApp/. ./Testing.WebApp/

WORKDIR /app/WebApp

## Change the connection string to use the database hosetd in Azure
#ENV
#ConnectionStrings:NpgsqlConnection="Host=localhost;Port=5432;Username=postgres;password=postgres;database=Yhistu;Include Error Detail=true",

# Compile the app with Release option and put files into dir "out"
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:latest AS runtime
WORKDIR /app

# Copy files from previous image ("build")
COPY --from=build /app/WebApp/out ./
ENTRYPOINT ["dotnet", "WebApp.dll"]
