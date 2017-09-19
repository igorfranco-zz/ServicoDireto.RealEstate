CREATE TABLE [dbo].[Attribute] (
    [IDAttribute]   INT            IDENTITY (1, 1) NOT NULL,
    [IDIcon]        INT            NULL,
    [EditMask]      VARCHAR (250)  NULL,
    [ModifyDate]    DATETIME       NULL,
    [DisplayMask]   VARCHAR (250)  NULL,
    [Status]        SMALLINT       DEFAULT ((1)) NOT NULL,
    [CreatedBy]     VARCHAR (50)   NOT NULL,
    [ModifiedBy]    VARCHAR (50)   NULL,
    [CreateDate]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [Acronym]       VARCHAR (500)  NULL,
    [Required]      BIT            NULL,
    [Index]         SMALLINT       NULL,
    [MaxValue]      SMALLINT       NULL,
    [MinValue]      SMALLINT       NULL,
    [Discriminator] NVARCHAR (254) DEFAULT ('X') NULL,
    CONSTRAINT [PK__Attribute__0425A276] PRIMARY KEY CLUSTERED ([IDAttribute] ASC),
    CONSTRAINT [FK_Attribute_Icon] FOREIGN KEY ([IDIcon]) REFERENCES [dbo].[Icon] ([IDIcon])
);

