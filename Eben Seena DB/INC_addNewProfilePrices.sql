USE [DB_A41508_ibn]
GO

/****** Object:  StoredProcedure [dbo].[INC_addNewProfilePrices]    Script Date: 12/24/2019 9:51:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INC_addNewProfilePrices]
(
@profile_name nvarchar(250),
@user_id int,
@user_ip nvarchar(50),
@is_default bit,
@p_id int out
)
AS
BEGIN
	
	IF @is_default = 1
	BEGIN
		UPDATE INC_PRICES_PROFILES SET is_default = 0 WHERE is_default = 1
	END

	INSERT INTO [dbo].[INC_PRICES_PROFILES] ([profile_name],[is_default],[user_id],[user_ip]) VALUES (@profile_name,@is_default,@user_id,@user_ip) SET @p_id=SCOPE_IDENTITY()
END
GO


