using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace app.Data.DataOperations
{
    public class DataExtensions<T>
    {
        List<T> queryResults = new List<T>();
        public List<T> GetObjectListFromReader(SqlDataReader reader) 
        {
            T t;
            while(reader.Read())
            {
                t = GetInstance(typeof(T));
                GetObjectFromReader(t,reader);
                
                queryResults.Add(t);
            }
            return queryResults;
        }
        public void GetObjectFromReader(T t, SqlDataReader reader)
        {
            string column_name;
            
            object column_value;
            if (t != null && reader != null)
            {
                Type type = t.GetType();
                
                for (int i = 0 ;i<reader.FieldCount;i++)
                {
                    column_name = reader.GetName(i);
                    
                    PropertyInfo propInfo = type.GetProperty(column_name);

                    if (propInfo != null)
                    {
                        column_value = reader.GetValue(i);
                        //Console.WriteLine(column_value);
                        if (column_value.GetType() != typeof(DBNull))
                        {
                        propInfo.SetValue(t,column_value);
                        }
                        
                    }
                        
                }
            }
        }
        public T GetInstance(Type type)
        {
            
            if (type != null)
                return (T)Activator.CreateInstance(type);
            else 
            {
                foreach(var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = a.GetType(type.Name);
                    if (type != null)
                        return (T)Activator.CreateInstance(type);
                }
            }
            return default(T);
        }

    }
}