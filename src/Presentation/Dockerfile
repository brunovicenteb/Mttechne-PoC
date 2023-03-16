#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
LABEL maintainer 'Senior Architect <Bruno Belchior at brunovicenteb@gmail.com>'
WORKDIR /app
ENV DOTNET_RUNNING_IN_CONTAINER=true
RUN apk add --no-cache --update musl musl-utils musl-locales tzdata
ENV TZ=America/Sao_Paulo
RUN cp /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Toolkit/Mttechne.Toolkit.csproj", "Toolkit/"]
COPY ["Domain/Mttechne.Domain.csproj", "Domain/"]
COPY ["Infra/Data/Mttechne.Infra.Data.csproj", "Infra/Data/"]
COPY ["Infra/IoC/Mttechne.Infra.IoC.csproj", "Infra/IoC/"]
COPY ["Presentation/Mttechne.UI.Web.csproj", "Presentation/"]
RUN dotnet restore "Presentation/Mttechne.UI.Web.csproj"
COPY . .
RUN dotnet build "Presentation/Mttechne.UI.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Presentation/Mttechne.UI.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mttechne.UI.Web.dll"]