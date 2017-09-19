CREATE function [dbo].[fGetPositionTable]
(
  @points xml
)
returns @result table(Latitude float, Longitude float)
as
--Description: convert a xml with postion to a structured table
--Parameters: @points : xml content containing all postion 
begin 
	insert into @result
	select 
		Position.ID.query('latitude').value('.', 'float') AS Latitude,
		Position.ID.query('longitude').value('.', 'float') AS Longitude
	from @points.nodes('/points/point') as Position(ID) 
	return 
end
