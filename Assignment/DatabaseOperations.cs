using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Assignment
{
    internal class DatabaseOperations
    {
        static public bool InsertBulkRecords<T>(List<T> records)
        {
            bool inserted = false;
            DataTable dt = GetTable(records);

            using(SqlConnection cn = GetConnection())
            {
                SqlBulkCopy bcp = new SqlBulkCopy(cn);
                cn.Open();
                bcp.DestinationTableName = "RecordsTable";
               
               bcp.WriteToServer(dt);
                inserted = true;

            }
            return inserted;
        }

        static private SqlConnection GetConnection()
        {
            return new SqlConnection("Server=DESKTOP-41BCSBQ;Database=AssignmentDB;Trusted_Connection=True");
        }
        static public DataTable GetTable<T>(List<T> records)
        {
            DataTable dt = new DataTable();
            var type = typeof(T);
            for(int i = 0; i< records.Count; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            foreach (var prop in type.GetProperties())
            {
                DataColumn cl = new DataColumn(prop.Name);
                cl.DataType = prop.PropertyType;
                dt.Columns.Add(cl);
                int rowIndex = 0;
                foreach(var item in records)
                {
                    DataRow dr = dt.Rows[rowIndex++];
                    dr[prop.Name] = prop.GetValue(item);
                }
            }
            return dt;
        }
    }
}
