USE [IBNSINAMAIN]
GO

/****** Object:  Table [dbo].[INC_BLOCK_SERVICES]    Script Date: 12/17/2019 6:19:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INC_BLOCK_SERVICES](
	[N] [int] IDENTITY(1,1) NOT NULL,
	[OBJECT_ID] [int] NULL,
	[SER_ID] [int] NULL,
	[BLOCK_TP] [int] NULL,
	[NOTES] [nvarchar](max) NULL,
	[USER_ID] [int] NULL,
	[USER_DATE] [date] NULL,
	[USER_IP] [nvarchar](50) NULL,
 CONSTRAINT [PK_INC_BLOCK_SERVICES] PRIMARY KEY CLUSTERED 
(
	[N] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[INC_BLOCK_SERVICES] ADD  CONSTRAINT [DF_INC_BLOCK_SERVICES_USER_DATE]  DEFAULT (getdate()) FOR [USER_DATE]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' —ﬁÌ„  ·ﬁ«∆Ì' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_BLOCK_SERVICES', @level2type=N'COLUMN',@level2name=N'N'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·„‘ —ﬂ √Ê —ﬁ„ «·‘—ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_BLOCK_SERVICES', @level2type=N'COLUMN',@level2name=N'OBJECT_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·Œœ„… √Ê —ﬁ„ «·ÿ»Ì»' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_BLOCK_SERVICES', @level2type=N'COLUMN',@level2name=N'SER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'‰Ê⁄ «·ÕŸ—: 1-ÕŸ— Œœ„… ⁄‰ „‘ —ﬂ 2-ÕŸ— ÿ»Ì» ⁄‰ ‘—ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_BLOCK_SERVICES', @level2type=N'COLUMN',@level2name=N'BLOCK_TP'
GO


