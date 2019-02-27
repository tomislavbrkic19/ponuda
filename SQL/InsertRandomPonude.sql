declare @id int 
select @id = 1
while @id >=1 and @id <= 9996
begin
    insert into Ponude values( CONVERT( DECIMAL(18, 2), 10 + (100-10)*RAND(CHECKSUM(NEWID()))),DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 365 ), '2018-02-20') )
    select @id = @id + 1
end

