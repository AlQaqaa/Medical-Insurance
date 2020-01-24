USE [DB_A41508_ibn]
GO

/****** Object:  StoredProcedure [dbo].[INC_addCompanyServices]    Script Date: 1/22/2020 6:27:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[INC_addCompanyServices]
@cID int,
@clinicID int, 
@serviceId int,
@contractNo int,
@maxServiceValue money,
@serviceSts bit,
@userId int,
@userIP nvarchar(100)
AS
BEGIN TRY
	BEGIN TRANSACTION

		INSERT INTO  INC_SERVICES_RESTRICTIONS(C_id,clinic_id,Service_ID,contract_no,max_value,service_sts,USER_ID,USER_IP) VALUES (@cID,@clinicID,@serviceId,@contractNo,@maxServiceValue,@serviceSts,@userId,@userIp)

	COMMIT TRANSACTION
END TRY

BEGIN CATCH

	if @@ERROR <> 0
		ROLLBACK TRANSACTION;

END CATCH
GO


