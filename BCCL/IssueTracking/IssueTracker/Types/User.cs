/* 
Copyright (c) 2011 BinaryConstruct
 
This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */

using BCCL.MvvmLight;

namespace BCCL.IssueTracking.IssueTracker.Types
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