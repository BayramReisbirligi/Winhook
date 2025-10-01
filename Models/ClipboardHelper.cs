using Windows.ApplicationModel.DataTransfer;
namespace ReisProduction.Winhook.Models;
public static class ClipboardHelper
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
    public static bool TrySetClipboardText(bool isWinUI, string text)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(text);
            if (isWinUI)
            {
                DataPackage pkg = new();
                pkg.SetText(text);
                Clipboard.SetContent(pkg);
            }
            else
                System.Windows.Forms.Clipboard.SetText(text);
            return true;
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
    public static bool TrySetClipboardBitmap(bool isWinUI, object bitmap)
    {
        try
        {
            if (bitmap is null)
                return false;
            if (isWinUI)
            {
                if (bitmap is Windows.Storage.Streams.RandomAccessStreamReference rasr)
                {
                    DataPackage pkg = new();
                    pkg.SetBitmap(rasr);
                    Clipboard.SetContent(pkg);
                    return true;
                }
                return false;
            }
            else
            {
                if (bitmap is System.Drawing.Image img)
                {
                    System.Windows.Forms.Clipboard.SetImage(img);
                    return true;
                }
                return false;
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
    public static bool TrySetClipboardContent(bool isWinUI, object content)
    {
        try
        {
            if (content is null)
                return false;
            if (isWinUI)
            {
                if (content is DataPackage pkg)
                {
                    Clipboard.SetContent(pkg);
                    return true;
                }
                return false;
            }
            else
            {
                if (content is System.Windows.Forms.IDataObject dataObj)
                {
                    System.Windows.Forms.Clipboard.SetDataObject(dataObj, true);
                    return true;
                }
                return false;
            }
        }
        catch { return false; }
    }
}