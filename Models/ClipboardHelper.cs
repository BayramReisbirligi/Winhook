using Windows.ApplicationModel.DataTransfer;
namespace ReisProduction.Winhook.Models;
public static class ClipboardHelper
{
    /// <summary>
    /// Attempts to read text from the clipboard. 
    /// Returns true if non-empty text is available, otherwise false.
    /// </summary>
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
    /// <summary>
    /// Returns true on success, otherwise false.
    /// </summary>
    /// <param name="name">Attempts to set text to the clipboard.</param>
    /// <exception cref="ArgumentNullException">Thrown if text is null or white space. And return false</exception>
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
    /// <summary>
    /// Returns true if an image is available, otherwise false.
    /// </summary>
    /// <param name="name">Attempts to read a bitmap/image from the clipboard.</param>
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
    /// <summary>
    /// Attempts to set a bitmap/image to the clipboard. 
    /// Returns true if the provided object is valid and successfully stored.
    /// </summary>
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
    /// <summary>
    /// Attempts to read the raw clipboard content object. 
    /// Returns true if content is available, otherwise false.
    /// </summary>
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
    /// <summary>
    /// Attempts to set the raw clipboard content object. 
    /// Returns true if the provided object is valid and successfully stored.
    /// </summary>
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