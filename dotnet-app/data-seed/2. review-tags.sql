insert into [reviewing].[ReviewTag] (ReviewId, [TagsName]) values
((select s.Id from [reviewing].[Reviews] as s order by s.Id offset (cast(rand()*(150) as int)) rows fetch next (1) rows only),
 (select t.[Name] from [reviewing].[Tags] as t order by t.[Name] offset (cast(rand()*(52) as int)) rows fetch next (1) rows only))
go 500