using System;
using System.Xml;
using System.Xml.Serialization;

namespace BCCL.IssueTracking.Redmine.Types
{
    [Serializable]
    [XmlRoot("tracker")]
    public class Tracker : IdentifiableName
    {
        public override void WriteXml(XmlWriter writer)
        {
        }
    }
}