using System;
using System.Collections.ObjectModel;
using BCCL.IssueTracking.Redmine;
using BCCL.IssueTracking.Redmine.Types;

namespace BCCL.Logging
{
    public class RedmineLogger : ILogService
    {
        private readonly string UserName = Environment.UserName;
        private readonly string CompName = Environment.MachineName;
        private static readonly string Os = Environment.OSVersion.VersionString;

        private readonly IRedmineManager _redmine;
        private readonly string _project;
        public RedmineLogger(string project, string server, string apikey)
        {
            //@"http://192.168.111.128/redmine", "1a2e1d9c9693d535c261d62d932c5a97601bba94");
            // operator console API key: 781716fe77e4f3a75803155aee93c55b6c5096ba
            // automated issue API Key:  1a2e1d9c9693d535c261d62d932c5a97601bba94
            _project = project;
            _redmine = new RedmineManager(server, apikey); 
        }

        public ObservableCollection<string> Messages
        {
            get { return null; }
        }

        public void Log(object sender, string message)
        {
            Log(sender, message, MessageLevel.Info);
        }

        public void Log(object sender, string message, MessageLevel level = MessageLevel.Info)
        {
            var issue = new Issue
            {
                Description = string.Format("{0}: {1}\r\n{2}\r\n{3}", UserName, CompName, Os, message),
                Project = new IdentifiableName { Name = _project },
                Subject = "Log - " + sender,
                Tracker = new IdentifiableName { Id = (int)Trackers.Support },
                Priority = new IdentifiableName { Id =  (int)ConvertLevelToPriority(level) }
            };

            _redmine.CreateObject(issue);
        }

        public void LogException(Exception error)
        {
            if (error.InnerException != null)
                LogException(error.InnerException);

            var issue = new Issue
            {
                Description = string.Format("{0}: {1}\r\n{2}\r\n{3}\r\n{4}", UserName, CompName, Os, error.Message, error.StackTrace),
                Project = new IdentifiableName { Name = _project },
                Subject = error.Source + " Exception",
                Tracker = new IdentifiableName { Id = (int)Trackers.Exception },
                Priority = new IdentifiableName { Id = (int)Priorities.Normal }
            };

            _redmine.CreateObject(issue);
        }

        private Priorities ConvertLevelToPriority(MessageLevel level)
        {
            switch (level)
            {
                case MessageLevel.Debug:
                    return Priorities.Low;
                case MessageLevel.Info:
                    return Priorities.Normal;
                case MessageLevel.Warning:
                    return Priorities.High;
                case MessageLevel.Error:
                    return Priorities.Urgent;
                case MessageLevel.Fatal:
                    return Priorities.Immediate;
                default:
                    return Priorities.Normal;

            }
        }
    }
}