CREATE TABLE [dbo].[HierarchyStructurePurpose] (
    [IDHierarchyStructure] INT NOT NULL,
    [IDPurpose]            INT NOT NULL,
    CONSTRAINT [PK_HierarchyStructurePurpose] PRIMARY KEY CLUSTERED ([IDHierarchyStructure] ASC, [IDPurpose] ASC),
    CONSTRAINT [FK_HierarchyStructurePurpose_HierarchyStructure] FOREIGN KEY ([IDHierarchyStructure]) REFERENCES [dbo].[HierarchyStructure] ([IDHierarchyStructure]),
    CONSTRAINT [FK_HierarchyStructurePurpose_Purpose] FOREIGN KEY ([IDPurpose]) REFERENCES [dbo].[Purpose] ([IDPurpose])
);

