using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregationTest.TestData
{
    public class DataSample
    {
        public DataSample() { }

        public string GetRssTuoiTreNewsDataMappingConfiguration()
        {
            string filePath = "D:\\INTERN\\BingNewsTest\\NewsAggregationTest\\TestData\\MappingDataSample_1.json";
            return File.ReadAllText(filePath);
        }
        public string GetGgTrendsNewsDataMappingConfiguration()
        {
            string filePath = "D:\\INTERN\\BingNewsTest\\NewsAggregationTest\\TestData\\MappingDataSample_2.json";
            return File.ReadAllText(filePath);
        }
        public string GetNewsDataIoMappingConfiguration()
        {
            string filePath = "D:\\INTERN\\BingNewsTest\\NewsAggregationTest\\TestData\\MappingDataSample_3.json";
            return File.ReadAllText(filePath);
        }
        public string GetWeatherInfoMappingConfiguration()
        {
            string filePath = "D:\\INTERN\\BingNewsTest\\NewsAggregationTest\\TestData\\MappingDataSample_4.json";
            return File.ReadAllText(filePath);
        }

    }
}
