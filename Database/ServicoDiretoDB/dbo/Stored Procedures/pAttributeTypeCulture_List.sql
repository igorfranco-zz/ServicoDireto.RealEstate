
create procedure pAttributeTypeCulture_List
(
	@IDAttributeType int = null, 
	@IDCulture varchar(5) = null
)
as
begin
	select 
		pc.IDCulture,
		pc.IDAttributeType,
		pc.Description,
		c.Name as CultureName,
		i.Path as IconPath
	from AttributeTypeCulture as pc
		inner join culture as c on (pc.IDCulture = c.IDCulture)
		inner join icon i on (c.IDIcon = i.IDIcon)
	where (pc.IDAttributeType = @IDAttributeType or @IDAttributeType is null)
	and (pc.IDCulture = @IDCulture or @IDCulture is null)
end