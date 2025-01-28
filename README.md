El proyecto usa DB en Contenedores Docker, con volumenes de manera local.

--Rabbit MQ--

1. Contenedor
   docker run -d --name rabbitmq-container -e RABBITMQ_DEFAULT_USER=invoiceUser -e RABBITMQ_DEFAULT_PASS=pruebaInvoice -p 5672:5672 -p 15672:15672 -v rabbitmq_data:/var/lib/rabbitmq rabbitmq:3-management

--My SQL Pay--

1. Crear Contenedor
  docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=prueba -e MYSQL_DATABASE=db_operation -e MYSQL_USER=securityPrueba -e MYSQL_PASSWORD=security -p 3306:3306 -v   
  mysql_data:/var/lib/mysql -d mysql:latest

--Sql Server Security--

1. Crear Contenedor
    docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=ben10alienforce*" -p 1433:1433 --name sqlserver-container -v sqlserver_data:/var/opt/mssql -d 
    mcr.microsoft.com/mssql/server:2022-latest

--Postgres Invoice--

1. Crear contenedor
   docker run -d --name postgres-container -e POSTGRES_USER=invoicePrueba -e POSTGRES_PASSWORD=prueba -e POSTGRES_DB=db_invoice -v postgres-data:/var/lib/postgresql/data -p 5432:5432 postgres

