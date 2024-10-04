CREATE DATABASE [HRM]
USE [HRM]
GO
/****** Object:  Table [dbo].[ImageEmployees]    Script Date: 15/09/2024 6:01:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageEmployees](
	[CodeEmployess] [varchar](20) NULL,
	[ImageData] [varbinary](max) NULL,
	[ImageType] [nvarchar](5) NULL,
	[ImageName] [nvarchar](50) NULL,
	[CoordinatePoint] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InfoEmployees]    Script Date: 15/09/2024 6:01:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InfoEmployees](
	[IsOnly] [varchar](20) NULL,
	[FullName] [nvarchar](50) NULL,
	[Birthday] [date] NULL,
	[EmployeeCode] [nvarchar](50) NULL,
	[CodeGender] [nvarchar](6) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[DeleteQuery]    Script Date: 15/09/2024 6:01:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteQuery]
(
 @tableQuery sysname,
 @whereQuery nvarchar(max)
)
AS
BEGIN
	declare @query nvarchar(max);
	set @query = 'DELETE FROM ' + @tableQuery +' WHERE '+ @whereQuery +' '
	EXEC sp_executesql @query
END
GO
/****** Object:  StoredProcedure [dbo].[InsertValueQuery]    Script Date: 15/09/2024 6:01:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertValueQuery]
(
 @tableQuery sysname,
 @whereQuery nvarchar(max)
)
AS
BEGIN
	declare @query nvarchar(max);
	set @query = 'INSERT INTO ' + @tableQuery +' VALUES ('+ @whereQuery +') '
	EXEC sp_executesql @query
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateValueQuery]    Script Date: 15/09/2024 6:01:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateValueQuery]
(
 @tableQuery sysname,
 @SetQuery nvarchar(max),
 @whereQuery nvarchar(max)
)
AS
BEGIN
	declare @query nvarchar(max);
	set @query = 'UPDATE ' + @tableQuery +' SET '+ @SetQuery +' WHERE '+ @whereQuery +''
	EXEC sp_executesql @query
END
GO
/****** Object:  StoredProcedure [dbo].[ViewOrSelectPagingQuery]    Script Date: 15/09/2024 6:01:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ViewOrSelectPagingQuery]
(
 @tableQuery sysname,
 @subQuery nvarchar(max) = '',
 @selectQuery nvarchar(max) = '*'
)
AS
BEGIN
	declare @query nvarchar(max);
	set @query = 'SELECT '+ @selectQuery +' FROM ' + @tableQuery +' '+@subQuery+''
	EXEC sp_executesql @query
END
GO
/****** Object:  StoredProcedure [dbo].[ViewOrSelectQuery]    Script Date: 15/09/2024 6:01:11 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ViewOrSelectQuery]
(
 @tableQuery sysname,
 @subQuery nvarchar(max) = '',
 @selectQuery nvarchar(max) = '*',
 @OFFSET int = 1,
 @NEXT int = 10
)
AS
BEGIN
	declare @query nvarchar(max);
	set @query = 'SELECT '+ @selectQuery +' FROM ' + @tableQuery +' '+ @subQuery + ' ORDER BY IsOnly OFFSET ' +@OFFSET+ ' ROWS FETCH NEXT' +@NEXT+ 'ROWS ONLY'
	EXEC sp_executesql @query
END
GO
