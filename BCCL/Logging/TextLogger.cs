using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Threading;

namespace BCCL.Logging
{
    public class TextLogger : ILogService
    {
        private static string Logfile = "log.txt";
        private static readonly string Header = "TIMESTAMP,PC,USER,SOURCE,TYPE,MSG";
        private static readonly string UserName = Environment.UserName;
        private static readonly string CompName = Environment.MachineName;
        private static readonly string Os = Environment.OSVersion.VersionString;

        private static MessageLevel MinimumLogLevel;
        private Dispatcher _dispatcher;

        private readonly ObservableCollection<string> _sessionLogs = new ObservableCollection<string>();

        public TextLogger()
        {
            System.Diagnostics.Debug.WriteLine("CTOR TextLogger");
            _dispatcher = Dispatcher.CurrentDispatcher;
            MinimumLogLevel = MessageLevel.Info;
        }
        public TextLogger(MessageLevel level, string file)
            : this()
        {
            MinimumLogLevel = level;
            Logfile = file;
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
            if (level < MinimumLogLevel)
                return;

            _dispatcher.Invoke(
                DispatcherPriority.Normal, new Action(
                delegate
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
                    }));
        }

        #endregion

        private static void WriteLogLine(string line)
        {
            try
            {
                bool exists = File.Exists(Logfile);
                
                using (TextWriter writer = new StreamWriter(Logfile, true))
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