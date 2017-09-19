CREATE procedure [dbo].[pCustomer_GetById] 
(	
	@IDCustomer int
)
as begin
	select 
		c.*,
		co.Name as CountryName,
        st.Name as StateProvinceName,
        ci.Name as CityName           
	from Customer c with(nolock)
		left join country co on (c.IDCountry = co.IDCountry)
		left join StateProvince st on (c.IDStateProvince = st.IDStateProvince)
		left join City ci on (c.IDCity = ci.IDCity)
	where c.IDCustomer = @IDCustomer
end
