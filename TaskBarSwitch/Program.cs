
namespace TaskBarSwitch
{
    internal static class Program
    {

        /// <summary>
        /// 多重起動防止Mutex
        /// </summary>
        private static Mutex? mutex;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew;
            mutex = new Mutex(true, nameof(TaskBarSwitch), out createdNew);

#if DEBUG
            createdNew = true;
#endif

            try
            {
                // 多重起動防止
                if (true == createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    var taskBar = new TaskBar();

                    // 引数のチェック
                    if (args.Length > 0)
                    {
                        foreach (var arg in args)
                        {
                            // "show" 引数が渡された場合の処理
                            if (arg.ToLower().Equals("--show", StringComparison.OrdinalIgnoreCase))
                            {
                                TaskBarSwitchAPI.SetTaskbarStatus(TaskBarSwitchAPI.ASB_STATUS.ABS_ALWAYSONTOP);
                            }

                            // "hide" 引数が渡された場合の処理
                            if (arg.ToLower().Equals("--hide", StringComparison.OrdinalIgnoreCase))
                            {
                                TaskBarSwitchAPI.SetTaskbarStatus(TaskBarSwitchAPI.ASB_STATUS.ABS_AUTOHIDE);
                            }
                        }
                    }

                    Application.Run(taskBar);
                }
            }
            finally
            {
                if ((true == createdNew) && (null != mutex))
                {
                    // Mutexの解放
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}