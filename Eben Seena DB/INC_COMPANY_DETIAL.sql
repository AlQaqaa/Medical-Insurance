USE [DB_A41508_ibn]
GO

/****** Object:  Table [dbo].[INC_COMPANY_DETIAL]    Script Date: 12/24/2019 6:53:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INC_COMPANY_DETIAL](
	[N] [int] IDENTITY(1,1) NOT NULL,
	[C_ID] [int] NOT NULL,
	[DATE_START] [date] NULL,
	[DATE_END] [date] NULL,
	[MAX_VAL] [decimal](18, 3) NULL,
	[MAX_CARD] [decimal](18, 3) NULL,
	[PYMENT_TYPE] [int] NULL,
	[CONTRCT_TYPE] [int] NULL,
	[PATIAINT_PER] [int] NULL,
	[CONTRACT_NO] [int] NULL,
	[PROFILE_PRICE_ID] [int] NULL,
	[USER_ID] [int] NULL,
	[USER_DATE] [datetime] NULL,
	[USER_IP] [nvarchar](50) NULL,
 CONSTRAINT [PK_INC_COMPANY_DETIAL] PRIMARY KEY CLUSTERED 
(
	[N] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[INC_COMPANY_DETIAL] ADD  CONSTRAINT [DF_INC_COMPANY_DETIAL_CONTRCT_TYPE]  DEFAULT ((1)) FOR [CONTRCT_TYPE]
GO

ALTER TABLE [dbo].[INC_COMPANY_DETIAL] ADD  CONSTRAINT [DF_INC_COMPANY_DETIAL_USER_DATE]  DEFAULT (getdate()) FOR [USER_DATE]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·‘—ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DETIAL', @level2type=N'COLUMN',@level2name=N'C_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'»œ«Ì… «· ⁄«ﬁœ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DETIAL', @level2type=N'COLUMN',@level2name=N'DATE_START'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'‰Â«Ì… «· ⁄«ﬁœ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DETIAL', @level2type=N'COLUMN',@level2name=N'DATE_END'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'’ﬁ› «·⁄«„ ··‘—ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DETIAL', @level2type=N'COLUMN',@level2name=N'MAX_VAL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'’ﬁ› «·»ÿ«ﬁ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DETIAL', @level2type=N'COLUMN',@level2name=N'MAX_CARD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ÿ—Ìﬁ… «·œ›⁄( √„Ì‰-›Ê —Â_‰ﬁœÌ)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DETIAL', @level2type=N'COLUMN',@level2name=N'PYMENT_TYPE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ÿ—Ìﬁ… «· ⁄«ﬁœ (1-ÃœÌœ 2- „œÌœ -3 ÃœÌœ)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DETIAL', @level2type=N'COLUMN',@level2name=N'CONTRCT_TYPE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ÿ—ﬁ… œ›⁄ ‰”… «·„—Ì÷' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DETIAL', @level2type=N'COLUMN',@level2name=N'PATIAINT_PER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·„” Œœ„ «·«œŒ«·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DETIAL', @level2type=N'COLUMN',@level2name=N'USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' «—ÌŒ «·«œŒ«·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_COMPANY_DETIAL', @level2type=N'COLUMN',@level2name=N'USER_DATE'
GO


