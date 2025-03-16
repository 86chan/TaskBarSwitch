using Microsoft.Win32;
using System.Reflection;
using SkiaSharp;
using Svg.Skia;

namespace TaskBarSwitch
{
    /// <summary>
    /// タスクバー
    /// </summary>
    public partial class TaskBar : Form
    {
        /// <summary>
        /// タスクバーのアイコンを設定します。
        /// </summary>
        private void SetTaskbarIcon()
        {
            var status = TaskBarSwitchAPI.GetTaskbarStatus();
            switch (status)
            {
                case TaskBarSwitchAPI.ASB_STATUS.ABS_AUTOHIDE:
                    notifyIcon.Icon = ChangeAccentColorIcon(AUTOHIDEIcon);
                    break;
                case TaskBarSwitchAPI.ASB_STATUS.ABS_ALWAYSONTOP:
                default:
                    notifyIcon.Icon = ChangeAccentColorIcon(ALWAYSONTOPIcon);
                    break;
            }
        }

        /// <summary>
        /// 埋め込みリソースからSVGをアイコンに変換します。
        /// </summary>
        /// <param name="resourceName">リソース名</param>
        /// <returns>変換されたアイコン</returns>
        /// <exception cref="FileNotFoundException">リソースが見つからない場合にスローされます。</exception>
        private SKSvg ConvertSvgToIconEmbedded(string resourceName)
        {
            var svg = new SKSvg();
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Resource not found: " + resourceName);
                }

                svg.Load(stream);
            }

            return svg;
        }

        /// <summary>
        /// SVGファイルをアイコンに変換します。
        /// </summary>
        /// <param name="svgPath">SVGファイルのパス</param>
        /// <returns>変換されたアイコン</returns>
        private static SKSvg ConvertSvgToIcon(string svgPath)
        {
            var svg = new SKSvg();
            using (var stream = new FileStream(svgPath, FileMode.Open))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException("Resource not found: " + svgPath);
                }

                svg.Load(stream);
            }

            return svg;
        }

        /// <summary>
        /// ストリームからSVGを読み込み、アクセントカラーを適用したアイコンに変換します。
        /// </summary>
        /// <param name="stream">SVGデータを含むストリーム</param>
        /// <returns>変換されたアイコン</returns>
        private static Icon ChangeAccentColorIcon(SKSvg svg)
        {
            // Windowsのアクセントカラーを取得
            var accentColor = GetWindowsAccentColor();

            if (svg.Picture == null)
            {
                throw new InvalidOperationException("SVG picture is null.");
            }

            var bitmap = new SKBitmap((int)svg.Picture.CullRect.Width, (int)svg.Picture.CullRect.Height);
            using (var canvas = new SKCanvas(bitmap))
            {
                #if DEBUG
                canvas.Clear(SKColors.WhiteSmoke); // Debug時は背景色を設定
                #else
                canvas.Clear(SKColors.Transparent);
                #endif
                
                var paint = new SKPaint
                {
                    ColorFilter = SKColorFilter.CreateBlendMode(accentColor, SKBlendMode.SrcIn)
                };
                canvas.DrawPicture(svg.Picture, paint);
            }

            using (var ms = new MemoryStream())
            {
                bitmap.Encode(ms, SKEncodedImageFormat.Png, 100);
                ms.Seek(0, SeekOrigin.Begin);

                using (var bmp = new Bitmap(ms))
                {
                    return Icon.FromHandle(bmp.GetHicon());
                }
            }
        }

        /// <summary>
        /// Windowsのアクセントカラーを取得します。
        /// </summary>
        /// <returns>取得したアクセントカラー</returns>
        private static SKColor GetWindowsAccentColor()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM");
            if (key != null)
            {
                var value = key.GetValue("AccentColor");
                if (value != null)
                {
                    var colorValue = (int)value;
                    return new SKColor(
                        (byte)colorValue,
                        (byte)(colorValue >> 8),
                        (byte)(colorValue >> 16),
                        (byte)(colorValue >> 24)
                    );
                }
            }
            return SKColors.Black; // デフォルトカラー
        }
    }
}
