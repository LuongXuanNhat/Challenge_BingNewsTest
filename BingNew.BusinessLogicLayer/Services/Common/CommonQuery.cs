using BingNew.DataAccessLayer.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
