using System;
using System.Runtime.InteropServices;

namespace RecordBookApplication.EntryPoint
{
    class Program
    {

        static Menu menu = new Menu();
        static bool close = false;
        public static bool wasCleared = false;
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);

        private delegate bool SetConsoleCtrlEventHandler(CtrlType sig);

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
        static void Main(string[] args)
        {
            SetConsoleCtrlHandler(Handler, true); // Register the handle 
            while (!close)
            {
                menu.LogIn();
                close = true;
            }
        }
        private static bool Handler(CtrlType signal)
        {
            switch (signal)
            {
                case CtrlType.CTRL_BREAK_EVENT:
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:

                    menu.EncryptAllFiles();


                    Environment.Exit(0);
                    return false;

                default:
                    return false;
            }
        }

    }
}
