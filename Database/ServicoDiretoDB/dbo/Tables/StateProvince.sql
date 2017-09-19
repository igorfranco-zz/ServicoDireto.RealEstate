CREATE TABLE [dbo].[StateProvince] (
    [IDStateProvince] INT           IDENTITY (1, 1) NOT NULL,
    [IDCountry]       VARCHAR (5)   NOT NULL,
    [Acronym]         NVARCHAR (10) NOT NULL,
    [Name]            NVARCHAR (50) NOT NULL,
    [Status]          SMALLINT      NULL,
    [CreatedBy]       VARCHAR (50)  NOT NULL,
    [ModifyDate]      DATETIME      NULL,
    [CreateDate]      DATETIME      DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]      VARCHAR (50)  NULL,
    CONSTRAINT [PK__StatePro__702FBF7A5165187F] PRIMARY KEY CLUSTERED ([IDStateProvince] ASC),
    CONSTRAINT [FK__StateProv__Count__534D60F1] FOREIGN KEY ([IDCountry]) REFERENCES [dbo].[Country] ([IDCountry])
);

