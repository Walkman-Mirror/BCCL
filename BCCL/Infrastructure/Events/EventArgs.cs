using System;

namespace BCCL.Infrastructure.Events
{
    /// <summary>
    /// EventArgs is the base class for classes containing event data. 
    /// </summary>
    /// <typeparam name="T">The type of the event data generated by the event.</typeparam>
    public class EventArgs<T> : EventArgs
    {
        private readonly T m_value1;

        /// <summary>
        /// Create a new instance of EventArgs.
        /// </summary>
        /// <param name="value1">The data generated by the event.</param>
        public EventArgs(T value1)
        {
            m_value1 = value1;
        }

        /// <summary>
        /// The value generated by the event.
        /// </summary>
        public T Value1
        {
            get { return m_value1; }
        }
    }
}