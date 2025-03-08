using System.Drawing;
using System.Runtime.InteropServices;

namespace TaskBarSwitch;

/// <summary>
/// タスクバーAPI
/// </summary>
public static class TaskBarSwitchAPI
{
    /// <summary>
    /// タスクバーの状態が変更されたときに発生するイベント
    /// </summary>
    public static event EventHandler? TaskbarStatusChanged;

    /// <summary>
    /// ウィンドウハンドルを取得する
    /// </summary>
    /// <param name="lpClassName"></param>
    /// <param name="lpWindowName"></param>
    /// <returns></returns>
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    extern static nint FindWindow(
        [MarshalAs(UnmanagedType.LPWStr), In] string lpClassName,
        [MarshalAs(UnmanagedType.LPWStr), In] string? lpWindowName
    );

    /// <summary>
    /// appbar メッセージをシステムに送信する
    /// </summary>
    /// <param name="dwMessage"></param>
    /// <param name="pData"></param>
    /// <returns></returns>
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    extern static nuint SHAppBarMessage(uint dwMessage, ref AppBarData pData);

    /// <summary>
    /// Windows タスクバーの自動表示と常時表示の状態を取得する
    /// </summary>
    const uint ABM_GETSTATE = 0x00000004;

    /// <summary>
    /// Windows タスクバーの自動表示と常時表示の状態を設定する
    /// </summary>
    const uint ABM_SETSTATE = 0x0000000a;

    /// <summary>
    /// ステータスバーの状態
    /// </summary>
    public enum ASB_STATUS
    {
        /// <summary>
        /// 自動非表示
        /// </summary>
        ABS_AUTOHIDE = 0x01,

        /// <summary>
        /// 常時表示
        /// </summary>
        ABS_ALWAYSONTOP = 0x02,

    }

    /// <summary>
    /// APPBARDATA 構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct AppBarData
    {
        /// <summary>
        /// 構造体のサイズ
        /// </summary>
        public UInt32 cbSize;
        /// <summary>
        /// ウィンドウハンドル
        /// </summary>
        public IntPtr hWnd;
        /// <summary>
        /// メッセージ
        /// </summary>
        public UInt32 uCallbackMessage;
        /// <summary>
        /// タスクバーの位置
        /// </summary>
        public UInt32 uEdge;
        /// <summary>
        /// タスクバーの矩形
        /// </summary>
        public Rectangle rc;
        /// <summary>
        /// パラメータ
        /// </summary>
        public Int32 lParam;
    }

    /// <summary>
    /// タスクバーの状態を取得する
    /// </summary>
    /// <returns>
    /// <para>ABS_AUTOHIDE: 自動的に隠す</para>
    /// <para>ABS_ALWAYSONTOP: 常に表示する</para>
    /// </returns>
    public static ASB_STATUS GetTaskbarStatus()
    {
        var data = new AppBarData { hWnd = FindWindow("System_TrayWnd", null) };
        data.cbSize = (uint)Marshal.SizeOf(data);
        return (ASB_STATUS)SHAppBarMessage(ABM_GETSTATE, ref data);
    }

    /// <summary>
    /// タスクバーの状態を変更する
    /// </summary>
    /// <param name="option">状態</param>
    /// <returns>
    /// 変更後の状態
    /// </returns>
    public static ASB_STATUS SetTaskbarStatus(ASB_STATUS option)
    {
        var data = new AppBarData
        {
            hWnd = FindWindow("System_TrayWnd", null),
            lParam = (int)option
        };
        data.cbSize = (uint)Marshal.SizeOf(data);
        var result = (ASB_STATUS)SHAppBarMessage(ABM_SETSTATE, ref data);

        // イベントを発火させる
        TaskbarStatusChanged?.Invoke(null, EventArgs.Empty);

        return result;
    }
}
