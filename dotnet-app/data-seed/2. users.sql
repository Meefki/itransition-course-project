insert into [identity_authorization].[dbo].[Users]
select
     r.AuthorUserId
    ,'test_user_' + cast(row_number() over (order by r.AuthorUserId) as varchar)
    ,null
    ,'test_user_' + cast(row_number() over (order by r.AuthorUserId) as varchar) + '@email.com'
    ,null
    ,0
    ,'64e604787cbf194841e7b68d7cd28786f6c9a0a3ab9f8b0a0e87cb4387ab0107'
    ,newid()
    ,newid()
    ,null
    ,0
    ,0
    ,null
    ,0
    ,0
  from [reviewing].[reviewing].[Reviews] as r

insert into [identity_authorization].[dbo].[UserRoles]
select u.Id, '9860daf9-3462-41a1-92a5-57dc252d2d00'
  from [identity_authorization].[dbo].[Users] as u
  left join [identity_authorization].[dbo].[UserRoles] as ur on ur.UserId = u.Id
  where ur.RoleId is null