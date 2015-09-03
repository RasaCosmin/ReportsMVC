using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCReports.Helpers
{
    public class DatabaseService
    {
        public List<String> GetList(string tableName)
        {
                       
            string sqlQuery;

            switch(tableName)
            {
                                
                case "Pie":
                    sqlQuery = @"SELECT DISTINCT [Project] FROM[operations].[dbo].[Cumulative_All]";
                    break;
                case "Accuracy":
                    sqlQuery = @"SELECT DISTINCT asetup.[CUSTOMER] "+
                                "FROM [operations].[dbo].[ACCURACY_STATISTIK] AS astat "+
                                "INNER JOIN [operations].[dbo].[Accuracy_Setup] "+
                                "AS asetup ON astat.[customer_id] = asetup.[ID]";

                    

                    break;
                case "Stacked":
                    sqlQuery = "SELECT DISTINCT [Project] FROM[operations].[dbo].[Cumulative_All]";
                    break;
                case "Email":
                default:
                    sqlQuery = "SELECT DISTINCT[SenderAddress] AS EmailAddress" +
                               " FROM[operations].[dbo].[eMailOffice365Log] UNION" +
                               " SELECT DISTINCT[RecipientAddress] AS EmailAddress" +
                               " FROM[operations].[dbo].[eMailOffice365Log]";
                    break;
            }

            var list = new List<string>();
             
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("",connection:conn))
            {
                conn.Open();

                cmd.CommandText = sqlQuery;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader[0].ToString());
                }
                conn.Close();
            }

            return list;
        }
    }
}
