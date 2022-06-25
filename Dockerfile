FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /app

COPY *.sln .
COPY . . 

FROM build AS publish 
WORKDIR /app
RUN dotnet publish App/Checkout.Api/Checkout.Api.csproj -c Release -o out -p:PublishWithAspNetCoreTargetManifest=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS runtime
WORKDIR /app
COPY --from=publish /app/out ./



ENTRYPOINT ["dotnet", "Checkout.Api.dll"]