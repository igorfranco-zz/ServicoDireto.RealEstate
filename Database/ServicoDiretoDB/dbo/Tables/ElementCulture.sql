CREATE TABLE [dbo].[ElementCulture] (
    [IDCulture]     VARCHAR (5)    NOT NULL,
    [IDElement]     BIGINT         NOT NULL,
    [Name]          NVARCHAR (150) NOT NULL,
    [Description]   NVARCHAR (500) NULL,
    [Discriminator] NVARCHAR (127) DEFAULT ('X') NULL,
    CONSTRAINT [PK_ElementCulture] PRIMARY KEY CLUSTERED ([IDCulture] ASC, [IDElement] ASC),
    CONSTRAINT [FK_ElementCulture_Culture] FOREIGN KEY ([IDCulture]) REFERENCES [dbo].[Culture] ([IDCulture])
);

