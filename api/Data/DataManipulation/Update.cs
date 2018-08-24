using System;
using System.Collections.Generic;
using System.Linq;
namespace app.Data.DataManipulation
{
    public static class Update
    {
       public static string UpdateSingleTableRow(string table,string column, string value)
       {
           string sql = $@" UPDATE [{table}] SET {column}='{value}' ";
           return sql;
       } 
       public static string UpdateMultipleTableRows(string table, List<List<string>> updates)
       {
           string sql = $@" UPDATE [{table}] SET ";
           foreach(var s in updates)
           {
               sql += string.Join("='",s) + "', ";
           }
            sql = sql.Remove(sql.Length - 2);
           return sql;
       }
    }
}