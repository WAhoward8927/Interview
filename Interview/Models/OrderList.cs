using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace Interview.Models
{
    public class OrderList 
    {
        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 產品識別碼
        /// </summary>
        public string ID{ get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string item { get; set; }

        /// <summary>
        /// 售價
        /// </summary>
        public int price { get; set; }

        /// <summary>
        /// 成本
        /// </summary>
        public int cost { get; set; }

        /// <summary>
        /// 目前狀態
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 是否已出貨
        /// </summary>
        public bool Ischeck { get; set; }
    }

    public class OrderListIndex
    {
        private string connStr = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();

        /// <summary>
        /// 取得指定User的資料
        /// </summary>
        /// <returns>按照item名由小到大排序</returns>
        public IEnumerable<OrderList> GetProduct()
        {
            OrderList products = new OrderList();
            //取得某位使用者的資料(不論有無購買)
            //string strSql = @"SELECT OrderList.ID, item, price, cost,status FROM OrderList LEFT JOIN (SELECT ID, account, itemid, status FROM MemberOrder WHERE account = 'UserA') AS queryresult ON OrderList.ID = queryresult.itemid ";
            string strSql = @"SELECT account, OrderList.ID, item, price, cost ,status FROM OrderList LEFT JOIN MemberOrder ON OrderList.ID = MemberOrder.itemid WHERE account = @account ";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                var result = conn.Query<OrderList>(strSql, new { account = DapperExtension.Tovarchar("UserA", 20)});
                foreach (var pair in result)
                {
                    if (pair.status == "Payment completed")
                    {
                        pair.Ischeck = false;
                    }
                    else if (pair.status == "To be Shipped")
                    {
                        pair.Ischeck = true;
                    }
                    else 
                    {
                        pair.Ischeck = false;
                    }
                }
                return result.OrderBy(e => e.item);
            }
        }

        /// <summary>
        /// 更新產品狀態
        /// </summary>
        /// <param name="id">產品ID</param>
        /// <param name="account">使用者</param>
        /// <returns></returns>
        public string updateItemstatus(string id, string account)
        {
            string strSql = @"UPDATE MemberOrder2 SET status = 'To be Shipped' WHERE itemid = @ID AND account = @Account ";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute(strSql, new
                        {
                            ID = DapperExtension.Tovarchar(id, 50),
                            Account = DapperExtension.Tovarchar(account, 20)
                        }, transaction: tran);
                        tran.Commit();
                        return string.Empty;
                    }
                    catch(Exception ex)
                    {
                        tran.Rollback();
                        return ex.Message;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Dapper Strong Type Extension
    /// </summary>
    public static class DapperExtension 
    {
        /// <summary>
        /// 強型別轉varchar
        /// </summary>
        /// <param name="me">參數</param>
        /// <param name="length">長度</param>
        /// <returns></returns>
        public static DbString Tovarchar(this string me, int length) 
        {
            return new DbString { Value = me, IsAnsi = true, IsFixedLength = false, Length = length };
        }
    }
}