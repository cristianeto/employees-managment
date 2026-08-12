# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/ProCredit.EmployeeManagement.Api/ProCredit.EmployeeManagement.Api.csproj src/ProCredit.EmployeeManagement.Api/
COPY src/ProCredit.EmployeeManagement.Application/ProCredit.EmployeeManagement.Application.csproj src/ProCredit.EmployeeManagement.Application/
COPY src/ProCredit.EmployeeManagement.Domain/ProCredit.EmployeeManagement.Domain.csproj src/ProCredit.EmployeeManagement.Domain/
COPY src/ProCredit.EmployeeManagement.Infrastructure/ProCredit.EmployeeManagement.Infrastructure.csproj src/ProCredit.EmployeeManagement.Infrastructure/
RUN dotnet restore src/ProCredit.EmployeeManagement.Api/ProCredit.EmployeeManagement.Api.csproj

COPY src/ src/
RUN dotnet publish src/ProCredit.EmployeeManagement.Api/ProCredit.EmployeeManagement.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ProCredit.EmployeeManagement.Api.dll"]
