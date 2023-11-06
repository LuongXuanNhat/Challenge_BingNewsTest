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
      ,[ChannelName]
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
 -- Supported languages
 SELECT * 
FROM sys.fulltext_languages  
ORDER BY lcid
 -- 1. Contains
 EXEC sp_fulltext_catalog 'SearchNews', 'start_full'

SELECT * FROM dbo.Article
WHERE CONTAINS(Title,'"Tai nan"')

SELECT * FROM dbo.Article
WHERE CONTAINS(Title,'viet NEAR kiều')
-- 2. Freetext
SELECT * FROM dbo.Article
WHERE FREETEXT((Title, Description),'phat triển')

SELECT * FROM dbo.Article
WHERE FREETEXT((Title, Description),'vụ thảm sát đẫm máu')
-- 3. Containstable 
select * 
FROM CONTAINSTABLE(
    dbo.Article,
    Title,
    'hay'
)
ORDER BY RANK desc

-- 4. FREETEXTTABLE
select * 
FROM FREETEXTTABLE(
    dbo.Article,
    Title,
    'hay'
)
ORDER BY RANK desc
go
create table Users (
	Id uniqueidentifier Primary Key,
	UserName varchar(100),
	Email varchar(100)
);
go
Create table UserInteraction(
	Id int identity(1,1),
	UserId uniqueidentifier REFERENCES Users(Id),
	ArticleId uniqueidentifier REFERENCES Article(Id),
	Likes int default 0,
	Dislike int default 0
);
SET IDENTITY_INSERT UserInteraction ON;

ALTER TABLE BingNews.dbo.ChannelBlocked
ALTER COLUMN UserId uniqueidentifier

ALTER TABLE BingNews.dbo.TopicFollow
ALTER COLUMN UserId uniqueidentifier

ALTER TABLE BingNews.dbo.ChannelFollow
ALTER COLUMN UserId uniqueidentifier

Alter table Article
Add constraint ck_totalLike check (LikeNumber = (SELECT COUNT(*) FROM UserInteraction WHERE IdArticle = Id and Likes != 0 ));
-- replace update function
go
CREATE FUNCTION dbo.GetLikeCountForArticle(@ArticleId uniqueidentifier)
RETURNS INT
AS
BEGIN
    DECLARE @LikeCount INT;
    
    IF EXISTS (SELECT 1 FROM UserInteraction WHERE ArticleId = @ArticleId AND Likes <> 0)
    BEGIN
        SET @LikeCount = (SELECT COUNT(*) FROM UserInteraction WHERE ArticleId = @ArticleId AND Likes <> 0);
    END
    ELSE
    BEGIN
        SET @LikeCount = 0;
    END
    RETURN @LikeCount;
END;

--go
--drop function GetLikeCountForArticle
go
ALTER TABLE Article 
ADD CONSTRAINT CK_TotalLike CHECK (LikeNumber = dbo.GetLikeCountForArticle(Id));

-- xóa bản ghi thì phải gọi lại
UPDATE Article
SET LikeNumber = dbo.GetLikeCountForArticle(Id);
-- remove constraint
-- Tạo trigger INSTEAD OF INSERT
CREATE TRIGGER CheckAndInsertUserInteraction
ON UserInteraction
INSTEAD OF INSERT
AS
BEGIN
    -- Kiểm tra xem dữ liệu mới có trùng lặp không
    IF EXISTS (
        SELECT 1
        FROM UserInteraction u
        INNER JOIN inserted i ON u.ArticleId = i.ArticleId AND u.UserId = i.UserId
    )
    BEGIN
        -- Nếu trùng lặp, bạn có thể thực hiện cập nhật thay vì thêm mới ở đây
        UPDATE UserInteraction SET Likes = (CASE WHEN Likes = 1 THEN 0 ELSE 1 END), Dislike = (CASE WHEN Dislike = 1 THEN 0 ELSE 1 END)
    END
    ELSE
    BEGIN
        -- Nếu không trùng lặp, bạn có thể thực hiện thêm mới dòng dữ liệu
        INSERT INTO UserInteraction (UserId, ArticleId, Likes, Dislike)
        SELECT UserId, ArticleId, Likes, Dislike
        FROM inserted;
    END
END;




-- off constraint
ALTER TABLE Article NOCHECK CONSTRAINT CK_TotalLike;
select * from article where LikeNumber = 0

DELETE FROM Article WHERE LikeNumber != 0;

-- Check dupplication
go
CREATE TRIGGER CheckDuplicateUserInteraction
ON UserInteraction
AFTER INSERT
AS
BEGIN
    -- Lấy IdArticle và UserId của bản ghi mới được thêm vào
    DECLARE @NewArticleId uniqueidentifier;
    DECLARE @NewUserId uniqueidentifier;
	Declare @newLike int;
	Declare @newDislike int;

    SELECT @NewArticleId = ArticleId, @NewUserId = UserId, @newLike = Likes, @newDislike = Dislike
    FROM inserted;

    -- Xóa tất cả bản ghi trùng lặp
    DELETE FROM UserInteraction
    WHERE ArticleId = @NewArticleId AND UserId = @NewUserId;

    -- Thêm bản ghi mới
    INSERT INTO UserInteraction (UserId, ArticleId, Likes, Dislike)
    VALUES (@NewUserId, @NewArticleId, @newLike, @newDislike);
END;
go
CREATE TRIGGER UpdateLikeNumberAfterDelete
ON UserInteraction
AFTER DELETE
AS
BEGIN
    -- Lấy IdArticle của dòng dữ liệu đã bị xóa
    DECLARE @DeletedArticleId uniqueidentifier;

    SELECT @DeletedArticleId = ArticleId
    FROM deleted;

    -- Cập nhật giá trị LikeNumber trong bảng Article sau khi xóa dữ liệu
    UPDATE Article
    SET LikeNumber = dbo.GetLikeCountForArticle(Id)
    WHERE Id = @DeletedArticleId;
END;
go


CREATE FUNCTION dbo.GetLikeCountForArticle(@ArticleId uniqueidentifier)
RETURNS INT
AS
BEGIN
    DECLARE @LikeCount INT;
    
    IF EXISTS (SELECT 1 FROM UserInteraction WHERE ArticleId = @ArticleId AND Likes <> 0)
    BEGIN
        SET @LikeCount = (SELECT COUNT(*) FROM UserInteraction WHERE ArticleId = @ArticleId AND Likes <> 0);
    END
    ELSE
    BEGIN
        SET @LikeCount = 0;
    END
    RETURN @LikeCount;
END

go
drop FUNCTION dbo.GetDislikeCountArticle
go
CREATE FUNCTION dbo.GetDislikeCountArticle(@ArticleId uniqueidentifier)
RETURNS INT
AS
BEGIN
    DECLARE @DislikeCount INT;

    SELECT @DislikeCount = COUNT(*)
    FROM UserInteraction
    WHERE ArticleId = @ArticleId AND Dislike <> 0;

    RETURN @DislikeCount;
END;
GO

CREATE TRIGGER UpdateDislikeNumberAfterDelete
ON UserInteraction
AFTER DELETE
AS
BEGIN
    -- Lấy IdArticle của dòng dữ liệu đã bị xóa
    DECLARE @DeletedArticleId uniqueidentifier;
    DECLARE @DislikeCount INT;

    SELECT @DeletedArticleId = ArticleId
    FROM deleted;

    -- Lấy giá trị DislikeCount từ hàm
    SET @DislikeCount = dbo.GetDislikeCountArticle(@DeletedArticleId);

    -- Cập nhật giá trị DislikeNumber trong bảng Article sau khi xóa dữ liệu
    UPDATE Article
    SET DislikeNumber = @DislikeCount
    WHERE Id = @DeletedArticleId;
END;

