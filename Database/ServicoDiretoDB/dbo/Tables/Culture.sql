CREATE TABLE [dbo].[Culture] (
    [IDCulture]  VARCHAR (5)   NOT NULL,
    [IDIcon]     INT           NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Status]     SMALLINT      DEFAULT ((1)) NULL,
    [CreateDate] DATETIME      DEFAULT (getdate()) NOT NULL,
    [ModifyDate] DATETIME      NULL,
    [CreatedBy]  VARCHAR (50)  NOT NULL,
    [ModifiedBy] VARCHAR (50)  NULL,
    CONSTRAINT [PK_Culture] PRIMARY KEY CLUSTERED ([IDCulture] ASC),
    CONSTRAINT [FK_Culture_Icon] FOREIGN KEY ([IDIcon]) REFERENCES [dbo].[Icon] ([IDIcon])
);

