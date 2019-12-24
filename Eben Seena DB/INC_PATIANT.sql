USE [IBNSINAMAIN]
GO

/****** Object:  Table [dbo].[INC_PATIANT]    Script Date: 12/17/2019 6:22:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INC_PATIANT](
	[N] [int] IDENTITY(1,1) NOT NULL,
	[PINC_ID] [int] NOT NULL,
	[CARD_NO] [nvarchar](100) NULL,
	[NAME_ARB] [nvarchar](250) NULL,
	[NAME_ENG] [nvarchar](250) NULL,
	[BIRTHDATE] [date] NULL,
	[BAGE_NO] [nvarchar](100) NULL,
	[C_ID] [int] NULL,
	[GENDER] [nvarchar](10) NULL,
	[NAL_ID] [int] NULL,
	[PHONE_NO] [bigint] NULL,
	[CONST_ID] [int] NULL,
	[EXP_DATE] [date] NULL,
	[NOTES] [nvarchar](250) NULL,
	[P_STATE] [bit] NULL,
	[NAT_NUMBER] [bigint] NULL,
	[KID_NO] [int] NULL,
	[CITY_ID] [int] NULL,
	[IMAGE_CARD] [nvarchar](250) NULL,
	[USER_ID] [int] NULL,
	[USER_DATE] [date] NULL,
	[USER_IP] [nvarchar](50) NULL,
 CONSTRAINT [PK_INC_INC_PATIANT] PRIMARY KEY CLUSTERED 
(
	[PINC_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'PINC_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'CARD_NO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ������� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'NAME_ARB'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ������� ���������� ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'NAME_ENG'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'BIRTHDATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'BAGE_NO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'C_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'GENDER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'NAL_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'PHONE_NO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ��� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'CONST_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����� ������ �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'EXP_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'NOTES'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� ������ : ����=1 ����=0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'P_STATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����� ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'NAT_NUMBER'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� �����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'KID_NO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'CITY_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'IMAGE_CARD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� �������� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����� �������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'USER_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��� ���� ��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'INC_PATIANT', @level2type=N'COLUMN',@level2name=N'USER_IP'
GO


