

--pHierarchyStructure_List

CREATE procedure pHierarchyStructureCulture_List
(
	@IDCulture varchar(5),
	@IDHierarchyStructure int = null
)
as
begin
	select 
		pc.IDCulture,
		pc.IDHierarchyStructure,
		pc.Description,
		c.Name as CultureName,
		i.Path as IconPath
	from HierarchyStructureCulture as pc
		inner join Culture as c on (pc.IDCulture = c.IDCulture)
		inner join Icon as i on (c.IDIcon = i.IDIcon)
	where pc.IDCulture = @IDCulture
	and (pc.IDHierarchyStructure = @IDHierarchyStructure or @IDHierarchyStructure is null)
end

