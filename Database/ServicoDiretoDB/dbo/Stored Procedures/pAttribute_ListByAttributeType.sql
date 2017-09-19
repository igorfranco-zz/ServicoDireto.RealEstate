  
CREATE procedure [dbo].[pAttribute_ListByAttributeType]	     
(
	@IDAttributeType int,
	@IDCulture varchar(5),
	@Status smallint,
	@IDElement bigint	
)
as
begin    
    select 		
    	ac.Name,	
    	convert(bit, (select count(*) from elementattribute ea where a.idattribute = ea.idattribute and ea.idelement = @IDElement)) as Checked, 
    	(select top 1 coalesce(ea.value, ac.value, '') from elementattribute ea where a.idattribute = ea.idattribute and ea.idelement = @IDElement) as Value,  
    	i.Path as IconPath,	
    	i.Name as IconName,
		a.IDAttribute,
		a.IDIcon,
		a.EditMask,
		a.ModifyDate,
		a.DisplayMask,
		a.Status,
		a.CreatedBy,
		a.ModifiedBy,
		a.CreateDate,
		a.Acronym,
		convert(bit, isnull(a.[Required],0)) as [Required],
		a.[Index],
		a.MaxValue,
		a.MinValue,
    	atc.description as AttributeTypeName
    from attribute a 		
    	inner join attributeculture ac on (a.idattribute = ac.idattribute)	
    	inner join attribute_attributetype aat on (a.idattribute = aat.idattribute)	
    	inner join attributetype at on (aat.idattributetype = at.idattributetype)	
    	inner join attributetypeculture atc on (at.idattributetype = atc.idattributetype and atc.idculture = @IDCulture)	
    	inner join Icon i on (a.idicon = i.idIcon)	
    	inner join Culture c on (ac.idculture = c.idculture)	
    where a.[status] = @Status		
    	and at.[status] = @Status	
    	and ac.idculture = @IDCulture	
    	and aat.idattributetype = @IDAttributeType	
    order by ac.name		
end

