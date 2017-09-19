CREATE PROCEDURE [dbo].[pElementCulture_GetAll]
(
	@IDElement bigint = -1,
	@IDCulture varchar(5) = NULL,
	@StartRowIndex int,
	@MaximumRows int,
	@RecordCount int out
)
AS 
BEGIN
	select distinct 
		ROW_NUMBER() OVER ( ORDER BY ec.IDElement, 
		ec.IDElement) as [ROW_NUMBER],
		ec.IDElement,
		IDCulture = ec.IDCulture,                            
        Name = ec.Name,
        Description = ec.Description,
        CultureName = c.Name,
        IconPath = i.Path,	
        IconName = i.Name
	into #ElementCultureRange	
		from ElementCulture ec with(nolock)
		inner join Culture c  with(nolock) on ( ec.IDCulture = c.IDCulture )
		inner join Icon i with(nolock) on (c.IDIcon = i.IDIcon)
	where (ec.IDElement = @IDElement OR @IDElement = -1)
	and (ec.IDCulture = @IDCulture OR @IDCulture = NULL)

	select @RecordCount = count(*) from #ElementCultureRange

	select * from #ElementCultureRange
	where [ROW_NUMBER] BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaximumRows
	
END;
