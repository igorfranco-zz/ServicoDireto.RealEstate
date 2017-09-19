CREATE TABLE [dbo].[AdsCategory] (
    [IDAdsCategory] INT          IDENTITY (1, 1) NOT NULL,
    [CreatedBy]     VARCHAR (50) NOT NULL,
    [ModifyDate]    DATETIME     NULL,
    [Status]        SMALLINT     DEFAULT ((1)) NOT NULL,
    [ModifiedBy]    VARCHAR (50) NULL,
    [CreateDate]    DATETIME     DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_AdsCategory] PRIMARY KEY CLUSTERED ([IDAdsCategory] ASC)
);

