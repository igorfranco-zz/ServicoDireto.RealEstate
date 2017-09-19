create procedure [dbo].[pIcon_List]
(
	@StartRowIndex int,
	@MaximumRows int,
	@RecordCount int output
)
AS 
BEGIN
	select @RecordCount = COUNT(*)
		from Icon with(nolock);
	
	 WITH ResultSet AS
    (
      select 
			ROW_NUMBER() OVER ( ORDER BY [i].[IDIcon], 
			[i].IDIcon) as [ROW_NUMBER],
			i.*			
		from Icon i with(nolock)
    )	

	select * from ResultSet
	where ([ROW_NUMBER] BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaximumRows or @StartRowIndex = 0)
	order by Name
	
END;


