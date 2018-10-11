using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using app.Data.DataManipulation;

namespace app.Data.DataOperations
{
    public enum DataOperations  {Select ,Update,Insert,Delete}
    public  class DataOperation<T>
    {
        private  string connectionString = "REDACTED";
        
        public string TableName {get;set;}
        public string Sql {get;set;}
        public List<string> whereClauseColumns;
        public List<string> whereClauseFilters;
        public List<Join> joins;
        public string WhereClause{get;set;}
        public bool hasWhereClause = false;
        public bool hasJoin = false;
        public DataOperation()
        {
            whereClauseColumns = new List<string>();
            whereClauseFilters = new List<string>();

            joins = new List<Join>();
            Sql = "";
        }
        public void AddFilterToWhereClause(string column,string filter)
        {
            
            whereClauseColumns.Add(column);
            whereClauseFilters.Add(filter);
            hasWhereClause = true;
        }
        public void JoinOnTable(string joinType, string tableToJoinOn, List<List<string>> leftTables, List<List<string>> rightTables)
        {
            Join j = new Join();
            j.JoinType = joinType;
            j.TableToJoinOn = tableToJoinOn;
            j.leftTables = leftTables;
            j.rightTables = rightTables;
            joins.Add(j);
            hasJoin = true;
        }
        public void ConstructSelectAllStatement(string table)
        {
            Sql += Select.SelectAllFromTable(table);
            
            AddJoinsAndWhereClause();
        }
        public void ConstructSelectByColumnStatement(string table,string column)
        {
            Sql += Select.SelectSingleColumnFromTable(table,column);
            
            AddJoinsAndWhereClause();
        }

        public void ConstructSelectByColumnStatement(string table, List<string> columns)
        {
            Sql += Select.SelectColumnsFromTable(table,columns);

            AddJoinsAndWhereClause();
        }
        public void ConstructDeleteStatement(string table,string column, string value)
        {
            Sql += Delete.DeleteRow(table,column,value);
            //Sql.Append(';');
        }
        public void ConstructUpdateStatement(string table,string column,string value)
        {
            Sql += Update.UpdateSingleTableRow(table,column,value);
            AddJoinsAndWhereClause();
        }
        public void ConstructUpdateStatement(string table, List<List<string>> updates)
        {
            Sql += Update.UpdateMultipleTableRows(table, updates);
            AddJoinsAndWhereClause();
        }

        public void ClearSql()
        {
            Sql = "";
        }

        private void AddJoinsAndWhereClause()
        {
            if(hasJoin)
            {
                foreach(Join j in joins)
                {
                    Sql += Select.CreateJoin(j.JoinType,j.TableToJoinOn,j.leftTables,j.rightTables);
                }
                hasJoin = false;
                joins.Clear();
            }
            if (hasWhereClause)
            {
                string whereClause = Select.GenerateWhereClause(whereClauseColumns,whereClauseFilters);
                Sql += whereClause;
                hasWhereClause = false;
                whereClauseColumns.Clear();
                whereClauseFilters.Clear();
            }
            Sql.Append(';');
        }
        public  DataSet FillDataSetWithAdapter(DataOperations operation)
        {
            if(string.IsNullOrWhiteSpace(Sql)) throw new Exception("You must first generate sql statement.");
            DataSet ds = new DataSet();
            try 
            {
                using (var connection = new SqlConnection(connectionString))
                {   
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    switch(operation)
                    {
                        case DataOperations.Select:
                            adapter.SelectCommand = new SqlCommand(Sql, connection);
                            adapter.Fill(ds);
                            break;
                        case DataOperations.Update:
                            adapter.UpdateCommand = new SqlCommand(Sql, connection);
                            adapter.Fill(ds);
                            break;
                        case DataOperations.Insert:
                            adapter.InsertCommand = new SqlCommand(Sql, connection);
                            adapter.Fill(ds);
                            break;
                        case DataOperations.Delete:
                            adapter.DeleteCommand = new SqlCommand(Sql, connection);
                            adapter.Fill(ds);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
            Console.WriteLine(ex.Message);
            }
            ClearSql();
            return ds;
        }
        public  int ExecuteNonQuery()
        {
            if(string.IsNullOrWhiteSpace(Sql)) throw new Exception("You must first generate sql statement.");
            int numRows = -1;
            using (var connection = new SqlConnection(connectionString))
            {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(Sql,connection);
                    SqlTransaction transaction = connection.BeginTransaction();
                    cmd.Transaction = transaction;
                    try
                    {
                        numRows = cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(Sql + "\n" + ex.Message);
                        transaction.Rollback();
                    }
            }
            ClearSql();
            return numRows;
        }
        public  List<T> Get() 
        {
            if(string.IsNullOrWhiteSpace(Sql)) throw new Exception("You must first generate sql statement.");
            List<T> allResults = new List<T>();
            try 
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var cmd = new SqlCommand(Sql,connection);
                    var reader = cmd.ExecuteReader();
                    DataExtensions<T> de = new DataExtensions<T>();
                    allResults = de.GetObjectListFromReader(reader);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return allResults;
        }
        public  DataTable GetDataTableFromQuery()
        {
            if(string.IsNullOrWhiteSpace(Sql)) throw new Exception("You must first generate sql statement.");
            DataTable dt = new DataTable();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var cmd = new SqlCommand(Sql,connection);
                    dt.Load(cmd.ExecuteReader());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            ClearSql();
            return dt;
        }
        public string ExecuteScalarForPasswordValidation(string pswd, string name)
        {

            string sql = $@"
            DECLARE @hashFromInput VARBINARY(MAX)
            DECLARE @hashFromTable VARBINARY(MAX)
            DECLARE @salt UNIQUEIDENTIFIER
            DECLARE @passToTest nvarchar(MAX)

            SELECT @hashFromTable=pass, @salt=salt 
            FROM [Membership] 
            WHERE name='{name}'

            SET @passToTest = '{pswd}' + CAST(@salt AS nvarchar(MAX))
            SET @hashFromInput = HASHBYTES('SHA2_512',@passToTest)
            
            SELECT 
            CASE
            WHEN
            @hashFromInput = @hashFromTable
            THEN (SELECT CAST(userGuid as nvarchar(MAX)) FROM dbo.[User] WHERE profileName='{name}')
            ELSE '0'
            END
            ";
            string result = "";
            try
            {
                using(var sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    var sqlcom = new SqlCommand(sql,sqlcon);
                    result = (string)sqlcom.ExecuteScalar();
                }
            }   
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }         
            if (Guid.TryParse(result,out Guid userGuid)){
                return result;
            }
            else 
            {
                return "";
            }
        }
        public bool InsertPassword(string pswd, string name, Guid userGUID)
        {
            string sql = $@"
            DECLARE @hash VARBINARY(MAX)
            DECLARE @salt UNIQUEIDENTIFIER
            DECLARE @pass NVARCHAR(MAX)
            SET @salt = NEWID()
            SET @pass = '{pswd}' + Cast(@salt AS NVARCHAR(MAX))
            SET @hash = HASHBYTES('SHA2_512',@pass)
            INSERT INTO [Membership]
            (
                memberGUID,salt,pass,userGUID,name
            )
            VALUES
            (
                NEWID(),@salt,@hash,'{userGUID}','{name}'
            )
            ";
            int numRows = 0;
            using(var con = new SqlConnection(connectionString))
            {
                con.Open();
                var com = new SqlCommand(sql,con);
                var trans = con.BeginTransaction();
                com.Transaction = trans;
                try 
                {
                    numRows = com.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    trans.Rollback();
                }
            }
            return (numRows == 1);
        }
        public  List<DataRow> GetRowList(DataTable table)
        {
            return table.Select().ToList();
            
        }

    }
}
