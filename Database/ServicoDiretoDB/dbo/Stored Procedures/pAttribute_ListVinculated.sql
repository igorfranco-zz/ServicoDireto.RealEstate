--declare @IDAttributeType int = 1
--declare @IDCulture varchar(5) = 'pt-br'

----vinculados
--select 
--    ac.Name,
--    a.IDAttribute,
--    a.IDIcon,
--    a.DisplayMask,
--    a.EditMask,
--    a.CreateDate,
--    a.CreatedBy,
--    a.ModifiedBy,
--    a.ModifyDate,
--    a.Status
--from Attribute_AttributeType as aat 
--	inner join Attribute as a on (aat.IDAttribute = a.IDAttribute)
--	inner join AttributeCulture as ac on (a.IDAttribute = ac.IDAttribute)--, IDCulture = ServiceContext.ActiveLanguage } equals new { IDAttribute = , IDCulture = ac.IDCulture }
--where --(aat.IDAttributeType = @IDAttributeType) 
----	and 
--a.Status = 1 --ativo
--and ac.IDCulture = @IDCulture
--order by ac.Name

 --inner join dbo.IntegerCommaSplit(@IDElements) as f on (e.IDElement = f.IntegerValue)	  
 -- @IDElements nvarchar(2000)
CREATE procedure pAttribute_ListVinculated
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
	where (a.IDAttribute in (select IntegerValue from dbo.IntegerCommaSplit(@IDAttributes)) or len(@IDAttributes) = 0)
		and	a.Status = 1
		and ac.IDCulture = @IDCulture
		and (aat.IDAttributeType = @IDAttributeType)
	order by ac.Name
end
