create procedure [dbo].[pCountry_List]
(
	@StartRowIndex int,
	@MaximumRows int,
	@RecordCount int output
)
AS 
BEGIN
	select @RecordCount = COUNT(*)
		from Country c with(nolock);
	
	 WITH ResultSet AS
    (
      select 
			ROW_NUMBER() OVER ( ORDER BY [c].[IDCountry], 
			[c].IDCountry) as [ROW_NUMBER],
			c.*			
		from Country c with(nolock)
    )	

	select * from ResultSet
	where ([ROW_NUMBER] BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaximumRows or @StartRowIndex = 0)
	order by Name
	
END;


