USE [IBNSINAMAIN]
GO

/****** Object:  StoredProcedure [dbo].[INC_newClinicGroup]    Script Date: 12/17/2019 6:26:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INC_newClinicGroup] 
(
@maxVal money,
@personPer int,
@groupNo int out
)
AS
BEGIN

		INSERT INTO [dbo].[INC_CLINIC_GROUP]([max_value],[per_t]) VALUES (@maxVal,@personPer) SET @groupNo=SCOPE_IDENTITY()

		return @groupNo
END
GO


