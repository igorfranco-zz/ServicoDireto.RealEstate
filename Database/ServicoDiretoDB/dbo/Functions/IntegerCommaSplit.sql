CREATE function [dbo].[IntegerCommaSplit](@ListofIds nvarchar(1000))
returns @rtn table (IntegerValue int)
AS
	begin
	While (Charindex(',',@ListofIds)>0)
	Begin
		Insert Into @Rtn 
		Select ltrim(rtrim(Substring(@ListofIds,1,Charindex(',',@ListofIds)-1)))
		Set @ListofIds = Substring(@ListofIds,Charindex(',',@ListofIds)+len(','),len(@ListofIds))
	end
	Insert Into @Rtn 
		Select  ltrim(rtrim(@ListofIds))
	return 
end
