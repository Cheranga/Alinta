FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Alinta.WebApi/Alinta.WebApi.csproj", "Alinta.WebApi/"]
COPY ["Alinta.Services.Abstractions/Alinta.Services.Abstractions.csproj", "Alinta.Services.Abstractions/"]
COPY ["Alinta.Core/Alinta.Core.csproj", "Alinta.Core/"]
COPY ["Alinta.Services/Alinta.Services.csproj", "Alinta.Services/"]
COPY ["Alinta.DataAccess.EntityFramework/Alinta.DataAccess.EntityFramework.csproj", "Alinta.DataAccess.EntityFramework/"]
COPY ["Alinta.DataAccess.Models/Alinta.DataAccess.Models.csproj", "Alinta.DataAccess.Models/"]
COPY ["Alinta.DataAccess.Abstractions/Alinta.DataAccess.Abstractions.csproj", "Alinta.DataAccess.Abstractions/"]
RUN dotnet restore "Alinta.WebApi/Alinta.WebApi.csproj"
COPY . .
WORKDIR "/src/Alinta.WebApi"
RUN dotnet build "Alinta.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Alinta.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Alinta.WebApi.dll"]