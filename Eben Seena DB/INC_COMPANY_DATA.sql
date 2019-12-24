USE [IBNSINAMAIN]
GO

/****** Object:  Table [dbo].[INC_COMPANY_DATA]    Script Date: 12/17/2019 6:21:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INC_COMPANY_DATA](
	[C_id] [int] NOT NULL,
	[n] [int] IDENTITY(1,1) NOT NULL,
	[C_NAME_ARB] [nvarchar](250) NULL,
	[C_NAME_ENG] [nvarchar](250) NULL,
	[C_STATE] [bit] NULL,
	[C_LEVEL] [int] NULL,
	[BIC_LINK] [nvarchar](150) NULL,
	[USER_ID] [int] NULL,
	[USER_DATE] [datetime] NULL,
	[USER_IP] [nvarchar](50) NULL,
 CONSTRAINT [PK_INC_COMPANY_DATA] PRIMARY KEY CLUSTERED 
(
	[C_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[INC_COMPANY_DATA] ADD  CONSTRAINT [DF_INC_COMPANY_DATA_USER_DATE]  DEFAULT (getdate()) FOR [USER_DATE]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·‘—ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DATA', @level2type=N'COLUMN',@level2name=N'C_id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'«”„ «·‘—ﬂ… »«·⁄—»Ì' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DATA', @level2type=N'COLUMN',@level2name=N'C_NAME_ARB'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'«”„ «·‘—ﬂ… »«·«‰Ã·Ì“Ì' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DATA', @level2type=N'COLUMN',@level2name=N'C_NAME_ENG'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Õ«·… «·‘—ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DATA', @level2type=N'COLUMN',@level2name=N'C_STATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·„” Œœ„ «·«œŒ«·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DATA', @level2type=N'COLUMN',@level2name=N'USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' «—ÌŒ «·«œŒ«·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DATA', @level2type=N'COLUMN',@level2name=N'USER_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'⁄‰Ê«‰ «·ÃÂ«“ ⁄ ‘»ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DATA', @level2type=N'COLUMN',@level2name=N'USER_IP'
GO


