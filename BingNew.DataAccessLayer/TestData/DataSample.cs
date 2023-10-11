// The path where the test data is stored | Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData")
// Example                                | D:\INTERN\BingNewsTest\NewsAggregationTest\bin\Debug\net6.0\TestData

namespace BingNew.DataAccessLayer.TestData
{
    public static class DataSample
    {
        private static readonly string TestDataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");

        private static string GetConfigFilePath(string fileName)
        {
            return Path.Combine(TestDataDirectory, fileName);
        }

        public static string GetRssTuoiTreNewsDataMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_1.json");
            return File.ReadAllText(configFilePath);
        }
        public static string GetGgTrendsNewsDataMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_2.json");
            return File.ReadAllText(configFilePath);
        }
        public static string GetNewsDataIoMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_3.json");
            return File.ReadAllText(configFilePath);
        }
        public static string GetWeatherMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_4.json");
            return File.ReadAllText(configFilePath);
        }
        public static string GetNewWeatherMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_4 - New.json");
            return File.ReadAllText(configFilePath);
        }
        public static string GetWeatherInforMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_5.json");
            return File.ReadAllText(configFilePath);
        }
        public static string GetApiKeyOfNewsDataIo()
        {
            string configFilePath = GetConfigFilePath("ApiKeyOfNewsDataIo.txt");
            return File.ReadAllText(configFilePath);
        }
        public static string GetApiKeyOfWeather()
        {
            string configFilePath = GetConfigFilePath("ApiKeyOfWeather.txt");
            return File.ReadAllText(configFilePath);
        }

        public static string GetDataMockupNewsDataIo()
        {
            string configFilePath = GetConfigFilePath("MappingData_1.json");
            return File.ReadAllText(configFilePath);
        }

        public static string GetDataMockupGgTrend()
        {
            string configFilePath = GetConfigFilePath("MappingData_2.json");
            return File.ReadAllText(configFilePath);
        }

        public static string GetWeatherConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingData_4.json");
            return File.ReadAllText(configFilePath);
        }
    }
}
