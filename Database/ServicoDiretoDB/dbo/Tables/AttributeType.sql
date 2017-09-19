CREATE TABLE [dbo].[AttributeType] (
    [IDAttributeType] INT            IDENTITY (1, 1) NOT NULL,
    [IDIcon]          INT            NULL,
    [Hidden]          BIT            DEFAULT ((1)) NULL,
    [Status]          SMALLINT       NOT NULL,
    [CreatedBy]       VARCHAR (50)   NOT NULL,
    [ModifyDate]      DATETIME       NULL,
    [ModifiedBy]      VARCHAR (50)   NULL,
    [CreateDate]      DATETIME       DEFAULT (getdate()) NOT NULL,
    [Acronym]         VARCHAR (5)    NULL,
    [Discriminator]   NVARCHAR (254) DEFAULT ('X') NULL,
    CONSTRAINT [PK__AttributeType__060DEAE8] PRIMARY KEY CLUSTERED ([IDAttributeType] ASC),
    CONSTRAINT [FK_AttributeType_Icon] FOREIGN KEY ([IDIcon]) REFERENCES [dbo].[Icon] ([IDIcon])
);

