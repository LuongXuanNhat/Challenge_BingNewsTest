using BingNew.DataAccessLayer.Constants;
using System.Data;
using System.Data.SqlClient;

namespace BingNew.BusinessLogicLayer
{
    public class DapperContext
    {
        private readonly ConstantCommon _constant;
        public DapperContext()
        {
            _constant = new ConstantCommon();
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_constant.connectString);
        }
    }
}
