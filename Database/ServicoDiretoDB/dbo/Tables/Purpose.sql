CREATE TABLE [dbo].[Purpose] (
    [IDPurpose]  INT          IDENTITY (1, 1) NOT NULL,
    [IDIcon]     INT          NULL,
    [Status]     SMALLINT     NOT NULL,
    [CreateDate] DATETIME     DEFAULT (getdate()) NOT NULL,
    [ModifyDate] DATETIME     NULL,
    [CreatedBy]  VARCHAR (50) NOT NULL,
    [ModifiedBy] VARCHAR (50) NULL,
    CONSTRAINT [PK__Purpose__00551192] PRIMARY KEY CLUSTERED ([IDPurpose] ASC),
    CONSTRAINT [FK_Purpose_Icon] FOREIGN KEY ([IDIcon]) REFERENCES [dbo].[Icon] ([IDIcon])
);

