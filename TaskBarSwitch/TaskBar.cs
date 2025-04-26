using System.Diagnostics;
using System.Management;
using Svg.Skia;

namespace TaskBarSwitch
{
    /// <summary>
    /// タスクバー
    /// </summary>
    public partial class TaskBar : Form
    {
        /// <summary>
        /// NotifyIcon
        /// </summary>
        private static NotifyIcon? notifyIcon;

        /// <summary>
        /// コンテキストメニュー
        /// </summary>
        private ContextMenuStrip contextMenu;

        /// <summary>
        /// 常時表示アイコン
        /// </summary>
        private SKSvg ALWAYSONTOPIcon;

        /// <summary>
        /// 自動非表示アイコン
        /// </summary>
        private SKSvg AUTOHIDEIcon;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskBar()
        {
            notifyIcon = new NotifyIcon();
            contextMenu = new ContextMenuStrip();

            AUTOHIDEIcon = ConvertSvgToIconEmbedded("AUTOHIDE.svg");
            ALWAYSONTOPIcon = ConvertSvgToIconEmbedded("ALWAYSONTOP.svg");

            // コンテキストメニューの項目を追加
            contextMenu.Items.Add("常時表示", null, TaskBarShow);
            contextMenu.Items.Add("自動非表示", null, TaskBarHide);
            contextMenu.Items.Add("切り替え", null, AutoSwitch);
            contextMenu.Items.Add("タスクバーを再起動", null, RestartTaskbar);
            contextMenu.Items.Add("終了", null, ExitApp);

            // NotifyIconの設定
            notifyIcon.Icon = ChangeAccentColorIcon(AUTOHIDEIcon);
            notifyIcon.ContextMenuStrip = contextMenu;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += AutoSwitch;
            notifyIcon.Text = "TaskBarSwitch";

            // フォームを非表示にする
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Load += (s, e) => this.Hide();

            // タスクバーの状態が変更されたときにアイコンを変更する
            TaskBarSwitchAPI.TaskbarStatusChanged += (s, e) => SetTaskbarIcon();

            // 現在の状態を取得してアイコンを設定する
            SetTaskbarIcon();
        }

        /// <summary>
        /// タスクバーの表示切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AutoSwitch(object? sender, EventArgs e)
        {
            var status = TaskBarSwitchAPI.GetTaskbarStatus();
            switch (status)
            {
                // タスクバーが常に表示されている場合は自動非表示にする
                case TaskBarSwitchAPI.ASB_STATUS.ABS_AUTOHIDE:
                    TaskBarSwitchAPI.SetTaskbarStatus(TaskBarSwitchAPI.ASB_STATUS.ABS_ALWAYSONTOP);
                    break;
                // タスクバーが自動非表示の場合は常に表示にする
                case TaskBarSwitchAPI.ASB_STATUS.ABS_ALWAYSONTOP:
                default:
                    TaskBarSwitchAPI.SetTaskbarStatus(TaskBarSwitchAPI.ASB_STATUS.ABS_AUTOHIDE);
                    break;
            }
        }

        /// <summary>
        /// タスクバーを非表示にする
        /// </summary>
        private static void TaskBarHide(object? sender, EventArgs e)
        {
            var status = TaskBarSwitchAPI.GetTaskbarStatus();
            if (status != TaskBarSwitchAPI.ASB_STATUS.ABS_AUTOHIDE)
            {
                TaskBarSwitchAPI.SetTaskbarStatus(TaskBarSwitchAPI.ASB_STATUS.ABS_AUTOHIDE);
            }
        }

        /// <summary>
        /// タスクバーを表示する
        /// </summary>
        private static void TaskBarShow(object? sender, EventArgs e)
        {
            var status = TaskBarSwitchAPI.GetTaskbarStatus();
            if (status != TaskBarSwitchAPI.ASB_STATUS.ABS_ALWAYSONTOP)
            {
                TaskBarSwitchAPI.SetTaskbarStatus(TaskBarSwitchAPI.ASB_STATUS.ABS_ALWAYSONTOP);
            }
        }

        /// <summary>
        /// タスクバーを再起動する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RestartTaskbar(object? sender, EventArgs e)
        {
            var procName = new[]
            {
                @"explorer".ToLower(),
                @"explorer.exe".ToLower(),
                @"C:\WINDOWS\Explorer.EXE".ToLower(),
            };

            var ps = Process.GetProcessesByName(procName[0]);

            //配列から1つずつ取り出す
            foreach (var p in ps)
            {
                try
                {
                    // WMI を使用してコマンドライン引数を取得
                    var query = $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {p.Id}";
                    using var searcher = new ManagementObjectSearcher(query);
                    using var results = searcher.Get();
                    foreach (var mo in results)
                    {
                        var commandLine = mo["CommandLine"]?.ToString() ?? string.Empty;
                        if (true == procName.Contains(commandLine.ToLower().Trim().Trim('"')))
                        {
                            // タスクバーを再起動
                            p.Kill();
                            p.WaitForExit();

                            TaskBarSwitchAPI.RestartTaskbar();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"エラー: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ExitApp(object? sender, EventArgs e)
        {
            if (notifyIcon != null)
                notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
