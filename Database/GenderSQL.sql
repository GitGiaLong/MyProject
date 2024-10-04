CREATE DATABASE [Gender]

USE [Gender]
GO
/****** Object:  Table [dbo].[BodyCharacteristic]    Script Date: 15/09/2024 5:59:09 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BodyCharacteristic](
	[IsOnly] [nvarchar](3) NOT NULL,
	[NameEN] [nvarchar](50) NULL,
	[NameVI] [nvarchar](50) NOT NULL,
	[IsCurently] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[UpdateOn] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_BodyCharacteristic] PRIMARY KEY CLUSTERED 
(
	[IsOnly] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sex]    Script Date: 15/09/2024 5:59:09 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sex](
	[IsOnly] [nvarchar](6) NOT NULL,
	[CodeBodyCharacteristic] [nvarchar](3) NOT NULL,
	[NameEN] [nvarchar](50) NULL,
	[NameVI] [nvarchar](50) NOT NULL,
	[Note] [nvarchar](150) NULL,
	[IsCurently] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [nvarchar](50) NULL,
	[UpdateOn] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Sex] PRIMARY KEY CLUSTERED 
(
	[IsOnly] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[BodyCharacteristic] ([IsOnly], [NameEN], [NameVI], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'F00', N'female', N'Nữ', 1, CAST(N'2024-03-06T01:36:31.853' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T01:36:31.853' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[BodyCharacteristic] ([IsOnly], [NameEN], [NameVI], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'M01', N'male', N'Nam', 1, CAST(N'2024-03-06T01:36:31.853' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T01:36:31.853' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[BodyCharacteristic] ([IsOnly], [NameEN], [NameVI], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'O02', N'other', N'Khác', 1, CAST(N'2024-03-06T01:36:31.853' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T01:36:31.853' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[Sex] ([IsOnly], [CodeBodyCharacteristic], [NameEN], [NameVI], [Note], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'GdBF00', N'F00', N'Bisexual Female', N'Lưỡng tính- song tính nữ', N'Có đặc tính cơ thể là nữ(Phụ nữ)', 1, CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[Sex] ([IsOnly], [CodeBodyCharacteristic], [NameEN], [NameVI], [Note], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'GdBM01', N'M01', N'Bisexual Male', N'Lưỡng tính- song tính nam', N'Có đặc tính cơ thể là nam(Đàn ông)', 1, CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[Sex] ([IsOnly], [CodeBodyCharacteristic], [NameEN], [NameVI], [Note], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'GdBO02', N'O02', N'Bisexual Other', N'Lưỡng tính- song', N'không rõ', 1, CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[Sex] ([IsOnly], [CodeBodyCharacteristic], [NameEN], [NameVI], [Note], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'GdFF00', N'F00', N'female', N'Nữ', N'Có đặc tính cơ thể là nữ(Phụ nữ)', 1, CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[Sex] ([IsOnly], [CodeBodyCharacteristic], [NameEN], [NameVI], [Note], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'GdGM01', N'M01', N'Gay', N'Đồng tính nam', N'Có đặc tính cơ thể là nam(Đàn ông)', 1, CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[Sex] ([IsOnly], [CodeBodyCharacteristic], [NameEN], [NameVI], [Note], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'GdLF00', N'F00', N'Lesbian', N'Đồng tính nữ', N'Có đặc tính cơ thể là nữ(Phụ nữ)', 1, CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[Sex] ([IsOnly], [CodeBodyCharacteristic], [NameEN], [NameVI], [Note], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'GdMM01', N'M01', N'male', N'Nam', N'Có đặc tính cơ thể là nam(Đàn ông)', 1, CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[Sex] ([IsOnly], [CodeBodyCharacteristic], [NameEN], [NameVI], [Note], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'GdTF00', N'F00', N'Transgender Female', N'Chuyển giới sang nữ', N'Có đặc tính cơ thể là nữ(Phụ nữ)', 1, CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR')
GO
INSERT [dbo].[Sex] ([IsOnly], [CodeBodyCharacteristic], [NameEN], [NameVI], [Note], [IsCurently], [CreateOn], [CreateBy], [UpdateOn], [UpdateBy]) VALUES (N'GdTM01', N'M01', N'Transgender Male', N'Chuyển giới sang nam', N'Có đặc tính cơ thể là nam(Đàn ông)', 1, CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR', CAST(N'2024-03-06T02:02:03.967' AS DateTime), N'ADMINSSR')
GO
ALTER TABLE [dbo].[BodyCharacteristic] ADD  CONSTRAINT [DF_BodyCharacteristic_IsCurently]  DEFAULT ((0)) FOR [IsCurently]
GO
ALTER TABLE [dbo].[BodyCharacteristic] ADD  CONSTRAINT [DF_BodyCharacteristic_CreateBy]  DEFAULT (NULL) FOR [CreateBy]
GO
ALTER TABLE [dbo].[BodyCharacteristic] ADD  CONSTRAINT [DF_BodyCharacteristic_UpdateBy]  DEFAULT (NULL) FOR [UpdateBy]
GO
ALTER TABLE [dbo].[Sex] ADD  CONSTRAINT [DF_Sex_IsCurently]  DEFAULT ((0)) FOR [IsCurently]
GO
ALTER TABLE [dbo].[Sex] ADD  CONSTRAINT [DF_Sex_CreateBy]  DEFAULT (NULL) FOR [CreateBy]
GO
ALTER TABLE [dbo].[Sex] ADD  CONSTRAINT [DF_Sex_UpdateBy]  DEFAULT (NULL) FOR [UpdateBy]
GO
ALTER TABLE [dbo].[Sex]  WITH CHECK ADD  CONSTRAINT [FK_Sex_BodyCharacteristic1] FOREIGN KEY([CodeBodyCharacteristic])
REFERENCES [dbo].[BodyCharacteristic] ([IsOnly])
GO
ALTER TABLE [dbo].[Sex] CHECK CONSTRAINT [FK_Sex_BodyCharacteristic1]
GO
