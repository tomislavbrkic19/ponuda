CREATE DATABASE [testDB];
SET QUOTED_IDENTIFIER OFF;
GO
USE [testDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Stavke_Artikli1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stavke] DROP CONSTRAINT [FK_Stavke_Artikli1];
GO
IF OBJECT_ID(N'[dbo].[FK_Stavke_Ponude]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stavke] DROP CONSTRAINT [FK_Stavke_Ponude];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Artikli]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Artikli];
GO
IF OBJECT_ID(N'[dbo].[Ponude]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ponude];
GO
IF OBJECT_ID(N'[dbo].[Stavke]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stavke];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Artikli'
CREATE TABLE [dbo].[Artikli] (
    [ArtikalId] int IDENTITY(1,1) NOT NULL,
    [NazivArtikla] nvarchar(255)  NULL,
    [JedCijenaArtikla] decimal(18,2)  NULL
);
GO

-- Creating table 'Ponude'
CREATE TABLE [dbo].[Ponude] (
    [PonudaID] int IDENTITY(1,1) NOT NULL,
    [UkupnaCijena] decimal(18,2)  NULL,
    [DatumPonude] datetime  NULL,
    [StavkeId] int  NULL
);
GO

-- Creating table 'Stavke'
CREATE TABLE [dbo].[Stavke] (
    [StavkaId] int IDENTITY(1,1) NOT NULL,
    [PonudaId] int  NULL,
    [ArtikalId] int  NULL,
    [Kolicina] int  NULL,
    [UkupnaCijenaStavke] decimal(18,2)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ArtikalId] in table 'Artikli'
ALTER TABLE [dbo].[Artikli]
ADD CONSTRAINT [PK_Artikli]
    PRIMARY KEY CLUSTERED ([ArtikalId] ASC);
GO

-- Creating primary key on [PonudaID] in table 'Ponude'
ALTER TABLE [dbo].[Ponude]
ADD CONSTRAINT [PK_Ponude]
    PRIMARY KEY CLUSTERED ([PonudaID] ASC);
GO

-- Creating primary key on [StavkaId] in table 'Stavke'
ALTER TABLE [dbo].[Stavke]
ADD CONSTRAINT [PK_Stavke]
    PRIMARY KEY CLUSTERED ([StavkaId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ArtikalId] in table 'Stavke'
ALTER TABLE [dbo].[Stavke]
ADD CONSTRAINT [FK_Stavke_Artikli1]
    FOREIGN KEY ([ArtikalId])
    REFERENCES [dbo].[Artikli]
        ([ArtikalId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Stavke_Artikli1'
CREATE INDEX [IX_FK_Stavke_Artikli1]
ON [dbo].[Stavke]
    ([ArtikalId]);
GO

-- Creating foreign key on [PonudaId] in table 'Stavke'
ALTER TABLE [dbo].[Stavke]
ADD CONSTRAINT [FK_Stavke_Ponude]
    FOREIGN KEY ([PonudaId])
    REFERENCES [dbo].[Ponude]
        ([PonudaID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Stavke_Ponude'
CREATE INDEX [IX_FK_Stavke_Ponude]
ON [dbo].[Stavke]
    ([PonudaId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

declare @id int 
select @id = 1
while @id >=1 and @id <= 1000
begin
    insert into Artikli values( SUBSTRING(CONVERT(varchar(255), NEWID()),0,255),  ROUND(RAND(CHECKSUM(NEWID())) * (100), 2))
    select @id = @id + 1
end


select @id = 1
while @id >=1 and @id <= 10000
begin
    insert into Ponude values( CONVERT( DECIMAL(18, 2), 10 + (100-10)*RAND(CHECKSUM(NEWID()))),DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 365 ), '2018-02-20') ,1)
    select @id = @id + 1
end



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CalculateStavke] @PonudaId int
AS
BEGIN
UPDATE Ponude SET UkupnaCijena= (SELECT SUM(UkupnaCijenaStavke) FROM Stavke where PonudaId=@PonudaId)
  where PonudaId=@PonudaId
END
GO
