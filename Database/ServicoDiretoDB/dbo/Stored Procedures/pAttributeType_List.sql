
create procedure pAttributeType_List
(
	@IDCulture	varchar(5)
)
as 
begin
	select 
		atc.Description,
        at.IDAttributeType,
        i.Name as IconName,
        i.Path as IconPath,
        at.IDIcon,
        at.Hidden,
        at.CreateDate,
        at.CreatedBy,
        at.ModifiedBy,
        at.ModifyDate,
        at.Status,
        at.Acronym		
	from attributetype at
		inner join AttributeTypeCulture atc on (at.idattributetype = atc.idattributetype)
		inner join icon i on (at.IDIcon = i.IDIcon)
	where atc.idculture = @IDCulture
	order by atc.Description
end

