GO
create procedure HasLocalAccount
@UserId nvarchar(200)
As
begin
select * from aspnet_Users where UserId=@UserId
End
GO
CREATE procedure [dbo].[aspnet_Membership_GetUserId]
@Username nvarchar(250)
As
Begin
select [UserId] from aspnet_Users where UserName=@Username
End
GO
CREATE procedure changepassword                          
 @password nvarchar(50),  
 @userId nvarchar(200)
 As  
 Begin 
update aspnet_Membership 
set Password=@password  
where UserId=@userId  
end