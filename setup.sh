#sudo -u postgres psql -U postgres -c "drop database dbfwmon";
#sudo -u postgres psql -U postgres -c "create user dbfwmon with encrypted password '1'";
#sudo -u postgres psql -U postgres -c "CREATE DATABASE dbfwmon WITH ENCODING='UTF8' OWNER=dbfwmon"
#sudo -u postgres psql -U postgres -d dbfwmon -c "create extension ltree;"

#dotnet add package Microsoft.EntityFrameworkCore.Design
#dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
#dotnet add package Microsoft.AspNetCore
#dotnet add package Microsoft.AspNetCore.Server.Kestrel
#dotnet add package Microsoft.EntityFrameworkCore
#dotnet ef dbcontext scaffold "Host=localhost;Database=dbfwmon;Username=dbfwmon;Password=1" Npgsql.EntityFrameworkCore.PostgreSQL -o DB -f --context uiContext
#dotnet ef dbcontext scaffold "Host=192.168.56.2;Database=db1;Username=db1;Password=1" Npgsql.EntityFrameworkCore.PostgreSQL -o DB -f --context uiContext
#pg_dump --host=localhost -U dbfwmon -d dbfwmon -f dump.sql
#cp fwmonitor.json tmp.json
#cp fwmonitor.wrk.json fwmonitor.json
