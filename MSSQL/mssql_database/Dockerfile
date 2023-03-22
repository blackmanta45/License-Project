FROM mcr.microsoft.com/mssql/server:2019-CU13-ubuntu-20.04

COPY initdbfull.sql initdbfull.sql

USER root

RUN chmod +x initdbfull.sql

ENV MSSQL_SA_PASSWORD=Access4DB
ENV ACCEPT_EULA=Y

EXPOSE 1433/tcp

USER mssql
RUN /opt/mssql/bin/sqlservr --accept-eula & sleep 1200 \
    && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'Access4DB' -i ./initdbfull.sql \
    && pkill sqlservr 