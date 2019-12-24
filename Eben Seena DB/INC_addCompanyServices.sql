USE [IBNSINAMAIN]
GO

/****** Object:  StoredProcedure [dbo].[INC_addCompanyServices]    Script Date: 12/17/2019 6:24:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INC_addCompanyServices]
(
@cID int,
@clinicID int, 
@contractNo int,
@serviceId int,
@serPersonPer int,
@serFamilyPer int,
@serParentPer int,
@serPersonMax money,
@serFamilyMax money,
@serState bit,
@serPaymentType int,
@userId int,
@userIP nvarchar(100)
)
As
BEGIN TRY
	BEGIN TRANSACTION

	declare @add_new int
	SET @add_new = (SELECT ISNULL(C_ID,0) AS C_ID FROM INC_SERVICES_RESTRICTIONS WHERE C_ID = @cID AND CONTRACT_NO = @contractNo AND SER_ID = @serviceId)

	if @add_new = 0 
		begin
			INSERT INTO  [dbo].[INC_SERVICES_RESTRICTIONS](c_id,clinic_id,ser_id,person_per,family_per,parent_per,max_person_val,max_family_val,ser_state,payment_type,contract_no,user_id,user_ip) VALUES (@cID,@clinicID,@serviceId,@serPersonPer,@serFamilyPer,@serParentPer,@serPersonMax,@serFamilyMax,@serState,@serPaymentType,@contractNo,@userId,@userIp)
		end
	else
		begin
			UPDATE  [dbo].[INC_SERVICES_RESTRICTIONS]SET person_per=@serPersonPer,family_per=@serFamilyPer,parent_per=@serParentPer,max_person_val=@serPersonMax,max_family_val=@serFamilyMax,ser_state=@serState,payment_type=@serPaymentType WHERE c_id=@cID AND clinic_id=@clinicID AND ser_id=@serviceId AND CONTRACT_NO = @contractNo
		end
	

		
	COMMIT TRANSACTION
END TRY

BEGIN CATCH

	if @@ERROR <> 0
		ROLLBACK TRANSACTION;

END CATCH
GO


