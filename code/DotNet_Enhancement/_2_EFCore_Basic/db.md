docker pull mcr.microsoft.com/mssql/server:2022-latest
docker run -itd --name mysql-test -p 3306:3306 -e MYSQL_ROOT_PASSWORD=123456 mysql
docker exec -it mysql-test bash
mysql -h localhost -u root -p

dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update

use efcore;
show databases;
show tables;
