# We are using ASP.NET Membership tables to create Roles, create users

You can create users and roles using ASP.NET Membership tables

To create ASP.NET Membership tables into your SQL Database. You will have to run (aspnet_regsql.exe) on your computer. 
You can find this exe to the this location--> C:\Windows\Microsoft.NET\Framework64\v4.0.30319

Please follow step in this tutorial -->https://www.c-sharpcorner.com/UploadFile/3d39b4/learn-membership-services-part-1-installing-Asp-Net-members/

After you have executed (aspnet_regsql.exe) you can now create roles and create users.

#You will require SQL Server Database on your local machine.
Create a Database and replace your connectionString in the web.config file of the project.

connectionString="Data Source=[YouServerName];Initial Catalog=[YourDataBaseName];User ID=[DatabaseUserName];pwd=[DataBasePassword]"
