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
            return queryResults.OfType<T?>().Select(result => MapDynamicToType<T>(result));
        }

        public static T? ReadFirst<T>(this IEnumerable<dynamic?> queryResults) where T : new()
        {
            return queryResults.OfType<T>().Select(result => MapDynamicToType<T>(result)).FirstOrDefault();
        }

        public static async IAsyncEnumerable<T?> ReadAsync<T>(this IAsyncEnumerable<dynamic?> queryResults) where T : new()
        {
            var typeMapping = new Dictionary<Type, Func<dynamic, T>>()
            {
                { typeof(T), queryResult => MapDynamicToType<T>(queryResult) },

            };

            await foreach (var queryResult in queryResults)
            {
                Type queryResultType = queryResult != null ? queryResult.GetType() : throw new InvalidOperationException("Type is null!") ;
                typeMapping.TryGetValue(queryResultType, out var mapper);
                yield return ( mapper != null) ? mapper(queryResult) : default ;
            }
        }

        public static async Task<T?> ReadFirstAsync<T>(this IAsyncEnumerable<dynamic?> queryResults) where T : new()
        {
            var resultList = new List<dynamic?>();

            await foreach (var result in queryResults)
            {
                resultList.Add(result);
            }

            var mappedResult = resultList
                .OfType<T>()
                .Select(queryResult => MapDynamicToType<T?>(queryResult))
                .FirstOrDefault();

            return mappedResult;
        }

        private static T MapDynamicToType<T>(dynamic? result) where T : new()
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