using UnityEngine;
using System.Data; // Add on
using Mono.Data.Sqlite; // Add on
using System; // Add on

public class SQliteAdapter
{
    public string DBFileName;
    public string DBFolder;

    private IDbConnection dbcon;
    private IDbCommand dbcommd;

    //--------------------------------------------------------------------------------------------------------
    public SQliteAdapter(string DBFileName, string DBFolder)
    {
        this.DBFileName = DBFileName;
        this.DBFolder   = DBFolder;
    }

    public void connectDatabase()
    {
        string connectionString = "URI=file:" + Application.dataPath + "/" + this.DBFolder + "/" + this.DBFileName;

        this.dbcon = new SqliteConnection(connectionString);
        this.dbcon.Open();


        this.dbcommd = this.dbcon.CreateCommand();
    }

    public void disconnectDatabase()
    {
        // Close Command
        this.dbcommd.Dispose();

        // Close Database Connection
        this.dbcon.Close();
        this.dbcon.Dispose();

    }

    public IDataReader select(string table = "", string columns = "*")
    {
        string sql = "SELECT " + columns + " FROM " + table;
        return query(sql);
    }

    public IDataReader selectSort(string table = "", string order_col = "", string columns = "*", string order_key = "ASC")
    {
        string sql = "SELECT " + columns + " FROM " + table;
        sql += " " + "ORDER BY " + order_col + " " + order_key;

        Debug.Log("\n" + sql + "\n");
        return query(sql);
    }
    
    public void update1Col(string table, string column, string update_value_str, string where_column, string where_var )
    {
        string      sql = "UPDATE " + table;
                    sql += " " + "SET " + column + " = "+ update_value_str + " ";
                    sql += " " + "WHERE " + where_column + " = " + where_var;

        Debug.Log(sql);

        IDataReader reader = query(sql);
        disconnectDatabase();
    }

    public IDataReader query(string sql)
    {
        IDataReader reader;
        try
        {
            connectDatabase();
            this.dbcommd.CommandText = sql;
            reader = this.dbcommd.ExecuteReader();

        }
        catch (Exception excp)
        {
            Debug.Log(excp);
            reader = null;
        }
        return reader;
    }

}
