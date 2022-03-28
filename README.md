# ASP_IDP

This project is divided into three web services.
- IDP
- Gateway
- ResourceServer

## IDP

The IDP uses Openiddict and a MSSQL server for login.
So if you want to use the IDP server you need a [MSSQL](https://docs.microsoft.com/de-de/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) installation.
The login data is stored encrypted.

## Gateway

The gateway web service uses [Ocelot](https://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html).
Via the Ocelot web service the incoming requests are forwarded to the resource web services.
Also, introspection against the IDP is used to check whether the client is authorized to make requests against the resource web services. If this is not the case, the requests are not forwarded.

## ResourceServer

The ResourceServer contains the controller that makes requests against the resource databases.
The responses are packaged in a JSON format and sent back to the client via the gateway server.
