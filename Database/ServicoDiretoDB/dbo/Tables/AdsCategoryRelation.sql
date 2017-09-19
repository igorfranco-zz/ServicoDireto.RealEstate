CREATE TABLE [dbo].[AdsCategoryRelation] (
    [IDAdsCategoryRelation] INT    IDENTITY (1, 1) NOT NULL,
    [IDPurpose]             INT    NULL,
    [IDHierarchyStructure]  INT    NULL,
    [IDElement]             BIGINT NULL,
    [IDAdsCategory]         INT    NULL,
    [IDCustomer]            INT    NULL,
    CONSTRAINT [PK_AdsCategoryRelation] PRIMARY KEY CLUSTERED ([IDAdsCategoryRelation] ASC),
    FOREIGN KEY ([IDAdsCategory]) REFERENCES [dbo].[AdsCategory] ([IDAdsCategory]),
    FOREIGN KEY ([IDCustomer]) REFERENCES [dbo].[Customer] ([IDCustomer])
);

