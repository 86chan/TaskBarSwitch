
namespace TaskBarSwitch
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
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
}