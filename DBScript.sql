USE [PM]
GO
/****** Object:  Table [dbo].[alert]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[alert](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[text] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AlertLog]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AlertLog](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[keyword] [text] NULL,
	[caused] [text] NULL,
	[action] [text] NULL,
	[score] [float] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AuthorLog]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AuthorLog](
	[authorID] [int] IDENTITY(1,1) NOT NULL,
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[date] [datetime] NULL,
	[author] [varchar](50) NULL,
	[version] [varchar](50) NULL,
	[changeReference] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[backupfile]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[backupfile](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[backupControlFile] [text] NULL,
	[backupAchieve] [text] NULL,
	[backupDatafile] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChkServerMacSpec]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChkServerMacSpec](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[hostname] [text] NULL,
	[ipAddress] [text] NULL,
	[oracleOwner_login] [text] NULL,
	[oracleOwner_homeDirectory] [text] NULL,
	[oracleOwner_shell] [text] NULL,
	[oracleGroup_firstGroup] [text] NULL,
	[oracleGroup_secondGroup] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CompareOracleRequirement]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CompareOracleRequirement](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[osNode] [int] NULL,
	[osType] [text] NULL,
	[osInfo] [text] NULL,
	[osMemSize] [text] NULL,
	[osSwap] [text] NULL,
	[osTmp] [text] NULL,
	[osJava] [text] NULL,
	[osKernel] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ControlFile]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ControlFile](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[controlFileName] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DailyCalendarWorksheet]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DailyCalendarWorksheet](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[timeDay] [text] NULL,
	[descOfHouse] [text] NULL,
	[estimatedDuration] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DatabaseConfiguration]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DatabaseConfiguration](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NULL,
	[header] [text] NULL,
	[value] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DatabaseFile]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DatabaseFile](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[tbsName] [varchar](255) NULL,
	[fileName] [text] NULL,
	[size] [text] NULL,
	[max] [text] NULL,
	[aut] [text] NULL,
	[inc] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DatabaseParameter]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DatabaseParameter](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NULL,
	[header] [varchar](200) NULL,
	[value] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DatabaseRegistry]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DatabaseRegistry](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[compId] [text] NULL,
	[version] [text] NULL,
	[status] [text] NULL,
	[lastModified] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DBGrowthRate]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DBGrowthRate](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[allocatedSpace] [float] NULL,
	[usedSpace] [float] NULL,
	[growthDay] [float] NULL,
	[growthMonth] [float] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EngineerInfo]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EngineerInfo](
	[employeeid] [varchar](10) NOT NULL,
	[employeeName] [varchar](45) NULL,
	[employeeLastName] [varchar](45) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EngineerInfo_PmInfo]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EngineerInfo_PmInfo](
	[engineerInfo_employeeid] [varchar](10) NOT NULL,
	[pmInfo_projectCode] [varchar](30) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[getHitRatio]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[getHitRatio](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[nameSpace] [varchar](45) NULL,
	[getHitRatio] [float] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HardwareConfiguration]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HardwareConfiguration](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[node] [int] NULL,
	[systemModel] [varchar](45) NULL,
	[machineSerialNumber] [varchar](45) NULL,
	[processorType] [varchar](45) NULL,
	[processorImplementationMode] [varchar](45) NULL,
	[processorVersion] [varchar](45) NULL,
	[numOfProc] [varchar](45) NULL,
	[procClockSpeed] [varchar](45) NULL,
	[cpuType] [varchar](45) NULL,
	[kernelType] [varchar](45) NULL,
	[ipaddress] [varchar](45) NULL,
	[subNetMask] [varchar](45) NULL,
	[crontab] [varchar](45) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MajorSecurityInitailization]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MajorSecurityInitailization](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[header] [text] NULL,
	[value] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MonthlyCalendarWorksheet]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MonthlyCalendarWorksheet](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[dayProject] [text] NULL,
	[descpOfHouse] [text] NULL,
	[estimatedDur] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OSDiskSpace]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OSDiskSpace](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[node] [int] NULL,
	[fileSystem] [varchar](45) NULL,
	[mbBlocks] [varchar](45) NULL,
	[free] [varchar](45) NULL,
	[percentUsed] [varchar](45) NULL,
	[iUsed] [varchar](45) NULL,
	[percentIUsed] [varchar](45) NULL,
	[mountedOn] [varchar](45) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[performanceReview]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[performanceReview](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[header] [text] NULL,
	[value] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Person]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[Person_id] [int] NULL,
	[Title] [text] NULL,
	[Firstname] [text] NULL,
	[Lastname] [text] NULL,
	[Username] [text] NULL,
	[Password] [text] NULL,
	[Email] [text] NULL,
	[Phone] [text] NULL,
	[Position] [text] NULL,
	[PermissionLevel] [int] NULL,
	[Permission] [int] NULL,
	[Image_File] [image] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Personinfo]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Personinfo](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[personID] [int] NULL,
	[personName] [varchar](45) NULL,
	[personLastName] [varchar](45) NULL,
	[personEmail] [varchar](45) NULL,
	[personPhone] [varchar](45) NULL,
	[personType] [varchar](30) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PinRatio]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PinRatio](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[execution] [text] NULL,
	[cacheMisses] [text] NULL,
	[sumPin] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PmInfo]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PmInfo](
	[projectCode] [varchar](30) NOT NULL,
	[customerCompanyFull] [varchar](45) NULL,
	[customerAbbv] [varchar](15) NULL,
	[projectName] [varchar](45) NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[pmstatus] [varchar](45) NULL,
	[Reviewer] [varchar](45) NULL,
	[Authun] [varchar](45) NULL,
	[databaseName] [text] NULL,
	[createdDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PmInfo_PersonInfo]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PmInfo_PersonInfo](
	[pmInfo_projectCode] [varchar](30) NOT NULL,
	[pmInfo_projectQuarter] [varchar](8) NULL,
	[personinfo_personID] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RedoLogFile]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RedoLogFile](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[groupMember] [int] NULL,
	[member] [text] NULL,
	[size] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[reviewerLog]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[reviewerLog](
	[reviewerID] [int] IDENTITY(1,1) NOT NULL,
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[reviewDate] [datetime] NULL,
	[reviewerName] [varchar](50) NULL,
	[position] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TablespaceAndTempTablespace]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TablespaceAndTempTablespace](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[userName] [text] NULL,
	[defaultTablespace] [text] NULL,
	[temporaryTablespace] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TablespaceFreespace]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TablespaceFreespace](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[tablespaceName] [varchar](45) NULL,
	[maxBlocks] [text] NULL,
	[countBlock] [text] NULL,
	[sumFreeBlock] [text] NULL,
	[pct_free] [varchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TablespaceName]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TablespaceName](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[tablespaceName] [text] NULL,
	[size] [float] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TempFile]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TempFile](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NULL,
	[tbsName] [varchar](255) NULL,
	[fileName] [text] NULL,
	[size] [text] NULL,
	[max] [text] NULL,
	[aut] [text] NULL,
	[inc] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TempTableSize]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TempTableSize](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[tempTableSpace] [text] NULL,
	[size] [float] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[undoSegmentsSize]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[undoSegmentsSize](
	[projectCode] [varchar](30) NOT NULL,
	[projectQuarter] [varchar](8) NOT NULL,
	[amount] [text] NULL,
	[segmentType] [text] NULL,
	[size] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserEnvironment]    Script Date: 5/2/2017 9:48:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserEnvironment](
	[projectCode] [varchar](30) NULL,
	[projectQuarter] [varchar](8) NULL,
	[header] [text] NULL,
	[value] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
