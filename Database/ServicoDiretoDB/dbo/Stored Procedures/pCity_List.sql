create procedure [dbo].[pCity_List]
(
	@StartRowIndex int,
	@MaximumRows int,
	@RecordCount int output
)
AS 
BEGIN
	select @RecordCount = COUNT(*)
		from City with(nolock);
	
	 WITH ResultSet AS
    (
      select 
			ROW_NUMBER() OVER ( ORDER BY [c].[IDCity], 
			[c].IDCity) as [ROW_NUMBER],
			c.*,
			co.Name as CountryName,
			sp.Name as StateName,
			co.IDCountry
		from City as c
		inner join StateProvince sp with(nolock) on (c.IDStateProvince = sp.IDStateProvince)
		inner join Country co  with(nolock) on (sp.IDCountry = co.IDCountry)
    )	

	select * from ResultSet
	where ([ROW_NUMBER] BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaximumRows) or @StartRowIndex =-1
	order by Name
	
END;


