CREATE TABLE [dbo].[AlertAttribute] (
    [IDAlertAttribute] BIGINT       IDENTITY (1, 1) NOT NULL,
    [IDAttribute]      INT          NOT NULL,
    [IDAlert]          BIGINT       NOT NULL,
    [InitialValue]     VARCHAR (50) NULL,
    [FinalValue]       VARCHAR (50) NULL,
    CONSTRAINT [PK_AlertAttribute] PRIMARY KEY CLUSTERED ([IDAlertAttribute] ASC),
    CONSTRAINT [FK_AlertAttribute_Alert] FOREIGN KEY ([IDAlert]) REFERENCES [dbo].[Alert] ([IDAlert]),
    CONSTRAINT [FK_AlertAttribute_Attribute] FOREIGN KEY ([IDAttribute]) REFERENCES [dbo].[Attribute] ([IDAttribute])
);

