USE [DB_A41508_ibn]
GO

/****** Object:  Table [dbo].[INC_SERVICES_PRICES]    Script Date: 12/24/2019 8:47:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INC_SERVICES_PRICES](
	[N] [int] IDENTITY(1,1) NOT NULL,
	[SER_ID] [int] NULL,
	[CASH_PRS] [decimal](18, 3) NULL,
	[INS_PRS] [decimal](18, 3) NULL,
	[INVO_PRS] [decimal](18, 3) NULL,
	[USER_ID] [int] NULL,
	[USER_DATE] [date] NULL,
	[USER_IP] [nvarchar](50) NULL,
	[PROFILE_PRICE_ID] [int] NULL,
 CONSTRAINT [PK_INC_TAM_SERVICES_PRICES] PRIMARY KEY CLUSTERED 
(
	[N] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„  ”·”·Ì' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_PRICES', @level2type=N'COLUMN',@level2name=N'N'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·Œœ„…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_PRICES', @level2type=N'COLUMN',@level2name=N'SER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'«·”⁄— «·‰ﬁœÌ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_PRICES', @level2type=N'COLUMN',@level2name=N'CASH_PRS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'”⁄— «· √„Ì‰' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_PRICES', @level2type=N'COLUMN',@level2name=N'INS_PRS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'”⁄— «·›Ê —…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_PRICES', @level2type=N'COLUMN',@level2name=N'INVO_PRS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'«·„” Œœ„ «·–Ì ﬁ«„ »«·≈–Œ«·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_PRICES', @level2type=N'COLUMN',@level2name=N'USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' «—ÌŒ «·≈–Œ«·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_PRICES', @level2type=N'COLUMN',@level2name=N'USER_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'⁄‰Ê«‰ «·ÃÂ«“ ⁄ «·‘»ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_PRICES', @level2type=N'COLUMN',@level2name=N'USER_IP'
GO


