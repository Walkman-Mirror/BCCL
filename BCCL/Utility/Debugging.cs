namespace BCCL.Utility
{
    public static class Debugging
    {
        public static bool IsInDesignMode
        {
            get
            {
                return (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv");
            }
        }
    }
}