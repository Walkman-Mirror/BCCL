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
using System.Collections.ObjectModel;

namespace BCCL.Logging
{
    public interface ILogService
    {
        ObservableCollection<string> Messages { get; }
        void Log(object sender, string message);
        void Log(object sender, string message, MessageLevel level = MessageLevel.Info);
        void LogException(Exception error);
    }
}