using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace jlx
{
    /*
    Copyright © 2015，Adam，All Rights Reserved.
 * Copyright ownership belongs to JLX,shall not be reproduced ,
 * copied,or used in other ways without permission.Otherwise Jlx will have the right to pursue legal responsibilities.
 */

    /// <summary>
    /// 数据库连接
    /// </summary>
    public class data
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        /// <param name="source">服务器地址</param>
        /// <param name="database">数据库名</param>
        /// <param name="id">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        
        string source = jlx.gin.OffKey("G0xv9//YHBznTd9m9LT58A==", "31464333");
        string bases = jlx.gin.OffKey("L4SYY/H38s0hcrm8C/ulhQ==", "31464333");
        string Uid = jlx.gin.OffKey("L4SYY/H38s0hcrm8C/ulhQ==", "31464333");
        string Pwd = jlx.gin.OffKey("VN+ErqHfMxePYD9mRKQA2Q==", "31464333");
         public bool Login(string id, string pwd)
         {
           string sql =string.Format(@"Data Source={0};Database={1};Uid={2};Pwd={3};pooling=true",source,bases,Uid,pwd);
           SqlConnection conn = new SqlConnection(sql);
           conn.Open();
           string sqlstr = "Select Pwd from GmLogin WHERE Uid='" + id + "'";
           SqlCommand cmd = new SqlCommand(sqlstr, conn);
           SqlDataReader dr = cmd.ExecuteReader();
           if (dr["Pwd"].ToString() == pwd)
           {
               return true;
           }
           else
               return false;
           conn.Close();
         }
      
        
    }

    /// <summary>
    /// SQL类
    /// </summary>
    public static class SqlHelper
    {

        //把连接字符串写在App.config文件中
        private static string connStr = @"Data Source=127.0.0.1;Database=Books;Uid=sa;Pwd=a123456;pooling=true";
        //参数使用可变参数，params，在需要传递参数的时候传递，不需要的时候可以不写

        /// <summary>
        /// 执行SQL语句 并返回受影响行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;

                    //foreach (SqlParameter  param in parameters)
                    //{
                    //    cmd.Parameters.Add(param);
                    //}
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 查询/返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    return dataset.Tables[0];
                }
            }
        }
    }
}
