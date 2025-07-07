#### Settings For Nacos

### Kubernetes

```json
{
  "cn": {
    "db-invoice-pg": "data source=postgres;initial catalog=db_invoice;user id=invoicePrueba;password=prueba;Encrypt=True;TrustServerCertificate=True;",
    "db-security-sqls": "Server=sqlserver,1433;Database=db_security;User Id=sa;Password=ben10alienforce*;Trusted_Connection=False;TrustServerCertificate=True;",
    "db-mongo-transaccion": "mongodb://admin:admin123@mongodb:27017/",
    "databaseName": "db_transaccion",
    "collection": "transaccion",
    "db-pay-mqsl": "Server=mysql;Port=3306;Database=db_operation;User=securityPrueba;Password=security;SslMode=Preferred;",
    "checkInvoiceApi": "http://invoice-api:8080/PolyGlot/Invoices/CheckInvoice"
  },
  "RabbitMQ": {
    "Host": "rabbitmq",
    "User": "invoiceUser",
    "Pass": "pruebaInvoice",
    "VirtualHost": "/",
    "QueueTransaccion": "polyglot_transaccion",
    "QueueInvoice": "polyglot_invoice",
    "Exchange": "polyglot",
    "RoutingKeyInvoice": "polyglot_invoice",
    "RoutingKeyTransaccion": "polyglot_transaccion"
  },
  "JWT": {
    "enabled": true,
    "issuer": "diegodev14",
    "audience": "web",
    "key": "millave_secreta$$$$$PruebaSecurityBasePolyglot",
    "expiration": "300"
  }
}
```

Forward Kong Gateway

kubectl port-forward --namespace kong svc/kong-kong-proxy 8080:80

Kong Url

http://kong-kong-admin.kong.svc:8001

### Localhost

Settings For Nacos

```json
{
  "cn": {
    "db-invoice-pg": "data source=localhost;initial catalog=db_invoice;user id=invoicePrueba;password=prueba;Encrypt=True;TrustServerCertificate=True;",
    "db-pay-mqsl": "Server=localhost;Port=3306;Database=db_operation;User=securityPrueba;Password=security;SslMode=Preferred;",
    "db-security-sqls": "Server=localhost,1433;Database=db_security;User Id=sa;Password=ben10alienforce*;Trusted_Connection=False;TrustServerCertificate=True;",
    "db-mongo-transaccion": "mongodb://admin:admin123@localhost:27017/",
    "databaseName": "db_transaccion",
    "collection": "transaccion",
    "checkInvoiceApi": "http://localhost:5022/PolyGlot/Invoices/CheckInvoice"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "User": "invoiceUser",
    "Pass": "pruebaInvoice",
    "VirtualHost": "/",
    "QueueTransaccion": "polyglot_transaccion",
    "QueueInvoice": "polyglot_invoice",
    "Exchange": "polyglot",
    "RoutingKeyInvoice": "polyglot_invoice",
    "RoutingKeyTransaccion": "polyglot_transaccion"
  },
  "JWT": {
    "enabled": true,
    "issuer": "diegodev14",
    "audience": "web",
    "key": "millave_secreta$$$$$PruebaSecurityBasePolyglot",
    "expiration": "300"
  }
}
```
