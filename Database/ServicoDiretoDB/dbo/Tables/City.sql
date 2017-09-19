CREATE TABLE [dbo].[City] (
    [IDCity]          INT           IDENTITY (1, 1) NOT NULL,
    [IDStateProvince] INT           NOT NULL,
    [Name]            NVARCHAR (50) NOT NULL,
    [Status]          SMALLINT      NULL,
    [CreatedBy]       VARCHAR (50)  NOT NULL,
    [ModifyDate]      DATETIME      NULL,
    [CreateDate]      DATETIME      DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]      VARCHAR (50)  NULL,
    CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED ([IDCity] ASC),
    CONSTRAINT [FK_City_StateProvince] FOREIGN KEY ([IDStateProvince]) REFERENCES [dbo].[StateProvince] ([IDStateProvince])
);

