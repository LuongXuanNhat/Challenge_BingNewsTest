using System;
using System.IO;

// The path where the test data is stored | Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData")
// Example                                | D:\INTERN\BingNewsTest\NewsAggregationTest\bin\Debug\net6.0\TestData

namespace BingNew.DataAccessLayer.TestData
{
    public class DataSample
    {
        private static readonly string TestDataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");

        public DataSample() { }

        private string GetConfigFilePath(string fileName)
        {
            return Path.Combine(TestDataDirectory, fileName);
        }

        public string GetRssTuoiTreNewsDataMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_1.json");
            return File.ReadAllText(configFilePath);
        }
        public string GetGgTrendsNewsDataMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_2.json");
            return File.ReadAllText(configFilePath);
        }
        public string GetNewsDataIoMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_3.json");
            return File.ReadAllText(configFilePath);
        }
        public string GetWeatherMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_4.json");
            return File.ReadAllText(configFilePath);
        }
        public string GetWeatherInforMappingConfiguration()
        {
            string configFilePath = GetConfigFilePath("MappingDataSample_5.json");
            return File.ReadAllText(configFilePath);
        }

    }
}
