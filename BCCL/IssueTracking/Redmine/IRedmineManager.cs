using System.Collections.Generic;
using System.Collections.Specialized;

namespace BCCL.IssueTracking.Redmine
{
    public interface IRedmineManager
    {
        /// <summary>
        /// Gets the object list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        IList<T> GetObjectList<T>(NameValueCollection parameters) where T : class;

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        T GetObject<T>(string id, NameValueCollection parameters) where T : class;

        /// <summary>
        /// Creates the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        void CreateObject<T>(T obj) where T : class;

        /// <summary>
        /// Updates the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="obj">The obj.</param>
        void UpdateObject<T>(string id, T obj) where T : class;

        /// <summary>
        /// Deletes the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="parameters">The parameters.</param>
        void DeleteObject<T>(string id, NameValueCollection parameters) where T : class;
    }
}