CREATE TABLE [dbo].[ElementAttribute] (
    [IDElementAttribute] BIGINT        IDENTITY (1, 1) NOT NULL,
    [IDElement]          BIGINT        NOT NULL,
    [IDAttribute]        INT           NOT NULL,
    [Value]              VARCHAR (300) NULL,
    CONSTRAINT [PK_ElementAttribute] PRIMARY KEY CLUSTERED ([IDElementAttribute] ASC),
    CONSTRAINT [FK_ElementAttribute_Attribute] FOREIGN KEY ([IDAttribute]) REFERENCES [dbo].[Attribute] ([IDAttribute])
);

