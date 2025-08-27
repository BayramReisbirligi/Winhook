using Windows.ApplicationModel.DataTransfer;
namespace ReisProduction.Winhook.Models;
internal static class ClipboardHelper
{
    public static bool TryGetClipboardText(bool isWinUI, out string? text)
    {
        text = null;
        try
        {
            if (isWinUI)
            {
                var content = Clipboard.GetContent();
                if (!content.Contains(StandardDataFormats.Text))
                    return false;
                text = content.GetTextAsync().GetAwaiter().GetResult();
            }
            else
            {
                if (!System.Windows.Forms.Clipboard.ContainsText())
                    return false;
                text = System.Windows.Forms.Clipboard.GetText();
            }
            return !string.IsNullOrWhiteSpace(text);
        }
        catch { return false; }
    }
    public static bool TryGetClipboardBitmap(bool isWinUI, out object? bitmap)
    {
        bitmap = null;
        try
        {
            if (isWinUI)
            {
                var content = Clipboard.GetContent();
                if (!content.Contains(StandardDataFormats.Bitmap))
                    return false;
                bitmap = content.GetBitmapAsync().GetAwaiter().GetResult();
                return bitmap is not null;
            }
            else
            {
                if (!System.Windows.Forms.Clipboard.ContainsImage())
                    return false;
                bitmap = System.Windows.Forms.Clipboard.GetImage();
                return bitmap is not null;
            }
        }
        catch { return false; }
    }
    public static bool TryGetClipboardContent(bool isWinUI, out object? content)
    {
        try
        {
            if (isWinUI)
            {
                content = Clipboard.GetContent();
                return content is not null;
            }
            else
            {
                content = System.Windows.Forms.Clipboard.GetDataObject();
                return content is not null;
            }
        }
        catch
        {
            content = null;
            return false;
        }
    }
}