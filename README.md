El proyecto usa DB en Contenedores Docker, con volumenes de manera local.

//My SQL Pay
docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=prueba -e MYSQL_DATABASE=db_operation -e MYSQL_USER=securityPrueba -e MYSQL_PASSWORD=security -p 3306:3306 -v mysql_data:/var/lib/mysql -d mysql:latest


//Sql Server Security
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=ben10alienforce*" -p 1433:1433 --name sqlserver-container -v sqlserver_data:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2022-latest
