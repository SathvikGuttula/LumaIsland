using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Media.Imaging;
using LumaIsland.Models;
using Windows.Media.Control;
using Windows.Storage.Streams;

namespace LumaIsland.Services;

public class MediaSessionService
{
    private GlobalSystemMediaTransportControlsSessionManager? _manager;
    private GlobalSystemMediaTransportControlsSession? _currentSession;

    public event EventHandler<MediaState>? MediaChanged;

    public async Task StartAsync()
    {
        _manager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
        _manager.CurrentSessionChanged += Manager_CurrentSessionChanged;
        _manager.SessionsChanged += Manager_SessionsChanged;

        await HookCurrentSessionAsync();
    }

    private async void Manager_SessionsChanged(GlobalSystemMediaTransportControlsSessionManager sender, SessionsChangedEventArgs args)
    {
        await HookCurrentSessionAsync();
    }

    private async void Manager_CurrentSessionChanged(GlobalSystemMediaTransportControlsSessionManager sender, CurrentSessionChangedEventArgs args)
    {
        await HookCurrentSessionAsync();
    }

    private async Task HookCurrentSessionAsync()
    {
        if (_currentSession != null)
        {
            _currentSession.MediaPropertiesChanged -= CurrentSession_MediaPropertiesChanged;
            _currentSession.PlaybackInfoChanged -= CurrentSession_PlaybackInfoChanged;
            _currentSession.TimelinePropertiesChanged -= CurrentSession_TimelinePropertiesChanged;
        }

        _currentSession = _manager?.GetCurrentSession();

        if (_currentSession == null)
        {
            MediaChanged?.Invoke(this, new MediaState());
            return;
        }

        _currentSession.MediaPropertiesChanged += CurrentSession_MediaPropertiesChanged;
        _currentSession.PlaybackInfoChanged += CurrentSession_PlaybackInfoChanged;
        _currentSession.TimelinePropertiesChanged += CurrentSession_TimelinePropertiesChanged;

        await PublishCurrentStateAsync();
    }

    private async void CurrentSession_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
    {
        await PublishCurrentStateAsync();
    }

    private async void CurrentSession_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
    {
        await PublishCurrentStateAsync();
    }

    private async void CurrentSession_TimelinePropertiesChanged(GlobalSystemMediaTransportControlsSession sender, TimelinePropertiesChangedEventArgs args)
    {
        await PublishCurrentStateAsync();
    }

    private async Task PublishCurrentStateAsync()
    {
        if (_currentSession == null)
        {
            MediaChanged?.Invoke(this, new MediaState());
            return;
        }

        try
        {
            var mediaProps = await _currentSession.TryGetMediaPropertiesAsync();
            var playbackInfo = _currentSession.GetPlaybackInfo();

            var state = new MediaState
            {
                Title = string.IsNullOrWhiteSpace(mediaProps.Title) ? "Nothing playing" : mediaProps.Title,
                Artist = string.IsNullOrWhiteSpace(mediaProps.Artist) ? "Unknown artist" : mediaProps.Artist,
                Album = mediaProps.AlbumTitle ?? "",
                IsPlaying = playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing,
                Artwork = await LoadArtworkAsync(mediaProps.Thumbnail),
                SourceAppId = _currentSession.SourceAppUserModelId ?? ""
            };

            MediaChanged?.Invoke(this, state);
        }
        catch
        {
            MediaChanged?.Invoke(this, new MediaState());
        }
    }

    public async Task TogglePlayPauseAsync()
    {
        if (_currentSession != null)
            await _currentSession.TryTogglePlayPauseAsync();
    }

    public async Task NextAsync()
    {
        if (_currentSession != null)
            await _currentSession.TrySkipNextAsync();
    }

    public async Task PreviousAsync()
    {
        if (_currentSession != null)
            await _currentSession.TrySkipPreviousAsync();
    }

    private static async Task<BitmapImage?> LoadArtworkAsync(IRandomAccessStreamReference? thumbnail)
    {
        if (thumbnail == null) return null;

        try
        {
            using var stream = await thumbnail.OpenReadAsync();
            using var netStream = stream.AsStreamForRead();
            using var memory = new MemoryStream();

            await netStream.CopyToAsync(memory);
            memory.Position = 0;

            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = memory;
            image.EndInit();
            image.Freeze();

            return image;
        }
        catch
        {
            return null;
        }
    }
}