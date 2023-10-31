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

ALTER TABLE BingNews.dbo.Article
ALTER COLUMN Url varchar(1000);

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






