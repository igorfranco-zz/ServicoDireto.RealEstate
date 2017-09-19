CREATE function [dbo].[fGetAttributeValue]
(
	@IDElement	bigint,
	@Acronym varchar(5),
	@IDCulture	varchar(5) = 'pt-BR'
)
returns varchar(100)
begin 
	declare @result varchar(100)
	--select 
	--	@result = MAX(ea.Value)
	--from elementattribute ea
	--	inner join attribute a on (ea.idattribute = a.idattribute)
	--where ea.idelement = @IDElement 
	--and a.acronym = @Acronym
	
	--set @result = isnull(ltrim(rtrim( @result )), '0')
	--if(isnumeric(@result) = 0) 
	--	set @result = '0'		
	
	--SELECT SQL#.Math_FormatDecimal(123.456, N'C', N'pt-br');	
	SELECT @result = STUFF( (	select '/ ' +
									case 
										when @Acronym = 'VALT' then
											SQL#.Math_FormatDecimal(ea.Value, N'C', @IDCulture)
										else	
											ea.Value
										end 
								 from elementattribute ea
									inner join attribute a on (ea.idattribute = a.idattribute)
								 where ea.idelement = @IDElement 
								 and a.acronym = @Acronym 
								 and isnumeric(ea.Value) = 1
								 order by convert(decimal, ea.Value) asc
	                         FOR XML PATH('')), 1, 1, ' ')	
	
	return @result
end		

