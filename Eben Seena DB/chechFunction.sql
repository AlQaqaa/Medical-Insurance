USE [DB_A41508_ibn]
GO

/****** Object:  UserDefinedFunction [dbo].[chechFunction]    Script Date: 2/3/2020 12:35:31 PM ******/
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
	result_msg nvarchar(250),
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
		INSERT INTO @result_tbl VALUES(1,'Ì„ﬂ‰  ﬁœÌ„ Â–Â «·Œœ„…',@service_price_cash,@service_price_cash,0)
		RETURN
	end
	--#########################################

	-- «· Õﬁﬁ „‰ «·„ÊŸ› ####################
	declare @patient_sts int;
	set @patient_sts = isnull((select [P_STATE] from [dbo].[INC_PATIANT] where [PINC_ID] = @patient_id and [P_STATE] = 0 and [EXP_DATE] > getdate()),1)
	-- ›Ì Õ«·… ≈–« ﬂ«‰ «·„‰ ›⁄ „ÊﬁÊ› √Ê »ÿ«ﬁ Â „‰ ÂÌ… «·’·«ÕÌ…
	if (@patient_sts = 1)
	begin
		INSERT INTO @result_tbl VALUES(0,'Â–« «·„‰ ›⁄ „ÊﬁÊ›',@service_price_cash,@service_price_cash,0)
		RETURN
	end
	-- ##################################################

	-- «· Õﬁﬁ „‰ «·‘—ﬂ… ###############################
	declare @company_id int;
	set @company_id = (select c_id from [INC_PATIANT] where PINC_ID = @patient_id);

	if((select [C_STATE] from [dbo].[INC_COMPANY_DATA] where C_id = @company_id) = 1)
	begin
		INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° «·‘—ﬂ… „ÊﬁÊ›…',@service_price_cash,@service_price_cash,0)
		RETURN
	end

	-- Ã·» —ﬁ„ «·⁄ﬁœ «·”«—Ì «·„›⁄Ê· ··‘—ﬂ…
	declare @contract_no int;
	set @contract_no = (isnull((select top (1) CONTRACT_NO from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and [DATE_START] < GETDATE() and [DATE_END] > GETDATE() order by n desc),0));

	if(@contract_no = 0)
	begin
		INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° ⁄ﬁœ «·‘—ﬂ… „‰ ÂÌ «·’·«ÕÌ…',@service_price_cash,@service_price_cash,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ «·⁄Ì«œ… ≈–« ﬂ«‰  „€ÿ«… √„ ·« ##########
	declare @clinic_no int;
	set @clinic_no = (select SubService_Clinic from Main_SubServices where SubService_ID = @subService_id);
	
	declare @clinic_sts int;
	set @clinic_sts = (select isnull((select clinic_id from INC_CLINICAL_RESTRICTIONS where C_ID = @company_id and CONTRACT_NO = @contract_no and CLINIC_ID = @clinic_no),0) as clinic_id from Main_Clinic where clinic_id = @clinic_no)

	if (@clinic_sts = 0)
	begin
		INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â–Â «·⁄Ì«œ… €Ì— „€ÿ«… ·Â–Â «·‘—ﬂ…',@service_price_cash,@service_price_cash,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ «·ﬁ”„ ≈–« ﬂ«‰  „€ÿ«… √„ ·« ##########
	declare @service_no int;
	set @service_no = (select SubService_Service_ID from [dbo].[Main_SubServices] where [SubService_ID] = @subService_id);

	if (isnull((select [Service_ID] from INC_SERVICES_RESTRICTIONS where C_ID = @company_id and CONTRACT_NO = @contract_no),0) = 0)
	begin
		INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â–« «·ﬁ”„ €Ì— „€ÿ«… ·Â–Â «·‘—ﬂ…',@service_price_cash,@service_price_cash,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ «·Œœ„… ≈–« ﬂ«‰  „€ÿ«… √„ ·« ##########
	if (isnull((select [SER_STATE] from [dbo].[INC_SUB_SERVICES_RESTRICTIONS] where C_ID = @company_id and CONTRACT_NO = @contract_no),1) = 1)
	begin
		INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â– «·Œœ„… €Ì— „€ÿ«… ·Â–Â «·‘—ﬂ…',@service_price_cash,@service_price_cash,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ «·Œœ„… ≈–« ﬂ«‰  „ÕŸÊ—… ⁄‰ «·„‰ ›⁄ √„ ·« ##########
	if (isnull((select top(1) N from [dbo].[INC_BLOCK_SERVICES] where [OBJECT_ID] = @patient_id and [SER_ID] = @subService_id and BLOCK_TP = 1 order by n desc),0) <> 0)
	begin
		INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â–Â «·Œœ„…  „ ≈Ìﬁ«›Â« ⁄‰ Â–« «·„‰ ›⁄',@service_price_cash,@service_price_cash,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ «·ÿ»Ì» ≈–« ﬂ«‰ „ÕŸÊ— ⁄‰ «·‘—ﬂ… √„ ·« ##########
	if(@doctor_id <> 0)
	begin
		if (isnull((select top(1) N from [dbo].[INC_BLOCK_SERVICES] where [OBJECT_ID] = @company_id and [SER_ID] = @doctor_id and BLOCK_TP = 2 order by n desc),0) <> 0)
		begin
			INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â–« «·ÿ»Ì» „ÊﬁÊ› ⁄‰ Â–Â «·‘—ﬂ…',@service_price_cash,@service_price_cash,0)
			RETURN
		end
	end
	-- ####################################################

	-- Ã·» »Ì«‰«  «·œ›⁄ ##########
	declare @payment_type int
	set @payment_type = (select isnull([PYMENT_TYPE], 0) as PYMENT_TYPE from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no)
	declare @profile_price decimal(18, 0)
	set @profile_price = (select isnull([PROFILE_PRICE_ID], 0) as PROFILE_PRICE_ID from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no)
	-- ####################################################

	-- Ã·» ”⁄— «·Œœ„… Õ”» »Ì«‰«  œ›⁄ «·‘—ﬂ… ##########
	if(@profile_price <> 0)
	begin
		declare @service_price_company numeric(18, 3);
		if(@payment_type = 2)
		begin
			set @service_price_company = (select [INS_PRS] from INC_SERVICES_PRICES where SER_ID = @subService_id and PROFILE_PRICE_ID = @profile_price);
		end
		if(@payment_type = 3)
		begin
			set @service_price_company = (select [INVO_PRS] from INC_SERVICES_PRICES where SER_ID = @subService_id and PROFILE_PRICE_ID = @profile_price);
		end
		else
		begin
			set @service_price_company = (select [CASH_PRS] from INC_SERVICES_PRICES where SER_ID = @subService_id and PROFILE_PRICE_ID = @profile_price);
		end
	end
	else
	begin
		INSERT INTO @result_tbl VALUES(1,'Ì„ﬂ‰  ﬁœÌ„ Â–Â «·Œœ„…',@service_price_cash,@service_price_cash,0)
		RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ ”ﬁ› «·‘—ﬂ… Â· Ìﬂ›Ì · ﬁœÌ„ «·Œœ„… √Ê ·« ##########
	declare @company_balance decimal(18, 3)
	set @company_balance = (select [MAX_VAL] from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no) - (select * from INC_totalCompanyExpenses(@company_id,(select DATE_START from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no),(select DATE_END from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no)))

	if(@company_balance < @service_price_company)
	begin
		INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° —’Ìœ «·‘—ﬂ… ·« Ìﬂ›Ì ·Â–Â «·Œœ„…',@service_price_cash,@service_price_cash,0)
			RETURN
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ ”ﬁ› «·„‰ ›⁄ Â· Ìﬂ›Ì · ﬁœÌ„ «·Œœ„… √Ê ·« ##########
	declare @patient_balance decimal(18, 3)
	set @patient_balance = (select [MAX_PERSON] from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no) - (select * from INC_totalPatientExpenses(@patient_id,(select DATE_START from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no),(select DATE_END from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no)))

	if(@patient_balance <> 0)
	begin
		if(@patient_balance < @service_price_company)
		begin
			INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â–« «·„‰ ›⁄  Ã«Ê“ «·”ﬁ› «·„Œ’’ ·Â',@service_price_cash,@service_price_cash,0)
				RETURN
		end
	end
	-- ####################################################

	-- «· Õﬁﬁ „‰ ”ﬁ› «·⁄«∆·… Â· Ìﬂ›Ì · ﬁœÌ„ «·Œœ„… √Ê ·« ##########
	declare @family_balance decimal(18, 3)
	set @family_balance = (select [MAX_CARD] from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no) - (select * from INC_totalFamilyExpenses(@patient_id,(select DATE_START from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no),(select DATE_END from [dbo].[INC_COMPANY_DETIAL] where C_ID = @company_id and CONTRACT_NO = @contract_no)))

	if(@family_balance <> 0)
	begin
		if(@family_balance < @service_price_company)
		begin
			INSERT INTO @result_tbl VALUES(0,'·« Ì„ﬂ‰  ﬁœÌ„ «·Œœ„…° Â–Â «·⁄«∆·…  Ã«Ê“  «·”ﬁ› «·„Œ’’ ·Â«',@service_price_cash,@service_price_cash,0)
				RETURN
		end
	end
	-- ####################################################

	INSERT INTO @result_tbl VALUES(1,'Ì„ﬂ‰  ﬁœÌ„ Â–Â «·Œœ„… ·Â–« «·„‰ ›⁄',@service_price_company,50,300)
	RETURN 
END
GO


