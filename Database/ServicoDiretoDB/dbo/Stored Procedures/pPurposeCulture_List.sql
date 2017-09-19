

CREATE procedure pPurposeCulture_List
	@IDCulture varchar(5)
as
begin
	select 
		pc.IDCulture,
		pc.IDPurpose,
		pc.Description,
		c.Name as CultureName,
		i.Path as IconPath
	from purposeculture pc 
		inner join culture c on (pc.idculture = c.idculture)
		inner join icon i on (c.idicon = i.idicon)
	where c.IDCulture = @IDCulture or @IDCulture is null	
end