/* ======================= TABLES ========================*/

CREATE TABLE [dbo].[accounts]( 
    [id] VARCHAR(12) NOT NULL PRIMARY KEY,
	[username] NVARCHAR(255) UNIQUE NOT NULL,
	[hash_password] NVARCHAR(max) NOT NULL,
)
GO

CREATE TABLE [dbo].[hall_types](
	[id] VARCHAR(12) NOT NULL PRIMARY KEY,
    -- [hall_type_id] AS 'HT' + RIGHT('00000000' + CAST(id AS VARCHAR(8)), 8) UNIQUE,
    [name] NVARCHAR(255) NOT NULL, -- normal, silver, gold, platinum, diamond
	[unit_price] MONEY NOT NULL CHECK ([unit_price] > 0)    
)
GO

CREATE TABLE [dbo].[halls](
	[id] VARCHAR(12) NOT NULL PRIMARY KEY,
	[type] VARCHAR(12) NOT NULL REFERENCES dbo.[hall_types] ([id]),
	[name] NVARCHAR(255) NOT NULL,
    [num_of_tables] INT NOT NULL CHECK ([num_of_tables] > 0),
    [address] NVARCHAR(255),
	[note] NVARCHAR(max) DEFAULT ('')
)
GO

CREATE TABLE [dbo].[services]( -- dich vu
	[id] VARCHAR(12) NOT NULL PRIMARY KEY,
	[name] NVARCHAR(max) NOT NULL,
	[unit_price] MONEY NOT NULL,
)
GO

CREATE TABLE dbo.[hall_services] (
    [service_id] VARCHAR(12) NOT NULL REFERENCES dbo.[services](id),
    [hall_id] VARCHAR(12) NOT NULL REFERENCES dbo.[halls](id),
    PRIMARY KEY (service_id, hall_id)
)

CREATE TABLE [dbo].[customers]( -- khach hang
	[id] VARCHAR(12) NOT NULL PRIMARY KEY,
	[fullname] NVARCHAR(255) NOT NULL,
	[phone] CHAR(10) NOT NULL,
    [email] VARCHAR(255) UNIQUE,
    [address] NVARCHAR(max),
    [nin] CHAR(12) NOT NULL UNIQUE-- national identification number
) 
GO

CREATE TABLE [dbo].[dishes]( -- mon an
	[id] VARCHAR(12) NOT NULL PRIMARY KEY,
	[name] NVARCHAR(255) NOT NULL,
	[unit_price] INT NOT NULL,
	[type] NVARCHAR(50) NOT NULL,
	[note] NVARCHAR(max) DEFAULT (''),
    CHECK(LOWER([type]) IN ('khai vi', 'trang mieng', 'mon chinh', N'khai vị', N'tráng miệng', N'món chính', 'starter', 'main', 'dessert' ))
)
GO

CREATE TABLE [dbo].[reservations](
	[id] VARCHAR(12) NOT NULL PRIMARY KEY,
	[customer_id] VARCHAR(12) NOT NULL REFERENCES [dbo].[customers] (id),
	[reserving_date] SMALLDATETIME NOT NULL,
	[organizing_date] SMALLDATETIME NOT NULL, -- format: mm-dd-yyy hh-mm-ss
	[hall_id] VARCHAR(12) NOT NULL REFERENCES [dbo].[halls] (id),
    [duration] INT,
    [estimating_cost] MONEY DEFAULT(0), -- chi phi uoc tinh	
	[num_of_tables] INT NOT NULL,
    [purpose] NVARCHAR(255), 
    [status] NVARCHAR(50) NOT NULL,
    CHECK (LOWER([status]) IN('completed', 'pending', 'canceled'))
)
GO

CREATE TABLE [dbo].[menus](
    [reservation_id] VARCHAR(12) NOT NULL REFERENCES dbo.[reservations] (id),
    [dish_id] VARCHAR(12) NOT NULL REFERENCES dbo.[dishes](id),
    PRIMARY KEY(reservation_id, dish_id)
)
GO

CREATE TABLE [dbo].[selected_services](
    [reservation_id] VARCHAR(12) NOT NULL REFERENCES dbo.[reservations] (id),
    [service_id] VARCHAR(12) NOT NULL REFERENCES dbo.[services](id),
    PRIMARY KEY(reservation_id, service_id)
)
GO

CREATE TABLE dbo.[billing] (
    [id] VARCHAR(12) NOT NULL PRIMARY KEY,
    [reservation_id] VARCHAR(12) UNIQUE NOT NULL REFERENCES dbo.[reservations] (id),
    [hall_cost] MONEY DEFAULT(0),
    [service_cost] MONEY DEFAULT(0),
    [food_cost] MONEY DEFAULT(0),
    [total_cost] AS [hall_cost] + [service_cost] + [food_cost],
    [deposit] AS ([hall_cost] + [service_cost] + [food_cost]) * 0.2, -- equals 20% of estimating cost
    [remaining_cost] AS ([hall_cost] + [service_cost] + [food_cost]) - (([hall_cost] + [service_cost] + [food_cost]) * 0.2),
    [fine] MONEY DEFAULT(0),
    [note] NVARCHAR(255),
    [is_done] BIT NOT NULL DEFAULT(0)
)
GO

/*======================== END TABLES ======================*/

/*==================== TRIGGERS AND FUNCTIONS & PRoCEDURES ======================*/

-- Create the stored procedure in the specified schema
CREATE OR ALTER PROCEDURE dbo.sp_autoGenerateId
    @table /*parameter name*/ nvarchar(255), /*datatype_for_param1*/
    @id /*parameter name*/ int, /*datatype_for_param1*/
    @res VARCHAR(12) OUT
AS
BEGIN
    DECLARE @substring VARCHAR(50), @newId VARCHAR(8) = ''
    DECLARE cs_substring CURSOR FOR SELECT value FROM STRING_SPLIT(@table, '_')
    
    OPEN cs_substring;  
    FETCH NEXT FROM cs_substring INTO @substring
    WHILE @@FETCH_STATUS = 0  
    BEGIN 
        SET @newId = CONCAT(@newId, SUBSTRING(@substring, 1,1))
        FETCH NEXT FROM cs_substring INTO @substring
    END
    
    CLOSE cs_substring
    DEALLOCATE cs_substring

    SET @res = UPPER(@newId) +  RIGHT('00000000' + CAST(@id AS VARCHAR(8)), 8)
END
GO
-- example to execute the stored procedure we just created
-- DECLARE @res VARCHAR(12)
-- EXECUTE dbo.sp_autoGenerateId 'hall_services' /*value_for_param1*/, 14, @res out
-- SELECT @res
-- GO


CREATE OR ALTER TRIGGER dbo.tg_insertAccount
ON dbo.accounts
INSTEAD OF INSERT 
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;

    DECLARE @tablename NVARCHAR(255) = (
        SELECT OBJECT_NAME(parent_object_id) 
        FROM sys.objects 
        WHERE sys.objects.object_id = @@PROCID
    )

    DECLARE @username NVARCHAR(50), @hash NVARCHAR(50)

    SELECT TOP 1 @username=username, @hash=hash_password FROM inserted
    DECLARE @id VARCHAR(12)
    DECLARE @count INT = (SELECT COUNT(*) FROM dbo.accounts)
    EXEC dbo.sp_autoGenerateId @tablename, @count, @id OUT

    INSERT INTO dbo.accounts VALUES(@id, @username, @hash)

END
GO


CREATE OR ALTER TRIGGER dbo.tg_insertHallType
ON dbo.hall_types
INSTEAD OF INSERT 
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;

    DECLARE @tablename NVARCHAR(255) = (
        SELECT OBJECT_NAME(parent_object_id) 
        FROM sys.objects 
        WHERE sys.objects.object_id = @@PROCID
    )    
    DECLARE @name NVARCHAR(255), @price MONEY
    SELECT TOP 1 @name=[name], @price=[unit_price] FROM inserted
    DECLARE @id VARCHAR(12)
    DECLARE @count INT = (SELECT COUNT(*) FROM dbo.hall_types)
    EXEC dbo.sp_autoGenerateId @tablename, @count, @id OUT

    INSERT INTO dbo.hall_types VALUES(@id, @name, @price)

END
GO

CREATE OR ALTER TRIGGER dbo.tg_insertHall
ON dbo.halls
INSTEAD OF INSERT 
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;
    DECLARE @tablename NVARCHAR(255) = (
        SELECT OBJECT_NAME(parent_object_id) 
        FROM sys.objects 
        WHERE sys.objects.object_id = @@PROCID
    )
    DECLARE @type NVARCHAR(12), @name NVARCHAR(255), @num_of_tables INT, @address NVARCHAR(255), @note NVARCHAR(max)

    SELECT TOP 1 @type=[type], @name=[name], @num_of_tables=[num_of_tables], @address=[address], @note=[note] FROM inserted
    DECLARE @id VARCHAR(12)
    DECLARE @count INT = (SELECT COUNT(*) FROM dbo.halls)
    EXEC dbo.sp_autoGenerateId @tablename, @count, @id OUT

    INSERT INTO dbo.halls VALUES(@id, @type, @name, @num_of_tables, @address, @note)

END
GO


CREATE OR ALTER TRIGGER dbo.tg_insertService
ON dbo.services
INSTEAD OF INSERT 
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;

    DECLARE @tablename NVARCHAR(255) = (
        SELECT OBJECT_NAME(parent_object_id) 
        FROM sys.objects 
        WHERE sys.objects.object_id = @@PROCID
    )
    DECLARE @name NVARCHAR(max), @unit_price MONEY

    SELECT TOP 1 @name=[name], @unit_price=[unit_price] FROM inserted
    DECLARE @id VARCHAR(12)
    DECLARE @count INT = (SELECT COUNT(*) FROM dbo.services)
    EXEC dbo.sp_autoGenerateId @tablename, @count, @id OUT

    INSERT INTO dbo.services VALUES(@id, @name, @unit_price)

END
GO

CREATE OR ALTER TRIGGER dbo.tg_insertDish
ON dbo.dishes
INSTEAD OF INSERT 
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;

    DECLARE @tablename NVARCHAR(255) = (
        SELECT OBJECT_NAME(parent_object_id) 
        FROM sys.objects 
        WHERE sys.objects.object_id = @@PROCID
    )
    DECLARE @name NVARCHAR(255), @unit_price MONEY, @type NVARCHAR(50), @note NVARCHAR(max)

    SELECT TOP 1 @name=[name], @unit_price=[unit_price], @type=[type], @note=[note] FROM inserted
    DECLARE @id VARCHAR(12)
    DECLARE @count INT = (SELECT COUNT(*) FROM dbo.dishes)
    EXEC dbo.sp_autoGenerateId @tablename, @count, @id OUT

    INSERT INTO dbo.dishes VALUES(@id, @name, @unit_price, @type, @note)

END
GO

CREATE OR ALTER TRIGGER dbo.tg_insertCustomer
ON dbo.customers
INSTEAD OF INSERT 
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;

    DECLARE @tablename NVARCHAR(255) = (
        SELECT OBJECT_NAME(parent_object_id) 
        FROM sys.objects 
        WHERE sys.objects.object_id = @@PROCID
    )
    DECLARE @name NVARCHAR(255), @phone CHAR(10), @email NVARCHAR(255), @address NVARCHAR(max), @nin CHAR(12)

    SELECT TOP 1 @name=[fullname], @phone=[phone], @email=[email], @address=[address], @nin=[nin] FROM inserted
    DECLARE @id VARCHAR(12)
    DECLARE @count INT = (SELECT COUNT(*) FROM dbo.customers)
    EXEC dbo.sp_autoGenerateId @tablename, @count, @id OUT

    INSERT INTO dbo.customers VALUES(@id, @name, @phone, @email, @address, @nin)

END
GO

CREATE OR ALTER TRIGGER dbo.tg_insertReservation
ON dbo.reservations
INSTEAD OF INSERT 
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;

    DECLARE @tablename NVARCHAR(255) = (
        SELECT OBJECT_NAME(parent_object_id) 
        FROM sys.objects 
        WHERE sys.objects.object_id = @@PROCID
    )
    DECLARE 
        @customer_id VARCHAR(12), 
        @reserving_date SMALLDATETIME, 
        @organizing_date SMALLDATETIME, 
        @hall_id VARCHAR(12), 
        @duration INT, 
        @estimating_cost MONEY, 
        @num_of_tables INT, 
        @purpose NVARCHAR(255), 
        @status NVARCHAR(50)

    SELECT TOP 1 
        @customer_id=[customer_id], 
        @reserving_date=[reserving_date], 
        @organizing_date=[organizing_date], 
        @hall_id=[hall_id], 
        @duration=[duration], 
        @num_of_tables=[num_of_tables], 
        @estimating_cost=[estimating_cost],
        @purpose=[purpose], 
        @status=[status]
    FROM inserted

    DECLARE @id VARCHAR(12)
    DECLARE @count INT = (SELECT COUNT(*) FROM dbo.reservations)
    EXEC dbo.sp_autoGenerateId @tablename, @count, @id OUT

    INSERT INTO dbo.reservations 
    VALUES(@id, @customer_id, @reserving_date, @organizing_date, @hall_id, @duration, @estimating_cost, @num_of_tables, @purpose, @status)

END
GO

CREATE OR ALTER TRIGGER dbo.tg_insertBilling
ON dbo.billing
INSTEAD OF INSERT 
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;

    DECLARE @tablename NVARCHAR(255) = (
        SELECT OBJECT_NAME(parent_object_id) 
        FROM sys.objects 
        WHERE sys.objects.object_id = @@PROCID
    )
    DECLARE 
        @reservation_id VARCHAR(12), 
        @hall_cost MONEY, 
        @service_cost MONEY, 
        @food_cost MONEY, 
        @fine MONEY, 
        @note NVARCHAR(255), 
        @is_done BIT

    SELECT TOP 1 
        @reservation_id=[reservation_id], 
        @hall_cost=[hall_cost], 
        @service_cost=[service_cost], 
        @food_cost=[food_cost], 
        @fine=[fine], 
        @note=[note],
        @is_done=[is_done]
    FROM inserted

    DECLARE @id VARCHAR(12)
    DECLARE @count INT = (SELECT COUNT(*) FROM dbo.billing)
    EXEC dbo.sp_autoGenerateId @tablename, @count, @id OUT

    INSERT INTO dbo.billing (id, reservation_id, hall_cost, service_cost, food_cost, fine, note, is_done)
    VALUES(@id, @reservation_id, @hall_cost, @service_cost, @food_cost, @fine, @note, @is_done)

END
GO

CREATE OR ALTER TRIGGER dbo.tg_autoUpdateBillingOnReserv
ON dbo.reservations
FOR INSERT
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;

    DECLARE @reserv_id VARcHAR(12), @hall_id VARCHAR(12), @duration INT, @price MONEY, @hall_cost MONEY
    SELECT @reserv_id=[id], @hall_id=[hall_id], @duration=[duration] FROM inserted

    SET @price = (
        SELECT unit_price 
        FROM hall_types 
        WHERE id=(SELECT [type] FROM halls WHERE id=@hall_id)
    )
    SET @hall_cost = @price * @duration

    INSERT INTO dbo.billing (reservation_id, hall_cost) VALUES (@reserv_id, @hall_cost)

END
GO

CREATE OR ALTER TRIGGER dbo.tg_updateBillingOnMenu
ON dbo.menus
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;
    DECLARE @reserv_id VARCHAR(12), @num_of_tables INT
    IF EXISTS (SELECT * FROM inserted)
        SELECT @reserv_id=[reservation_id] FROM inserted
    ELSE SELECT @reserv_id=[reservation_id] FROM deleted
    
    DECLARE @price MONEY = (
        SELECT SUM(unit_price) FROM dbo.dishes WHERE id IN (
            SELECT dish_id FROM menus WHERE reservation_id=@reserv_id
        )
    )

    SET @num_of_tables = (SELECT num_of_tables FROM reservations WHERE id=@reserv_id)
    IF EXISTS (SELECT * FROM dbo.billing WHERE reservation_id=@reserv_id)
        UPDATE dbo.billing SET food_cost = @price * @num_of_tables WHERE reservation_id=@reserv_id
    ELSE
        INSERT INTO dbo.billing (reservation_id, food_cost) VALUES (@reserv_id, @price)
END
GO

CREATE OR ALTER TRIGGER dbo.tg_updateEstimateCost
ON dbo.billing
AFTER UPDATE
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;
    DECLARE @estimate_cost MONEY, @reserv_id VARCHAR(12)
    SELECT @reserv_id=[reservation_id] FROM INSERTED 
    SELECT @estimate_cost=[total_cost] FROM dbo.billing WHERE reservation_id=@reserv_id

    UPDATE dbo.reservations SET estimating_cost=@estimate_cost WHERE id=@reserv_id
END
GO

CREATE OR ALTER TRIGGER dbo.tg_validateSelectedServices
ON dbo.selected_services
FOR INSERT, UPDATE, DELETE
AS
BEGIN
    IF (ROWCOUNT_BIG() = 0)
        RETURN;
    DECLARE @reserv_id VARcHAR(12), @service_id VARCHAR(12)
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        SELECT @reserv_id=[reservation_id], @service_id=[service_id] FROM inserted
        DECLARE @hall_id VARCHAR(12) = (SELECT hall_id FROM reservations WHERE id=@reserv_id)
        DECLARE @message NVARCHAR(max) = 'The service ' + @service_id + ' does not exist in hall ' + @hall_id
        IF NOT EXISTS (SELECT * FROM hall_services WHERE service_id=@service_id AND hall_id=@hall_id)
        BEGIN
            RAISERROR(@message, 16, 1)
            ROLLBACK TRANSACTION
        END

    END
    ELSE SELECT @reserv_id=[reservation_id], @service_id=[service_id] FROM deleted

    DECLARE @price MONEY = (
        SELECT SUM(unit_price) FROM dbo.services WHERE id IN (
            SELECT service_id FROM selected_services WHERE reservation_id=@reserv_id
    ))
    IF EXISTS (SELECT * FROM dbo.billing WHERE reservation_id=@reserv_id)
        UPDATE dbo.billing SET service_cost = @price WHERE reservation_id=@reserv_id
    ELSE
        INSERT INTO dbo.billing (reservation_id, service_cost) VALUES (@reserv_id, @price)

END
GO

-- Create a new stored procedure called 'sp_createReport' in schema 'dbo'
-- Drop the function if it already exists
IF OBJECT_ID (N'dbo.fc_createReport', N'IF') IS NOT NULL
    DROP FUNCTION dbo.fc_createReport;
GO
-- Create the function in the specified schema
CREATE FUNCTION dbo.fc_createReport()
RETURNS @res TABLE (
    [updated_at] SMALLDATETIME DEFAULT(CURRENT_TIMESTAMP),
    [month] INT NOT NULL,
    [year] INT NOT NULL,
    [num_of_reserv] INT,
    [num_of_canceled_reserv] INT,
    [num_of_completed_reserv] INT,
    [total_income] MONEY
)
AS
BEGIN
    DECLARE @month INT, @year INT
    DECLARE cs_reserv CURSOR FOR
        SELECT reserv_year, reserv_month 
        FROM (
            SELECT id, MONTH(reserving_date) as reserv_month, YEAR(reserving_date) AS reserv_year 
            FROM reservations
        ) as t
        GROUP BY reserv_month, reserv_year
    OPEN cs_reserv
    FETCH NEXT FROM cs_reserv INTO @month, @year
    WHILE @@FETCH_STATUS = 0  
    BEGIN 
        DECLARE @num_of_reserv INT, @num_of_canceled_reserv INT, @num_of_completed_reserv INT, @income MONEY
        SET @num_of_reserv = (
            SELECT COUNT(*) FROM reservations 
            WHERE MONTH(reserving_date)=@month 
            AND YEAR(reserving_date)=@year
        )
        SET @num_of_canceled_reserv = (
            SELECT COUNT(*) FROM reservations 
            WHERE MONTH(reserving_date)=@month 
            AND YEAR(reserving_date)=@year
            AND [status]='canceled'
        )
        SET @num_of_canceled_reserv = (
            SELECT COUNT(*) FROM reservations 
            WHERE MONTH(reserving_date)=@month 
            AND YEAR(reserving_date)=@year
            AND [status]='completed'
        )
        SET @income = (
            SELECT SUM(total_cost) FROM billing
            WHERE reservation_id IN (
                SELECT id FROM reservations wHERE MONTH(reserving_date)=@month 
                AND YEAR(reserving_date)=@year
            ) AND is_done=1
        )

        INSERT INTO @res ([month], [year], [num_of_reserv], [num_of_canceled_reserv], [num_of_completed_reserv], [total_income])
        VALUES (@month, @year, @num_of_reserv, @num_of_canceled_reserv, @num_of_completed_reserv, @income)
        FETCH NEXT FROM cs_reserv INTO @month, @year
    END
    
    CLOSE cs_reserv
    DEALLOCATE cs_reserv

    RETURN
END
GO


/*==================== POPULATING TABLES ======================*/

-- Insert rows into table 'hall_types' in schema '[dbo]'
INSERT INTO [dbo].[hall_types] ([name], [unit_price]) VALUES ( 'Normal', 1000000)
INSERT INTO [dbo].[hall_types] ([name], [unit_price]) VALUES ( 'Silver', 1100000)
INSERT INTO [dbo].[hall_types] ([name], [unit_price]) VALUES ( 'Gold', 1200000)
INSERT INTO [dbo].[hall_types] ([name], [unit_price]) VALUES ( 'Platinum', 1400000)
INSERT INTO [dbo].[hall_types] ([name], [unit_price]) VALUES ( 'Diamond', 1600000)
SELECT * FROM hall_types
GO

-- Insert rows into table 'halls' in schema '[dbo]'
INSERT INTO [dbo].[halls] ( [type], [name], [num_of_tables], [address] ) VALUES ( 'HT00000004', N'Như lập', 130, 'Tan Phu, Sai Gon')
INSERT INTO [dbo].[halls] ( [type], [name], [num_of_tables], [address] ) VALUES ( 'HT00000003', N'Cát tường', 140, 'Go Vap, Sai Gon')
INSERT INTO [dbo].[halls] ( [type], [name], [num_of_tables], [address] ) VALUES ( 'HT00000002', N'Huy Hoàng', 150, 'Tan Binh, Sai Gon')
INSERT INTO [dbo].[halls] ( [type], [name], [num_of_tables], [address] ) VALUES ( 'HT00000001', N'Như ý', 120, 'Binh Thanh, Sai Gon')
INSERT INTO [dbo].[halls] ( [type], [name], [num_of_tables], [address] ) VALUES ( 'HT00000000', N'Hạnh Phúc', 100, 'District 1, Sai Gon')
SELECT * FROM halls
GO

-- Insert rows into table 'services' in schema '[dbo]'
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'MC', 2000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Biểu diễn văn nghệ', 12000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Âm Thanh Ánh Sáng', 4000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Trang Trí Cổng Vào', 3000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Ban Nhạc', 7000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Pháo Sáng', 1000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Màn hình Leb 100inches', 1000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Đầu Bếp Nhà Hàng 5 sao', 4000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Bàn Ghế cao cấp', 7000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Trang trí Hội Nghị ', 10000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Thiết kế hình ảnh, thiệp mời', 3000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Dựng clip ,trình chiếu suốt tiệc', 5000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Gấu bông ', 4000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Nhân viên bê đỡ tráp ăn hỏi ', 3000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Xe đón dâu', 2000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Xe đưa đón khách', 3000000 )
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Trang điểm cô dâu', 3000000)
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Mâm quả', 2000000)
INSERT INTO [dbo].[services] ( [name], [unit_price] ) VALUES ( N'Quần áo cô dâu, chú rể', 4000000)
SELECT * FROM services
GO


-- Insert rows into table 'hall_services' in schema '[dbo]'
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000000', 'S00000000')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000000', 'S00000015')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000000', 'S00000010')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000000', 'S00000017')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000000', 'S00000016')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000000', 'S00000018')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000000', 'S00000004')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000001', 'S00000000')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000001', 'S00000015')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000001', 'S00000010')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000001', 'S00000017')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000001', 'S00000018')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000001', 'S00000016')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000001', 'S00000004')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000000')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000015')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000011')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000010')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000017')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000016')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000018')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000004')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000000')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000015')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000011')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000010')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000017')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000018')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000016')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000004')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000000')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000015')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000011')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000010')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000017')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000018')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000016')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000004')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000001', 'S00000001')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000001', 'S00000003')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000001', 'S00000013')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000002')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000001')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000013')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000005')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000002', 'S00000012')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000002')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000001')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000013')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000005')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000012')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000003')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000006')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000009')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000003', 'S00000008')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000002')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000001')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000013')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000005')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000012')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000003')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000006')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000009')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000008')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000014')
INSERT INTO [dbo].[hall_services] ([hall_id], [service_id]) VALUES ('H00000004', 'S00000007')
SELECT * FROM hall_services order by hall_id
GO

-- Insert rows into table 'dishes' in schema '[dbo]'
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Rau muống xào ', 40000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gỏi mực kiểu thái', 200000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp cua gà xé', 150000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Cá Chẽm Sốt Tứ Xuyên ', 300000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Sườn heo nấu đậu bánh mì', 300000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Cơm chiên dương châu ', 80000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chè nhãn nhục thạch dừa', 200000, N'Trang Mieng')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gỏi sứa mực', 300000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp cua cay Thượng Hải', 250000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gà Hấp Đông Cô', 300000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Bò nấu patê-Bánh mì ', 300000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Cơm gói lá sen ', 250000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chè đậu đỏ', 250000, N'Trang Mieng')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Bánh cuộn xúc xích', 200000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Pate Chaub', 200000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Bò cuộn nấm kim chi ', 250000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Cá chiên mè', 200000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Jambon bao trứng ', 150000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Tôm sú bao cốm xanh ', 200000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gỏi bò bóp thấu', 250000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gỏi sứ gà xé', 300000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp tam tơ ', 200000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp măng tây cua', 270000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Cơm chiên cá mặn thịt gà ', 300000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Bún xào Singapore', 280000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Mì Hấp Tam Tơ', 300000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Giò heo muối chiên giòn', 350000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Bò xào bắp hạt Thượng Hải ', 300000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Cá diêu hồng sốt chua ngọt', 250000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Sườn non nấu pate', 300000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Nhãn ', 200000, N'Trang Mieng')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Nho Mĩ', 200000, N'Trang Mieng')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Rau câu nghệ thuật 3D', 300000, N'Trang Mieng')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Heo sữa quay+ Bánh Bao', 400000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Phi lê cá Chẽm sốt chua ngọt ', 500000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Bò nấu đốp', 300000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Tôm sú ủ muối', 300000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Sườn non đút lò BBQ', 400000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Lẩu gà nấu lá giang+Bún', 450000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Bò cuộn phô mai', 300000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chả Tôm hạt điều ', 300000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Sườn kinh đô ', 250000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Bò nấu tiêu xanh+Bánh mì ', 300000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Mì hấp dầu xào xá xíu', 400000, N'Mon Chinh')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chè hạt sen', 300000, N'Trang Mieng' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gỏi ngói sen tôm thịt ', 350000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp tứ vị nấm Đông Cô', 300000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Cá Bớp nướng muối ớt xanh', 300000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gà nấu táo đỏ +Bánh mì ', 250000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Lẩu Thái Chua cay ', 300000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chè tuyết nhĩ bạch quả ', 250000, N'Trang Mieng' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Mực chiên giòn ', 300000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Nem nướng ', 300000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chả đùm ', 300000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp sò điệp tóc tiên ', 250000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Tôm sú hoàng kim', 400000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Lẩu gà nấu nấm +Bánh Mì', 300000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Trái cây thập cẩm', 400000, N'Trang Mieng' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gỏi củ hủ dừa tôm thịt ', 300000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Tôm chiên cốm xanh', 400000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp cua Cá Bông', 300000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Mực ống nhòi trứng muối ', 400000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Vịt nướng Tứ Xuyên +Bánh Bao', 400000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Lẩu sa tế hải sản ', 400000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chè đậu xanh', 240000, N'Trang Mieng' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chè đậu đen ', 230000, N'Trang Mieng' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gỏi chuối hải sản chân gà rút xương', 300000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp hải sản ngọc bích', 400000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Cá Hồi nướng muối ớt xanh', 360000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Bồ Câu tiềm hạt sen', 400000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gân nai xào Đông Cô', 390000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Cơm chiên hoàng bảo', 400000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chè Hông Kông', 250000, N'Trang Mieng' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gỏi tôm ngự thuyền ', 280000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp bào ngư vi cá thịt cua', 400000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Bồ câu tay cầm', 250000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Lẩu Cá Mú Thái Lan', 300000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chè dừa tây', 210000, N'Trang Mieng' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Gỏi tiến vua tôm thịt ', 270000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chả giò hải sản ', 230000, N'Khai Vi')
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp vi cá thịt cua', 250000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Đông cô Bách Hoa', 300000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Cua lột lăn mè chiên', 400000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Lẩu hải sản Hông Kông +Mì undon', 300000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Chè hạt sen bạch quả táo đỏ ', 400000, N'Trang Mieng' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Tôm chiên hạnh nhân', 360000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Súp bào ngư phú quý', 400000, N'Khai Vi' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Hàu đút lò phô mai', 340000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Tôm càng rang muối Hồng Kông', 350000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Thân bò Mỹ sốt tiêu đen ', 400000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Lẩu cua 2 miền ', 500000, N'Mon Chinh' )
INSERT INTO [dbo].[dishes] ([name], [unit_price], [type]) VALUES ( N'Sâm bổ lượng ', 230000, N'Trang Mieng' )
SELECT * FROM dishes
GO

/*============================== END POPULATING TABLES ==============================*/
