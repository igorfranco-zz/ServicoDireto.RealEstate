CREATE TABLE [dbo].[AttributeCulture] (
    [IDCulture]     VARCHAR (5)    NULL,
    [IDAttribute]   INT            NULL,
    [Name]          NVARCHAR (250) NOT NULL,
    [Value]         NVARCHAR (50)  NULL,
    [Discriminator] NVARCHAR (254) DEFAULT ('X') NULL,
    CONSTRAINT [FK_AttributeCulture_Attribute] FOREIGN KEY ([IDAttribute]) REFERENCES [dbo].[Attribute] ([IDAttribute]),
    CONSTRAINT [FK_AttributeCulture_Culture] FOREIGN KEY ([IDCulture]) REFERENCES [dbo].[Culture] ([IDCulture])
);

