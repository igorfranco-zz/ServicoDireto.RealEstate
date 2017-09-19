CREATE TABLE [dbo].[Filter] (
    [IDFilter]      INT          IDENTITY (1, 1) NOT NULL,
    [IDAttribute]   INT          NULL,
    [InitialValue]  VARCHAR (15) NULL,
    [FinalValue]    VARCHAR (15) NULL,
    [DefaultValue]  VARCHAR (15) NULL,
    [ComponentType] SMALLINT     NULL,
    [Status]        SMALLINT     DEFAULT ((1)) NOT NULL,
    [ModifyDate]    DATETIME     NULL,
    [CreatedBy]     VARCHAR (50) NOT NULL,
    [ModifiedBy]    VARCHAR (50) NULL,
    [CreateDate]    DATETIME     DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Filter] PRIMARY KEY CLUSTERED ([IDFilter] ASC),
    CONSTRAINT [FK_Filter_Attribute] FOREIGN KEY ([IDAttribute]) REFERENCES [dbo].[Attribute] ([IDAttribute])
);

