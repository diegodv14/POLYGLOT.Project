El proyecto usa DB en Contenedores Docker, con volumenes de manera local.

# Configuración descentralizada

   1. Contenedor
      docker run -d --name config-server-test -p 8848:8848 -p 9848:9848 -v nacos_data:/home/nacos/data -e MODE=standalone nacos/nacos-server:latest 

# Rabbit MQ

   1. Contenedor
      **docker run -d --name rabbitmq-container -e RABBITMQ_DEFAULT_USER=invoiceUser -e RABBITMQ_DEFAULT_PASS=pruebaInvoice -p 5672:5672 -p 15672:15672 -v rabbitmq_data:/var/lib/rabbitmq rabbitmq:3-management**
   
   2. Ingresar Contenedor
      **docker exec -it rabbitmq-container /bin/bash**
   
   3. Crear Exchange
      **rabbitmqctl add_exchange polyglot direct**
   
   4. Crear Queue
      **rabbitmqctl add_queue polyglot_invoice**
   
   5. Vincular Queue con Exchange mediante RoutingKey
      **rabbitmqctl set_queue_binding polyglot_invoice polyglot polyglot_invoice**

# My SQL Pay

   1. Crear Contenedor
     **docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=prueba -e MYSQL_DATABASE=db_operation -e MYSQL_USER=securityPrueba -e MYSQL_PASSWORD=security -p 3306:3306 -v   
     mysql_data:/var/lib/mysql -d mysql:latest**

# Sql Server Security

   1. Crear Contenedor
       ***docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=ben10alienforce\*" -p 1433:1433 --name sqlserver-container -v sqlserver_data:/var/opt/mssql -d 
       mcr.microsoft.com/mssql/server:2022-latest***

# Postgres Invoice

   1. Crear contenedor
      **docker run -d --name postgres-container -e POSTGRES_USER=invoicePrueba -e POSTGRES_PASSWORD=prueba -e POSTGRES_DB=db_invoice -v postgres-data:/var/lib/postgresql/data -p 5432:5432 postgres**

