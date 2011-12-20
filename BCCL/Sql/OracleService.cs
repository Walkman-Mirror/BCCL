/*
This uses the Oracle ODAC
Source: http://www.oracle.com/technetwork/developer-tools/visual-studio/downloads/index.html
 */

using System;
using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;

namespace BCCL.Sql
{
    public class OracleService : SqlServiceBase
    {
        private string _authenticatedConnection;
        private bool _authenticated;

        public OracleService(string connection)
            : base(connection)
        {
        }

        public OracleService(Dictionary<string, string> connectionOpts)
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
                string authenticatedConnection = string.Format("User Id={0};Password={1};{2}",user, pass, DbConnection);
                using (var conn = new OracleConnection(authenticatedConnection))
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

            using (var conn = new OracleConnection(connString))
            using (var adapter = new OracleDataAdapter(sql, conn))
            {
                var result = new DataTable();

                /* REMOVED
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

            using (var conn = new OracleConnection(connString))
            using (var cmd = new OracleCommand(sql, conn) { CommandType = CommandType.Text})
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

            using (var conn = new OracleConnection(connString))
            using (var cmd = new OracleCommand(sql, conn) { CommandType = CommandType.Text })
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


        /*
        private string OracleErrorMessage(int error)
        {
            switch (error)
            {
                case 1:
                    return "Error attempting to insert duplicate data.";
                case 12560:
                    return "The database is unavailable.";
                case 1017:
                    return "Invalid SQL Username/Password.";
                case 28001:
                    return "SQL Password Expired, please change your password before attempting to login again.";
                case 28000:
                    return "SQL Account Locked from too many invalid password attempts, please contact the DBA or support.";
                case 12170:
                    return "Error Connecting to SQL database, possible connection or firewall issue.";
                default:
                    return "Unknown connection issue.";
            }
        }
        */
    }
}