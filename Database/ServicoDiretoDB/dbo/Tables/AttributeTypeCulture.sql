CREATE TABLE [dbo].[AttributeTypeCulture] (
    [IDAttributeType] INT            NOT NULL,
    [IDCulture]       VARCHAR (5)    NOT NULL,
    [Description]     NVARCHAR (150) NOT NULL,
    [Discriminator]   NVARCHAR (254) DEFAULT ('X') NULL,
    CONSTRAINT [FK_AttributeTypeCulture_AttributeType] FOREIGN KEY ([IDAttributeType]) REFERENCES [dbo].[AttributeType] ([IDAttributeType]),
    CONSTRAINT [FK_AttributeTypeCulture_Culture] FOREIGN KEY ([IDCulture]) REFERENCES [dbo].[Culture] ([IDCulture])
);

