CREATE PROCEDURE [dbo].[ContactAddOrEdit]
	@mode nvarchar(10),
	@Id int,
	@rida int,
	@koht int

AS
	if @mode ='Add'
	Begin
	Insert into piletidTable(Id,rida,koht)VALUES(@Id,@rida,@koht)
	END