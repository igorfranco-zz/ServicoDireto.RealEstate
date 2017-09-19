CREATE TABLE [dbo].[HierarchyStructureCulture] (
    [IDHierarchyStructure] INT            NOT NULL,
    [IDCulture]            VARCHAR (5)    NOT NULL,
    [Description]          NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_HierarchyStructureCulture] PRIMARY KEY CLUSTERED ([IDHierarchyStructure] ASC, [IDCulture] ASC),
    CONSTRAINT [FK_HierarchyStructureCulture_Culture] FOREIGN KEY ([IDCulture]) REFERENCES [dbo].[Culture] ([IDCulture]),
    CONSTRAINT [FK_HierarchyStructureCulture_HierarchyStructure] FOREIGN KEY ([IDHierarchyStructure]) REFERENCES [dbo].[HierarchyStructure] ([IDHierarchyStructure])
);

