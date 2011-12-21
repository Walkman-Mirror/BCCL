using System;
using System.Collections.Generic;
using System.Data;
using BCCL.Framework.Events;

namespace BCCL.Sql
{
    public abstract class SqlServiceBase : ISqlService
    {
        protected string DbConnection = string.Empty;

        /// <summary>
        ///     Single Param Constructor for specifying the DB file.
        /// </summary>
        /// <param name="connection">Connection string</param>
        protected SqlServiceBase(string connection)
        {
            DbConnection = connection;
        }

        /// <summary>
        ///     Single Param Constructor for specifying advanced connection options.
        /// </summary>
        /// <param name="connectionOpts">A dictionary containing all desired options and their values</param>
        protected SqlServiceBase(Dictionary<string, string> connectionOpts)
        {
            string str = "";

            foreach (KeyValuePair<string, string> row in connectionOpts)
            {
                str += String.Format("{0}={1}; ", row.Key, row.Value);
            }

            str = str.Trim().Substring(0, str.Length - 1);
            DbConnection = str;
        }

        /// <summary>
        /// Occurs when a log message is available
        /// </summary>
        public virtual event EventHandler<EventArgs<string>> LogMessage;

        /// <summary>
        /// Triggers a log message
        /// </summary>
        /// <param name="sender">Source of the message</param>
        /// <param name="message">Message to be logged</param>
        public virtual void OnLogMessage(object sender, string message)
        {
            var e = LogMessage;
            if (e != null)
                e(sender, new EventArgs<string>(message));
        }

        /// <summary>
        /// Gets the database connection string
        /// </summary>
        public abstract string ConnectionString { get; }

        /// <summary>
        /// Gets the login status. True if success, otherwise false
        /// </summary>
        public abstract bool IsValidated { get; }

        /// <summary>
        /// Set the username and password for the database
        /// </summary>
        /// <param name="user">SQL Username</param>
        /// <param name="pass">SQL Password</param>
        /// <returns>True if login succes, otherwise false</returns>
        public abstract bool Login(string user, string pass);

        /// <summary>
        ///     Allows the programmer to run a query against the Database.
        /// </summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataTable containing the result set.</returns>
        public abstract DataTable ExecuteQuery(string sql);

        /// <summary>
        ///     Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>Number of records affected.</returns>
        public abstract int ExecuteNonQuery(string sql);

        /// <summary>
        ///     Allows the programmer to retrieve single items from the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        public abstract string ExecuteScalar(string sql);

        /// <summary>
        /// Fast insert of many records
        /// </summary>
        /// <param name="tableName">SQL Database table name</param>
        /// <param name="data">Data to insert</param>
        /// <returns>Number of records affected.</returns>
        public abstract int BulkInsert(string tableName, DataTable data);

        /// <summary>
        /// Fast update of many records
        /// </summary>
        /// <param name="tableName">SQL Database table name</param>
        /// <param name="data">Data to update</param>
        /// <returns>Number of records affected.</returns>
        public abstract int BulkUpdate(string tableName, DataTable data);

        /// <summary>
        /// Create a sql table based on a DataTable
        /// </summary>
        /// <param name="tableName">SQL Database table name</param>
        /// <param name="data">Source DataTable</param>
        /// <param name="insert">Overload to populate rows</param>
        /// <param name="dropOld">Remove old tables if exists</param>
        /// <returns>Number of records or tables affected.</returns>
        public abstract int CreateTable(string tableName, DataTable data, bool insert = false, bool dropOld = false);

        /// <summary>
        /// Create a sql schema based on a DataSet
        /// </summary>
        /// <param name="schema">Name of the schema to create</param>
        /// <param name="data">Source DataSet</param>
        /// <param name="insert">Overload to populate rows</param>
        /// <param name="dropOld">Remove old tables if exists</param>
        /// <returns>Number of records affected.</returns>
        public abstract int CreateSchema(string schema, DataSet data, bool insert = false, bool dropOld = false);


        /// <summary>
        ///     Allows the programmer to easily update rows in the DB.
        /// </summary>
        /// <param name="tableName">The table to update.</param>
        /// <param name="data">A dictionary containing Column names and their new values.</param>
        /// <param name="where">The where clause for the update statement.</param>
        /// <returns>Number of records affected.</returns>
        public virtual int Update(string tableName, Dictionary<string, string> data, string where)
        {
            string vals = "";
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<string, string> val in data)
                {
                    vals += String.Format(" {0} = '{1}',", val.Key, val.Value);
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            return ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
        }

        /// <summary>
        ///     Allows the programmer to easily delete rows from the DB.
        /// </summary>
        /// <param name="tableName">The table from which to delete.</param>
        /// <param name="where">The where clause for the delete.</param>
        /// <returns>Number of records affected.</returns>
        public virtual int Delete(string tableName, string where)
        {
            return ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
        }

        /// <summary>
        ///     Allows the programmer to easily insert into the DB
        /// </summary>
        /// <param name="tableName">The table into which we insert the data.</param>
        /// <param name="data">A dictionary containing the column names and data for the insert.</param>
        /// <returns>Number of records affected.</returns>
        public virtual int Insert(string tableName, Dictionary<string, string> data)
        {
            string columns = "";
            string values = "";
            foreach (KeyValuePair<string, string> val in data)
            {
                columns += String.Format(" {0},", val.Key);
                values += String.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            return ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
        }


        /// <summary>
        ///     Allows the programmer to easily delete all data from the DB.
        /// </summary>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        /// <returns>Number of tables affected.</returns>
        public virtual int ClearDb()
        {
            DataTable tables = ExecuteQuery("select NAME from SQLITE_MASTER where type='table' order by NAME;");

            int tablecount = 0;
            foreach (DataRow table in tables.Rows)
            {
                tablecount += ClearTable(table["NAME"].ToString());
            }
            return tablecount;
        }

        /// <summary>
        ///     Allows the user to easily clear all data from a specific table.
        /// </summary>
        /// <param name="tableName">The name of the table to clear.</param>
        /// <returns>Number of tables affected.</returns>
        public virtual int ClearTable(string tableName)
        {
            return ExecuteNonQuery(String.Format("delete from {0};", tableName));
        }


    }
}