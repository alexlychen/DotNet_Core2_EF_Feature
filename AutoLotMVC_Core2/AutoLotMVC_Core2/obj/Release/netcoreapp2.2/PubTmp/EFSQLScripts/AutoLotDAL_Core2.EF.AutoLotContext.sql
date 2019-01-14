IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190108001149_Initial')
BEGIN
    CREATE TABLE [CreditRisks] (
        [Id] int NOT NULL IDENTITY,
        [Timestamp] rowversion NULL,
        [FirstName] nvarchar(50) NULL,
        [LastName] nvarchar(50) NULL,
        CONSTRAINT [PK_CreditRisks] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190108001149_Initial')
BEGIN
    CREATE TABLE [Customers] (
        [Id] int NOT NULL IDENTITY,
        [Timestamp] rowversion NULL,
        [FirstName] nvarchar(50) NULL,
        [LastName] nvarchar(50) NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190108001149_Initial')
BEGIN
    CREATE TABLE [Inventory] (
        [Id] int NOT NULL IDENTITY,
        [Timestamp] rowversion NULL,
        [Make] nvarchar(50) NULL,
        [Color] nvarchar(50) NULL,
        [PetName] nvarchar(50) NULL,
        CONSTRAINT [PK_Inventory] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190108001149_Initial')
BEGIN
    CREATE TABLE [Orders] (
        [Id] int NOT NULL IDENTITY,
        [Timestamp] rowversion NULL,
        [CustomerId] int NOT NULL,
        [CarId] int NOT NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_Inventory_CarId] FOREIGN KEY ([CarId]) REFERENCES [Inventory] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Orders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190108001149_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_CreditRisks_FirstName_LastName] ON [CreditRisks] ([FirstName], [LastName]) WHERE [FirstName] IS NOT NULL AND [LastName] IS NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190108001149_Initial')
BEGIN
    CREATE INDEX [IX_Orders_CarId] ON [Orders] ([CarId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190108001149_Initial')
BEGIN
    CREATE INDEX [IX_Orders_CustomerId] ON [Orders] ([CustomerId]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190108001149_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190108001149_Initial', N'2.2.1-servicing-10028');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190112052245_Staging')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190112052245_Staging', N'2.2.1-servicing-10028');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190112153154_Product')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190112153154_Product', N'2.2.1-servicing-10028');
END;

GO

