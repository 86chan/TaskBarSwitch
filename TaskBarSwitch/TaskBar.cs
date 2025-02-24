using System.Drawing;
using System.Windows.Forms;
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
        private NotifyIcon notifyIcon;

        /// <summary>
        /// コンテキストメニュー
        /// </summary>
        private ContextMenuStrip contextMenu;

        /// <summary>
        /// 自動非表示アイコン
        /// </summary>
        private SKSvg AUTOHIDEIcon;

        /// <summary>
        /// 常時表示アイコン
        /// </summary>
        private SKSvg ALWAYSONTOPIcon;

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
            contextMenu.Items.Add("表示", null, TaskBarShow);
            contextMenu.Items.Add("非表示", null, TaskBarHide);
            contextMenu.Items.Add("切り替え", null, SwitchShowHide);
            contextMenu.Items.Add("終了", null, ExitApp);

            // NotifyIconの設定
            notifyIcon.Icon = ChangeAccentColorIcon(AUTOHIDEIcon);
            notifyIcon.ContextMenuStrip = contextMenu;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += SwitchShowHide;
            notifyIcon.Text = "TaskBarSwitch";

            // フォームを非表示にする
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Load += (s, e) => this.Hide();

            // タスクバーの状態が変更されたときにアイコンを変更する
            TaskBarSwitchAPI.TaskbarStatusChanged += (s, e) => SetTaskbarIcon();
        }

        /// <summary>
        /// タスクバーの表示切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchShowHide(object? sender, EventArgs e)
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
        private void TaskBarHide(object? sender, EventArgs e)
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
        private void TaskBarShow(object? sender, EventArgs e)
        {
            var status = TaskBarSwitchAPI.GetTaskbarStatus();
            if (status != TaskBarSwitchAPI.ASB_STATUS.ABS_ALWAYSONTOP)
            {
                TaskBarSwitchAPI.SetTaskbarStatus(TaskBarSwitchAPI.ASB_STATUS.ABS_ALWAYSONTOP);
            }
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitApp(object? sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}
