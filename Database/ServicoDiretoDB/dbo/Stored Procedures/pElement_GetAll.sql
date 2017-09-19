
CREATE PROCEDURE [dbo].[pElement_GetAll]
(
	@IDElement int = NULL,
	@IDCustomer int = NULL,
	@Status smallint = NULL,
	@IDCulture varchar(5),
	@SortExpr varchar(20),
	@SortDir varchar(20),
	@StartRowIndex int,
	@MaximumRows int,
	@RecordCount int out
)
AS 
BEGIN
	select distinct 
		ROW_NUMBER() OVER ( ORDER BY [e].[IDElement], 
		[e].IDElement) as [ROW_NUMBER],
		[e].IDElement,
		ec.Name,			
		[e].IDHierarchyStructure,
		[e].IDHierarchyStructureParent,
		[e].IDPurpose,
		[e].IDCity,
		[e].IDCustomer,
		[e].IDItemSite,
		[e].Latitude,
		[e].Longitude,
		[e].ShowAddress,
		[e].Address,
		[e].PageView,
		[e].DetailView,
		[e].IsPromoted,
		[e].Url,
		[e].Status,
		[e].ModifyDate,
		[e].CreateDate,
		[e].CreatedBy,
		[e].ModifiedBy,
		[e].Acronym,
		[e].Discriminator,
		ec.Description,
		category.[Description] as CategoryName,
		[type].[Description] as TypeName,
		cy.IDCountry,
		cy.Name as CountryName,
		sp.Name as StateProvinceName,
		sp.IDStateProvince,		
		c.Name as CityName,
		cr.Name as CustomerName,
		pc.Description as PurposeName,
		pc.Description + '/' + category.[Description] + '/' + [type].[Description] as HierarchyPath,
		dbo.fGetAttributeValue (e.idElement, 'TA', @IDCulture) as TotalArea,
		dbo.fGetAttributeValue (e.idElement, 'BEDS', @IDCulture) as Beds ,
		dbo.fGetAttributeValue (e.idElement, 'BATH', @IDCulture) as Bathrooms ,
		dbo.fGetAttributeValue (e.idElement, 'GAR', @IDCulture) as Garages ,
		dbo.fGetAttributeValue (e.idElement, 'VALT', @IDCulture) as Price 				

	into #ElementRange	
	from Element e with(nolock)
		inner join Customer as cr with(nolock) on (e.IDCustomer = cr.IDCustomer)
		inner join City as c with(nolock) on (e.IDCity = c.IDCity)
		inner join StateProvince as sp with(nolock) on (c.IDStateProvince = sp.IDStateProvince)
		inner join Country as cy with(nolock) on (sp.IDCountry = cy.IDCountry)
		inner join HierarchyStructureCulture as category with(nolock) on (e.IDHierarchyStructureParent = category.IDHierarchyStructure and category.IDCulture = @IDCulture )
		inner join HierarchyStructureCulture as [type] with(nolock) on (e.IDHierarchyStructure = [type].IDHierarchyStructure and [type].IDCulture = @IDCulture )
		inner join PurposeCulture as pc with(nolock) on (e.IDPurpose = pc.IDPurpose and pc.IDCulture = @IDCulture )
		inner join ElementCulture ec with(nolock) on (e.IDElement = ec.IDElement and ec.IDCulture = @IDCulture)
	where (e.IDElement = @IDElement OR @IDElement = -1)
	AND (e.IDCustomer = @IDCustomer OR @IDCustomer = -1)
	AND (e.Status = @Status OR @Status = -1)

select @RecordCount = count(*) from #ElementRange

select * from #ElementRange
where [ROW_NUMBER] BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaximumRows
	
END;



