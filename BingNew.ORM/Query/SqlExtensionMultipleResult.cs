using System.Data;
using System.Data.SqlClient;
using System.Reflection;


namespace BingNew.ORM.Query
{
    public static class SqlExtensionMultipleResult
    {
        public static async Task<IAsyncEnumerable<dynamic?>> QueryMultipleAsync(this SqlConnection connection, string sql)
        {
            var sqlCommands = sql.Split(';');
            var resultList = new List<IAsyncEnumerable<dynamic?>>();

            resultList = sqlCommands
                 .Where(sqlCommand => !string.IsNullOrWhiteSpace(sqlCommand))
                 .Select(sqlCommand => connection.QueryAsync(sqlCommand))
                 .ToList();

            async IAsyncEnumerable<dynamic?> CombineResults()
            {
                foreach (var result in resultList)
                {
                    await foreach (var item in result)
                    {
                        yield return item;
                    }
                }
            }
            return await Task.Run(() => CombineResults());
        }

        public static IEnumerable<dynamic?> QueryMultiple(this SqlConnection connection, string sql)
        {
            var sqlCommands = sql.Split(';')
                                 .Where(s => !string.IsNullOrWhiteSpace(s))
                                 .Select(s => connection.Query(s))
                                 .SelectMany(results => results);
            return sqlCommands;
        }

   

        public static IEnumerable<T?> Read<T>(this IEnumerable<dynamic?> queryResults) where T : new()
        {
            return queryResults.OfType<T?>().Select(result => MapResult<T>(result));
        }

        public static T? ReadFirst<T>(this IEnumerable<dynamic?> queryResults) where T : new()
        {
            return queryResults.OfType<T?>().Select(result => MapResult<T>(result)).FirstOrDefault();
        }

        public static async IAsyncEnumerable<T?> ReadAsync<T>(this IAsyncEnumerable<dynamic?> queryResults) where T : new()
        {
            await foreach (var queryResult in queryResults)
            {
                switch (queryResult)
                {
                    case T typedResult:
                        yield return MapResult<T?>(typedResult);
                        break;
                }
            }
        }

        public static async Task<T?> ReadFirstAsync<T>(this IAsyncEnumerable<dynamic?> queryResults) where T : new()
        {
            await foreach (var queryResult in queryResults)
            {
                switch (queryResult)
                {
                    case T typedResult:
                        return MapResult<T?>(typedResult);
                }
            }
            return default;
        }

        private static T MapResult<T>(dynamic? result) where T : new()
        {
            var mappedResult = new T();
            var fields = typeof(T).GetProperties();

            foreach (var field in fields)
            {
                var fieldValue = field.GetValue(result);
                field.SetValue(mappedResult, fieldValue);
            }

            return mappedResult;
        }
    }
}