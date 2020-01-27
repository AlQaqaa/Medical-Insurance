USE [IBNSINAMAIN]
GO

/****** Object:  StoredProcedure [dbo].[INC_editPatiant]    Script Date: 12/17/2019 6:25:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INC_editPatiant]
(


    @PINC_ID int,
	@CARD_NO nvarchar(100) ,
	@NAME_ARB nvarchar(250) ,
	@NAME_ENG nvarchar(250) ,
	@BIRTHDATE date ,
	@BAGE_NO nvarchar(100) ,
	@C_ID int ,
	@GENDER nvarchar(10) ,
	@NAL_ID int ,
	@PHONE_NO int ,
	@CONST_ID int ,
	@EXP_DATE date ,
	@NOTES nvarchar(250) ,
	@NAT_NUMBER bigint ,
    @KID_NO int ,
	@CITY_ID int
)
As
BEGIN TRY
	BEGIN TRANSACTION
	
		UPDATE  dbo.INC_PATIANT  SET CARD_NO=@CARD_NO,NAME_ARB=@NAME_ARB,NAME_ENG=@NAME_ENG,BIRTHDATE=@BIRTHDATE,BAGE_NO=@BAGE_NO,C_ID=@C_ID,GENDER=@GENDER,NAL_ID=@NAL_ID,PHONE_NO=@PHONE_NO,CONST_ID=@CONST_ID,EXP_DATE=@EXP_DATE,NOTES=@NOTES,NAT_NUMBER=@NAT_NUMBER,KID_NO=@KID_NO,CITY_ID=@CITY_ID WHERE PINC_ID = @PINC_ID

		COMMIT TRANSACTION
END TRY

BEGIN CATCH

	if @@ERROR <> 0
		ROLLBACK TRANSACTION;

END CATCH
GO

