using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace Interview.Models
{
    public class OrderListDetail
    {
        /// <summary>
        /// 產品ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 產品圖片檔名
        /// </summary>
        public string photo { get; set; }

        /// <summary>
        /// 產品描述
        /// </summary>
        public string description { get; set; }
    }

    public class Getdetaildata 
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        private static string conStr = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();

        /// <summary>
        /// 取得產品內容
        /// </summary>
        /// <param name="id">產品識別碼</param>
        /// <returns></returns>
        public IEnumerable<OrderListDetail> GetDetails(string id)
        {
            string strSql = @"SELECT ID, photo, description FROM OrderListDetail WHERE ID = @ID ";
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                var result = conn.Query<OrderListDetail>(strSql, new 
                {
                    ID = DapperExtension.Tovarchar(id, 20)
                });
                return result;
            }
        }
    }
}