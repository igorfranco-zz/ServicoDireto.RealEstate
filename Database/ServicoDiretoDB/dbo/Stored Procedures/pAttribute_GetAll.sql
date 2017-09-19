CREATE PROCEDURE [dbo].[pAttribute_GetAll]
(
	@Status smallint = -1,
	@IDAttribute smallint = -1,
	@IDCulture varchar(5),
	@StartRowIndex int,
	@MaximumRows int,
	@RecordCount int out
)
AS 
BEGIN
	select distinct 
		ROW_NUMBER() OVER ( ORDER BY a.IDAttribute, 
		a.IDAttribute) as [ROW_NUMBER],
		ac.Value,
		ac.Name,
		a.IDAttribute,
		i.Name as IconName,
		i.Path as IconPath,
		a.IDIcon,
		a.DisplayMask,
		a.EditMask,
		a.CreateDate,
		a.CreatedBy,
		a.ModifiedBy,
		a.ModifyDate,
		a.Status,
		a.Acronym,
		convert(bit,0) as checked,
		'' as AttributeTypeName
	into #AttributeRange	
		from Attribute  a with(nolock)
		inner join AttributeCulture ac  with(nolock) on ( a.IDAttribute = ac.IDAttribute and ac.IDCulture = @IDCulture)
		inner join Icon  i with(nolock) on (a.IDIcon = i.IDIcon)
	where (a.Status = @Status OR @Status = -1)
	and (a.IDAttribute = @IDAttribute or @IDAttribute = -1)

	select @RecordCount = count(*) from #AttributeRange

	select * from #AttributeRange
	where [ROW_NUMBER] BETWEEN @StartRowIndex + 1 AND @StartRowIndex + @MaximumRows
	
END;
