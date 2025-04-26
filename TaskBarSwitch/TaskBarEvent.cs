using System;
using System.Management;

namespace TaskBarSwitch;

public partial class TaskBar : Form, IDisposable
{
    private ManagementEventWatcher _watcher; // WMIイベントウォッチャー

    /// <summary>
    /// プロセス生成イベント監視オブジェクトを初期化します。
    /// </summary>
    public void ProcessCreationEventDetector()
    {
        // プロセス生成イベント (Win32_ProcessStartTrace) を購読するためのWQLクエリ
        // "__InstanceCreationEvent" は新しいインスタンスが作成されたことを示すイベントです。
        // "TargetInstance isa 'Win32_Process'" は、対象のインスタンスがWin32_Processクラスであることを指定します。
        // "WITHIN 1" は、イベントの発生をポーリングする間隔を秒単位で指定します。
        // これにより、イベント発生後、指定した時間内に通知が来ます。リアルタイムに近い検知が可能になります。
        string query = "SELECT * FROM __InstanceCreationEvent " + 
                       "WITHIN 1 " + 
                       "WHERE TargetInstance isa 'Win32_Process'"
                        +
                       " and TargetInstance.Name = 'dllhost.exe'";

        _watcher = new ManagementEventWatcher(query);
        _watcher.EventArrived += Watcher_EventArrived; // イベントハンドラを設定
    }

    /// <summary>
    /// プロセス生成イベントの監視を開始します。
    /// </summary>
    public void StartMonitoring()
    {
        // Console.WriteLine("プロセス生成イベントの監視を開始します...");
        try
        {
            ProcessCreationEventDetector();
            _watcher.Start(); // 監視を開始
            // Console.WriteLine("監視中です。新しいプロセスを起動してみてください。");
        }
        catch (ManagementException ex)
        {
            // Console.WriteLine($"WMI監視の開始中にエラーが発生しました: {ex.Message}");
            // Console.WriteLine("管理者権限が必要な場合があります。");
        }
    }

    /// <summary>
    /// プロセス生成イベントの監視を停止します。
    /// </summary>
    public void StopMonitoring()
    {
        // Console.WriteLine("プロセス生成イベントの監視を停止します。");
        _watcher.Stop(); // 監視を停止
    }

    /// <summary>
    /// プロセス生成イベントが発生したときに呼び出されるイベントハンドラ。
    /// </summary>
    private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
    {
        SetTaskbarIcon();
    }

    /// <summary>
    /// リソースを解放します。
    /// </summary>
    public new void Dispose()
    {
        StopMonitoring(); // 監視を停止
        _watcher?.Dispose(); // ウォッチャーのリソースを解放
        base.Dispose(); // 基底クラスのDisposeメソッドを呼び出す

        GC.SuppressFinalize(this); // ファイナライザを抑制
    }
}
