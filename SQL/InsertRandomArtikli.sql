declare @id int 
select @id = 1
while @id >=1 and @id <= 100
begin
    insert into Artikli values( SUBSTRING(CONVERT(varchar(255), NEWID()),0,255),  ROUND(RAND(CHECKSUM(NEWID())) * (100), 2))
    select @id = @id + 1
end

