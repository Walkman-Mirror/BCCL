using BCCL.MvvmLight;

namespace BCCL.IssueTracker.Types
{
    public class User : ObservableObject
    {
        private string _username;
        private string _email;
         

        public string email
        {
            get { return _email; }
            set { Set("email", ref _email, value); }
        } 

        public string Username
        {
            get { return _username; }
            set { Set("Username", ref _username, value); }
        } 
    }
}