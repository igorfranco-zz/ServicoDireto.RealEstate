CREATE procedure [dbo].[pElement_ListFavorite]
(
	@IDCulture varchar(5),
	@IDElements nvarchar(2000)
)
as 
begin 
	select distinct
		e.*,
		pc.Description + '/' + category.[Description] + '/' + [type].[Description] as HierarchyPath,
		ec.Description  as Name	
	from dbo.Element e 
	inner join dbo.ElementCulture ec on (e.IDElement = ec.IDElement and ec.IDCulture = @IDCulture)
	inner join dbo.IntegerCommaSplit(@IDElements) as f on (e.IDElement = f.IntegerValue)	
	inner join HierarchyStructureCulture as category with(nolock) on (e.IDHierarchyStructureParent = category.IDHierarchyStructure and category.IDCulture = @IDCulture )
	inner join HierarchyStructureCulture as [type] with(nolock) on (e.IDHierarchyStructure = [type].IDHierarchyStructure and [type].IDCulture = @IDCulture )
	inner join PurposeCulture as pc with(nolock) on (e.IDPurpose = pc.IDPurpose and pc.IDCulture = @IDCulture )
	
end
