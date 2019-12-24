USE [DB_A41508_ibn]
GO

/****** Object:  StoredProcedure [dbo].[INC_editCompany]    Script Date: 12/24/2019 6:59:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INC_editCompany]
(
@cId int,
@cNameAr nvarchar(250),
@cNameEn nvarchar(250),
@cState bit,
@startDt date,
@endDt date,
@maxVal decimal(18, 3),
@maxCard decimal(18, 3),
@paymentType int,
@contractType int,
@profile_price_id int,
@patiaintPer bit,
@userId int,
@userIp nvarchar(50)
)
As
BEGIN TRY
	BEGIN TRANSACTION
		
		update dbo.inc_COMPANY_DATA  set C_NAME_ARB=@cNameAr,C_NAME_ENG=@cNameEn,C_STATE=@cState,USER_ID=@userId,USER_IP=@userIp where c_id=@cId

		update [dbo].inc_COMPANY_DETIAL  set DATE_START=@startDt,DATE_END=@endDt,MAX_VAL=@maxVal,MAX_CARD=@maxCard,PYMENT_TYPE=@paymentType,CONTRCT_TYPE=@contractType,PATIAINT_PER=@patiaintPer,PROFILE_PRICE_ID=@profile_price_id,USER_ID=@userId,USER_IP=@userIp where c_id=@cId
	COMMIT TRANSACTION
END TRY

BEGIN CATCH

	if @@ERROR <> 0
		ROLLBACK TRANSACTION;

END CATCH
GO


