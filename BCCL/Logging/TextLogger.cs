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
using System.IO;
using System.Windows.Threading;

namespace BCCL.Logging
{
    public class TextLogger : ILogService
    {
        private const string Header = "TIMESTAMP,PC,USER,SOURCE,TYPE,MSG";
        private static readonly string UserName = Environment.UserName;
        private static readonly string CompName = Environment.MachineName;
        private static readonly string Os = Environment.OSVersion.VersionString;

        private readonly MessageLevel _minimumLogLevel;
        private readonly string _logfile = "log.txt";

        private readonly ObservableCollection<string> _sessionLogs = new ObservableCollection<string>();

        public TextLogger()
        {
            _minimumLogLevel = MessageLevel.Info;
        }
        public TextLogger(MessageLevel minLogLevel, string file)
            : this()
        {
            _minimumLogLevel = minLogLevel;
            _logfile = file;
        }

        #region ILogService Members

        public ObservableCollection<string> Messages
        {
            get { return _sessionLogs; }
        }

        public void LogException(Exception error)
        {
            if (error.InnerException != null)
                LogException(error.InnerException);

            Log(error.Source, error.Message + "\r\n" + error.StackTrace, MessageLevel.Error);
        }


        public void Log(object sender, string message)
        {
            Log(sender, message, MessageLevel.Info);
        }

        public void Log(object sender, string message, MessageLevel level)
        {
            if (level < _minimumLogLevel)
                return;

            lock (_logfile)
            {
                message = "\"" + message.Replace('\"', '\'') + "\"";
                string source = "\"" + sender.ToString().Replace('\"', '\'') + "\"";
                string line = String.Format("{0:yyyy/MM/dd HH:mm:ss},{1},{2},{3},{4},{5},{6}", DateTime.Now, CompName, Os, UserName, source, level, message);
                WriteLogLine(line);
                Messages.Insert(0, line);

                while (Messages.Count > 100)
                {
                    Messages.RemoveAt(Messages.Count - 1);
                }
            };
        }

        #endregion

        private void WriteLogLine(string line)
        {
            try
            {
                bool exists = File.Exists(_logfile);

                using (TextWriter writer = new StreamWriter(_logfile, true))
                {
                    if (!exists)
                        writer.WriteLine(Header);

                    writer.WriteLine(line);
                    writer.Close();
                }
            }
            catch
            {
                // swallow log writing exceptions 
            }
        }
    }
}