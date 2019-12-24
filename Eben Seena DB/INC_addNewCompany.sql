USE [DB_A41508_ibn]
GO

/****** Object:  StoredProcedure [dbo].[INC_addNewCompany]    Script Date: 12/24/2019 6:56:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INC_addNewCompany]
(
@cNameAr nvarchar(250),
@cNameEn nvarchar(250),
@cState bit,
@c_level int,
@startDt date,
@endDt date,
@maxVal decimal(18, 3),
@maxCard decimal(18, 3),
@paymentType int,
@contractType int,
@patiaintPer bit,
@bic_pic nvarchar(150),
@profile_price_id int,
@userId int,
@userIp nvarchar(50)
)
As
BEGIN TRY
	BEGIN TRANSACTION
		declare @companyID int
		SELECT @companyID = MAX(C_id) FROM [dbo].[INC_COMPANY_DATA] 
		SET @companyID = @companyID + 1

		INSERT INTO dbo.INC_COMPANY_DATA (C_id,C_NAME_ARB,C_NAME_ENG,C_STATE,C_LEVEL,BIC_LINK,USER_ID,USER_IP) VALUES (@companyID,@cNameAr,@cNameEn,@cState,@c_level,@bic_pic,@userId,@userIp)

		declare @con_no int
		SELECT @con_no = MAX(CONTRACT_NO) FROM [dbo].INC_COMPANY_DETIAL
		SET @con_no = @con_no + 1

		INSERT INTO [dbo].INC_COMPANY_DETIAL (C_ID,DATE_START,DATE_END,MAX_VAL,MAX_CARD,PYMENT_TYPE,CONTRCT_TYPE,PATIAINT_PER,CONTRACT_NO,PROFILE_PRICE_ID,USER_ID,USER_IP) VALUES (@companyID,@startDt,@endDt,@maxVal,@maxCard,@paymentType,@contractType,@patiaintPer,@con_no,@profile_price_id,@userId,@userIp)
	COMMIT TRANSACTION
END TRY

BEGIN CATCH

	if @@ERROR <> 0
		ROLLBACK TRANSACTION;

END CATCH
GO


