CREATE TABLE [dbo].[Country] (
    [IDCountry]  VARCHAR (5)   NOT NULL,
    [Name]       NVARCHAR (50) NULL,
    [Status]     SMALLINT      NULL,
    [CreateDate] DATETIME      DEFAULT (getdate()) NOT NULL,
    [ModifyDate] DATETIME      NULL,
    [CreatedBy]  VARCHAR (50)  NOT NULL,
    [ModifiedBy] VARCHAR (50)  NULL,
    CONSTRAINT [PK__Country__10D160BF4D94879B] PRIMARY KEY CLUSTERED ([IDCountry] ASC)
);

