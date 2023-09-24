insert into [reviewing].[ReviewLikes] (UserId, ReviewId) values
((select s.AuthorUserId from [reviewing].[Reviews] as s order by s.Id offset (cast(rand()*(150) as int)) rows fetch next (1) rows only), 
 (select s.Id from [reviewing].[Reviews] as s order by s.Id offset (cast(rand()*(150) as int)) rows fetch next (1) rows only))
go 600