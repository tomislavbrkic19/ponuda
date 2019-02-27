USE [testDB]
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


