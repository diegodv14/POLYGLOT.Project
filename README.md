El proyecto usa DB en Contenedores Docker, con volumenes de manera local.

--My SQL Pay--

1. Crear Contenedor
  docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=prueba -e MYSQL_DATABASE=db_operation -e MYSQL_USER=securityPrueba -e MYSQL_PASSWORD=security -p 3306:3306 -v   
  mysql_data:/var/lib/mysql -d mysql:latest

--Sql Server Security--

1. Crear Contenedor
   
  docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=ben10alienforce*" -p 1433:1433 --name sqlserver-container -v sqlserver_data:/var/opt/mssql -d 
  mcr.microsoft.com/mssql/server:2022-latest

2. Acceder al Contenedor con usuario de root
   
   docker exec -it --user root sqlserver-container bash

3. Ejecutar Script


    CREATE DATABASE db_security;
    
    USE db_security;
    
    CREATE TABLE users (
      id_user INT PRIMARY KEY,   -- Campo id_user como clave primaria
      username VARCHAR(100),     -- Campo username con un máximo de 100 caracteres
      password VARCHAR(100)      -- Campo password con un máximo de 100 caracteres
    );

--Postgres Invoice--


docker run -d --name postgres-container -e POSTGRES_USER=invoicePrueba -e POSTGRES_PASSWORD=prueba -e POSTGRES_DB=db_invoice -v postgres-data:/var/lib/postgresql/data -p 5432:5432 postgres

