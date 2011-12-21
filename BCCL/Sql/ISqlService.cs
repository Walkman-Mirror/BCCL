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
using BCCL.Framework.Events;

namespace BCCL.Sql
{
    public interface ISqlService
    {
        string ConnectionString { get; }
        bool IsValidated { get; }

        bool Login(string user, string pass);

        void OnLogMessage(object sender, string message);
        event EventHandler<EventArgs<string>> LogMessage;

        DataTable ExecuteQuery(string sql);
        int ExecuteNonQuery(string sql);
        string ExecuteScalar(string sql);

        int Update(string tableName, Dictionary<string, string> data, string where);
        int Delete(string tableName, string where);
        int Insert(string tableName, Dictionary<string, string> data);

        int CreateTable(string tablename, DataTable data, bool insert = false, bool dropExisting = false);
        int CreateSchema(string schema, DataSet data, bool insert = false, bool dropExisting = false);
        int BulkInsert(string tableName, DataTable data);
        int BulkUpdate(string tableName, DataTable data);
        int ClearDb();
        int ClearTable(string tableName);
    }
}