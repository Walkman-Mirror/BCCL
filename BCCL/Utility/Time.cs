using System;

namespace BCCL.Utility
{
    public static class Time
    {
        public static void SetSystemTime(DateTime time)
        {
            Microsoft.VisualBasic.DateAndTime.TimeOfDay = time;
        }
    }
}