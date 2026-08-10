using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LumaIsland.Models;
using LumaIsland.Services;
using Application = System.Windows.Application;

namespace LumaIsland.ViewModels;

public partial class IslandViewModel : ObservableObject
{
    private readonly AppSettings _settings;
    private readonly SettingsService _settingsService;
    private readonly MediaSessionService _mediaSessionService;

    public AppSettings Settings => _settings;
    public ObservableCollection<CalendarDay> WeekDays { get; } = new();

    [ObservableProperty]
    private bool isExpanded;

    [ObservableProperty]
    private string trackTitle = "Nothing playing";

    [ObservableProperty]
    private string artist = "Waiting for media session";

    [ObservableProperty]
    private string album = "";

    [ObservableProperty]
    private ImageSource? artwork;

    [ObservableProperty]
    private bool isPlaying;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private string daySummary = "Nothing for today";

    public string MonthLabel => SelectedDate.ToString("MMM");
    public string FullMonthLabel => SelectedDate.ToString("MMMM yyyy");
    public string SelectedDayLabel => SelectedDate.ToString("dddd, dd");

    public IslandViewModel(AppSettings settings, SettingsService settingsService, MediaSessionService mediaSessionService)
    {
        _settings = settings;
        _settingsService = settingsService;
        _mediaSessionService = mediaSessionService;

        BuildWeek();
        UpdateSummary();

        _mediaSessionService.MediaChanged += MediaSessionService_MediaChanged;
    }

    private void MediaSessionService_MediaChanged(object? sender, MediaState e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            TrackTitle = e.Title;
            Artist = e.Artist;
            Album = e.Album;
            Artwork = e.Artwork;
            IsPlaying = e.IsPlaying;
        });
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        OnPropertyChanged(nameof(MonthLabel));
        OnPropertyChanged(nameof(FullMonthLabel));
        OnPropertyChanged(nameof(SelectedDayLabel));
        BuildWeek();
        UpdateSummary();
    }

    private void BuildWeek()
    {
        WeekDays.Clear();

        int offset = ((int)SelectedDate.DayOfWeek + 6) % 7;
        var monday = SelectedDate.AddDays(-offset);

        for (int i = 0; i < 7; i++)
        {
            var date = monday.AddDays(i);
            WeekDays.Add(new CalendarDay
            {
                ShortDay = date.ToString("ddd").Substring(0, 1).ToUpperInvariant(),
                DayNumber = date.Day,
                Date = date,
                IsSelected = date.Date == SelectedDate.Date
            });
        }
    }

    private void UpdateSummary()
    {
        DaySummary = SelectedDate.Date == DateTime.Today
            ? "Nothing for today"
            : $"No events on {SelectedDate:dddd}";
    }

    [RelayCommand]
    private void ToggleExpanded()
    {
        IsExpanded = !IsExpanded;
    }

    [RelayCommand]
    private void Collapse()
    {
        IsExpanded = false;
    }

    [RelayCommand]
    private void Expand()
    {
        IsExpanded = true;
    }

    [RelayCommand]
    private async Task PlayPauseAsync()
    {
        await _mediaSessionService.TogglePlayPauseAsync();
    }

    [RelayCommand]
    private async Task PreviousAsync()
    {
        await _mediaSessionService.PreviousAsync();
    }

    [RelayCommand]
    private async Task NextAsync()
    {
        await _mediaSessionService.NextAsync();
    }

    [RelayCommand]
    private void SelectDay(CalendarDay? day)
    {
        if (day == null) return;
        SelectedDate = day.Date;
    }

    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        await _settingsService.SaveAsync(_settings);
    }
}