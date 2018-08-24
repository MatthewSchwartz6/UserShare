using System;
using System.Collections.Generic;
using System.Linq;

namespace app.Data.DataManipulation
{
    public static class Select
    {
        public static string SelectAllFromTable(string table)
        {
            string sql = $@" SELECT * FROM [{table}] ";
            return sql;
        }
        public static string GenerateWhereClause(List<string> columns,List<string> filters)
        {   
            string whereClause = " WHERE ";
            string and_SubString = " AND ";
            if (columns.Count != filters.Count) throw new Exception("Columns and filters must be the same amount.");
            if (columns != null && filters != null)
            {
                if(columns.Count > 1)
                {
                    foreach(var s in columns.Zip(filters, (col,filter) => new {
                            expression = " " + col + "='" + filter + "' "
                            }).Select(x=>x.expression))
                    {
                        whereClause += s + and_SubString;
                    }
                    whereClause = whereClause.Remove(whereClause.Count() - and_SubString.Count());
                }
                else 
                {
                    whereClause += " " + columns.FirstOrDefault() + "='" + filters.FirstOrDefault() + "' ";
                }
                
            }
            return whereClause;
        }
        public static string SelectColumnsFromTable(string table,List<string> columns)
        {
            string sql = "";
            if(columns != null && columns.Count > 1)
            {
                string col = string.Join(",",columns);
                sql += $@"SELECT {col} FROM  [{table}]";
            }
            return sql;
        }
        public static string SelectSingleColumnFromTable(string table, string col)
        {
            string sql = $@"SELECT {col} FROM [{table}]";
            return sql;
        }
        public static string CreateJoin(string joinType, string tableToJoinOn, List<List<string>> table1, List<List<string>> table2)
        {
            //each list in a list has 2 elements, the table then the column
            string innerJoin = joinType + " JOIN " + tableToJoinOn + " ON ";
            foreach (var s in table1.Zip(table2,(col1,col2) => new {
                expression = string.Join(".",col1) + "=" + string.Join(".",col2) 
            }).Select(x => x.expression))
            {
             innerJoin += s + ", ";   
            }
            innerJoin = innerJoin.Remove(innerJoin.Length - 2);
            return innerJoin;
        }
    }
}