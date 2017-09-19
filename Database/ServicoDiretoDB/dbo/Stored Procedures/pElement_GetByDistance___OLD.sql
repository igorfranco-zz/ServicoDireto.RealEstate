CREATE procedure [dbo].[pElement_GetByDistance___OLD]
(
	@Filter xml, 
	@IDCulture varchar(5),
	@StartRowIndex int,
	@MaximumRows int,
	@RecordCount int output
)
AS 
BEGIN
	
	--declare @Filter xml, 
	--@IDCulture varchar(5),
	--@StartRowIndex int,
	--@MaximumRows int,
	--@RecordCount int

	set @IDCulture = 'pt-BR'
	set @StartRowIndex = 0
	set @MaximumRows = 100
	set @RecordCount  = 0

	--set @Filter=N'<ElementFilter xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><IDCountry>0</IDCountry><IDStateProvince>0</IDStateProvince><IDCity>0</IDCity><Purposes /><IDHierarchyStructureParent>0</IDHierarchyStructureParent><IDHierarchyStructure>0</IDHierarchyStructure><Radius>0</Radius><LatitudeBase>0</LatitudeBase><LongitudeBase>0</LongitudeBase><Title>ap</Title><BaseAddress /><Attributes /></ElementFilter>'
	----set @Filter = N'<ElementFilter xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><IDStateProvince>0</IDStateProvince><IDCity>0</IDCity><Purposes /><IDHierarchyStructureParent>0</IDHierarchyStructureParent><IDHierarchyStructure>0</IDHierarchyStructure><Radius>0</Radius><LatitudeBase>0</LatitudeBase><LongitudeBase>0</LongitudeBase><Title>ap</Title><BaseAddress /><Attributes /></ElementFilter>'
	
	set @Filter='<ElementFilter xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><IDCustomer>0</IDCustomer><IDCountry /><Radius>10</Radius><LatitudeBase>-30.02000000</LatitudeBase><LongitudeBase>-51.11000000</LongitudeBase><OrderBy /><BaseAddress /><Attributes /></ElementFilter>'

	declare @countPurposes int
	declare @countAttributes int
	declare @IDHierarchyStructureParent int
	declare @IDCity int
	declare @IDStateProvince int
	declare @IDCountry varchar(5)
	declare @Title nvarchar(300)	
	declare @IDHierarchyStructure int
	declare @LatitudeBase decimal(18,11)
	declare @LongitudeBase decimal(18,11)	
	declare @Radius decimal	

	--drop table #purposes
	--drop table #attributes
	--drop table #element
		
	select tab.col.value('text()[1]','int') as IDPurpose into #purposes from @Filter.nodes('//Purposes/IDPurpose') tab(col)
	select tab.col.value('IDAttribute[1]','int') as IDAttribute, 
		   isnull(tab.col.value('InitialValue[1]','float'),0) as InitialValue,
		   isnull(tab.col.value('FinalValue[1]','float'),0) as FinalValue
    into #attributes from @Filter.nodes('//Attributes/Attribute') tab(col)
    
    select @IDHierarchyStructureParent = (tab.col.value('text()[1]','int')) from @Filter.nodes('//IDHierarchyStructureParent') tab(col) -- categoria
	select @IDHierarchyStructure = tab.col.value('text()[1]','int') from @Filter.nodes('//IDHierarchyStructure') tab(col) --tipo
	select @IDStateProvince = tab.col.value('text()[1]','int') from @Filter.nodes('//IDStateProvince') tab(col) 
	select @IDCity = tab.col.value('text()[1]','int') from @Filter.nodes('//IDCity') tab(col) 
	select @IDCountry  = tab.col.value('text()[1]','varchar(5)') from @Filter.nodes('//IDCountry') tab(col) 
	select @Title		= tab.col.value('text()[1]','nvarchar(300)') from @Filter.nodes('//Title') tab(col) 
	select @LatitudeBase = tab.col.value('text()[1]','decimal(18,11)') from @Filter.nodes('//LatitudeBase') tab(col) 
	select @LongitudeBase = tab.col.value('text()[1]','decimal(18,11)') from @Filter.nodes('//LongitudeBase') tab(col) 
	select @Radius = tab.col.value('text()[1]','decimal') from @Filter.nodes('//Radius') tab(col) 	
	select @countPurposes = COUNT(*) from #purposes
	select @countAttributes = COUNT(*) from #attributes
	
	--if(@Radius = 0 or @Radius is null)
	--	set @Radius = 5
	
	set @IDHierarchyStructureParent = isnull(@IDHierarchyStructureParent, 0)
	set @IDHierarchyStructure = isnull(@IDHierarchyStructure, 0)
	set @IDStateProvince = isnull(@IDStateProvince, 0)
	set @IDCity = isnull(@IDCity, 0)
	set @IDCountry = isnull(@IDCountry, 0)
	--set @Title = isnull(@Title, 0)
	set @LatitudeBase = isnull(@LatitudeBase, 0)
	set @LongitudeBase = isnull(@LongitudeBase, 0)
	set @Radius = isnull(@Radius, 0)
	
	select @countPurposes = COUNT(*) from #purposes
	select @countAttributes = COUNT(*) from #attributes	
		
	--print '@IDHierarchyStructureParent: ' + convert(varchar, isnull(@IDHierarchyStructureParent, 0))	
	--print '@IDHierarchyStructure: ' + convert(varchar, @IDHierarchyStructure)	
	--print '@IDStateProvince:' + convert(varchar, @IDStateProvince)	
	--print '@IDCity:' + convert(varchar, @IDCity)	
	--print '@IDCountry' + @IDCountry
	--print '@countAttributes' + convert(varchar, @countAttributes)	
	--print '@countPurposes' + convert(varchar, @countPurposes)	
	--print '@LatitudeBase' + convert(varchar(1000), @LatitudeBase)			
	--print '@LongitudeBase' + convert(varchar(1000), @LongitudeBase)		
	--print 'Raio: ' + convert(varchar, @Radius)							
	
	select * into #element from
	(
		select distinct 
			ROW_NUMBER() OVER ( ORDER BY [e].[IDElement], 
			[e].IDElement) as [ROW_NUMBER],
			[e].IDElement,
			--ea.Description,
			--ea.IDAttribute,
			--ea.Value,
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
			ec.Description,
			[e].Discriminator,
			category.[Description] as CategoryName,
			[type].[Description] as TypeName,
			cy.Name as CountryName,
			sp.Name as StateProvinceName,
			c.Name as CityName,
			cr.Name as CustomerName,
			pc.Description as PurposeName,
			pc.Description + '/' + category.[Description] + '/' + [type].[Description] as HierarchyPath
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
			and (dbo.fGetDistance(@LatitudeBase, @LongitudeBase, e.Latitude, e.Longitude) <= @Radius or @Radius = 0)	
			and (e.IDElement in 
			(				
					select ea.IDElement from ElementAttribute ea with(nolock)
						inner join #attributes afilter on (ea.IDAttribute = afilter.IDAttribute ) 		
					where e.IDElement = ea.IDElement 
						and ea.Value is not null				
						and convert(float, replace( replace(ea.Value, '.',''),',','.')) between afilter.InitialValue and afilter.FinalValue
			) or @countAttributes = 0)
	) as ResultSet	
	
	if(@StartRowIndex <> -1 and @MaximumRows <> -1)	
	begin	
		select * from #element
		where [ROW_NUMBER] BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaximumRows
		order by [ROW_NUMBER]
	end
	else begin	
		select * from #element		
		order by [ROW_NUMBER]
	end	
	----Atualizando a visualização	
	select @recordCount = count(*) from #element
	
	update Element
		set PageView  = isnull(PageView,0) + 1
	where IDElement in (select IDElement from #element)	

END;
