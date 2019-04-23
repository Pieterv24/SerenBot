# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.1
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "SerenBot.dll"]