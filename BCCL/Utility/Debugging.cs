/* 
Copyright (c) 2011 BinaryConstruct
 
This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */

namespace BCCL.Utility
{
    public static class Debugging
    {
        private static string procName;
        public static bool IsInDesignMode;

        static Debugging()
        {
            procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            IsInDesignMode = procName.Contains("devenv");
        }
    }
}