using BCCL.MvvmLight;

namespace BCCL.IssueTracker.Types
{
    public class Issue : ObservableObject
    {
        private Status _status;
        private Priority _priority;
        private IssueType _issueType;
        private string _version;
        private string _milestone;
        private string _title;
        private string _project;
        private string _assignee;
        private int _issueNumber;
        private int _issueId;
        private string _comment;
        private bool _isOriginal;

        public Issue(int issueNumber, string project, bool isOriginal)
        {
            _issueNumber = issueNumber;
            _project = project;
            _isOriginal = isOriginal;
        }
        
        public bool IsOriginal
        {
            get { return _isOriginal; }
            set { Set("IsOriginal", ref _isOriginal, value); }
        }

        public string Comment
        {
            get { return _comment; }
            set { Set("Comment", ref _comment, value); }
        }

        public int IssueId
        {
            get { return _issueId; }
            set { Set("IssueId", ref _issueId, value); }
        } 

        public int IssueNumber
        {
            get { return _issueNumber; }
            set { Set("IssueNumber", ref _issueNumber, value); }
        } 

        public string Assignee
        {
            get { return _assignee; }
            set { Set("Assignee", ref _assignee, value); }
        } 

        public string Project
        {
            get { return _project; }
            set { Set("Project", ref _project, value); }
        } 

        public string Title
        {
            get { return _title; }
            set { Set("Title", ref _title, value); }
        } 

        public string Milestone
        {
            get { return _milestone; }
            set { Set("Milestone", ref _milestone, value); }
        } 

        public string Version
        {
            get { return _version; }
            set { Set("Version", ref _version, value); }
        }

        public IssueType IssueType
        {
            get { return _issueType; }
            set { Set("IssueType", ref _issueType, value); }
        }

        public Priority Priority
        {
            get { return _priority; }
            set { Set("Priority", ref _priority, value); }
        } 

        public Status Status
        {
            get { return _status; }
            set { Set("Status", ref _status, value); }
        }
    }
}