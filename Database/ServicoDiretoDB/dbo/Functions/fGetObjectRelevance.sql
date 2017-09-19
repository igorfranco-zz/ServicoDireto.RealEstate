-- Batch submitted through debugger: SQLQuery3.sql|7|0|C:\Users\Administrator\AppData\Local\Temp\~vsFAF1.sql
-- =============================================
-- Author:		Igor Franco Brum
-- Create date: 07/04/2010
-- Description:	Retorna a relevancia do objeto baseado nos filtros informados
-- =============================================
CREATE FUNCTION [dbo].[fGetObjectRelevance]
(
	@ObjectID int,
	@Filter xml
)
RETURNS float
AS
BEGIN

declare @Relevance		   float
declare @RelevanceCategory int
declare @RelevanceType	   int
declare @RelevancePurpose  int
declare @RelevancePrice    int
declare @RelevanceRoom     int
declare @RelevanceArea     int
--declare @RelevanceAttribute    int
declare @countfilterAttribute int


set @RelevanceCategory = 0
set @RelevanceType	   = 0
set @RelevancePrice    = 0
set @RelevanceRoom     = 0
set @RelevanceArea     = 0
set @RelevancePurpose  = 0
--set @RelevanceAttribute  = 0
set @countfilterAttribute  = 0


declare @tableFilter table( 
							PurposeID int,
							ObjectTypeID int, 
							ObjectCategoryID int,
							RangePriceStart float,
							RangePriceEnd float,
							RoomQty float,
							MinimumArea float,
							Accuracy int)
							
declare @tableFilterAttribute table( AttributeID int)

--set @filter = 
--'<Filter xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
--  <AttributeIDs>
--    <string>1</string>
--    <string>2</string>
--    <string>3</string>
--  </AttributeIDs>
--  <ObjectTypeID>17</ObjectTypeID>
--  <ObjectCategoryID>24</ObjectCategoryID>
--  <RangePriceStart>1000</RangePriceStart>
--  <RangePriceEnd>9000</RangePriceEnd>
--  <RoomQty>3</RoomQty>
--  <MinimumArea>15</MinimumArea>
--</Filter>'

insert into @tableFilterAttribute values(1)
insert into @tableFilterAttribute values(2)
insert into @tableFilterAttribute values(3)

insert into  @tableFilter
select 
	Filter.ID.query('PurposeID').value('.', 'int')	 AS PurposeID,
	Filter.ID.query('ObjectTypeID').value('.', 'int')	 AS ObjectTypeID,
	Filter.ID.query('ObjectCategoryID').value('.', 'int')AS ObjectCategoryID,
	Filter.ID.query('RangePriceStart').value('.', 'float')	 AS RangePriceStart,
	Filter.ID.query('RangePriceEnd').value('.', 'float')	 AS RangePriceEnd,
	Filter.ID.query('RoomQty').value('.', 'float')		 AS RoomQty,
	Filter.ID.query('MinimumArea').value('.', 'float')	 AS MinimumArea,
	Filter.ID.query('Accuracy').value('.', 'int')	 AS Accuracy
from @filter.nodes('/Filter') as Filter(ID) 


--caso a quantidade de attributos relacionados ao objeto tenha o que foi informado ao filtro teremos 
--100% de relevancia
--select @countfilterAttribute = COUNT(*) from @tableFilterAttribute
--select 
--	@RelevanceAttribute = case 
--		when COUNT(*) = @countfilterAttribute then 100
--		when COUNT(*) between  dbo.fAddPercent(@countfilterAttribute,-10) and dbo.fAddPercent(@countfilterAttribute,10) then 90
--		when COUNT(*) between  dbo.fAddPercent(@countfilterAttribute,-20) and dbo.fAddPercent(@countfilterAttribute,20) then 80
--		when COUNT(*) between  dbo.fAddPercent(@countfilterAttribute,-30) and dbo.fAddPercent(@countfilterAttribute,30) then 70
--		when COUNT(*) between  dbo.fAddPercent(@countfilterAttribute,-40) and dbo.fAddPercent(@countfilterAttribute,40) then 60
--		when COUNT(*) between  dbo.fAddPercent(@countfilterAttribute,-50) and dbo.fAddPercent(@countfilterAttribute,50) then 50
--		when COUNT(*) between  dbo.fAddPercent(@countfilterAttribute,-60) and dbo.fAddPercent(@countfilterAttribute,60) then 40
--		when COUNT(*) between  dbo.fAddPercent(@countfilterAttribute,-70) and dbo.fAddPercent(@countfilterAttribute,70) then 30
--		when COUNT(*) between  dbo.fAddPercent(@countfilterAttribute,-80) and dbo.fAddPercent(@countfilterAttribute,80) then 20
--		when COUNT(*) between  dbo.fAddPercent(@countfilterAttribute,-90) and dbo.fAddPercent(@countfilterAttribute,90) then 10
--		else 0
--	end --as RelevanceAttribute
--from ObjectAttribute as OA
--where OA.AttributeID IN (select AttributeID from @tableFilterAttribute)
--and OA.ObjectID = @ObjectID

if(exists(select * from @tableFilter as tf where tf.PurposeID = -1 ))
begin
	set @RelevancePurpose = 100
end 
else 
begin
	select 
		@RelevancePurpose = case 
			when tf.PurposeID is not null then 100
			else 0
		end
	from [Object] as O 
	inner join @tableFilter as tf on (tf.PurposeID = O.PurposeID )
	where O.ObjectID = @ObjectID
end

if(exists(select * from @tableFilter as tf where tf.ObjectCategoryID = -1 ))
begin
	set @RelevanceCategory = 100
end 
else 
begin
	select 
		@RelevanceCategory = case 
			when tf.ObjectCategoryID is not null then 100
			else 0
		end
	from [Object] as O 
	left join @tableFilter as tf on (tf.ObjectCategoryID = O.ObjectCategoryID )
	where O.ObjectID = @ObjectID

	if(exists(select * from @tableFilter as tf where tf.ObjectCategoryID = -1 ))
		set @RelevanceCategory = 100
end

------
if(exists(select * from @tableFilter as tf where tf.ObjectTypeID = -1))
begin
	set @RelevanceCategory = 100
	set @RelevanceType = 100
end 
else 
begin
	--OK--Caso o valor de categoria for igual ao do objeto em questão a relevancia sera de 100% caso contrário 0%,
	--a mesma regra se aplica ao ObjectType
	select 
		@RelevanceType = case 
			when tf.ObjectTypeID is not null then 100
			else 0
		end
	from [Object] as O 
	left join @tableFilter as tf on (tf.ObjectTypeID = O.ObjectTypeID)
	where O.ObjectID = @ObjectID

	if(exists(select * from @tableFilter as tf where tf.ObjectTypeID = -1))
		set @RelevanceType = 100
end

------


if(exists(select * from @tableFilter as tf where tf.RangePriceStart = -1 and tf.RangePriceEnd = -1))
begin
	set @RelevancePrice = 100
end
else 
begin

	--OK--Verificacao do range de preco, 
	--quando o o valor do objeto estive entre o range informado de valores nos filtro a relevancia eh de 100%,
	----caso o preco do objeto estive entre o preco inicial - 10 porcento e o preco final + 10 a relevancia sera de 80% e asism por diante
	select 
		@RelevancePrice = case 
			when convert( float, isnull(OA.Value,0)) between tf.RangePriceStart and tf.RangePriceEnd then 100
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.RangePriceStart,-10) and dbo.fAddPercent(tf.RangePriceEnd, 10) then 90
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.RangePriceStart,-20) and dbo.fAddPercent(tf.RangePriceEnd, 20) then 80
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.RangePriceStart,-30) and dbo.fAddPercent(tf.RangePriceEnd, 30) then 70
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.RangePriceStart,-40) and dbo.fAddPercent(tf.RangePriceEnd, 40) then 60
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.RangePriceStart,-50) and dbo.fAddPercent(tf.RangePriceEnd, 50) then 50
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.RangePriceStart,-60) and dbo.fAddPercent(tf.RangePriceEnd, 60) then 40
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.RangePriceStart,-70) and dbo.fAddPercent(tf.RangePriceEnd, 70) then 30
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.RangePriceStart,-80) and dbo.fAddPercent(tf.RangePriceEnd, 80) then 20	
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.RangePriceStart,-90) and dbo.fAddPercent(tf.RangePriceEnd, 90) then 10
			else 0
		end
	from ObjectAttribute as OA
	inner join Attribute as A on (OA.AttributeID = A.AttributeID)
	inner join @tableFilter as tf on (1=1)
	where OA.ObjectID = @ObjectID and A.Name = 'Valor do Imóvel' 
end

if(exists(select * from @tableFilter as tf where tf.RoomQty = -1))
begin
	set @RelevanceRoom = 100
end
else 
begin
	----OK--Verificacao do numero de quartos, caso o valor informado nos filtros for igual ao do registro do objeto
	----a relevancia é de 100%, caso o valor informado nos filtros for -1 ou +1 ao do registro do objeto
	----a relevancia é de 80 e assim por diante
	select 
		@RelevanceRoom = 
		case 
			when convert(float, isnull(OA.Value,0)) = tf.RoomQty then 100
			when convert(float, isnull(OA.Value,0)) = tf.RoomQty - 1 or convert(float, isnull(OA.Value,0)) = tf.RoomQty + 1 then 90
			when convert(float, isnull(OA.Value,0)) = tf.RoomQty - 2 or convert(float, isnull(OA.Value,0)) = tf.RoomQty + 2 then 80
			when convert(float, isnull(OA.Value,0)) = tf.RoomQty - 3 or convert(float, isnull(OA.Value,0)) = tf.RoomQty + 3 then 70
			when convert(float, isnull(OA.Value,0)) = tf.RoomQty - 4 or convert(float, isnull(OA.Value,0)) = tf.RoomQty + 4 then 60
			when convert(float, isnull(OA.Value,0)) = tf.RoomQty - 5 or convert(float, isnull(OA.Value,0)) = tf.RoomQty + 5 then 50
			when convert(float, isnull(OA.Value,0)) = tf.RoomQty - 6 or convert(float, isnull(OA.Value,0)) = tf.RoomQty + 6 then 40
			when convert(float, isnull(OA.Value,0)) = tf.RoomQty - 7 or convert(float, isnull(OA.Value,0)) = tf.RoomQty + 7 then 30
			when convert(float, isnull(OA.Value,0)) = tf.RoomQty - 8 or convert(float, isnull(OA.Value,0)) = tf.RoomQty + 8 then 20
			when convert(float, isnull(OA.Value,0)) = tf.RoomQty - 9 or convert(float, isnull(OA.Value,0)) = tf.RoomQty + 9 then 10
			else 0
		end
	from ObjectAttribute as OA
	inner join Attribute as A on (OA.AttributeID = A.AttributeID)
	inner join @tableFilter as tf on (1=1)
	where OA.ObjectID = @ObjectID and A.Name = 'Dormitórios' 
end

if(exists(select * from @tableFilter as tf where tf.MinimumArea = -1))
begin
	set @RelevanceArea = 100
end
else 
begin
	----OK--caso o valor da area total do objeto for igual ao valor da area minima informado no filtro a relevância será de 100%,
	----caso o valor da area total do objeto estiver entre o valor da area minima informado no filtro -10 porcento e 
	---- o valor da area minima informado no filtro +10 porcento a relevancia será de 90% e assim por diante
	select 
		@RelevanceArea = 
		case 
			when convert(float, isnull(OA.Value,0)) = tf.MinimumArea then 100
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.MinimumArea,-10) and dbo.fAddPercent(tf.MinimumArea, 10) then 90
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.MinimumArea,-20) and dbo.fAddPercent(tf.MinimumArea, 20) then 80
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.MinimumArea,-30) and dbo.fAddPercent(tf.MinimumArea, 30) then 70
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.MinimumArea,-40) and dbo.fAddPercent(tf.MinimumArea, 40) then 60
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.MinimumArea,-50) and dbo.fAddPercent(tf.MinimumArea, 50) then 50
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.MinimumArea,-60) and dbo.fAddPercent(tf.MinimumArea, 60) then 40
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.MinimumArea,-70) and dbo.fAddPercent(tf.MinimumArea, 70) then 30
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.MinimumArea,-80) and dbo.fAddPercent(tf.MinimumArea, 80) then 20
			when convert( float, isnull(OA.Value,0)) between dbo.fAddPercent(tf.MinimumArea,-90) and dbo.fAddPercent(tf.MinimumArea, 90) then 10
			else 0
		end
	from ObjectAttribute as OA
	inner join Attribute as A on (OA.AttributeID = A.AttributeID)
	inner join @tableFilter as tf on (1=1)
	where OA.ObjectID = @ObjectID and A.Name = 'Área Total' 
	
end
--RETURN (@RelevanceCategory + @RelevanceType + @RelevancePrice + @RelevanceRoom + @RelevanceArea + @RelevanceAttribute) / 6 -- numero de relevancias
SET @Relevance = (@RelevanceCategory + @RelevanceType + @RelevancePrice + @RelevanceRoom + @RelevanceArea + @RelevancePurpose) / 6 -- numero de relevancias

declare @accuracy int

select @accuracy = tf.Accuracy  from @tableFilter as tf 

if( @accuracy <> -1 )
begin 
	if(@accuracy = 1 and not @Relevance <= 10)
		set @Relevance = -1
	else if(@accuracy = 2 and not (@Relevance >= 10 and @Relevance <= 20))
			set @Relevance = -1
	else if(@accuracy = 3 and not (@Relevance >= 20 and @Relevance <= 30))
			set @Relevance = -1
	else if(@accuracy = 4 and not (@Relevance >= 30 and @Relevance <= 40))
			set @Relevance = -1
	else if(@accuracy = 5 and not (@Relevance >= 40 and @Relevance <= 50))
			set @Relevance = -1
	else if(@accuracy = 6 and not (@Relevance >= 50 and @Relevance <= 60))
			set @Relevance = -1
	else if(@accuracy = 7 and not (@Relevance >= 60 and @Relevance <= 70))
			set @Relevance = -1
	else if(@accuracy = 8 and not (@Relevance >= 70 and @Relevance <= 80))
			set @Relevance = -1
	else if(@accuracy = 9 and not (@Relevance >= 80 and @Relevance <= 90))
			set @Relevance = -1
	else if(@accuracy = 10 and not (@Relevance = 100))
			set @Relevance = -1
		
end

RETURN ISNULL(@Relevance, 0)


END
