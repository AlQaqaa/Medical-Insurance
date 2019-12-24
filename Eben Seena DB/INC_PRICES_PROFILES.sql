USE [DB_A41508_ibn]
GO

/****** Object:  Table [dbo].[INC_PRICES_PROFILES]    Script Date: 12/24/2019 7:57:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INC_PRICES_PROFILES](
	[profile_Id] [int] IDENTITY(1,1) NOT NULL,
	[profile_name] [nvarchar](250) NULL,
	[profile_dt] [date] NULL,
	[profile_sts] [bit] NULL,
	[is_default] [bit] NULL,
	[user_id] [int] NULL,
	[user_ip] [nvarchar](50) NULL,
 CONSTRAINT [PK_INC_PRICES_PROFILES] PRIMARY KEY CLUSTERED 
(
	[profile_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[INC_PRICES_PROFILES] ADD  CONSTRAINT [DF_INC_PRICES_PROFILES_profile_dt]  DEFAULT (getdate()) FOR [profile_dt]
GO

ALTER TABLE [dbo].[INC_PRICES_PROFILES] ADD  CONSTRAINT [DF_INC_PRICES_PROFILES_profile_sts]  DEFAULT ((0)) FOR [profile_sts]
GO


