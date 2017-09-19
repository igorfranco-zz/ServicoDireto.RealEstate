CREATE TABLE [dbo].[Icon] (
    [IDIcon]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50)  NOT NULL,
    [Path]       NVARCHAR (200) NOT NULL,
    [Status]     SMALLINT       NOT NULL,
    [CreateDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifyDate] DATETIME       NULL,
    [CreatedBy]  VARCHAR (50)   NOT NULL,
    [ModifiedBy] VARCHAR (50)   NULL,
    CONSTRAINT [PK_Icon] PRIMARY KEY CLUSTERED ([IDIcon] ASC)
);

