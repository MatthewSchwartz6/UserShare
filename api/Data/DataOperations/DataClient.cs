using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using app.model;
using app.Data.DataManipulation;
namespace app.Data.DataOperations
{
    public class DataClient<T>
    {
        private string _table;
        private DataOperation<T> dataOperation ;
        
        public DataClient()
        {
            dataOperation = new DataOperation<T>();
            
        }
        public void SetTable(string table)
        {
            _table = table;
        }
        public List<T> GetAll()
        {
            dataOperation.ConstructSelectAllStatement(_table);
            return dataOperation.Get();
        }
        public T GetSingle(string columnName, string columnValue)
        {
            DataOperation<T> dataOp = new DataOperation<T>();
            dataOp.AddFilterToWhereClause(columnName,columnValue);
            dataOp.ConstructSelectAllStatement(_table);
            return dataOp.Get().SingleOrDefault();
        }
        public List<T> GetMultiple(List<string> columns)
        {
            dataOperation.ConstructSelectByColumnStatement(_table,columns);
            return dataOperation.Get();
        }
        public List<T> GetAllWithFilter(List<string> columns,List<string> filters)
        {
            
            var columnFilters = columns.Zip(filters, (col,fil) => new {
                Column = col, Filter = fil
            });
            foreach(var cF in columnFilters)
            
            {
                dataOperation.AddFilterToWhereClause(cF.Column,cF.Filter);
            }
            
            dataOperation.ConstructSelectAllStatement(_table);
            return dataOperation.Get();
        }
        public void AddJoin(string joinType, string tableToJoinOn, List<List<string>> leftTables, List<List<string>> rightTables)
        {
            dataOperation.JoinOnTable(joinType,tableToJoinOn,leftTables,rightTables);
        }
        public void AddWhereClause(string column,string filter)
        {
            dataOperation.AddFilterToWhereClause(column,filter);
        }
        public bool InsertItem(string sql)
        {
            dataOperation.Sql = sql;
            int rows = dataOperation.ExecuteNonQuery();
            return (rows>=1);
        }
        public bool Insert(T t)
        {
            dataOperation.Sql = Insert<T>.InsertItem(t);
            int rows = dataOperation.ExecuteNonQuery();
            return (rows >= 1);
        }
        public bool DeleteOne(string guidColumn,string guidValue)
        {
            dataOperation.ConstructDeleteStatement(_table,guidColumn,guidValue);
            int rows = dataOperation.ExecuteNonQuery();
            return (rows>=1);
        }
        public bool UpdateOne(string column, string value,string guidColumn,string guidValue)
        {
            dataOperation.AddFilterToWhereClause(guidColumn,guidValue);
            dataOperation.ConstructUpdateStatement(_table,column,value);
            int rows = dataOperation.ExecuteNonQuery();
            return (rows>=1);
        }
        public bool UpdateManyFieldsOnOne(List<List<string>> values,string guidColumn,string guidValue)
        {
            dataOperation.AddFilterToWhereClause(guidColumn,guidValue);
            dataOperation.ConstructUpdateStatement(_table,values);
            int rows = dataOperation.ExecuteNonQuery();
            return (rows>=1);
        }
        public void ExecuteNonQuery()
        {
            dataOperation.ExecuteNonQuery();
        }
        public DataSet GetDataSetAfterOperation(DataOperations op)
        {
            return dataOperation.FillDataSetWithAdapter(op);
        }
        public DataTable GetTable()
        {
            return dataOperation.GetDataTableFromQuery();
        }
        public string PassValidation(string pswd,string name)
        {
            return dataOperation.ExecuteScalarForPasswordValidation(pswd,name);
        }
        public bool InsertPassword(string pswd,string nm, Guid userGUID)
        {
            return dataOperation.InsertPassword(pswd,nm,userGUID);
        }
        
        
    }
}