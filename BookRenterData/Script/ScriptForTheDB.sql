USE [master];


DROP DATABASE IF EXISTS [BookRenterDb];

IF EXISTS
  (SELECT *
   FROM sys.databases
   WHERE name = 'BookRenterDb') BEGIN USE [BookRenterDb];

IF EXISTS
  (SELECT *
   FROM sys.objects
   WHERE object_id = OBJECT_ID(N'[dbo].[Inventories]')
     AND TYPE in (N'U'))
ALTER TABLE [dbo].[Inventories]
DROP CONSTRAINT IF EXISTS [FK__Inventori__BookI__2C3393D0];

IF EXISTS
  (SELECT *
   FROM sys.objects
   WHERE object_id = OBJECT_ID(N'[dbo].[CartBooks]')
     AND TYPE in (N'U')) BEGIN
ALTER TABLE [dbo].[CartBooks]
DROP CONSTRAINT IF EXISTS [FK__CartBooks__UserI__34C8D9D1];


ALTER TABLE [dbo].[CartBooks]
DROP CONSTRAINT IF EXISTS [FK__CartBooks__BookI__35BCFE0A];

END
DROP TABLE IF EXISTS [dbo].[Users];


DROP TABLE IF EXISTS [dbo].[Inventories];


DROP TABLE IF EXISTS [dbo].[CartBooks];


DROP TABLE IF EXISTS [dbo].[Books];



END /****** Object:  Database [BookRenterDb]    Script Date: 12-03-2024 02:28:27 ******/ IF NOT EXISTS
  (SELECT 1
   FROM sys.databases
   WHERE name = 'BookRenterDb') BEGIN
CREATE DATABASE [BookRenterDb];

END;
/****** Object:  Database [BookRenterDb]    Script Date: 12-03-2024 02:28:27 ******/
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'BookRenterDb')
BEGIN
    CREATE DATABASE [BookRenterDb];
END;

 
GO
USE [BookRenterDb]
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[BookId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Author] [nvarchar](100) NOT NULL,
	[Genre] [nvarchar](100) NULL,
	[Price] [float] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[RentPrice] [float] NULL,
 CONSTRAINT [PK__Books__3DE0C2071C5AEFCB] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartBooks]    Script Date: 12-03-2024 02:28:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartBooks](
	[CartBookId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[BookId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CartBookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inventories]    Script Date: 12-03-2024 02:28:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventories](
	[InventoryId] [int] IDENTITY(1,1) NOT NULL,
	[BookId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[InventoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12-03-2024 02:28:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[Role] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Books] ON;

INSERT [dbo].[Books] ([BookId], [Title], [Description], [Author], [Genre], [Price], [CreatedDate], [RentPrice]) 
VALUES 
(1, N'2 States: The Story of My Marriage', N'A romantic novel about the journey of a couple from different cultural backgrounds.', N'Chetan Bhagat', N'Romance', 9.99, CAST(N'2024-03-12T08:00:00.000' AS DateTime), 1.99),
(2, N'Half Girlfriend', N'A love story between a Bihari boy and a rich Delhi girl.', N'Chetan Bhagat', N'Romance', 10.49, CAST(N'2024-03-12T08:30:00.000' AS DateTime), 2.49),
(3, N'Revolution 2020', N'A love triangle set amidst corruption and politics in India.', N'Chetan Bhagat', N'Fiction', 11.99, CAST(N'2024-03-12T09:00:00.000' AS DateTime), 2.99),
(4, N'The Immortals of Meluha', N'First book in the Shiva Trilogy by Amish Tripathi.', N'Amish Tripathi', N'Mythological Fiction', 11.99, CAST(N'2024-03-12T09:30:00.000' AS DateTime), 2.99),
(5, N'The Secret of the Nagas', N'Second book in the Shiva Trilogy by Amish Tripathi.', N'Amish Tripathi', N'Mythological Fiction', 12.49, CAST(N'2024-03-12T10:00:00.000' AS DateTime), 3.49),
(6, N'The Oath of the Vayuputras', N'Final book in the Shiva Trilogy by Amish Tripathi.', N'Amish Tripathi', N'Mythological Fiction', 12.99, CAST(N'2024-03-12T10:30:00.000' AS DateTime), 3.99),
(7, N'The Girl on the Train', N'A psychological thriller about a woman who witnesses a shocking event during her daily commute.', N'Paula Hawkins', N'Thriller', 9.99, CAST(N'2024-03-12T11:00:00.000' AS DateTime), 2.49),
(8, N'One Indian Girl', N'A story about a successful Indian woman''s struggle with love and career.', N'Chetan Bhagat', N'Romance', 10.99, CAST(N'2024-03-12T11:30:00.000' AS DateTime), 2.99),
(9, N'The Alchemist', N'A philosophical novel about a young shepherd named Santiago who sets out on a journey to find his treasure.', N'Paulo Coelho', N'Fiction', 8.99, CAST(N'2024-03-12T12:00:00.000' AS DateTime), 1.99),
(10, N'The Fountainhead', N'A novel about an individualistic young architect named Howard Roark who refuses to compromise his artistic and personal vision.', N'Ayn Rand', N'Fiction', 12.99, CAST(N'2024-03-12T12:30:00.000' AS DateTime), 3.49),
(11, N'Life of Pi', N'A survival novel about a young Indian boy named Pi who is stranded on a lifeboat in the Pacific Ocean with a Bengal tiger.', N'Yann Martel', N'Adventure', 11.49, CAST(N'2024-03-12T13:00:00.000' AS DateTime), 2.99),
(12, N'Big Little Lies', N'A story about three women whose lives intersect when their children attend the same kindergarten.', N'Liane Moriarty', N'Mystery', 10.49, CAST(N'2024-03-12T13:30:00.000' AS DateTime), 2.49),
(13, N'The Girl with the Dragon Tattoo', N'A gripping mystery about a journalist and a hacker who team up to solve a decades-old disappearance case.', N'Stieg Larsson', N'Thriller', 9.99, CAST(N'2024-03-12T14:00:00.000' AS DateTime), 2.49),
(14, N'Harry Potter and the Sorcerer''s Stone', N'The first book in the Harry Potter series, following the adventures of a young wizard named Harry Potter.', N'J.K. Rowling', N'Fantasy', 9.99, CAST(N'2024-03-12T14:30:00.000' AS DateTime), 2.49),
(15, N'The Kite Runner', N'A powerful story of friendship, betrayal, and redemption set in Afghanistan.', N'Khaled Hosseini', N'Fiction', 11.99, CAST(N'2024-03-12T15:00:00.000' AS DateTime), 2.99),
(16, N'The Immortal Rules', N'A dystopian novel set in a world overrun by vampires, where a young girl struggles to survive.', N'Julie Kagawa', N'Young Adult', 10.99, CAST(N'2024-03-12T15:30:00.000' AS DateTime), 2.99),
(17, N'The Night Circus', N'A magical novel about a mysterious competition between two young illusionists.', N'Erin Morgenstern', N'Fantasy', 12.49, CAST(N'2024-03-12T16:00:00.000' AS DateTime), 3.49),
(18, N'The Fault in Our Stars', N'A heartbreaking story about two teenagers who fall in love while battling cancer.', N'John Green', N'Young Adult', 9.49, CAST(N'2024-03-12T16:30:00.000' AS DateTime), 2.49),
(19, N'The Martian', N'A sci-fi novel about an astronaut stranded on Mars who must use his ingenuity to survive.', N'Andy Weir', N'Science Fiction', 11.49, CAST(N'2024-03-12T17:00:00.000' AS DateTime), 2.99),
(20, N'Eleanor & Park', N'A coming-of-age story about two misfit teenagers who fall in love over comic books and music.', N'Rainbow Rowell', N'Romance', 10.49, CAST(N'2024-03-12T17:30:00.000' AS DateTime), 2.49);

SET IDENTITY_INSERT [dbo].[Books] OFF;


SET IDENTITY_INSERT [dbo].[Inventories] ON;

-- Inventory for Book ID 1 (2 States: The Story of My Marriage)
DECLARE @Quantity1 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
INSERT [dbo].[Inventories] ([InventoryId], [BookId], [Quantity], [CreatedDate]) 
VALUES (1, 1, @Quantity1, CAST(N'2024-03-10T10:37:09.087' AS DateTime));

-- Inventory for Book ID 2 (Half Girlfriend)
DECLARE @Quantity2 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
INSERT [dbo].[Inventories] ([InventoryId], [BookId], [Quantity], [CreatedDate]) 
VALUES (2, 2, @Quantity2, CAST(N'2024-03-10T10:37:09.087' AS DateTime));

-- Inventory for Book ID 3 (Revolution 2020)
DECLARE @Quantity3 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
INSERT [dbo].[Inventories] ([InventoryId], [BookId], [Quantity], [CreatedDate]) 
VALUES (3, 3, @Quantity3, CAST(N'2024-03-10T10:37:09.087' AS DateTime));

-- Continue adding inventory for the rest of the books up to Book ID 20
DECLARE @Quantity4 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity5 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity6 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity7 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity8 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity9 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity10 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity11 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity12 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity13 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity14 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity15 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity16 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity17 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity18 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity19 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20
DECLARE @Quantity20 INT = (SELECT ABS(CHECKSUM(NEWID()) % 20) + 1); -- Random quantity between 1 and 20

INSERT [dbo].[Inventories] ([InventoryId], [BookId], [Quantity], [CreatedDate]) 
VALUES 
(4, 4, @Quantity4, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(5, 5, @Quantity5, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(6, 6, @Quantity6, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(7, 7, @Quantity7, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(8, 8, @Quantity8, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(9, 9, @Quantity9, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(10, 10, @Quantity10, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(11, 11, @Quantity11, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(12, 12, @Quantity12, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(13, 13, @Quantity13, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(14, 14, @Quantity14, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(15, 15, @Quantity15, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(16, 16, @Quantity16, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(17, 17, @Quantity17, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(18, 18, @Quantity18, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(19, 19, @Quantity19, CAST(N'2024-03-10T10:37:09.087' AS DateTime)),
(20, 20, @Quantity20, CAST(N'2024-03-10T10:37:09.087' AS DateTime));

SET IDENTITY_INSERT [dbo].[Inventories] OFF;

SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Role]) VALUES (1, N'admin', N'$2a$10$Dabgxt7AZ7owHwch5m2C9.hi9rG/JskmQx6um8WRa8dYJh84TlIBa', N'Admin')
GO
INSERT [dbo].[Users] ([UserId], [Username], [PasswordHash], [Role]) VALUES (2, N'user', N'$2a$10$a/qcxCI5GqhMO5FMhivQfOPQMMuegvTnuAnvNMBqug11ekI/2Eho.', N'User')
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[CartBooks]  WITH CHECK ADD FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([BookId])
GO
ALTER TABLE [dbo].[CartBooks]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Inventories]  WITH CHECK ADD  CONSTRAINT [FK__Inventori__BookI__2C3393D0] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([BookId])
GO
ALTER TABLE [dbo].[Inventories] CHECK CONSTRAINT [FK__Inventori__BookI__2C3393D0]
