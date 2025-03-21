# 使用官方 ASP.NET Core 運行時映像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# 建立應用程式並發佈
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 複製 .csproj 檔案並還原 NuGet 套件
COPY ["Api.csproj", "./"]
RUN dotnet restore "Api.csproj"

# 複製所有程式碼並建置
COPY . .
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

# 執行時環境
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
