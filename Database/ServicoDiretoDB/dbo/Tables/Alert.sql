CREATE TABLE [dbo].[Alert] (
    [IDAlert]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [IDHierarchyStructure]       INT             NULL,
    [IDHierarchyStructureParent] INT             NULL,
    [IDPurpose]                  INT             DEFAULT ((1)) NULL,
    [IDCity]                     INT             NULL,
    [IDCustomer]                 INT             NULL,
    [Name]                       NVARCHAR (200)  NOT NULL,
    [Description]                NVARCHAR (400)  NULL,
    [Latitude]                   DECIMAL (11, 8) NOT NULL,
    [Longitude]                  DECIMAL (11, 8) NOT NULL,
    [Radius]                     DECIMAL (11, 8) NOT NULL,
    [Address]                    NVARCHAR (1000) NOT NULL,
    [Status]                     SMALLINT        NOT NULL,
    [ModifyDate]                 DATETIME        NULL,
    [CreateDate]                 DATETIME        DEFAULT (getdate()) NULL,
    [CreatedBy]                  VARCHAR (50)    NULL,
    [ModifiedBy]                 VARCHAR (50)    NULL,
    CONSTRAINT [PK_Alert] PRIMARY KEY CLUSTERED ([IDAlert] ASC),
    CONSTRAINT [FK_Alert_City] FOREIGN KEY ([IDCity]) REFERENCES [dbo].[City] ([IDCity]),
    CONSTRAINT [FK_Alert_Customer] FOREIGN KEY ([IDCustomer]) REFERENCES [dbo].[Customer] ([IDCustomer]),
    CONSTRAINT [FK_Alert_Purpose] FOREIGN KEY ([IDPurpose]) REFERENCES [dbo].[Purpose] ([IDPurpose])
);

