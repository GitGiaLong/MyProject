CREATE DATABASE [UserManagement]

USE [UserManagement]
GO
/****** Object:  Table [dbo].[AccountUser]    Script Date: 15/09/2024 6:05:40 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountUser](
	[IsOnly] [varchar](20) NOT NULL,
	[Username] [nvarchar](150) NOT NULL,
	[Password] [varchar](150) NOT NULL,
	[Role] [varchar](10) NOT NULL,
 CONSTRAINT [PK_AccountUser] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[AccountUser] ([IsOnly], [Username], [Password], [Role]) VALUES (N'AdMinSSR', N'AdminSSR', N'123456', N'SSR')
GO
