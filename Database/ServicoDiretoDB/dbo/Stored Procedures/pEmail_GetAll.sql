CREATE procedure [dbo].[pEmail_GetAll]
(	
	@IDCulture varchar(5),
	@IDEmail bigint = NULL,
	@IDCustomerTo int = NULL,
	@IDCustomerFrom int = NULL,
	@Status smallint = NULL,	
	@StartRowIndex int,
	@MaximumRows int,
	@RecordCount int out
)
as 
begin
	select 
			ROW_NUMBER() OVER ( ORDER BY [e].[IDEmail], 
			[e].IDEmail) as [ROW_NUMBER],
			[e].IDEmail,
			[e].IDCustomerFrom,
			[e].IDCustomerTo,
			cF.Name as IDCustomerFromName,
			cT.Name as IDCustomerToName,			
			[e].IDEmailParent,
			[e].[Subject],
			[e].Content,
			[e].[Status],
			[e].ModifyDate,
			[e].CreatedBy,
			[e].ModifiedBy,
			[e].CreateDate,
			[e].IDElement,
			[e].[read], 
			ec.Description ElementName
		into #EmailRange	
		from Email e with(nolock)
			inner join Customer as cF with(nolock) on ([e].IDCustomerFrom = cF.IDCustomer)
			inner join Customer as cT with(nolock) on ([e].IDCustomerTo = cT.IDCustomer)
			inner join dbo.ElementCulture as ec on (e.IDElement = ec.IDElement and ec.IDCulture = @IDCulture)
		where (e.IDCustomerFrom = @IDCustomerFrom OR @IDCustomerFrom = -1)
		AND (e.IDCustomerTo = @IDCustomerTo OR @IDCustomerTo = -1)
		AND (e.Status = @Status OR @Status = -1)
		AND (e.IDEmail = @IDEmail OR @IDEmail = -1)
		

	select @RecordCount = count(*) from #EmailRange

	select * from #EmailRange
	where [ROW_NUMBER] BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaximumRows
end
