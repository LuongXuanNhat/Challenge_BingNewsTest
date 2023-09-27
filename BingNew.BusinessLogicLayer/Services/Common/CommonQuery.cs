using BingNew.DataAccessLayer.Constants;
using System.Data.SqlClient;
using System.Data;

namespace BingNew.BusinessLogicLayer.Services.Common
{
    public class CommonQuery 
    {
        private readonly ConstantCommon _constant;
        public CommonQuery()
        {
            _constant = new ConstantCommon();
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_constant.connectString);
        }


    }


}
