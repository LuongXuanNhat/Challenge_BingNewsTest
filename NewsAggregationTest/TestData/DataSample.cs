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

        public string MappingData_RssTuoiTreNews()
        {
            string filePath = "D:\\INTERN\\BingNewsTest\\NewsAggregationTest\\TestData\\MappingDataSample_1.json";
            return File.ReadAllText(filePath);
        }
        public string MappingData_GgTrends()
        {
            string filePath = "D:\\INTERN\\BingNewsTest\\NewsAggregationTest\\TestData\\MappingDataSample_2.json";
            return File.ReadAllText(filePath);
        }
        public string MappingData_NewsDataIo()
        {
            string filePath = "D:\\INTERN\\BingNewsTest\\NewsAggregationTest\\TestData\\MappingDataSample_3.json";
            return File.ReadAllText(filePath);
        }

    }
}
