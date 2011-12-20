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