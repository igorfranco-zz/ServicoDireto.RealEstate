CREATE procedure pHierarchyStructure_List
(
	@IDHierarchyStructure int = -1,
	@Status smallint = -1,
	@IDCulture	varchar(5) = null
)	
as
begin
	select
		hsc.Description,
		hs.IDHierarchyStructure,
		hs.IDHierarchyStructureParent,
		i.Name as IconName,
		i.Path as IconPath,
		hs.IDIcon as IDIcon,
		hs.CreateDate,
		hs.CreatedBy,
		hs.ModifiedBy,
		hs.ModifyDate,
		hs.Status	
	from HierarchyStructure as hs
		inner join HierarchyStructureCulture as hsc on (hs.IDHierarchyStructure = hsc.IDHierarchyStructure)
		inner join Icon as i on (hs.IDIcon = i.IDIcon)
	where hsc.idculture	= @IDCulture
	and (hs.IDHierarchyStructure = @IDHierarchyStructure or @IDHierarchyStructure = -1)
	and (hs.[Status] = @Status or @Status = -1)	
	order by hsc.Description
end	
