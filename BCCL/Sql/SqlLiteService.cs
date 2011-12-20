/*
This uses the SQLite .net API
Source: http://system.data.sqlite.org
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace BCCL.Sql
{
    public class SqLiteService : SqlServiceBase
    {
        private string _pass = string.Empty;

        public SqLiteService(string connection)
            : base(connection)
        {
        }

        public SqLiteService(Dictionary<string, string> connectionOpts)
            : base(connectionOpts)
        {
        }

        public SqLiteService(string connection, bool create)
            : base(connection)
        {
            string inputfile = null;
            string[] args = connection.Split(';');
            foreach (string arg in args)
            {
                string[] keyvalue = arg.Split('=');
                if (string.Equals(keyvalue[0], "Data Source"))
                {
                    inputfile = keyvalue[1];
                }
            }

            if (create && !string.IsNullOrWhiteSpace(inputfile))
                CreateDb(inputfile);
        }

        public SqLiteService(Dictionary<string, string> connectionOpts, bool create)
            : base(connectionOpts)
        {
            string inputfile;
            connectionOpts.TryGetValue("Data Source", out inputfile);

            if (create && !string.IsNullOrWhiteSpace(inputfile))
                CreateDb(inputfile);
        }

        private void CreateDb(string inputFile)
        {
            if (!System.IO.File.Exists(inputFile))
            {
                SQLiteConnection.CreateFile(inputFile);
            }
        }

        /// <summary>
        /// Gets the database connection string
        /// </summary>
        public override string ConnectionString
        {
            get { return DbConnection; }
        }

        /// <summary>
        /// Returns true if login success, otherwise false
        /// </summary>
        public override bool IsValidated
        {
            get { return Login(string.Empty, _pass); }
        }

        /// <summary>
        /// Set the username and password for the database
        /// </summary>
        /// <param name="user">SQL Username</param>
        /// <param name="pass">SQL Password</param>
        /// <returns>True if login succes, otherwise false</returns>
        public override bool Login(string user, string pass)
        {
            using (var cnn = new SQLiteConnection(DbConnection))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(pass))
                    {
                        cnn.SetPassword(pass);

                        cnn.Open();
                        cnn.Close();

                        _pass = pass;
                    }
                }

                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }


        /// <summary>
        ///     Allows the programmer to run a query against the Database.
        /// </summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataTable containing the result set.</returns>
        public override DataTable ExecuteQuery(string sql)
        {
            using (var cnn = new SQLiteConnection(DbConnection))
            using (var mycommand = new SQLiteCommand(cnn))
            {
                var dt = new DataTable();

                if (string.IsNullOrWhiteSpace(_pass))
                    cnn.SetPassword(_pass);

                cnn.Open();
                mycommand.CommandText = sql;
                SQLiteDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                cnn.Close();
                return dt;
            }
        }

        /// <summary>
        ///     Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>Number of records affected.</returns>
        public override int ExecuteNonQuery(string sql)
        {
            using (var cnn = new SQLiteConnection(DbConnection))
            using (var mycommand = new SQLiteCommand(cnn))
            {
                if (string.IsNullOrWhiteSpace(_pass))
                    cnn.SetPassword(_pass);

                cnn.Open();
                mycommand.CommandText = sql;

                int recordsAffected = mycommand.ExecuteNonQuery();
                cnn.Close();
                return recordsAffected;
            }
        }

        /// <summary>
        ///     Allows the programmer to retrieve single items from the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        public override string ExecuteScalar(string sql)
        {
            using (var cnn = new SQLiteConnection(DbConnection))
            {
                if (string.IsNullOrWhiteSpace(_pass))
                    cnn.SetPassword(_pass);

                cnn.Open();
                using (var mycommand = new SQLiteCommand(cnn))
                {
                    mycommand.CommandText = sql;
                    object value = mycommand.ExecuteScalar();
                    cnn.Close();

                    if (value != null)
                        return value.ToString();

                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Fast insert of many records
        /// </summary>
        /// <param name="tableName">SQL Database table name</param>
        /// <param name="data">Data to insert</param>
        /// <returns>Number of records affected.</returns>
        public override int BulkInsert(string tableName, DataTable data)
        {
            int recordsAffected = 0;
            using (var cnn = new SQLiteConnection(DbConnection))
            {
                if (string.IsNullOrWhiteSpace(_pass))
                    cnn.SetPassword(_pass);
                cnn.Open();
                using (SQLiteTransaction trans = cnn.BeginTransaction())
                using (SQLiteCommand cmd = cnn.CreateCommand())
                {
                    string cols = data.Columns.Cast<DataColumn>().Aggregate(string.Empty, (current, col) => current + (string.Format("{0},", col.ColumnName ))).TrimEnd(',');
                    string colsp = data.Columns.Cast<DataColumn>().Aggregate(string.Empty, (current, col) => current + (string.Format("@{0},", col.ColumnName))).TrimEnd(',');
                    cmd.CommandText = string.Format("INSERT INTO {0}({1}) VALUES({2})", tableName, cols, colsp);

                    foreach (DataColumn col in data.Columns)
                    {
                        cmd.Parameters.Add(new SQLiteParameter(col.ColumnName));
                    }
                    for (int rowIndex = 0; rowIndex < data.Rows.Count; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < data.Columns.Count; colIndex++)
                        {
                            string colName = data.Columns[colIndex].ColumnName;
                            cmd.Parameters[colName].Value = data.Rows[rowIndex][colName];
                        }
                        recordsAffected += cmd.ExecuteNonQuery();
                    }

                    trans.Commit();
                }

                cnn.Close();
            }
            return recordsAffected;
        }

        /// <summary>
        /// Fast update of many records
        /// </summary>
        /// <param name="tableName">SQL Database table name</param>
        /// <param name="data">Data to update</param>
        /// <returns>Number of records affected.</returns>
        public override int BulkUpdate(string tableName, DataTable data)
        {
            throw new NotImplementedException();

            int recordsAffected = 0;
            using (var cnn = new SQLiteConnection(DbConnection))
            {
                if (string.IsNullOrWhiteSpace(_pass))
                    cnn.SetPassword(_pass);
                cnn.Open();
                using (SQLiteTransaction trans = cnn.BeginTransaction())
                using (SQLiteCommand cmd = cnn.CreateCommand())
                {
                    string cols = data.Columns.Cast<DataColumn>().Aggregate(string.Empty, (current, col) => current + string.Format("{0}=@{0},", col.ColumnName)).TrimEnd(',');
                    string pkey = data.PrimaryKey.Aggregate(string.Empty, (current, col) => current + string.Format("{0}=@{0},", col.ColumnName)).TrimEnd(',');

                    cmd.CommandText = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, cols, pkey);

                    foreach (DataColumn col in data.Columns)
                    {
                        cmd.Parameters.Add(col.ColumnName);
                    }
                    for (int rowIndex = 0; rowIndex < data.Rows.Count; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < data.Columns.Count; colIndex++)
                        {
                            cmd.Parameters[data.Columns[colIndex].ColumnName].Value = data.Rows[rowIndex][colIndex];
                        }
                        recordsAffected += cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                cnn.Close();
            }
            return recordsAffected;
        }

        /// <summary>
        /// Create a sql table based on a DataTable
        /// </summary>
        /// <param name="tableName">SQL Database table name</param>
        /// <param name="data">Source DataTable</param>
        /// <param name="insert">Overload to populate rows</param>
        /// <param name="dropOld">Drop old table if exists.</param>
        /// <returns>Number of records or tables affected.</returns>
        public override int CreateTable(string tableName, DataTable data, bool insert = false, bool dropOld = false)
        {
            if (dropOld)
                ExecuteNonQuery(string.Format("DROP TABLE IF EXISTS {0}", tableName));

            int recordsAffected = 0;
            using (var cnn = new SQLiteConnection(DbConnection))
            {
                if (string.IsNullOrWhiteSpace(_pass))
                    cnn.SetPassword(_pass);
                cnn.Open();
                
                using (SQLiteTransaction trans = cnn.BeginTransaction())
                using (SQLiteCommand cmd = cnn.CreateCommand())
                {
                    string cols = data.Columns.Cast<DataColumn>().Aggregate(string.Empty, (current, col) => current + string.Format("{0},", col.ColumnName)).TrimEnd(',');
                    string pkey = null;
                    if (data.PrimaryKey.Length > 0)
                        pkey = data.PrimaryKey.Aggregate(", PRIMARY KEY (", (current, col) => current + string.Format("{0},", col.ColumnName)).TrimEnd(',') + ")";

                    cmd.CommandText = string.Format("CREATE TABLE {0} ({1}{2})", tableName, cols, pkey);
                    recordsAffected = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                cnn.Close();
            }

            if (insert)
            {
                recordsAffected = BulkInsert(tableName, data);
            }

            return recordsAffected;
        }

        /// <summary>
        /// Create a sql schema based on a DataSet
        /// </summary>
        /// <param name="schema">Name of the schema to create</param>
        /// <param name="data">Source DataSet</param>
        /// <param name="insert">Overload to populate rows</param>
        /// <param name="dropOld">Drop old table if exists.</param>
        /// <returns>Number of records or tables affected.</returns>
        public override int CreateSchema(string schema, DataSet data, bool insert = false, bool dropOld = false)
        {
            int recordsAffected = 0;
            foreach (DataTable table in data.Tables)
            {
                recordsAffected += CreateTable(string.Format("{0}_{1}", schema, table.TableName), table, insert, dropOld);
            }
            return recordsAffected;
        }
    }

}