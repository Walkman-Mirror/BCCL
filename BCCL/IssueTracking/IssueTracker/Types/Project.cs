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
using BCCL.MvvmLight;

namespace BCCL.IssueTracking.IssueTracker.Types
{
    public class Project : ObservableObject
    {
        private string _name;
        private DateTime _startDate;
        private string _version;
        private readonly ObservableCollection<string> _availableVersions = new ObservableCollection<string>();
        private string _sourceControl;
        private string _description;
        private string _creator;

        public string Creator
        {
            get { return _creator; }
            set { Set("Creator", ref _creator, value); }
        }

        public string Description
        {
            get { return _description; }
            set { Set("Description", ref _description, value); }
        } 

        public string SourceControl
        {
            get { return _sourceControl; }
            set { Set("SourceControl", ref _sourceControl, value); }
        } 

        public ObservableCollection<string> AvailableVersions
        {
            get { return _availableVersions; }
        }

        public string Version
        {
            get { return _version; }
            set { Set("Version", ref _version, value); }
        } 

        public DateTime StartDate
        {
            get { return _startDate; }
            set { Set("StartDate", ref _startDate, value); }
        } 

        public string Name
        {
            get { return _name; }
            set { Set("Name", ref _name, value); }
        }
         
    }
}