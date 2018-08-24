using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using app.model;
using app.model.Dto;
using app.Data.DataOperations;
using app.Data.DataManipulation;

namespace app.repository
{
    public class BaseRepository<T>
    {
        //get all , //get single , //add, //delete, //update
        protected DataClient<T> dataClient;
        public string guidColumnName;
        
        public BaseRepository()
        {
            dataClient = new DataClient<T>();
        }
        public void SetFields(string table,string guid)
        {
            this.dataClient.SetTable(table);
            this.guidColumnName = guid;
        }
        public void SetTable(string table)
        {
            dataClient.SetTable(table);
        }
        public List<T> GetAll()
        {
            return dataClient.GetAll();
        }
        public T GetSingle(string columnName,string columnValue)
        {
            return dataClient.GetSingle(columnName,columnValue);
        }
        public List<T> GetAllFilteredByGuid(string guidValue)
        {
            dataClient.AddWhereClause(guidColumnName,guidValue);
            return dataClient.GetAll();
        }
        public List<T> GetAllFilteredByGuid(string filterColumn,string guidValue)
        {
            dataClient.AddWhereClause(filterColumn,guidValue);
            return dataClient.GetAll();
        }
        public List<T> GetAllFiltered(Dictionary<string,string> filters)
        {
            foreach(var cf in filters)
            {
                dataClient.AddWhereClause(cf.Key,cf.Value);
            }
            return dataClient.GetAll();
        }
        public bool Remove(string columnName, string guid)
        {
            return dataClient.DeleteOne(columnName,guid);
        }
        public bool Update(string columnToUpdate, string updateValue,string guidColumnName, string guidValue)
        {
            return dataClient.UpdateOne(columnToUpdate,updateValue,guidColumnName,guidValue);
        }
        public bool Update(T t,string guidColumnName,string guidValue)
        {
            
            return dataClient.UpdateManyFieldsOnOne(GetColumnValuePairs(t),guidColumnName,guidValue);
        }
        public bool Add(T t)
        {
            return dataClient.Insert(t);
        }
        public List<List<string>> GetColumnValuePairs(T t)
        {
            Type type = t.GetType();
            PropertyInfo [] propInfo = type.GetProperties();
            List<List<string>> columnAndValuePairs = new List<List<string>>();

            foreach(PropertyInfo prop in propInfo)
            {
                if (prop.GetValue(t) != null )

                {
                    if (!string.IsNullOrWhiteSpace(prop.GetValue(t).ToString()))
                    {
                        
                        columnAndValuePairs.Add(new List<string>(new string [] {prop.Name,prop.GetValue(t).ToString()}));
                    }
                }
            }
            return columnAndValuePairs;
        }
        public Dictionary<string,string> GetObjectAsDict(T t)
        {
            Type type = t.GetType();
            Dictionary<string,string> dict = new Dictionary<string,string>();
            int temp;
            string tempStr;
            Guid dummyGuid;
            DateTime dummyDate;
            Guid blankGuid = new Guid();
            dict.Add("PropertyCount", type.GetProperties().Count().ToString());
            foreach (var prop in type.GetProperties())
            {
                if (prop.GetValue(t) != null)
                {
                    tempStr = prop.GetValue(t).ToString();
                    if(!string.IsNullOrEmpty(tempStr) && !int.TryParse(tempStr, out temp))
                    {
                        if (Guid.TryParse(tempStr,out dummyGuid))
                        {
                            if(dummyGuid == blankGuid)
                            {
                                continue;
                            }
                        }
                        if (DateTime.TryParse(tempStr,out dummyDate))
                        {
                            continue;
                        }

                        dict.Add(prop.Name,prop.GetValue(t).ToString());
                    }
                }
            }
            return dict;
        }
    
    }
}