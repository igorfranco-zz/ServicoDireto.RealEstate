
create procedure [dbo].[pStateProvince_List]
(
	@StartRowIndex int,
	@MaximumRows int,
	@RecordCount int output
)
AS 
BEGIN
	select @RecordCount = COUNT(*)
		from StateProvince with(nolock);
	
	 WITH ResultSet AS
    (
      select 
			ROW_NUMBER() OVER ( ORDER BY [sp].[IDStateProvince], 
			[sp].IDStateProvince) as [ROW_NUMBER],
			sp.*,
			c.IDCountry + '-' + c.Name as CountryName
		from StateProvince sp with(nolock)
		inner join Country c  with(nolock) on (sp.IDCountry = c.IDCountry)		
    )	

	select * from ResultSet
	where [ROW_NUMBER] BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaximumRows
	order by Name
	
END;


