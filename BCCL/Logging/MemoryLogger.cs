using System;
using System.Collections.ObjectModel;

namespace BCCL.Logging
{
    public class MemoryLogger : ILogService
    {
        private static readonly string UserName = Environment.UserName;
        private static readonly string CompName = Environment.MachineName;
        private static readonly string Os = Environment.OSVersion.VersionString;
        private readonly ObservableCollection<string> _sessionLogs = new ObservableCollection<string>();

        public ObservableCollection<string> Messages
        {
            get { return _sessionLogs; }
        }

        public void Log(object sender, string message)
        {
            Log(message, sender.ToString(), MessageLevel.Info);
        }

        public void Log(object sender, string message, MessageLevel level)
        {
            message = "\"" + message.Replace('\"', '\'') + "\"";
            string source = "\"" + sender.ToString().Replace('\"', '\'') + "\"";
            string line = String.Format("{0:yyyy/MM/dd HH:mm:ss},{1},{2},{3},{4},{5},{6}", DateTime.Now, CompName, Os, UserName, source, level, message);
            Messages.Insert(0, line);

            while (Messages.Count > 100)
            {
                Messages.RemoveAt(Messages.Count - 1);
            }
        }

        public void LogException(Exception error)
        {
            if (error.InnerException != null)
                LogException(error.InnerException);

            Log(error.Source, error.Message + "\r\n" + error.StackTrace, MessageLevel.Error);
        }
    }
}