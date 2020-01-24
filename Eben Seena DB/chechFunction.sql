USE [DB_A41508_ibn]
GO

/****** Object:  UserDefinedFunction [dbo].[chechFunction]    Script Date: 1/22/2020 9:27:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[chechFunction] 
(
	@patient_id int,
	@subService_id int,
	@doctor_id int
)
RETURNS 
@result_tbl TABLE 
(
	ser_sts int,
	result_msg nvarchar(50),
	total_price numeric(18, 3),
	personal_price numeric(18, 3),
	company_price numeric(18, 3)
)
AS
BEGIN
	--Ã·» «·”⁄— «·‰ﬁœÌ ··Œœ„… #############
	declare @default_profile int;
	declare @service_price_cash numeric(18, 3);
	set @default_profile = (select profile_id from INC_PRICES_PROFILES where is_default = 1);
	set @service_price_cash = (select cash_prs from INC_SERVICES_PRICES where SER_ID = @subService_id and PROFILE_PRICE_ID = @default_profile);
	-- #######################################

	-- ›Ì Õ«· ≈–« ﬂ«‰ «·„—Ì÷ ·Ì” „‰ ÷„‰ «· √„Ì‰ «·ÿ»Ì
	if (@patient_id = 0)
	begin
		INSERT INTO @result_tbl VALUES(1,'Ì„ﬂ‰  ﬁœÌ„ Â–Â «·Œœ„…',@service_price_cash,0,0)
		RETURN
	end
	--#########################################

	-- «· Õﬁﬁ „‰ «·„ÊŸ› ####################
	declare @patient_sts int;
	set @patient_sts = isnull((select [P_STATE] from [dbo].[INC_PATIANT] where [PINC_ID] = @patient_id and [P_STATE] = 0 and [EXP_DATE] > getdate()),1)
	-- ›Ì Õ«·… ≈–« ﬂ«‰ «·„‰ ›⁄ „ÊﬁÊ› √Ê »ÿ«ﬁ Â „‰ ÂÌ… «·’·«ÕÌ…
	if (@patient_sts = 1)
	begin
		INSERT INTO @result_tbl VALUES(0,'Â–« «·„‰ ›⁄ „ÊﬁÊ›',0,0,0)
		RETURN
	end
	-- ##################################################

	-- «· Õﬁﬁ „‰ «·‘—ﬂ… ###############################
	declare @company_id int;
	set @company_id = (select c_id from [INC_PATIANT] where PINC_ID = @patient_id);

	if((select [C_STATE] from [dbo].[INC_COMPANY_DATA] where C_id = @company_id) = 1)
	begin
	INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° «·‘—ﬂ… „ÊﬁÊ›…',0,0,0)
		RETURN
	end

	-- Ã·» —ﬁ„ «·⁄ﬁœ «·”«—Ì «·„›⁄Ê· ··‘—ﬂ…
	declare @contract_no int;
	set @contract_no = (isnull((select top (1) CONTRACT_NO from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and [DATE_START] < GETDATE() and [DATE_END] > GETDATE() order by n desc),0));

	if(@contract_no = 0)
	begin
	INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° ⁄ﬁœ «·‘—ﬂ… „‰ ÂÌ «·’·«ÕÌ…',0,0,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ «·⁄Ì«œ… ≈–« ﬂ«‰  „€ÿ«… √„ ·« ##########
	declare @clinic_no int;
	set @clinic_no = (select SubService_Clinic from Main_SubServices where SubService_ID = @subService_id);

	if (isnull((select clinic_id from INC_CLINICAL_RESTRICTIONS where C_ID = @company_id and CONTRACT_NO = @contract_no),0) = 0)
	begin
	INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â–Â «·⁄Ì«œ… €Ì— „€ÿ«… ·Â–Â «·‘—ﬂ…',0,0,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ «·ﬁ”„ ≈–« ﬂ«‰  „€ÿ«… √„ ·« ##########
	declare @service_no int;
	set @service_no = (select SubService_Service_ID from [dbo].[Main_SubServices] where [SubService_ID] = @subService_id);

	if (isnull((select [Service_ID] from INC_SERVICES_RESTRICTIONS where C_ID = @company_id and CONTRACT_NO = @contract_no),0) = 0)
	begin
	INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â–« «·ﬁ”„ €Ì— „€ÿ«… ·Â–Â «·‘—ﬂ…',0,0,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ «·Œœ„… ≈–« ﬂ«‰  „€ÿ«… √„ ·« ##########
	if (isnull((select [SER_STATE] from [dbo].[INC_SUB_SERVICES_RESTRICTIONS] where C_ID = @company_id and CONTRACT_NO = @contract_no),1) = 1)
	begin
	INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â– «·Œœ„… €Ì— „€ÿ«… ·Â–Â «·‘—ﬂ…',0,0,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ «·Œœ„… ≈–« ﬂ«‰  „ÕŸÊ—… ⁄‰ «·„‰ ›⁄ √„ ·« ##########
	if (isnull((select top(1) N from [dbo].[INC_BLOCK_SERVICES] where [OBJECT_ID] = @patient_id and [SER_ID] = @subService_id and BLOCK_TP = 1 order by n desc),0) <> 0)
	begin
	INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â–Â «·Œœ„…  „ ≈Ìﬁ«›Â« ⁄‰ Â–« «·„‰ ›⁄',0,0,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ «·ÿ»Ì» ≈–« ﬂ«‰ „ÕŸÊ— ⁄‰ «·‘—ﬂ… √„ ·« ##########
	if(@doctor_id <> 0)
	begin
		if (isnull((select top(1) N from [dbo].[INC_BLOCK_SERVICES] where [OBJECT_ID] = @company_id and [SER_ID] = @doctor_id and BLOCK_TP = 2 order by n desc),0) <> 0)
		begin
		INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â–« «·ÿ»Ì» „ÊﬁÊ› ⁄‰ Â–Â «·‘—ﬂ…',0,0,0)
			RETURN
		end
	end
	-- ####################################################

	INSERT INTO @result_tbl VALUES(1,'Ì„ﬂ‰  ﬁœÌ„ Â–Â «·Œœ„… ·Â–« «·„‰ ›⁄',350,50,300)
	RETURN 
END
GO


