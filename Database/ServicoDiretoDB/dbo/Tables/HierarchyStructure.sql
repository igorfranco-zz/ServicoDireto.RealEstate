CREATE TABLE [dbo].[HierarchyStructure] (
    [IDHierarchyStructure]       INT          IDENTITY (1, 1) NOT NULL,
    [IDHierarchyStructureParent] INT          NULL,
    [IDIcon]                     INT          NULL,
    [Status]                     SMALLINT     DEFAULT ((1)) NOT NULL,
    [CreateDate]                 DATETIME     DEFAULT (getdate()) NULL,
    [CreatedBy]                  VARCHAR (50) NULL,
    [ModifyDate]                 DATETIME     NULL,
    [ModifiedBy]                 VARCHAR (50) NULL,
    CONSTRAINT [PK_HierarchyStructure] PRIMARY KEY CLUSTERED ([IDHierarchyStructure] ASC),
    CONSTRAINT [FK_HierarchyStructure_HierarchyStructure] FOREIGN KEY ([IDHierarchyStructureParent]) REFERENCES [dbo].[HierarchyStructure] ([IDHierarchyStructure]),
    CONSTRAINT [FK_HierarchyStructure_Icon] FOREIGN KEY ([IDIcon]) REFERENCES [dbo].[Icon] ([IDIcon])
);

