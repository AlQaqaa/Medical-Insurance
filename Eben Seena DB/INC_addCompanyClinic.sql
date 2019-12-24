USE [IBNSINAMAIN]
GO

/****** Object:  StoredProcedure [dbo].[INC_addCompanyClinic]    Script Date: 12/17/2019 6:23:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INC_addCompanyClinic]
(
@cID int,
@clinicID int, 
@contractNo int,
@maxClinicValue money,
@clinicPersonPer int,
@sessionCount int,
@group_no int,
@userId int,
@userIP nvarchar(100)
)
AS
BEGIN TRY
	BEGIN TRANSACTION

		INSERT INTO  [dbo].[INC_CLINICAL_RESTRICTIONS](C_id,clinic_id,contract_no,max_value,per_t,SESSION_COUNT,GROUP_NO,USER_ID,USER_IP) VALUES (@cID,@clinicID,@contractNo,@maxClinicValue,@clinicPersonPer,@sessionCount,@group_no,@userId,@userIp)

	COMMIT TRANSACTION
END TRY

BEGIN CATCH

	if @@ERROR <> 0
		ROLLBACK TRANSACTION;

END CATCH
GO


