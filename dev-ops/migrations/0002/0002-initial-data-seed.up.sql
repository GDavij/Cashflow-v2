--SQL UP 2-initial-data-seed: 2024-11-15
-- Created By: gdavi

USE CASHFLOW;
GO

INSERT INTO [dbo].[Roles] 
VALUES
(1, 'Sudo'),
(2, 'User');
GO

INSERT INTO [dbo].[RecurrencyTimes]
VALUES
(1, 'Monthly'),
(2, 'Weekly'),
(3, 'Daily'),
(4, 'Quarterly'),
(5, 'Bi-Monthly');
GO

INSERT INTO [dbo].[AccountTypes]
VALUES
(1, 'Debit Card'),
(2, 'Credit Card'),
(3, 'Digital'),
(4, 'Benefit Card'),
(5, 'Savings');
GO

INSERT INTO [dbo].[TransactionMethods]
VALUES
(1, 'Debit'),
(2, 'Credit'),
(3, 'Alter');
