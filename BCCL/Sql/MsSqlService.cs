/* 
Copyright (c) 2011 BinaryConstruct
 
This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BCCL.Sql
{
    /// <summary>
    /// Microsoft SQL Service
    /// Additional info: http://www.codeproject.com/KB/database/sql_in_csharp.aspx
    /// </summary>
    public class MsSqlService : SqlServiceBase
    {
        private string _authenticatedConnection;
        private bool _authenticated;

        public MsSqlService(string connection)
            : base(connection)
        {
        }

        public MsSqlService(Dictionary<string, string> connectionOpts)
            : base(connectionOpts)
        {
        }

        /// <summary>
        /// Gets the database connection string
        /// </summary>
        public override string ConnectionString
        {
            get { return DbConnection; }
        }

        /// <summary>
        /// Gets the login status. True if success, otherwise false
        /// </summary>
        public override bool IsValidated
        {
            get { return _authenticated; }
        }

        /// <summary>
        /// Set the username and password for the database
        /// </summary>
        /// <param name="user">SQL Username</param>
        /// <param name="pass">SQL Password</param>
        /// <returns>True if login succes, otherwise false</returns>
        public override bool Login(string user, string pass)
        {
            if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass))
            {
                string authenticatedConnection = string.Format("user id={0};password={1};{2}",user, pass, DbConnection);
                using (var conn = new SqlConnection(authenticatedConnection))
                {
                    try
                    {
                        conn.Open();
                        conn.Close();
                        _authenticatedConnection = authenticatedConnection;
                        _authenticated = true;
                        return true;
                    }

                    catch (Exception err)
                    {
                        OnLogMessage(this, err.Message);
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Allows the programmer to run a query against the Database.
        /// </summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataTable containing the result set.</returns>
        public override DataTable ExecuteQuery(string sql)
        {
            string connString = _authenticatedConnection ?? DbConnection;

            using (var conn = new SqlConnection(connString))
            using (var adapter = new SqlDataAdapter(sql, conn))
            {
                var result = new DataTable();

                /*
                // Force all decimal columns to double
                adapter.FillSchema(result, SchemaType.Source);
                foreach (DataColumn col in result.Columns)
                {
                    if (col.DataType == typeof(decimal) || col.DataType == typeof(Decimal))
                    {
                        col.DataType = typeof(Double);
                    }
                    col.ReadOnly = false;
                }
                 */

                adapter.Fill(result);

                conn.Close();
                return result;
            }
        }

        /// <summary>
        ///     Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        public override int ExecuteNonQuery(string sql)
        {
            string connString = _authenticatedConnection ?? DbConnection;

            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(sql, conn) { CommandType = CommandType.Text })
            {
                int recordsAffected = cmd.ExecuteNonQuery();
                conn.Close();
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
            string connString = _authenticatedConnection ?? DbConnection;

            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(sql, conn) { CommandType = CommandType.Text })
            {  
                var result = cmd.ExecuteScalar();
                return (result ?? string.Empty).ToString();
            }
        }

        public override int CreateSchema(string schema, DataSet data, bool insert = false, bool dropOld = false)
        {
            throw new NotImplementedException();
        }

        public override int BulkInsert(string tableName, DataTable data)
        {
            throw new NotImplementedException();
        }

        public override int BulkUpdate(string tableName, DataTable data)
        {
            throw new NotImplementedException();
        }

        public override int CreateTable(string tableName, DataTable data, bool insert = false, bool dropOld = false)
        {
            throw new NotImplementedException();
        }

        public override int Delete(string tableName, string where)
        {
            throw new NotImplementedException();
        }

        public override int Insert(string tableName, Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }

        public override int Update(string tableName, Dictionary<string, string> data, string where)
        {
            throw new NotImplementedException();
        }

        public override int ClearDb()
        {
            throw new NotImplementedException();
        }

        public override int ClearTable(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}