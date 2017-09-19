CREATE procedure [dbo].[pFilter_GetAll]
(
	@IDCulture varchar(5)
)
as begin
	select 
		f.IDAttribute,
		f.IDFilter,
		ac.Name as AttributeName,
		a.DisplayMask,
		f.InitialValue,
		f.FinalValue,
		f.DefaultValue,
		f.ComponentType,
		f.CreateDate,
		f.CreatedBy,
		f.ModifiedBy,
		f.ModifyDate,
		f.Status	
	from filter as f
        inner join Attribute as a on (f.IDAttribute = a.IDAttribute)
        inner join AttributeCulture as ac on (a.IDAttribute = ac.IDAttribute and ac.IDCulture = @IDCulture)
    order by ac.Name
end
