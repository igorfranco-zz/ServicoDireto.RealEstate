CREATE TABLE [dbo].[AdsCategoryCulture] (
    [IDAdsCategory] INT            NOT NULL,
    [IDCulture]     VARCHAR (5)    NOT NULL,
    [Name]          NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_AdsCategoryCulture] PRIMARY KEY CLUSTERED ([IDAdsCategory] ASC, [IDCulture] ASC),
    CONSTRAINT [FK_AdsCategoryCulture_AdsCategory] FOREIGN KEY ([IDAdsCategory]) REFERENCES [dbo].[AdsCategory] ([IDAdsCategory]),
    CONSTRAINT [FK_AdsCategoryCulture_Culture] FOREIGN KEY ([IDCulture]) REFERENCES [dbo].[Culture] ([IDCulture])
);

