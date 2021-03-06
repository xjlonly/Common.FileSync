USE [master]
GO
/****** Object:  Database [FileSync]    Script Date: 2016/11/14 15:09:54 ******/
CREATE DATABASE [FileSync] ON  PRIMARY 
( NAME = N'FileSync', FILENAME = N'd:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\FileSync.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FileSync_log', FILENAME = N'd:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\FileSync_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [FileSync] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FileSync].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FileSync] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FileSync] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FileSync] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FileSync] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FileSync] SET ARITHABORT OFF 
GO
ALTER DATABASE [FileSync] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FileSync] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FileSync] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FileSync] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FileSync] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FileSync] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FileSync] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FileSync] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FileSync] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FileSync] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FileSync] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FileSync] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FileSync] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FileSync] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FileSync] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FileSync] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FileSync] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FileSync] SET RECOVERY FULL 
GO
ALTER DATABASE [FileSync] SET  MULTI_USER 
GO
ALTER DATABASE [FileSync] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FileSync] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'FileSync', N'ON'
GO
USE [FileSync]
GO
/****** Object:  Table [dbo].[NBlock_Error]    Script Date: 2016/11/14 15:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NBlock_Error](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BlockCode] [nvarchar](100) NOT NULL,
	[Command] [nvarchar](200) NOT NULL,
	[SendTime] [datetime] NULL CONSTRAINT [DF__NBlock_Er__SendT__1FCDBCEB]  DEFAULT (getdate()),
	[Action] [nvarchar](500) NULL CONSTRAINT [DF__NBlock_Er__Actio__20C1E124]  DEFAULT (''),
	[CreateTime] [datetime] NULL CONSTRAINT [DF__NBlock_Er__Creat__21B6055D]  DEFAULT (getdate()),
	[IsDel] [int] NOT NULL CONSTRAINT [DF__NBlock_Er__IsDel__22AA2996]  DEFAULT ((0)),
 CONSTRAINT [PK__NBlock_E__3214EC271DE57479] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NBlock_Info]    Script Date: 2016/11/14 15:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NBlock_Info](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BlockCode] [nvarchar](100) NOT NULL,
	[Status] [int] NOT NULL CONSTRAINT [DF_NBlock_Info_Status]  DEFAULT ((0)),
	[StartTime] [datetime] NOT NULL CONSTRAINT [DF_NBlock_Info_StartTime]  DEFAULT (getdate()),
	[UploadSuccess] [datetime] NOT NULL CONSTRAINT [DF_NBlock_Info_UploadSuccess]  DEFAULT (((1900)-(1))-(1)),
	[BackupSuccess] [datetime] NOT NULL CONSTRAINT [DF_NBlock_Info_BackupSuccess]  DEFAULT (((1900)-(1))-(1)),
	[CoverSuccess] [datetime] NOT NULL CONSTRAINT [DF_NBlock_Info_CoverSuccess]  DEFAULT (((1900)-(1))-(1)),
	[CancelTime] [datetime] NOT NULL CONSTRAINT [DF_NBlock_Info_CancelTime]  DEFAULT (((1900)-(1))-(1)),
	[ErrorFinishTime] [datetime] NOT NULL CONSTRAINT [DF_NBlock_Info_ErrorFinishTime]  DEFAULT (((1900)-(1))-(1)),
	[CoverStatus] [nvarchar](200) NOT NULL CONSTRAINT [DF_NBlock_Info_CoverStatus]  DEFAULT (''),
	[UploadLog] [nvarchar](max) NOT NULL CONSTRAINT [DF_NBlock_Info_UploadLog]  DEFAULT (''),
	[ActionMark] [nvarchar](200) NOT NULL CONSTRAINT [DF_NBlock_Info_ActionMark]  DEFAULT (''),
	[IsDel] [int] NOT NULL CONSTRAINT [DF_NBlock_Info_IsDel]  DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_NBlock_Info_CreateTime]  DEFAULT (((1900)-(1))-(1)),
 CONSTRAINT [PK__NBlock_Info] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NBlock_Task]    Script Date: 2016/11/14 15:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NBlock_Task](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BlockCode] [nvarchar](100) NOT NULL CONSTRAINT [DF_NBlock_Task_BlockCode]  DEFAULT (''),
	[FilePath] [nvarchar](1000) NOT NULL CONSTRAINT [DF_NBlock_Task_FilePath]  DEFAULT (''),
	[Status] [int] NOT NULL CONSTRAINT [DF_NBlock_Task_Status]  DEFAULT ((0)),
	[UploadLog] [nvarchar](1000) NOT NULL CONSTRAINT [DF_NBlock_Task_UploadLog]  DEFAULT (''),
	[CoverStatus] [nvarchar](50) NOT NULL CONSTRAINT [DF_NBlock_Task_CoverStatus]  DEFAULT (''),
	[ErrorServer] [nvarchar](200) NOT NULL CONSTRAINT [DF_NBlock_Task_ErrorServer]  DEFAULT (''),
	[IsDel] [int] NOT NULL CONSTRAINT [DF_NBlock_Task_IsDel]  DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_NBlock_Task_CreateTime]  DEFAULT (((1900)-(1))-(1)),
 CONSTRAINT [PK_NBlock_Task] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NBlock_Trace]    Script Date: 2016/11/14 15:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NBlock_Trace](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Site] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[BlockCode] [nvarchar](100) NULL CONSTRAINT [DF__NBlock_Trace_BlockCode]  DEFAULT (''),
	[YearVal] [nvarchar](50) NOT NULL CONSTRAINT [DF__NBlock_Tr__YearV__173876EA]  DEFAULT (''),
	[MonthVal] [nvarchar](50) NOT NULL CONSTRAINT [DF__NBlock_Tr__Month__182C9B23]  DEFAULT (''),
	[DayVal] [nvarchar](50) NOT NULL CONSTRAINT [DF__NBlock_Tr__DayVa__1920BF5C]  DEFAULT (''),
	[IsDel] [int] NOT NULL CONSTRAINT [DF_NBlock_Trace_IsDel]  DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_NBlock_Trace_CreateTime]  DEFAULT (((1900)-(1))-(1)),
 CONSTRAINT [PK__NBlock_Trace] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NRoll_Action]    Script Date: 2016/11/14 15:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NRoll_Action](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RollCode] [nvarchar](50) NOT NULL,
	[ServerIP] [nvarchar](30) NOT NULL,
	[ServerPort] [int] NOT NULL,
	[ServerBakUri] [nvarchar](300) NOT NULL,
	[ServerAimUri] [nvarchar](300) NOT NULL,
	[SiteName] [nvarchar](200) NOT NULL,
	[Op] [nvarchar](30) NOT NULL,
	[IsUsed] [int] NOT NULL CONSTRAINT [DF__NRoll_Action_IsUse]  DEFAULT ((0)),
	[AutoRollBack] [int] NOT NULL CONSTRAINT [DF_NRoll_Action_AutoRollBack]  DEFAULT ((0)),
	[BlockCode] [nvarchar](100) NOT NULL CONSTRAINT [DF_NRoll_Action_BlockCode]  DEFAULT (''),
	[MonthVal] [nvarchar](50) NOT NULL CONSTRAINT [DF__NRoll_Act__Month__31EC6D26]  DEFAULT (''),
	[DayVal] [nvarchar](50) NOT NULL CONSTRAINT [DF__NRoll_Act__DayVa__32E0915F]  DEFAULT (''),
	[IsDel] [int] NOT NULL CONSTRAINT [DF_NRoll_Action_IsDel]  DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_NRoll_Action_CreateTime]  DEFAULT (((1900)-(1))-(1)),
	[YearVal] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__NRoll_Action] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NUser_AuthDetail]    Script Date: 2016/11/14 15:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NUser_AuthDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[DetailName] [nvarchar](200) NOT NULL,
	[AllowList] [nvarchar](10) NOT NULL,
	[AllowSync] [nvarchar](10) NOT NULL,
	[AllowRoll] [nvarchar](10) NOT NULL,
	[SiteName] [nvarchar](200) NOT NULL,
	[IsDir] [nvarchar](10) NOT NULL,
	[IsDel] [int] NOT NULL CONSTRAINT [DF_NUser_AuthDetail_IsDel]  DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_NUser_AuthDetail_CreateTime]  DEFAULT (((1900)-(1))-(1)),
 CONSTRAINT [PK_NUser_AuthDetail] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NUser_AuthSite]    Script Date: 2016/11/14 15:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NUser_AuthSite](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[SiteName] [nvarchar](200) NOT NULL,
	[AllowList] [nvarchar](10) NOT NULL CONSTRAINT [DF_NUser_AuthSite_AllowList]  DEFAULT (N'False'),
	[AllowSync] [nvarchar](10) NOT NULL CONSTRAINT [DF_NUser_AuthSite_AllowSync]  DEFAULT (N'False'),
	[AllowRoll] [nvarchar](10) NOT NULL CONSTRAINT [DF_NUser_AuthSite_AllowRoll]  DEFAULT (N'False'),
	[IsDel] [int] NOT NULL CONSTRAINT [DF_NUser_AuthSite_IsDel]  DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_NUser_AuthSite_CreateTime]  DEFAULT (((1900)-(1))-(1)),
 CONSTRAINT [PK_NUser_AuthSite] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[SiteName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NUser_Info]    Script Date: 2016/11/14 15:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NUser_Info](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[RefUserId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[IsDel] [int] NOT NULL CONSTRAINT [DF_NUser_Info_IsDel]  DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_NUser_Info_CreateTime]  DEFAULT (((1900)-(1))-(1)),
 CONSTRAINT [PK__NUser_Info] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NUser_Log]    Script Date: 2016/11/14 15:09:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NUser_Log](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Action] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](300) NOT NULL,
	[IsDel] [int] NOT NULL CONSTRAINT [DF_NUser_Log_IsDel]  DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF_NUser_Log_CreateTime]  DEFAULT (((1900)-(1))-(1)),
PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [FileSync] SET  READ_WRITE 
GO
