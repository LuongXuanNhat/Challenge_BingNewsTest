using System.Data.SqlClient;
using System.Reflection;


namespace BingNew.ORM.Query
{
    public static class SqlExtensionMultipleResult
    {
        public static IEnumerable<dynamic> QueryMultiple(this SqlConnection connection, string sql)
        {
            var sqlCommands = sql.Split(';');
            var result = new List<dynamic>();

            foreach (var sqlCommand in sqlCommands)
            {
                if (!string.IsNullOrWhiteSpace(sqlCommand))
                {
                    var queryResult = connection.Query(sqlCommand).ToList();
                    result.AddRange(queryResult);
                }
            }
            return result;
        }

        public static async Task<IAsyncEnumerable<dynamic>> QueryMultipleAsync(this SqlConnection connection, string sql)
        {
            var sqlCommands = sql.Split(';');
            var resultList = new List<IAsyncEnumerable<dynamic>>();

            foreach (var sqlCommand in sqlCommands)
            {
                if (!string.IsNullOrWhiteSpace(sqlCommand))
                {
                    var queryResult = connection.QueryAsync(sqlCommand);
                    resultList.Add(queryResult);
                }
            }

            async IAsyncEnumerable<dynamic> CombineResults()
            {
                foreach (var result in resultList)
                {
                    await foreach (var item in result)
                    {
                        yield return item;
                    }
                }
            }

            return CombineResults();
        }

        #region Extension methods for Query MultipleResult

        public static IEnumerable<T> Read<T>(this IEnumerable<dynamic> queryResults) where T : new()
        {
            foreach (var queryResult in queryResults)
            {
                if (queryResult is T typedResult)
                {
                    var result = new T();
                    var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

                    foreach (var field in fields)
                    {
                        var fieldValue = field.GetValue(typedResult);
                        field.SetValue(result, fieldValue);
                    }
                    yield return result;
                }
            }
        }
        public static T? ReadFirst<T>(this IEnumerable<dynamic> queryResults) where T : new()
        {
            foreach (var queryResult in queryResults)
            {
                if (queryResult is T typedResult)
                {
                    var result = new T();
                    var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

                    foreach (var field in fields)
                    {
                        var fieldValue = field.GetValue(typedResult);
                        field.SetValue(result, fieldValue);
                    }

                    return result;
                }
            }
            return default;
        }

        public static async IAsyncEnumerable<T> ReadAsync<T>(this IAsyncEnumerable<dynamic> queryResults) where T : new()
        {
            await foreach (var queryResult in queryResults)
            {
                if (queryResult is T typedResult)
                {
                    var result = new T();
                    var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

                    foreach (var field in fields)
                    {
                        var fieldValue = field.GetValue(typedResult);
                        field.SetValue(result, fieldValue);
                    }
                    yield return result;
                }
            }
        }

        public static async Task<T?> ReadFirstAsync<T>(this IAsyncEnumerable<dynamic> queryResults) where T : new()
        {
            await foreach (var queryResult in queryResults)
            {
                if (queryResult is T typedResult)
                {
                    var result = new T();
                    var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

                    foreach (var field in fields)
                    {
                        var fieldValue = field.GetValue(typedResult);
                        field.SetValue(result, fieldValue);
                    }
                    return result;
                }
            }
            return default;
        }


        #endregion
    }
}

////return List<dynamic> for each object 
////foreach (var sqlCommand in sqlCommands)
////{
////    if (!string.IsNullOrWhiteSpace(sqlCommand))
////    {
////        var result = connection.Query(sqlCommand).ToList();
////        yield return result;
////        foreach (var item in result)
////        {
////            yield return item;
////        }
////    }
////}
