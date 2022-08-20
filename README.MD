# Data Translate (Backend)

This project is a migration of the backend of the project [Data-translate](https://github.com/faouziMohamed/Data-translate), 
from Nextjs (under the hood likely nodejs server) and MongoDb to Asp.Net Core and Sql Server

## Getting started 
- First open the solution using either Rider or Visual Studio 2022 (Mandatory for dotnet 6+)
- Install the [Entity Framework CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) (if you are not using Visual Studio)
- Make sure to have [Microsoft Sql Server (Express)](https://www.microsoft.com/en-gb/sql-server/sql-server-downloads) installed on your computer

### Creating the database (MS SQL Server)
Run the migration commande to create the database.  
_Feel free to change connection string to set other values. By default the database that will be created is called *Data-Translate*_ 
1. If you are using Rider or other IDE than Visual Studio, open a terminal in the ```km.Translate.DataLib``` project and run the command below:  
    --> creating the first migration Then applying it to create the database with tables
    ```bash
      dotnet ef migrations add InitialMigration -o Data/Migrations 
      dotnet ef database update
   ```
1. If you are using Visual Studio, set `km.Translate.DataLib` as Startup project, open the Package Manager Console and run the command below:  
    ```
    Add-Migration InitialMigration -o Data/Migrations
    Update-Database
    ```
###  Initializing the database with seed Data
This step use seed data located on the [km.Translate.DataLib/dataSettings.json](km.Translate.DataLib/dataSettings.json) to populate the database

1. If you are in Visual Studio, set `km.Translate.Api` as Startup Project
1. Start the development server (`km.Translate.Api`), Swagger should Open exposing available Endpoints
1. Use the endpoint `/api/Starter/initialize` to initialize the database. It may take a moment before it finish initialization
    
