# AuthCodeGenerator
Test for a auth code generator 

1. Run Docker compose file in the "Deploy" Folder in Visual Studio or  the base folder of the project

2. After PostgresSQL and Redis containers are up and running clone the code and run it in Visual Studio

3. Run in the visual studio console the Entity framework commands in the "Deploy" folder file "EntityFramework.txt" to setup the database (entity Framework CodeFirst)

**Note: by default the docker-compose file containers have the ports that the code has in the appsettings.json file conn strings to servers 
6000 port for Redis 
5432 port for PostgreSQL

if you don't have those ports available please change the ports and update the appsettings.json file to be able to run the code
