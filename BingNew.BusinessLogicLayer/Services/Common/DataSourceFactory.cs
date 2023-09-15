using BingNew.BusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    // Factory Method Design Pattern
    public class DataSourceFactory
    {
        public enum DataSource
        {
            ApiDataSource,
            RssDataSource
        }

        public static IDataSource CreateDataSource(DataSource dataSource)
        {
            IDataSource data = null;
            switch (dataSource)
            {
                case DataSource.ApiDataSource:
                    return new ApiDataSource();
                case DataSource.RssDataSource:
                    return new RssDataSource(); 
            }
            return data;
        }
    }
}
