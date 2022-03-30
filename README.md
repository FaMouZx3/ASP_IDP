# ASP.NET identity provider

This project is divided into three web services.
- Identity provider (IDP)
- Gateway
- Resource server

## IDP

The IDP uses Openiddict and a MSSQL server for login.
So if you want to use the IDP server you need a [MSSQL](https://docs.microsoft.com/de-de/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) installation.
The login data is stored encrypted.

## Gateway

The gateway web service uses [Ocelot](https://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html).
Via the Ocelot web service the incoming requests are forwarded to the resource web services.
Also, introspection against the IDP is used to check whether the client is authorized to make requests against the resource web services. If this is not the case, the requests are not forwarded.

## Resource server

The ResourceServer contains the controller that makes requests against the resource databases.
The responses are packaged in a JSON format and sent back to the client via the gateway server.

## Inizialization

After you open the IDP in Visual Studio, you may need to consider a few things.

First of all, it must be ensured that the startup for all web services is set to the name of the respective service. This must be done so that the web services are started with the correct port.

![alt text](https://i.postimg.cc/QCFr5GDw/Webservice-Properties.png)


So that you do not have to start each project individually, a multiple startup must be set in the properties of the solution.

![alt text](https://i.postimg.cc/j29QbLX2/Multiple-Startup.png)

## MSSQL

The tables are created automatically when the web services are started. It is important that the connection string in appsettings.json is set correctly before the first start of the applications. Who would like to take over the connectionstring so, which should create before the databases "DB_IdentityProvider" and "DB_ResourceServer", so that no problems occur.

![alt text](https://i.postimg.cc/L41wXyv0/Connectionstring.png)

![alt text](https://i.postimg.cc/7Ztb6YZ4/connectionstring-resourceserver.png)

## Frontend project

The frontend project which can login to the IDP and fetch data from the resource server was written in Angular and can be fetched [here](https://github.com/FaMouZx3/Angular-IDP). This project is not a high end frontend project but only serves as a test project for the IDP web service.
