using System.Diagnostics;
using System.Management;
namespace ReisProduction.Winhook.Models;
public partial class Winhook
{
    /// <summary>
    /// Parses an integer property from the WMI event or returns 0 if not found.
    /// </summary>
    private static int GetInt(EventArrivedEventArgs e, string key) => int.TryParse(e.NewEvent.Properties[key]?.Value?.ToString(), out var v) ? v : 0;
    /// <summary>
    /// Parses a string property from the WMI event or returns an empty string if not found.
    /// </summary>
    private static string GetString(EventArrivedEventArgs e, string key) => e.NewEvent.Properties[key]?.Value?.ToString() ?? "";
    /// <summary>
    /// Gets the name of a process given its Process ID (PID).
    /// </summary>
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
    /// <summary>
    /// Gets the Process ID (PID) of a process given its name.
    /// </summary>
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
    /// <summary>
    /// Gets the class name of the WMI event.
    /// </summary>
    public static string GetClassName(EventArrivedEventArgs e) => e.NewEvent.ClassPath.ClassName;
    /// <summary>
    /// Gets the creation date of the process from the event.
    /// </summary>
    public static string GetProcessCreationDate(EventArrivedEventArgs e) => GetString(e, "ProcessCreationDate");
    /// <summary>
    /// Gets the name of the process associated with the event.
    /// </summary>
    public static string GetProcessName(EventArrivedEventArgs e) => GetString(e, "ProcessName");
    /// <summary>
    /// Gets the name of the service associated with the event.
    /// </summary>
    public static string GetServiceName(EventArrivedEventArgs e) => GetString(e, "ServiceName");
    /// <summary>
    /// Gets the state of the service associated with the event.
    /// </summary>
    public static string GetServiceState(EventArrivedEventArgs e) => GetString(e, "State");
    /// <summary>
    /// Gets the filename associated with the event.
    /// </summary>
    public static string GetFilename(EventArrivedEventArgs e) => GetString(e, "Filename");
    /// <summary>
    /// Gets the path of the module associated with the event.
    /// </summary>
    public static string GetModulePath(EventArrivedEventArgs e) => GetString(e, "ModulePath");
    /// <summary>
    /// Gets the name of the module associated with the event.
    /// </summary>
    public static string GetModuleName(EventArrivedEventArgs e) => GetString(e, "ModuleName");
    /// <summary>
    /// Gets the volume name (drive letter) associated with the event.
    /// </summary>
    public static string GetVolumeName(EventArrivedEventArgs e) => GetString(e, "DriveName");
    /// <summary>
    /// Gets the device ID associated with the event.
    /// </summary>
    public static string GetDeviceID(EventArrivedEventArgs e) => GetString(e, "DeviceID");
    /// <summary>
    /// Gets the property name that changed in a WMI event.
    /// </summary>
    public static string GetProperty(EventArrivedEventArgs e) => GetString(e, "PropertyName");
    /// <summary>
    /// Gets the old value of a property that changed in a WMI event.
    /// </summary>
    public static string GetOldTime(EventArrivedEventArgs e) => GetString(e, "OldTime");
    /// <summary>
    /// Gets the new value of a property that changed in a WMI event.
    /// </summary>
    public static string GetNewTime(EventArrivedEventArgs e) => GetString(e, "NewTime");
    /// <summary>
    /// Gets the IP route or address associated with the event.
    /// </summary>
    public static string GetIPRoute(EventArrivedEventArgs e) => GetString(e, "Route");
    /// <summary>
    /// Gets the owner or MAC address associated with the event.
    /// </summary>
    public static string GetOwner(EventArrivedEventArgs e) => GetString(e, "Owner");
    /// <summary>
    /// Gets the adapter name associated with the event.
    /// </summary>
    public static string GetAdapterName(EventArrivedEventArgs e) => GetString(e, "Description");
    /// <summary>
    /// Gets the executable path of the process associated with the event.
    /// </summary>
    public static string GetExecutablePath(EventArrivedEventArgs e) => GetString(e, "ExecutablePath");
    /// <summary>
    /// Gets the command line used to start the process associated with the event.
    /// </summary>
    public static string GetCommandLine(EventArrivedEventArgs e) => GetString(e, "CommandLine");
    /// <summary>
    /// Gets the session name associated with the event.
    /// </summary>
    public static string GetSessionName(EventArrivedEventArgs e) => GetString(e, "SessionName");
    /// <summary>
    /// Gets the parent process ID (PPID) of the process associated with the event.
    /// </summary>
    public static int GetParentProcessID(EventArrivedEventArgs e) => GetInt(e, "ParentProcessId");
    /// <summary>
    /// Gets the Process ID (PID) of the process associated with the event.
    /// </summary>
    public static int GetProcessID(EventArrivedEventArgs e) => GetInt(e, "ProcessID");
    /// <summary>
    /// Gets the Thread ID of the thread associated with the event.
    /// </summary>
    public static int GetThreadID(EventArrivedEventArgs e) => GetInt(e, "ThreadID");
    /// <summary>
    /// Gets the Session ID associated with the event.
    /// </summary>
    public static int GetSessionID(EventArrivedEventArgs e) => GetInt(e, "SessionId");
    /// <summary>
    /// Gets the exit status code of the process associated with the event.
    /// </summary>
    public static int GetExitStatus(EventArrivedEventArgs e) => GetInt(e, "ExitStatus");
    /// <summary>
    /// Gets the event type associated with the event.
    /// </summary>
    public static int GetEventType(EventArrivedEventArgs e) => GetInt(e, "EventType");
    /// <summary>
    /// Gets the event type code associated with the event.
    /// </summary>
    public static int GetEventTypeCode(EventArrivedEventArgs e) => GetInt(e, "EventTypeCode");
}