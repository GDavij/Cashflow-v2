--SQL UP 1-initial_tables: 2024-11-15
-- Created By: gdavi

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Cashflow')
BEGIN
    CREATE DATABASE Cashflow;
END;
GO

USE CASHFLOW;
GO

CREATE TABLE Roles (
    [Id]                        SMALLINT NOT NULL,
    [Name]                      VARCHAR(20) NOT NULL,
    CONSTRAINT PK_Roles_Id PRIMARY KEY(Id)
)
GO

CREATE TABLE Users (
    [Id]                        BIGINT IDENTITY(1,1),
    [Active]                    BIT NOT NULL,
    [Deleted]                   BIT NOT NULL,
    [Email]                     VARCHAR(80) NOT NULL,
    [Passphrase]                VARCHAR(512) NOT NULL,
    [Username]                  VARCHAR(60) NOT NULL,
    [BirthDate]                 DATETIME2 NOT NULL,
    [CreatedAt]                 DATETIME2 NOT NULL,
    [CreatedBy]                 BIGINT NULL,
    [LastUpdatedAt]             DATETIME2 NULL,
    [LastModifiedBy]            BIGINT NULL,
    [RoleId]                    SMALLINT NOT NULL,
    CONSTRAINT [PK_Users_Id] PRIMARY KEY(Id),
    CONSTRAINT [FK_Users_CreatedBy_Users_Id] FOREIGN KEY([CreatedBy]) REFERENCES Users([Id]),
    CONSTRAINT [FK_Users_LastModifiedBy_Users_Id] FOREIGN KEY([LastModifiedBy]) REFERENCES Users([Id]),
    CONSTRAINT [FK_Users_RoleId_Roles_Id] FOREIGN KEY([RoleId]) REFERENCES Roles([Id])
);
GO

CREATE TABLE RecurrencyTimes (
    [Id]                        SMALLINT NOT NULL,
    [Name]                      VARCHAR(60) NOT NULL,
    CONSTRAINT [PK_RecurrencyTimes_Id] PRIMARY KEY([Id])
);
GO

CREATE TABLE Recurrencies (
    [Id]                        BIGINT IDENTITY(1,1),
    [Active]                    BIT NOT NULL,
    [Deleted]                   BIT NOT NULL,
    [OwnerId]                   BIGINT NOT NULL,
    [RecurrencyTimeId]          SMALLINT NOT NULL,
    [CreatedAt]                 DATETIME2 NOT NULL,
    [LastModifiedAt]             DATETIME2 NULL,
    [LastModifiedBy]             BIGINT NULL,
    [Times]                     INT NOT NULL,
    CONSTRAINT [PK_Recurrencies_Id] PRIMARY KEY([Id]),
    CONSTRAINT [FK_Recurrencies_OwnerId_Users_Id] FOREIGN KEY([OwnerId]) REFERENCES Users([Id]),
    CONSTRAINT [FK_Recurrencies_RecurrencyTimeId_RecurrenceTimes_Id] FOREIGN KEY([RecurrencyTimeId]) REFERENCES RecurrencyTimes([Id]),
    CONSTRAINT [FK_Recurrencies_LastModifiedBy_Users_Id] FOREIGN KEY([LastModifiedBy]) REFERENCES Users([Id])
);
GO

CREATE TABLE AccountTypes (
    [Id]                        SMALLINT NOT NULL,
    [Name]                      VARCHAR(60) NOT NULL,
    CONSTRAINT [PK_AccountType_Id] PRIMARY KEY([Id])
);
GO

CREATE TABLE BankAccounts (
    [Id]                        BIGINT IDENTITY(1,1),
    [Active]                    BIT NOT NULL,
    [Deleted]                   BIT NOT NULL,
    [Name]                      VARCHAR(60) NOT NULL,
    [CurrentValue]              NUMERIC NOT NULL,
    [OwnerId]                    BIGINT NOT NULL,
    [AccountTypeId]             SMALLINT NOT NULL,
    [CreatedAt]                 DATETIME2 NOT NULL,
    [LastModifiedBy]            BIGINT NULL,
    [LastModifiedAt]            DATETIME2 NULL,
    CONSTRAINT [PK_BankAccounts_Id] PRIMARY KEY([Id]),
    CONSTRAINT [FK_BankAccounts_OwnerId_Users_Id] FOREIGN KEY([OwnerId]) REFERENCES Users([Id]),
    CONSTRAINT [FK_BankAccounts_AccountTypesId_AccountTypes_Id] FOREIGN KEY([AccountTypeId]) REFERENCES AccountTypes([Id]),
    CONSTRAINT [FK_BankAccounts_LastModifiedBy_Users_Id] FOREIGN KEY([LastModifiedBy]) REFERENCES Users([Id])
);
GO

CREATE TABLE TransactionMethods (
    [Id]                        SMALLINT NOT NULL,
    [Name]                      VARCHAR(40) NOT NULL,
    CONSTRAINT [PK_TransactionMethods_Id] PRIMARY KEY([Id])
);
GO

CREATE TABLE Categories (
    [Id]                        BIGINT IDENTITY(1,1),
    [Active]                    BIT NOT NULL,
    [Deleted]                   BIT NOT NULL,
    [OwnerId]                   BIGINT NOT NULL,
    [Name]                      VARCHAR(60) NOT NULL,
    [MaximumBudgetInvestment]   FLOAT NULL,
    [MaximumMoneyInvestment]    NUMERIC NULL,
    [CreatedAt]                 DATETIME2 NOT NULL,
    [LastModifiedAt]            DATETIME2 NULL,
    [LastModifiedBy]            BIGINT NULL,
    CONSTRAINT [PK_Categories_Id] PRIMARY KEY([Id]),
    CONSTRAINT [FK_Categories_OwnerId_Users_Id] FOREIGN KEY([OwnerId]) REFERENCES Users([Id]),
    CONSTRAINT [FK_Categories_LastModifiedBy_Users_Id] FOREIGN KEY([LastModifiedBy]) REFERENCES Users([Id])
);
GO

CREATE TABLE Transactions (
    [Id]                        BIGINT IDENTITY(1, 1),
    [Active]                    BIT NOT NULL,
    [Deleted]                   BIT NOT NULL,
    [CategoryId]                BIGINT NULL,
    [TransactionMethodId]       SMALLINT NOT NULL,
    [OwnerId]                   BIGINT NOT NULL,
    [BankAccountId]             BIGINT NULL,
    [Description]               VARCHAR(120) NOT NULL,
    [Value]                     NUMERIC NOT NULL,
    [DoneAt]                    DATETIME2 NOT NULL,
    [Month]                     SMALLINT NOT NULL,
    [Year]                      INT NOT NULL,
    [CreatedAt]                 DATETIME2 NOT NULL,
    [LastModifiedAt]            DATETIME2 NULL,
    [LastModifiedBy]            BIGINT NULL,
    CONSTRAINT [PK_Transactions_Id] PRIMARY KEY([Id]),
    CONSTRAINT [FK_Transactions_CategoryId_Categories_Id] FOREIGN KEY([CategoryId]) REFERENCES Categories([Id]),
    CONSTRAINT [FK_Transactions_TransactionMethodId_TransactionMethods_Id] FOREIGN KEY([TransactionMethodId]) REFERENCES TransactionMethods([Id]),
    CONSTRAINT [FK_Transactions_OwnerId_Users_Id] FOREIGN KEY([OwnerId]) REFERENCES Users([Id]),
    CONSTRAINT [FK_Transactions_BankAccountId_BankAccounts_Id] FOREIGN KEY([BankAccountId]) REFERENCES BankAccounts([Id]),
    CONSTRAINT [FK_Transactions_LastModifiedBy_Users_Id] FOREIGN KEY([LastModifiedBy]) REFERENCES Users([Id])
);
GO

CREATE TABLE AuditionEvents (
    [Id]                        BIGINT IDENTITY(1,1),
    [TraceId]                   UNIQUEIDENTIFIER NOT NULL,
    [Event]                     VARCHAR(255) NOT NULL,
    [UserId]                    BIGINT NOT NULL,
    [OccurredAt]                DATETIME2 NOT NULL,
    [IpAddress]                 VARCHAR(45) NULL,
    [UserAgent]                 VARCHAR(255) NULL,
    [PrivateEvent]              BIT NOT NULL
);

