# Found-It -- Lost and Found Management System
## To get the program running


### Install NuGet packages

1: Microsoft.EntityFrameworkCore

2: Microsoft.EntityFrameworkCore.SqlServer

3: Microsoft.EntityFrameworkCore.Tools

4: Microsoft.Extensions.Configuration

5: Microsoft.Extensions.Configuration.Json


### Hook up the database

Using the database makefile provided in the makefile folder, generate the database.

Get the connection string for the database, then paste it into appsettings.json replacing "CONNECTION STRING HERE" with the connection string.


## Using the program

The program is a simple console app, it provides simple menus in the console for manipulating data.

The program accounts for two types of users; admins and regular users. Admins are intended for staff working on the premises the app is deployed on. Regular users are meant to be customers of the premises.

An admin user is already seeded with the makefile for the database the login information is as follows: email: admin@admin.com password: admin123

To create and use a "User" please follow the appropriate methods once the program is run in the console.

The underlying idea with the two user system is "Users" may report their belongings as a lost item. They may also make claims on found items posted by "Admins".

Admins upon finding an item on the premises of which no lost item request was posted for may make a found item entry that regular use can make claims on. They may also mark lost items as found or claimed.

The program also offers ways to update, delete found item, lost item and claim entries alike accounting for ownership when applicable.



TESTING FOR THE LOST AND FOUND MANAGEMENT FOUND HERE:

https://docs.google.com/document/d/18Rc26MJzbVMUJIk4zoVUiUlpgQukwWlpirYxUlkTY4E/edit?tab=t.0
