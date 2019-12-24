USE [IBNSINAMAIN]
GO

/****** Object:  Table [dbo].[INC_CLINICAL_RESTRICTIONS]    Script Date: 12/17/2019 6:21:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INC_CLINICAL_RESTRICTIONS](
	[n] [int] IDENTITY(1,1) NOT NULL,
	[C_ID] [int] NULL,
	[CLINIC_ID] [int] NULL,
	[CONTRACT_NO] [int] NULL,
	[MAX_VALUE] [money] NULL,
	[PER_T] [int] NULL,
	[SESSION_COUNT] [int] NULL,
	[GROUP_NO] [int] NULL,
	[USER_ID] [int] NULL,
	[USER_DATE] [date] NULL,
	[USER_IP] [nvarchar](50) NULL,
 CONSTRAINT [PK_INC_TAM_CLINICAL_RESTRICTIONS] PRIMARY KEY CLUSTERED 
(
	[n] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[INC_CLINICAL_RESTRICTIONS] ADD  CONSTRAINT [DF_INC_CLINICAL_RESTRICTIONS_SESSION_COUNT]  DEFAULT ((0)) FOR [SESSION_COUNT]
GO

ALTER TABLE [dbo].[INC_CLINICAL_RESTRICTIONS] ADD  CONSTRAINT [DF_INC_CLINICAL_RESTRICTIONS_USER_DATE]  DEFAULT (getdate()) FOR [USER_DATE]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„  ”·”·Ì' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_CLINICAL_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'n'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·‘—ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_CLINICAL_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'C_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·⁄Ì«œ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_CLINICAL_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'CLINIC_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·⁄ﬁœ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_CLINICAL_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'CONTRACT_NO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'”ﬁ› «·⁄Ì«œ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_CLINICAL_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'MAX_VALUE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'‰”»… œ›⁄ «·„‰ ›⁄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_CLINICAL_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'PER_T'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'⁄œœ «·Ã·”«  ·⁄Ì«œ«  «·⁄·«Ã «·ÿ»Ì⁄Ì' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_CLINICAL_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'SESSION_COUNT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'«·„” Œœ„ «·–Ì ﬁ«„ »«·√œŒ«·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_CLINICAL_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' «—ÌŒ «·≈œŒ«·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_CLINICAL_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'USER_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'⁄‰Ê«‰ «·ÃÂ«“ ⁄ ‘»ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_CLINICAL_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'USER_IP'
GO


