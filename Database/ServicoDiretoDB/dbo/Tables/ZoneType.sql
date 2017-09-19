CREATE TABLE [dbo].[ZoneType] (
    [IDZoneType]  INT           IDENTITY (1, 1) NOT NULL,
    [Description] VARCHAR (250) NULL,
    [Status]      SMALLINT      NULL,
    [CreatedBy]   VARCHAR (50)  NOT NULL,
    [ModifyDate]  DATETIME      NULL,
    [CreateDate]  DATETIME      DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]  VARCHAR (50)  NULL,
    CONSTRAINT [PK__ZoneType__7C8480AE] PRIMARY KEY CLUSTERED ([IDZoneType] ASC)
);

