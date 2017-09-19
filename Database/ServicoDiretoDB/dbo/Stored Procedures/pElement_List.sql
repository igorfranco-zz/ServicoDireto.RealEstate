CREATE procedure [dbo].[pElement_List]
(
	@IDCulture varchar(5),
	@IDCustomer int = null
)
as
begin 
	select 
		e.*,
		ec.Name,		
		c.Name as CityName,
		sp.Name as StateProvinceName,
		co.Name as CountryName,
		2.0 as TotalArea,
		2 as Beds ,
		2 as Bathrooms ,
		2 as Garages ,
		2.0 as Price 						
		
	from dbo.Element as e
	inner join dbo.ElementCulture as ec on (e.IDElement = ec.IDElement and ec.IDCulture = @IDCulture)
	inner join City as c on (e.IDCity = c.IDCity)
	inner join dbo.StateProvince AS sp ON (c.IDStateProvince = sp.IDStateProvince)	
	inner join dbo.Country AS Co ON (sp.IDCountry = co.IDCountry)		
	where (e.IDCustomer = @IDCustomer or IDCustomer is null)
		
	
end
