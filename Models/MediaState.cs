using System.Windows.Media;

namespace LumaIsland.Models;

public class MediaState
{
    public string Title { get; set; } = "Nothing playing";
    public string Artist { get; set; } = "Open Spotify, YouTube, Apple Music, etc.";
    public string Album { get; set; } = "";
    public bool IsPlaying { get; set; }
    public ImageSource? Artwork { get; set; }
    public string SourceAppId { get; set; } = "";
}