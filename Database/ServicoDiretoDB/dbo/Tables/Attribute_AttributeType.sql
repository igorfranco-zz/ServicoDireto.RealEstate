CREATE TABLE [dbo].[Attribute_AttributeType] (
    [IDAttribute]     INT NOT NULL,
    [IDAttributeType] INT NOT NULL,
    CONSTRAINT [PK__AttributeAttribu__108B795B] PRIMARY KEY CLUSTERED ([IDAttribute] ASC, [IDAttributeType] ASC),
    CONSTRAINT [FK__Attribute__Attri__117F9D94] FOREIGN KEY ([IDAttribute]) REFERENCES [dbo].[Attribute] ([IDAttribute]),
    CONSTRAINT [FK__Attribute__Attri__1273C1CD] FOREIGN KEY ([IDAttributeType]) REFERENCES [dbo].[AttributeType] ([IDAttributeType])
);

