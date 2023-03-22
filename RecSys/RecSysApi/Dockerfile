FROM mcr.microsoft.com/dotnet/aspnet:6.0
FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /RecSys
EXPOSE 5001
EXPOSE 5003
EXPOSE 1433

ARG COPY_FROM_PATH
COPY ${COPY_FROM_PATH} .

ENTRYPOINT ["dotnet", "RecSysApi.Presentation.dll"]