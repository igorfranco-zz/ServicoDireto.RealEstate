CREATE TABLE [dbo].[Zone] (
    [IDZone]     INT            IDENTITY (1, 1) NOT NULL,
    [IDZoneType] INT            NOT NULL,
    [CityID]     INT            NULL,
    [Name]       NVARCHAR (100) NOT NULL,
    [Status]     SMALLINT       NOT NULL,
    [CreatedBy]  VARCHAR (50)   NOT NULL,
    [ModifyDate] DATETIME       NULL,
    [CreateDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [ModifiedBy] VARCHAR (50)   NULL,
    CONSTRAINT [PK__Zone__07F6335A] PRIMARY KEY CLUSTERED ([IDZone] ASC),
    CONSTRAINT [FK__Zone__ZoneTypeID__08EA5793] FOREIGN KEY ([IDZoneType]) REFERENCES [dbo].[ZoneType] ([IDZoneType]),
    CONSTRAINT [FK_Zone_City] FOREIGN KEY ([CityID]) REFERENCES [dbo].[City] ([IDCity])
);

