CREATE procedure pAttributeCulture_List
(
	@IDAttribute int = null, 
	@IDCulture varchar(5) = null
)
as
begin
	select ac.IDCulture,
		   ac.IDAttribute,
		   ac.Name,
		   ac.Value,
		   c.Name as CultureName,
		   i.Path as IconPath
	 from AttributeCulture as ac with(nolock)
		inner join Culture as c with(nolock) on (ac.IDCulture = c.IDCulture)
		inner join Icon as i with(nolock) on (c.IDIcon = i.IDIcon)
	where (ac.IDAttribute =	@IDAttribute or @IDAttribute is null)
	and (ac.IDCulture = @IDCulture or @IDCulture is null)
end
