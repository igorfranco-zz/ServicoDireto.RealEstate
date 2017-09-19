CREATE TABLE [dbo].[PurposeCulture] (
    [IDPurpose]   INT            NOT NULL,
    [IDCulture]   VARCHAR (5)    NOT NULL,
    [Description] NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_PurposeCulture] PRIMARY KEY CLUSTERED ([IDPurpose] ASC, [IDCulture] ASC),
    CONSTRAINT [FK_PurposeCulture_Culture] FOREIGN KEY ([IDCulture]) REFERENCES [dbo].[Culture] ([IDCulture]),
    CONSTRAINT [FK_PurposeCulture_Purpose] FOREIGN KEY ([IDPurpose]) REFERENCES [dbo].[Purpose] ([IDPurpose])
);

