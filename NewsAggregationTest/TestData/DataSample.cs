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

        public string MappingData_1()
        {
            string filePath = "D:\\INTERN\\BingNewsTest\\NewsAggregationTest\\TestData\\MappingDataSample_1.json";
            return File.ReadAllText(filePath);
        }
    }
}
