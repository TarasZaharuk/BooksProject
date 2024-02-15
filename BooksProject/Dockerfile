FROM nginx AS base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BooksProject/BooksProject.csproj", "BooksProject/"]
RUN dotnet restore "BooksProject/BooksProject.csproj"

COPY . .
WORKDIR "/src/BooksProject"
RUN dotnet build "BooksProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BooksProject.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM  base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
#COPY  BooksProject/nginx.conf /etc/nginx/nginx.conf