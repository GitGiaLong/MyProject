USE [HRM]
GO
/****** Object:  Table [dbo].[InfoEmployees]    Script Date: 28/02/2024 4:59:01 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InfoEmployees](
	[IsOnly] [varchar](20) NULL,
	[FullName] [nvarchar](50) NULL,
	[Birthday] [date] NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[DeleteQuery]    Script Date: 28/02/2024 4:59:01 CH ******/
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
/****** Object:  StoredProcedure [dbo].[InsertValueQuery]    Script Date: 28/02/2024 4:59:01 CH ******/
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
/****** Object:  StoredProcedure [dbo].[UpdateValueQuery]    Script Date: 28/02/2024 4:59:01 CH ******/
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
/****** Object:  StoredProcedure [dbo].[ViewOrSelectPagingQuery]    Script Date: 28/02/2024 4:59:01 CH ******/
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
/****** Object:  StoredProcedure [dbo].[ViewOrSelectQuery]    Script Date: 28/02/2024 4:59:01 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ViewOrSelectQuery]
(
 @tableQuery sysname,
 @subQuery nvarchar(max) = '',
 @selectQuery nvarchar(max) = '*'
)
AS
BEGIN
	declare @query nvarchar(max);
	set @query = 'SELECT '+ @selectQuery +' FROM ' + @tableQuery +' '+ @subQuery +''
	EXEC sp_executesql @query
END
GO
