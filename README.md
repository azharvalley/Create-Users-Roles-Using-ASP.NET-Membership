# We are using ASP.NET Membership tables to create Roles, create users

You can create users and roles using ASP.NET Membership tables

To create ASP.NET Membership tables into your SQL Database. You will have to run (aspnet_regsql.exe) on your computer. 
You can find this exe to the this location--> C:\Windows\Microsoft.NET\Framework64\v4.0.30319

Please follow step in this tutorial -->https://www.c-sharpcorner.com/UploadFile/3d39b4/learn-membership-services-part-1-installing-Asp-Net-members/

After you have executed (aspnet_regsql.exe) you can now create roles and create users.

You will require SQL Server Database on your local machine.
--------------------------------------------------------------

Create a Database and replace your connectionString in the web.config file of the project.

connectionString="Data Source=[YouServerName];Initial Catalog=[YourDataBaseName];User ID=[DatabaseUserName];pwd=[DataBasePassword]"


To enable ASP.NET Membership: add below code into your web.config file.
----------------------------------------------------------------------------------------------------
  ```
  <appSettings>
  <add key="enableSimpleMembership" value="false" />
  </appSettings>
```

```
<system.web>
<!--To Activate Membership tables-->
    <membership defaultProvider="DefaultMembershipProvider" userIsOnlineTimeWindow="30" hashAlgorithmType="">
      <providers>
        <clear />
        <add connectionStringName="MyDBConnection" enablePasswordRetrieval="false" enablePasswordReset="true" requiresQuestionAndAnswer="false" applicationName="MyTestApplication" requiresUniqueEmail="false" passwordFormat="Clear" maxInvalidPasswordAttempts="9999" passwordAttemptWindow="10" passwordStrengthRegularExpression="" minRequiredPasswordLength="4" minRequiredNonalphanumericCharacters="0" name="DefaultMembershipProvider" type="System.Web.Security.SqlMembershipProvider, System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
      </providers>
    </membership>

    <profile>
      <providers>
        <clear />
        <add name="AspNetSqlProfileProvider" connectionStringName="MyDBConnection" applicationName="MyTestApplication" type="System.Web.Profile.SqlProfileProvider, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
      </providers>
    </profile>

    <roleManager enabled="true">
      <providers>
        <clear />
        <add connectionStringName="MyDBConnection" applicationName="MyTestApplication" name="AspNetSqlRoleProvider" type="System.Web.Security.SqlRoleProvider, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
      </providers>
    </roleManager>
    <!--End -->
 </system.web>
```

To use Form Authentication: add below code into the web.config file
```
<!--To use Authorize in ActionResult-->
    <authentication mode="Forms">
      <forms loginUrl="~/Home/Login" timeout="2880"/>
    </authentication>
    <!--End -->
```
