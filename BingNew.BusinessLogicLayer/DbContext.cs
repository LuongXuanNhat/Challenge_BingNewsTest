using BingNew.DataAccessLayer.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingNew.BusinessLogicLayer
{
    public class DbContext
    {
        private readonly ConstantCommon _constant;
        public DbContext()
        {
            _constant = new ConstantCommon();
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_constant.connectString);
        }
    }
}
