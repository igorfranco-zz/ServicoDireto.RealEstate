CREATE procedure [dbo].[pElement_ListSimilar]
(
	@IDCulture varchar(5),
	@IDCustomer int,
	@IDElement bigint
)
as 
begin 
		declare @IDHierarchyStructure	int
		declare @IDHierarchyStructureParent	int
		--
		select top 1
			@IDHierarchyStructure = IDHierarchyStructure ,
			@IDHierarchyStructureParent = IDHierarchyStructure 
		from Element e with(nolock)
		where e.IDElement = @IDElement		
		--	
		select  
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
			[e].[Address],
			[e].PageView,
			[e].DetailView,
			[e].IsPromoted,
			[e].Url,
			[e].[Status],
			[e].ModifyDate,
			[e].CreateDate,
			[e].CreatedBy,
			[e].ModifiedBy,
			[e].Acronym,
			ec.[Description],
			[e].Discriminator,
			category.[Description] as CategoryName,
			[type].[Description] as TypeName,
			cy.Name as CountryName,
			sp.Name as StateProvinceName,
			c.Name as CityName,
			cr.Name as CustomerName,
			pc.[Description] as PurposeName,
			pc.[Description] + '/' + category.[Description] + '/' + [type].[Description] as HierarchyPath,
			2.0 as TotalArea,
			2 as Beds ,
			2 as Bathrooms ,
			2 as Garages ,
			2.0 as Price 							
		from Element e with(nolock)
			inner join Customer as cr with(nolock) on (e.IDCustomer = cr.IDCustomer)
			inner join City as c with(nolock) on (e.IDCity = c.IDCity)
			inner join StateProvince as sp with(nolock) on (c.IDStateProvince = sp.IDStateProvince)
			inner join Country as cy with(nolock) on (sp.IDCountry = cy.IDCountry)
			inner join HierarchyStructureCulture as category with(nolock) on (e.IDHierarchyStructureParent = category.IDHierarchyStructure and category.IDCulture = @IDCulture )
			inner join HierarchyStructureCulture as [type] with(nolock) on (e.IDHierarchyStructure = [type].IDHierarchyStructure and [type].IDCulture = @IDCulture )
			inner join PurposeCulture as pc with(nolock) on (e.IDPurpose = pc.IDPurpose and pc.IDCulture = @IDCulture )
			inner join ElementCulture ec with(nolock) on (e.IDElement = ec.IDElement and ec.IDCulture = @IDCulture)
		where e.Status = 1 --Ativos	
		and e.IDElement <> @IDElement
		and e.IDHierarchyStructure = @IDHierarchyStructure
		and e.IDHierarchyStructureParent = @IDHierarchyStructureParent
		and (e.IDCustomer = @IDCustomer or @IDCustomer is null)
end
