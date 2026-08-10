namespace LumaIsland.Models;

public class AppSettings
{
    public bool IsFirstRun { get; set; } = true;
    public bool LaunchAtStartup { get; set; } = true;
    public bool ExpandOnHover { get; set; } = true;

    public bool ShowMediaWidget { get; set; } = true;
    public bool ShowCalendarWidget { get; set; } = true;

    public double ExpandedWidth { get; set; } = 980;
    public double ExpandedHeight { get; set; } = 180;

    public double CollapsedWidth { get; set; } = 118;
    public double CollapsedHeight { get; set; } = 18;

    public string AccentHex { get; set; } = "#7C3AED";
}