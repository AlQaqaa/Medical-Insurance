USE [DB_A41508_ibn]
GO

/****** Object:  StoredProcedure [dbo].[INC_addServicesPrice]    Script Date: 12/24/2019 8:48:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INC_addServicesPrice] 
	@service_id int,
	@private_prc decimal(18, 3),
	@inc_prc decimal(18, 3),
	@inv_prc decimal(18, 3),
	@user_id int,
	@user_ip nvarchar(50),
	@profile_price_id int
AS
BEGIN
	INSERT INTO INC_SERVICES_PRICES ([SER_ID],[CASH_PRS],[INS_PRS],[INVO_PRS],[USER_ID],[USER_IP],PROFILE_PRICE_ID) VALUES (@service_id, @private_prc, @inc_prc, @inv_prc, @user_id, @user_ip, @profile_price_id)
END
GO


