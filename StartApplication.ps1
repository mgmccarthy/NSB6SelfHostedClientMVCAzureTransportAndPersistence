docker run -d -p 1433:1433 --network nat --name sql -e sa_password=Password123! -e ACCEPT_EULA=Y microsoft/mssql-server-windows-developer

docker-compose up -d