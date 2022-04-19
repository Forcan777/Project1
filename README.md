# Project1
Web API - Restaurant X

## Project Description
A REST API design to handling customer orders and keep the records in a database.

## Technologies Used
* Visual Studio 2022
* Microsoft SQL server 2019
* Microsoft SQL Server Management Studio
* Swagger
* Serilog
* ADO.NET
* ASP.NET Core
* C#
* SQL

## Features
List of features ready and TODOs for future development
* Customer list: Create, Read/access, update, delete
* Customer order: create, read/access, update, delete
* Food item list for customer order: create, read/access, update, delete
* Food menu : create, read/access, update, delete
* Food type: create, read/access, update, delete
* Logs – tracking system information and error occurred with above functionalities

## To-do list:
* Continue to add/update necessary CRUD operation to further streamline the customer and order records
* Extra food selection:  BBQ Meat selection for food items
* Customer order types (dine-in, pick-up, delivery)
* Customer Payment methods (credit card info) and selection (cash, card)

## Getting Started
### To clone the project to local folder
Use the following command in the command prompt while inside the local folder of your choice
git clone https://github.com/Forcan777/Project1

### To use the Database backup file 
Below are the instruction to restore the database backup file on your SSMS:
1. Launch SQL Server Management Studio (SSMS) and connect to your SQL Server instance.
2. Right-click the Databases node in Object Explorer and select Restore Database....
3. Select Device:, and then select the ellipses (...) to locate your backup file.
4. Select Add and navigate to where your .bak file is located. Select the .bak file and then select OK.
5. Select OK to close the Select backup devices dialog box.
6. Select OK to restore the backup of your database.

Once both the project and database is ready, open the project solution file (.sln) with Visual Studio, and press F5 to build and run the API.

Once Swagger had appeared the API can now to tested for its functionalities.

