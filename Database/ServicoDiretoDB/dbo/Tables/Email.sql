CREATE TABLE [dbo].[Email] (
    [IDEmail]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [IDCustomerFrom] INT            NOT NULL,
    [IDCustomerTo]   INT            NOT NULL,
    [IDEmailParent]  BIGINT         NULL,
    [Subject]        NVARCHAR (500) NOT NULL,
    [Content]        TEXT           NULL,
    [Status]         SMALLINT       DEFAULT ((1)) NOT NULL,
    [ModifyDate]     DATETIME       NULL,
    [CreatedBy]      VARCHAR (50)   NOT NULL,
    [ModifiedBy]     VARCHAR (50)   NULL,
    [CreateDate]     DATETIME       DEFAULT (getdate()) NOT NULL,
    [IDElement]      BIGINT         NOT NULL,
    [read]           BIT            NOT NULL,
    CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED ([IDEmail] ASC),
    CONSTRAINT [FK_Email_Customer_From] FOREIGN KEY ([IDCustomerTo]) REFERENCES [dbo].[Customer] ([IDCustomer]),
    CONSTRAINT [FK_Email_Customer_To] FOREIGN KEY ([IDCustomerTo]) REFERENCES [dbo].[Customer] ([IDCustomer]),
    CONSTRAINT [FK_Email_EmailParent] FOREIGN KEY ([IDEmailParent]) REFERENCES [dbo].[Email] ([IDEmail])
);

