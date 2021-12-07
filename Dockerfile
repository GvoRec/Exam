ARG NODE_IMAGE=node:16.13.0

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PFRAppAngular/PFRAppAngular.csproj", "PFRAppAngular/"]
RUN dotnet restore "PFRAppAngular/PFRAppAngular.csproj"
COPY . .
WORKDIR "/src/PFRAppAngular"
RUN dotnet build "PFRAppAngular.csproj" -c Release -o /app
RUN dotnet publish "PFRAppAngular.csproj" -c Release -o /app

FROM ${NODE_IMAGE} as node-build
WORKDIR /src
COPY PFRAppAngular/ClientApp .
RUN npm install
RUN npm rebuild node-sass
RUN npm run build -- --prod

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app .
COPY --from=node-build /src/dist ./ClientApp/dist
ENTRYPOINT ["dotnet", "PFRAppAngular.dll"]
