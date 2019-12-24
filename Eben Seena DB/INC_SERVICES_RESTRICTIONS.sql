USE [IBNSINAMAIN]
GO

/****** Object:  Table [dbo].[INC_SERVICES_RESTRICTIONS]    Script Date: 12/17/2019 6:23:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INC_SERVICES_RESTRICTIONS](
	[N] [int] IDENTITY(1,1) NOT NULL,
	[C_ID] [int] NULL,
	[CLINIC_ID] [int] NULL,
	[SER_ID] [int] NULL,
	[PERSON_PER] [int] NULL,
	[FAMILY_PER] [int] NULL,
	[PARENT_PER] [int] NULL,
	[MAX_PERSON_VAL] [money] NULL,
	[MAX_FAMILY_VAL] [money] NULL,
	[SER_STATE] [int] NULL,
	[PAYMENT_TYPE] [int] NULL,
	[CONTRACT_NO] [int] NULL,
	[USER_ID] [int] NULL,
	[USER_DATE] [date] NULL,
	[USER_IP] [nvarchar](50) NULL,
 CONSTRAINT [PK_INC_SERVICES_RESTRICTIONS] PRIMARY KEY CLUSTERED 
(
	[N] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[INC_SERVICES_RESTRICTIONS] ADD  CONSTRAINT [DF_INC_SERVICES_RESTRICTIONS_PERSON_PER]  DEFAULT ((0)) FOR [PERSON_PER]
GO

ALTER TABLE [dbo].[INC_SERVICES_RESTRICTIONS] ADD  CONSTRAINT [DF_INC_SERVICES_RESTRICTIONS_FAMILY_PER]  DEFAULT ((0)) FOR [FAMILY_PER]
GO

ALTER TABLE [dbo].[INC_SERVICES_RESTRICTIONS] ADD  CONSTRAINT [DF_INC_SERVICES_RESTRICTIONS_PARENT_PER]  DEFAULT ((0)) FOR [PARENT_PER]
GO

ALTER TABLE [dbo].[INC_SERVICES_RESTRICTIONS] ADD  CONSTRAINT [DF_INC_SERVICES_RESTRICTIONS_MAX_PERSON_VAL]  DEFAULT ((0)) FOR [MAX_PERSON_VAL]
GO

ALTER TABLE [dbo].[INC_SERVICES_RESTRICTIONS] ADD  CONSTRAINT [DF_INC_SERVICES_RESTRICTIONS_MAX_FAMILY_VAL]  DEFAULT ((0)) FOR [MAX_FAMILY_VAL]
GO

ALTER TABLE [dbo].[INC_SERVICES_RESTRICTIONS] ADD  CONSTRAINT [DF_INC_SERVICES_RESTRICTIONS_USER_DATE]  DEFAULT (getdate()) FOR [USER_DATE]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„  ”·”·Ì' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'N'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·‘—ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'C_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·⁄Ì«œ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'CLINIC_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·Œœ„…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'SER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'‰”»… «·›—œ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'PERSON_PER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'‰”»… «·⁄«∆·Â' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'FAMILY_PER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'‰”»… «·Ê«·œÌ‰' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'PARENT_PER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'”ﬁ› «·›—œ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'MAX_PERSON_VAL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'”ﬁ› «·⁄«∆·Â' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'MAX_FAMILY_VAL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Õ«·… «·„‰›⁄…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'SER_STATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ÿ—Ìﬁ… «·œ›⁄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'PAYMENT_TYPE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'—ﬁ„ «·⁄ﬁœ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'CONTRACT_NO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'«·„” Œœ„ «·–Ì ﬁ«„ »«·≈–Œ«·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' «—ÌŒ «·≈–Œ«·' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'USER_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'⁄‰Ê«‰ «·ÃÂ«“ ⁄ ‘»ﬂ…' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_SERVICES_RESTRICTIONS', @level2type=N'COLUMN',@level2name=N'USER_IP'
GO


