CREATE procedure [dbo].[pAttribute_List]
(
	@IDCulture varchar(5),
	@Status smallint = null,
	@IDElement bigint = null,	
	@IDAttributeType int = null,
	@Acronym varchar(5) = null
)
as
begin  
	select
      Value = coalesce(ea.Value, ac.Value, ''),
      ac.Name,
      a.IDAttribute,
      i.Name as IconName,
      i.Path as IConPath,
      a.IDIcon,
      a.DisplayMask,
      a.EditMask,
      a.CreateDate,
      a.CreatedBy,
      a.ModifiedBy,
      a.ModifyDate,
      a.Status,
      a.Acronym,
      convert(bit,1) as Checked,
      atc.Description as AttributeTypeName
    from Attribute a
	  inner join AttributeCulture as ac on (a.IDAttribute = ac.IDAttribute and ac.IDCulture = @IDCulture)
	  inner join Attribute_AttributeType aat on a.IDAttribute = aat.IDAttribute
	  inner join AttributeType as at on (aat.IDAttributeType = at.IDAttributeType )
	  inner join AttributeTypeCulture as atc on (at.IDAttributeType = atc.IDAttributeType and atc.IDCulture = @IDCulture)
	  inner join ElementAttribute as ea on a.IDAttribute = ea.IDAttribute
	  inner join Icon as i on a.IDIcon = i.IDIcon
	  where (at.Acronym = @Acronym or @Acronym is null)
		and (ea.IDElement = @IDElement or @IDElement is null)
		and (a.Status = @Status or @Status is null)	
		and (at.IDAttributeType = @IDAttributeType or @IDAttributeType is null)			
    order by ac.Name
end
