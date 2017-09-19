create procedure [dbo].[pElement_ListByAlert]
(
	@IDCustomer int = 0,
	@IDCulture nvarchar(5),	
	@IDAlert bigint = 0	
)
as
begin
	select * from dbo.Element as e
	--select
	--	a.*,
	--	sp.IDStateProvince,
	--	sp.IDCountry,
	--	c.Name as CityName,
	--	sp.Name as StateProvinceName,
	--	co.Name as CountryName,	
	--	pc.Description as PurposeName,
	--	category.[Description] as CategoryName,
	--	[type].[Description] as TypeName,
	--	pc.Description + '/' + category.[Description] + '/' + [type].[Description] as HierarchyPath				
	--from dbo.Alert as a with(nolock)
	--inner join City as c with(nolock) on (a.IDCity = c.IDCity)
	--inner join dbo.StateProvince AS sp with(nolock)ON (c.IDStateProvince = sp.IDStateProvince)	
	--inner join dbo.PurposeCulture AS pc with(nolock) on(a.IDPurpose = pc.IDPurpose)
	--inner join dbo.Country AS Co with(nolock) ON (sp.IDCountry = co.IDCountry)	
	--inner join HierarchyStructureCulture as category with(nolock) on (a.IDHierarchyStructureParent = category.IDHierarchyStructure and category.IDCulture = @IDCulture )
	--inner join HierarchyStructureCulture as [type] with(nolock) on (a.IDHierarchyStructure = [type].IDHierarchyStructure and [type].IDCulture = @IDCulture )
	--where (a.IDAlert = @IDAlert or @IDAlert = 0)
	--and (a.IDCustomer = @IDCustomer or @IDCustomer = 0)
end
