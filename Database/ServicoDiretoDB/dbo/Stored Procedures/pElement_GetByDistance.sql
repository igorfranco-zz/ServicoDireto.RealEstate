
CREATE procedure [dbo].[pElement_GetByDistance]
(
	@Filter xml, 
	@IDCulture varchar(5),
	@StartRowIndex int,
	@MaximumRows int,
	@SortExpr varchar(50),
	@SortDir varchar(50),
	@RecordCount int output
)
AS 
BEGIN
	--declare @Filter xml, 
	--@IDCulture varchar(5),
	--@StartRowIndex int,
	--@MaximumRows int,
	--@RecordCount int,
	--@SortExpr varchar(50),
	--@SortDir varchar(50)
	--
	--set @SortExpr=N'CREATEDATE'
	--set @SortDir=N'DESC'
	--set @IDCulture = 'pt-BR'
	--set @StartRowIndex = 0
	--set @MaximumRows = 100
	--set @RecordCount  = 0

	--set @Filter=N'
	--<ElementFilter xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
	--	<Purposes>
	--		<IDPurpose>1</IDPurpose>
	--	</Purposes>
	--	<IDHierarchyStructureParent>64</IDHierarchyStructureParent>
	--	<IDHierarchyStructure>66</IDHierarchyStructure>
	--	<Radius>0</Radius>
	--	<LatitudeBase>-30.0233901</LatitudeBase>
	--	<LongitudeBase>-51.1103122</LongitudeBase>
	--	<OrderBy>createDate DESC</OrderBy>
	--	<BaseAddress>Petrópolis, Porto Alegre - RS, 91250-180, Brazil</BaseAddress>
	--	<Attributes>
	--		<Attribute>
	--			<Acronym>VALT</Acronym>
	--			<InitialValue>1</InitialValue>
	--			<FinalValue>500</FinalValue>
	--		</Attribute>
	--	</Attributes>
	--	<PageIndex>0</PageIndex>
	--	<TotalRecodsPerPage>12</TotalRecodsPerPage>
	--	<GroupIn>3</GroupIn>
	--	<UseInternalAuth>false</UseInternalAuth>
	--</ElementFilter>'

	declare @countPurposes int
	declare @countAttributes int
	declare @IDHierarchyStructureParent int
	declare @IDCity int
	declare @IDCustomer int
	declare @IDStateProvince int
	declare @IDCountry varchar(5)
	declare @Title nvarchar(300)	
	declare @IDHierarchyStructure int
	declare @LatitudeBase decimal(18,11)
	declare @LongitudeBase decimal(18,11)	
	declare @Radius decimal	

	--drop table #purposes
	--drop table #attributes
	--drop table #filtered_attributes
	----drop table #element
	
	--carregando valor xml em tabela	
	select tab.col.value('text()[1]','int') as IDPurpose into #purposes from @Filter.nodes('//Purposes/IDPurpose') tab(col)
	select tab.col.value('Acronym[1]','varchar(5)') as Acronym, 
		   isnull(tab.col.value('InitialValue[1]','float'),0) as InitialValue,
		   isnull(tab.col.value('FinalValue[1]','float'),0) as FinalValue
    into #attributes from @Filter.nodes('//Attributes/Attribute') tab(col)    
    
    --buscando possiveis imoveis baseados nos filtros
    select 
		ea.idelement
	into #filtered_attributes	
	from #attributes at
		inner join attribute a with(nolock) on (at.acronym = a.acronym)
		inner join elementattribute ea with(nolock) on (a.idattribute = ea.idattribute)
		inner join element e with(nolock) on (e.idelement = ea.idelement)
	where ea.Value is not null 
		and isnumeric(ea.Value) = 1
		and e.status = 1--ativo
		and convert(float, ea.Value) between at.InitialValue and at.FinalValue									
	group by ea.idelement    
    
    select @IDHierarchyStructureParent = isnull((tab.col.value('text()[1]','int')),0) from @Filter.nodes('//IDHierarchyStructureParent') tab(col) -- categoria
	select @IDHierarchyStructure = isnull(tab.col.value('text()[1]','int'),0) from @Filter.nodes('//IDHierarchyStructure') tab(col) --tipo
	select @IDStateProvince = isnull(tab.col.value('text()[1]','int'),0) from @Filter.nodes('//IDStateProvince') tab(col) 
	select @IDCity = isnull(tab.col.value('text()[1]','int'),0) from @Filter.nodes('//IDCity') tab(col) 
	select @IDCustomer = isnull(tab.col.value('text()[1]','int'),0) from @Filter.nodes('//IDCustomer') tab(col) 	
	select @IDCountry  = isnull(tab.col.value('text()[1]','varchar(5)'),0) from @Filter.nodes('//IDCountry') tab(col) 
	select @Title  = isnull(tab.col.value('text()[1]','nvarchar(300)'),0) from @Filter.nodes('//Title') tab(col) 
	select @LatitudeBase = isnull(tab.col.value('text()[1]','decimal(18,11)'),0) from @Filter.nodes('//LatitudeBase') tab(col) 
	select @LongitudeBase = isnull(tab.col.value('text()[1]','decimal(18,11)'),0) from @Filter.nodes('//LongitudeBase') tab(col) 
	select @Radius = isnull(tab.col.value('text()[1]','decimal'),0) from @Filter.nodes('//Radius') tab(col) 	
	select @countPurposes = COUNT(*) from #purposes
	select @countAttributes = COUNT(*) from #attributes
	
	if(@Radius = 0 or @Radius is null)
		set @Radius = 5
	
	set @IDHierarchyStructureParent = ISNULL(@IDHierarchyStructureParent, 0)
	set @IDHierarchyStructure = ISNULL(@IDHierarchyStructure, 0)
	set @IDStateProvince = ISNULL(@IDStateProvince, 0)
	set @IDCustomer = ISNULL(@IDCustomer, 0)
	set @IDCity = ISNULL(@IDCity, 0)
	set @IDCountry = ISNULL(@IDCountry, 0)
	set @Radius = ISNULL(@Radius, 20)
	set @LatitudeBase = ISNULL(@LatitudeBase, 0)
	set @LongitudeBase = ISNULL(@LongitudeBase, 0)
		
	print '@IDHierarchyStructureParent: ' + convert(varchar, @IDHierarchyStructureParent)	
	print '@IDHierarchyStructure: ' + convert(varchar, @IDHierarchyStructure)	
	print '@IDStateProvince: ' + convert(varchar, @IDStateProvince)	
	print '@IDCity: ' + convert(varchar, @IDCity)	
	print '@IDCountry: ' + @IDCountry
	print '@countAttributes: ' + convert(varchar, @countAttributes)	
	print '@countPurposes: ' + convert(varchar, @countPurposes)	
	print '@LatitudeBase: ' + convert(varchar(1000), @LatitudeBase)			
	print '@LongitudeBase: ' + convert(varchar(1000), @LongitudeBase)		
	print 'Raio: ' + convert(varchar, @Radius)							

	select @RecordCount = COUNT(*)
		from Element e with(nolock)
			inner join Customer as cr with(nolock) on (e.IDCustomer = cr.IDCustomer)
			inner join City as c with(nolock) on (e.IDCity = c.IDCity)
			inner join StateProvince as sp with(nolock) on (c.IDStateProvince = sp.IDStateProvince)
			inner join Country as cy with(nolock) on (sp.IDCountry = cy.IDCountry)
			inner join HierarchyStructureCulture as category with(nolock) on (e.IDHierarchyStructureParent = category.IDHierarchyStructure and category.IDCulture = @IDCulture )
			inner join HierarchyStructureCulture as [type] with(nolock) on (e.IDHierarchyStructure = [type].IDHierarchyStructure and [type].IDCulture = @IDCulture )
			inner join PurposeCulture as pc with(nolock) on (e.IDPurpose = pc.IDPurpose and pc.IDCulture = @IDCulture )
			inner join ElementCulture ec with(nolock) on (e.IDElement = ec.IDElement and ec.IDCulture = @IDCulture)
		--			inner join ElementAttribute ea with(nolock) on (e.IDElement = ea.IDElement)		
		--			inner join #attributes afilter on (ea.IDAttribute = afilter.IDAttribute and convert(float, replace( replace(ea.Value, '.',''),',','.')) between afilter.InitialValue and afilter.FinalValue	) 		
		where e.Status = 1 --Ativos
			and (ec.[Description] like '%' + @Title + '%' or @Title is null)
			and (e.IDPurpose in (select IDPurpose from #purposes) or @countPurposes = 0)	
			and (e.IDHierarchyStructureParent = @IDHierarchyStructureParent or @IDHierarchyStructureParent = 0)
			and (e.IDHierarchyStructure = @IDHierarchyStructure or @IDHierarchyStructure = 0)
			and (e.IDCity = @IDCity or @IDCity = 0)
			and (sp.IDStateProvince = @IDStateProvince or @IDStateProvince = 0)
			and (cy.IDCountry = @IDCountry or @IDCountry = '0')
			and (e.IDCustomer = @IDCustomer or @IDCustomer = 0)			
			and ((dbo.fGetDistance(@LatitudeBase, @LongitudeBase, e.Latitude, e.Longitude) <= @Radius or @Radius = 0) or (@LatitudeBase=0 and  @LongitudeBase = 0))
			and (e.IDElement in 
			(				
				select idelement from #filtered_attributes	
			) or @countAttributes = 0
			);
			
	 print '@RecordCount: ' + convert(varchar, @RecordCount);
	
	 WITH ElementsResultSet AS
    (
      select distinct 
			ROW_NUMBER() OVER ( ORDER BY [e].[IDElement], 
			[e].IDElement) as [ROW_NUMBER],
			[e].IDElement,
			ec.Name,	
			[e].DefaultPicturePath,
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
			ec.Description,
			[e].Discriminator,
			category.[Description] as CategoryName,
			[type].[Description] as TypeName,
			cy.Name as CountryName,
			sp.Name as StateProvinceName,
			c.Name as CityName,
			cr.Name as CustomerName,
			pc.Description as PurposeName,
			pc.Description + '/' + category.[Description] + '/' + [type].[Description] as HierarchyPath,			
			dbo.fGetAttributeValue (e.idElement, 'TA', @IDCulture) as TotalArea,
			dbo.fGetAttributeValue (e.idElement, 'BEDS', @IDCulture) as Beds ,
			dbo.fGetAttributeValue (e.idElement, 'BATH', @IDCulture) as Bathrooms ,
			dbo.fGetAttributeValue (e.idElement, 'GAR', @IDCulture) as Garages ,
			dbo.fGetAttributeValue (e.idElement, 'VALT', @IDCulture) as Price 				
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
			and (ec.[Description] like '%' + @Title + '%' or @Title is null)
			and (e.IDPurpose in (select IDPurpose from #purposes) or @countPurposes = 0)	
			and (e.IDHierarchyStructureParent = @IDHierarchyStructureParent or @IDHierarchyStructureParent = 0)
			and (e.IDHierarchyStructure = @IDHierarchyStructure or @IDHierarchyStructure = 0)
			and (e.IDCity = @IDCity or @IDCity = 0)
			and (sp.IDStateProvince = @IDStateProvince or @IDStateProvince = 0)
			and (cy.IDCountry = @IDCountry or @IDCountry = '0')
			and (e.IDCustomer = @IDCustomer or @IDCustomer = 0)			
			and ((dbo.fGetDistance(@LatitudeBase, @LongitudeBase, e.Latitude, e.Longitude) <= @Radius or @Radius = 0) or (@LatitudeBase=0 and  @LongitudeBase = 0))	 
			and (e.IDElement in 
			(				
				select idelement from #filtered_attributes	
			)
			 or @countAttributes = 0
			)
    )	

	select * from ElementsResultSet
		where [ROW_NUMBER] BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaximumRows
	ORDER BY 
	CASE WHEN @sortDir = 'ASC' AND @SortExpr = 'NAME' THEN name END,
	CASE WHEN @sortDir = 'DESC' AND @SortExpr = 'NAME' THEN name END DESC,
	--
	CASE WHEN @sortDir = 'ASC' AND @SortExpr = 'PRICE' THEN price END,
	CASE WHEN @sortDir = 'DESC' AND @SortExpr = 'PRICE' THEN price END DESC,
	--
	CASE WHEN @sortDir = 'ASC' AND @SortExpr = 'ISPROMOTED' THEN isPromoted END,
	CASE WHEN @sortDir = 'DESC' AND @SortExpr = 'ISPROMOTED' THEN isPromoted END DESC,
	--
	CASE WHEN @sortDir = 'ASC' AND @SortExpr = 'CREATEDATE' THEN CreateDate END,	
	CASE WHEN @sortDir = 'DESC' AND @SortExpr = 'CREATEDATE' THEN CreateDate END DESC,
	--
	CASE WHEN @sortDir = 'ASC' AND @SortExpr = 'STATUS' THEN [Status] END,	
	CASE WHEN @sortDir = 'DESC' AND @SortExpr = 'STATUS' THEN [Status] END DESC
END;


