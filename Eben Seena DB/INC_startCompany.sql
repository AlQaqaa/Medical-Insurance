USE [IBNSINAMAIN]
GO

/****** Object:  StoredProcedure [dbo].[INC_startCompany]    Script Date: 12/17/2019 6:26:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INC_startCompany]
(
@comID int
)
AS
BEGIN
	declare @com_level int
	SET @com_level = (SELECT C_LEVEL FROM [dbo].[INC_COMPANY_DATA] WHERE C_id = @comID)

	IF @com_level <> 0
	BEGIN
	-- Update one Company
		UPDATE [dbo].[INC_COMPANY_DATA] SET C_STATE = 0 WHERE C_id = @comID
	END
	ELSE
	BEGIN
	-- Update Main Company and All Companies Affiliates
		UPDATE[dbo].[INC_COMPANY_DATA] SET C_STATE = 0 WHERE C_LEVEL = @comID OR C_id = @comID
	END
END
GO


