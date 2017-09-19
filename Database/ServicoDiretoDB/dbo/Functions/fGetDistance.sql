/*
Polygon Coordinates:
select dbo.fGetDistance(-30.038076,-51.118856,-30.0381,-51.1356)
select dbo.fGetDistance(-30.030847,-51.121093,-30.0381,-51.1356)
select dbo.fGetDistance(-30.025555,-51.127206,-30.0381,-51.1356)
select dbo.fGetDistance(-30.023618,-51.135557,-30.0381,-51.1356)
select dbo.fGetDistance(-30.025555,-51.143908,-30.0381,-51.1356)
select dbo.fGetDistance(-30.030847,-51.150021,-30.0381,-51.1356)
select dbo.fGetDistance(-30.038076,-51.152259,-30.0381,-51.1356)
select dbo.fGetDistance(-30.045305,-51.150021,-30.0381,-51.1356)
select dbo.fGetDistance(-30.050597,-51.143908,-30.0381,-51.1356)
select dbo.fGetDistance(-30.052534,-51.135557,-30.0381,-51.1356)
select dbo.fGetDistance(-30.050597,-51.127206,-30.0381,-51.1356)
select dbo.fGetDistance(-30.045305,-51.121093,-30.0381,-51.1356)
select dbo.fGetDistance(-30.038076,-51.118856,-30.0381,-51.1356)
Radius Information
Miles:1
Meters: 1609
Center: ,-30.0381,-51.1356


*/

--http://www.sqlteam.com/forums/topic.asp?TOPIC_ID=81360
CREATE FUNCTION [dbo].[fGetDistance]
	(
	@Latitude1  float,
	@Longitude1 float,
	@Latitude2  float,
	@Longitude2 float
	)
returns float
as
/*
http://www.sqlteam.com/forums/topic.asp?TOPIC_ID=81360
fUNCTION: fGetDistance - fGetDistance

	Computes the Great Circle distance in kilometers
	between two points on the Earth using the
	Haversine formula distance calculation.

Input Parameters:
	@Longitude1 - Longitude in degrees of point 1
	@Latitude1  - Latitude  in degrees of point 1
	@Longitude2 - Longitude in degrees of point 2
	@Latitude2  - Latitude  in degrees of point 2
Output:
	Result in KM
*/
begin
declare @radius float

declare @lon1  float
declare @lon2  float
declare @lat1  float
declare @lat2  float

declare @a float
declare @distance float

-- Sets average radius of Earth in Kilometers
set @radius = 6371.0E

-- Convert degrees to radians
set @lon1 = radians( @Longitude1 )
set @lon2 = radians( @Longitude2 )
set @lat1 = radians( @Latitude1 )
set @lat2 = radians( @Latitude2 )

set @a = sqrt(square(sin((@lat2-@lat1)/2.0E)) + (cos(@lat1) * cos(@lat2) * square(sin((@lon2-@lon1)/2.0E))) )

set @distance =	@radius * ( 2.0E *asin(case when 1.0E < @a then 1.0E else @a end ))

return @distance

end
