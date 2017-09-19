CREATE TABLE [dbo].[ElementBookmarked] (
    [IDElementBookmarked] INT      IDENTITY (1, 1) NOT NULL,
    [IDElement]           BIGINT   NOT NULL,
    [IDCustomer]          INT      NOT NULL,
    [CreateDate]          DATETIME DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_ElementBookmarked] PRIMARY KEY CLUSTERED ([IDElementBookmarked] ASC),
    CONSTRAINT [FK_ElementBookmarked_Customer] FOREIGN KEY ([IDCustomer]) REFERENCES [dbo].[Customer] ([IDCustomer])
);

