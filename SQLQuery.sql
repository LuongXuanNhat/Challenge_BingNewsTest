select * from Article where Description is null
DELETE FROM Article WHERE Description IS NULL;

select * from Article where PubDate > GetDate();
DELETE FROM Article WHERE PubDate > GETDATE();

DELETE a
FROM Article a
INNER JOIN (
    SELECT  PubDate
    FROM Article
    GROUP BY  PubDate
    HAVING COUNT(*) > 1
) b ON  a.PubDate = b.PubDate ;

ALTER TABLE BingNews.dbo.Provider
ALTER COLUMN ChannelName nvarchar(255);

select * from Article order by PubDate desc;
SELECT * FROM Article WHERE CAST(PubDate AS DATE) = CAST(GETDATE() AS DATE) order by PubDate desc;
Go
CREATE PROCEDURE RemoveDuplicateArticles
AS
BEGIN
    ;WITH CTE AS (
        SELECT Title, PubDate,
               ROW_NUMBER() OVER(PARTITION BY Title, PubDate ORDER BY (SELECT 0)) AS RowNumber
        FROM Article
    )
    DELETE FROM CTE WHERE RowNumber > 1;
END
Go
CREATE PROCEDURE RemoveDuplicateProvider
AS
BEGIN
    ;WITH CTE AS (
        SELECT ChannelName,
               ROW_NUMBER() OVER(PARTITION BY ChannelName ORDER BY (SELECT 0)) AS RowNumber
        FROM Provider
    )
    DELETE FROM CTE WHERE RowNumber > 1;
END
Go
CREATE PROCEDURE InsertProviderIfNotExists
    @ChannelName NVARCHAR(255),
	@Id NVARCHAR(255)
AS
BEGIN
    -- Kiểm tra xem bản ghi đã tồn tại hay chưa
    IF NOT EXISTS (SELECT 1 FROM Provider WHERE ChannelName = @ChannelName)
    BEGIN
        -- Bản ghi không tồn tại, thêm vào cơ sở dữ liệu
        INSERT INTO Provider (ChannelName, Id) -- Thêm tất cả các cột cần thiết
        VALUES (@ChannelName, @Id); -- Thêm giá trị tương ứng với các cột
    END
END
Go
CREATE PROCEDURE RemoveDuplicateWeather
AS
BEGIN
    ;WITH CTE AS (
        SELECT Id, ChannelName,
               ROW_NUMBER() OVER(PARTITION BY Id, ChannelName ORDER BY (SELECT 0)) AS RowNumber
        FROM Provider
    )
    DELETE FROM CTE WHERE RowNumber > 1;
END



SELECT TOP (1000) [Id]
      ,[Title]
      ,[ImgUrl]
      ,[Description]
      ,[PubDate]
      ,[Url]
      ,[LikeNumber]
      ,[DisLikeNumber]
      ,[ViewNumber]
      ,[CommentNumber]
      ,[ProviderId]
      ,[TopicId]
  FROM [BingNews].[dbo].[Article]
  order by PubDate desc;

ALTER TABLE BingNews.dbo.AdArticle
DROP CONSTRAINT PK__AdArticl__3213E83F7776233C;
ALTER TABLE BingNews.dbo.AdArticle
ALTER COLUMN id uniqueidentifier

ALTER TABLE Article
ADD CONSTRAINT FK_Article_Provider
FOREIGN KEY (ProviderId) REFERENCES Provider(Id);

ALTER TABLE Article
Drop CONSTRAINT FK_Name; --FK_Article_Provider

ALTER TABLE BingNews.dbo.AdArticle
ALTER COLUMN id uniqueidentifier NOT NULL;
ALTER TABLE BingNews.dbo.AdArticle
ADD CONSTRAINT PK_Temp PRIMARY KEY (id);

ALTER TABLE BingNews.dbo.AdArticle
ALTER COLUMN PubDate Datetime

-- FTS
SELECT FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') 
AS [FULLTEXTSERVICE]

USE BingNews 
GO
CREATE FULLTEXT CATALOG SearchNews

USE BingNews
GO
CREATE UNIQUE INDEX ui_Search ON Article(Id);  

CREATE FULLTEXT INDEX ON dbo.Article 
(  
    Title Language 1066 ,       --1033 is the LCID for English - United States  
    Description LANGUAGE 1066
)  
KEY INDEX ui_Search ON SearchNews
WITH CHANGE_TRACKING AUTO          
GO  
--DROP FULLTEXT INDEX ON dbo.Article;
--DROP FULLTEXT CATALOG FTS_News;

 --CONTAINS , FREETEXT , CONTAINSTABLE và FREETEXTTABLE | there are 4 options
 -- 1. Contains
 EXEC sp_fulltext_catalog 'SearchNews', 'start_full'

SELECT * FROM dbo.Article
WHERE CONTAINS(Title,'"Tai nan"')

-- 2. Freetext
SELECT * FROM dbo.Article
WHERE FREETEXT(Title,'phát trien')

