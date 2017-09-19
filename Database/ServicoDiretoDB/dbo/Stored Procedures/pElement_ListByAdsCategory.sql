CREATE PROCEDURE [dbo].[pElement_ListByAdsCategory]
(
	@IDCulture VARCHAR(5), 
	@IDAdsCategory INT, 
	@IDCustomer INT
)
AS 
BEGIN
	SELECT 
		[e].IDElement, 
		[ec].Name, 
		[ec].Description,
		pc.Description + '/' + category.[Description] + '/' + [type].[Description] as HierarchyPath
	FROM dbo.Element[e] 
		INNER JOIN ElementCulture [ec] WITH(NOLOCK) ON (e.IDElement = ec.IDElement and ec.IDCulture = @IDCulture)
		INNER JOIN PurposeCulture AS pc WITH(NOLOCK) ON (e.IDPurpose = pc.IDPurpose and pc.IDCulture = @IDCulture )
		INNER JOIN HierarchyStructureCulture AS category WITH(NOLOCK) ON (e.IDHierarchyStructureParent = category.IDHierarchyStructure and category.IDCulture = @IDCulture )
		INNER JOIN HierarchyStructureCulture AS [type] WITH(NOLOCK) ON (e.IDHierarchyStructure = [type].IDHierarchyStructure and [type].IDCulture = @IDCulture )
	--	INNER JOIN dbo.AdsCategoryRelation [acr] ON ([e].IDElement = [acr].IDElement)		
	--WHERE ([e].IDCustomer = @IDCustomer OR @IDCustomer = 0)
	  --AND [acr].IDAdsCategory = @IDAdsCategory
	ORDER BY PageView
END
