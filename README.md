# WatchList

## Overview

WatchList is a Web Project for user to keep track of their favorite series and movies. Currently, this project does not allow to create an account. So there are two Usernames and passwords that you may use to access their respective account (of course this only connects to your local database, so have fun messing around with it!). The users have been seeded when cloning and building this project. This Project allows you to add, edit, and delete series. There is also a "About site" page that has a short description of the website and a little about myself as well.  

The two Usernames & Passwords:

- User: Mapache

   Pass: 123
- User: Ivan

   Pass: 456

## Features
- User Authentication: Checks for valid usernames and passwords.
- Management: Users can add, remove, and update shows in their watchlist.
- Genre Filtering: Filter shows by genre when looking at your full list.
- Navigation bar: After login, you can use the nav bar to navigate to other pages
- Responsive Design: Responsive and works on desktop and mobile devices.

## How it was developed
- ASP.NET Core MVC for back-end
- HTML, CSS, Bootstrap 5 and javascript for front-end
- Entity Framework Core for database management.
- Microsoft SQL Server for database storage.

## Use
- Login with the usernames and passwrods provided.
- Home page after login shows last 10 series recently added
- Manage and add series
- Log out

## Installation
1) Clone the repository to your local machine.
2) Open the solution in Visual Studio.
3) Restore NuGet packages and build the solution.
4) In your appsettings.json, you may update to your connection strings you wish or you may leave it alone. Just as long you have Microsoft SQL Server in your machine.
5) you might need to run the database migration to create the needed tables. If so, run the command, "dotnet ef database update".
6) Build and Run Application

   

  


