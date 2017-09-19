CREATE TABLE [dbo].[FilterPurpose] (
    [IDPurpose] INT NULL,
    [IDFilter]  INT NULL,
    CONSTRAINT [FK_FilterPurpose_Filter] FOREIGN KEY ([IDFilter]) REFERENCES [dbo].[Filter] ([IDFilter]),
    CONSTRAINT [FK_FilterPurpose_Purpose] FOREIGN KEY ([IDPurpose]) REFERENCES [dbo].[Purpose] ([IDPurpose])
);

