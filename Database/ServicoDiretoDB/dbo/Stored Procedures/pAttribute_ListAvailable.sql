

CREATE procedure pAttribute_ListAvailable
(
	@IDAttributes varchar(3000) = null,
	@IDCulture varchar(5) = 'pt-br',
	@IDAttributeType int = null
)
as
begin
	select 
		--aat.*,
		ac.Name,
		a.IDAttribute,
		a.IDIcon,
		a.DisplayMask,
		a.EditMask,
		a.CreateDate,
		a.CreatedBy,
		a.ModifiedBy,
		a.ModifyDate,
		a.Status		
	from Attribute as a
		inner join AttributeCulture as ac on (a.IDAttribute = ac.IDAttribute)
		left join Attribute_AttributeType as aat on (aat.IDAttribute = a.IDAttribute)
	where (a.IDAttribute not in (select IntegerValue from dbo.IntegerCommaSplit(@IDAttributes)) or len(@IDAttributes) = 0)
		and	a.Status = 1
		and ac.IDCulture = @IDCulture
		and (aat.IDAttributeType = @IDAttributeType or @IDAttributeType is null or aat.IDAttributeType is null)
	order by ac.Name
end

