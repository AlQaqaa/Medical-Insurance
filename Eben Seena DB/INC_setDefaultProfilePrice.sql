CREATE PROCEDURE setDefaultProfilePrice
(
@profile_no int
)
AS
BEGIN
	UPDATE INC_PRICES_PROFILES SET is_default = 1 WHERE profile_Id = @profile_no;

	UPDATE INC_PRICES_PROFILES SET is_default = 0 WHERE profile_Id <> @profile_no;
END
GO
