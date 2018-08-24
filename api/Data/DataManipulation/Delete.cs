using System;
namespace app.Data.DataManipulation
{
    public static class Delete
    {
        public static string DeleteRow(string table, string column, string value)
        {
            string sql = $@"DELETE FROM [{table}] WHERE {column} = '{value}'";
            return sql;
        }
    }
}