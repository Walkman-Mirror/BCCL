

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BCCL.Redmine
{
    public static class RestApi
    {
        public static Dictionary<string, int> GetProjects(string server, string key)
        {
            var reader = new XmlTextReader(string.Format("{0}/projects.xml", server)) {WhitespaceHandling = WhitespaceHandling.Significant};
            var doc = XElement.Load(reader);

            var result = new Dictionary<string, int>();
            foreach (var project in doc.Elements("project"))
            {
                result.Add(project.Element("name").Value, int.Parse(project.Element("id").Value));
            }
            return result;
        }

        public static Dictionary<string, int> GetTrackers(string server, string key, int projectId)
        {
            var reader = new XmlTextReader(string.Format("{0}/projects/{1}.xml?include=trackers", server, projectId)) { WhitespaceHandling = WhitespaceHandling.Significant };
            var doc = XElement.Load(reader);

            var result = new Dictionary<string, int>();
            foreach (var tracker in doc.Elements("trackers").Elements("tracker"))
            {
                result.Add((string)tracker.Attribute("name"), (int)tracker.Attribute("id"));
            }
            return result;
        }

        // operator console API key: 781716fe77e4f3a75803155aee93c55b6c5096ba
        // automated issue API Key:  1a2e1d9c9693d535c261d62d932c5a97601bba94
        public static void CreateIssue(string server, string key, string subject, int projectId, int trackerId, string description)
        {
            var address = new Uri(string.Format("{0}/issues.xml", server));
            var request = WebRequest.Create(address) as HttpWebRequest;
            if (request != null)
            {
                //request.Credentials = new NetworkCredential(user, pass);
                request.Headers.Add("X-Redmine-API-Key", key);
                request.Method = "POST";
                request.ContentType = "application/xml";
                request.Accept = "application/xml";


                string issue =
                    string.Format(@"<?xml version=""1.0""?>
<issue>
  <subject>{0}</subject>
  <project_id>{1}</project_id>
  <tracker_id>{2}</tracker_id>
  <description>{3}</description>
</issue>", subject, projectId, trackerId, description);

                byte[] bytes = Encoding.UTF8.GetBytes(issue);
                request.ContentLength = bytes.Length;
                request.KeepAlive = true;

                using (Stream putStream = request.GetRequestStream())
                {
                    putStream.Write(bytes, 0, bytes.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        var reader = new StreamReader(response.GetResponseStream());
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
            }
        }
    }
}