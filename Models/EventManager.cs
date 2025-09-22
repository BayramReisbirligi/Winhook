using System.Diagnostics;
using System.Management;
namespace ReisProduction.Winhook.Models;
public partial class Winhook
{
    private static int GetInt(EventArrivedEventArgs e, string key) => int.TryParse(e.NewEvent.Properties[key]?.Value?.ToString(), out var v) ? v : 0;
    private static string GetString(EventArrivedEventArgs e, string key) => e.NewEvent.Properties[key]?.Value?.ToString() ?? "";
    public static string GetProcessName(int pid)
    {
        try
        {
            using var process = Process.GetProcessById(pid);
            return process.ProcessName;
        }
        catch
        {
            return "Unknown";
        }
    }
    public static int GetProcessID(string processName)
    {
        try
        {
            using var process = Process.GetProcessesByName(processName).FirstOrDefault();
            return process?.Id ?? -1;
        }
        catch
        {
            return -1;
        }
    }
    public static string GetClassName(EventArrivedEventArgs e) => e.NewEvent.ClassPath.ClassName;
    public static string GetProcessCreationDate(EventArrivedEventArgs e) => GetString(e, "ProcessCreationDate");
    public static string GetProcessName(EventArrivedEventArgs e) => GetString(e, "ProcessName");
    public static string GetServiceName(EventArrivedEventArgs e) => GetString(e, "ServiceName");
    public static string GetServiceState(EventArrivedEventArgs e) => GetString(e, "State");
    public static string GetFilename(EventArrivedEventArgs e) => GetString(e, "Filename");
    public static string GetModulePath(EventArrivedEventArgs e) => GetString(e, "ModulePath");
    public static string GetModuleName(EventArrivedEventArgs e) => GetString(e, "ModuleName");
    public static string GetVolumeName(EventArrivedEventArgs e) => GetString(e, "DriveName");
    public static string GetDeviceID(EventArrivedEventArgs e) => GetString(e, "DeviceID");
    public static string GetProperty(EventArrivedEventArgs e) => GetString(e, "PropertyName");
    public static string GetOldTime(EventArrivedEventArgs e) => GetString(e, "OldTime");
    public static string GetNewTime(EventArrivedEventArgs e) => GetString(e, "NewTime");
    public static string GetIPRoute(EventArrivedEventArgs e) => GetString(e, "Route");
    public static string GetOwner(EventArrivedEventArgs e) => GetString(e, "Owner");
    public static string GetAdapterName(EventArrivedEventArgs e) => GetString(e, "Description");
    public static string GetExecutablePath(EventArrivedEventArgs e) => GetString(e, "ExecutablePath");
    public static string GetCommandLine(EventArrivedEventArgs e) => GetString(e, "CommandLine");
    public static string GetSessionName(EventArrivedEventArgs e) => GetString(e, "SessionName");
    public static int GetParentProcessID(EventArrivedEventArgs e) => GetInt(e, "ParentProcessId");
    public static int GetProcessID(EventArrivedEventArgs e) => GetInt(e, "ProcessID");
    public static int GetThreadID(EventArrivedEventArgs e) => GetInt(e, "ThreadID");
    public static int GetSessionID(EventArrivedEventArgs e) => GetInt(e, "SessionId");
    public static int GetExitStatus(EventArrivedEventArgs e) => GetInt(e, "ExitStatus");
    public static int GetEventType(EventArrivedEventArgs e) => GetInt(e, "EventType");
    public static int GetEventTypeCode(EventArrivedEventArgs e) => GetInt(e, "EventTypeCode");
}