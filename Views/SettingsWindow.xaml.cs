using System.Windows;
using LumaIsland.Models;
using LumaIsland.Services;

namespace LumaIsland.Views;

public partial class SettingsWindow : Window
{
    private readonly AppSettings _settings;
    private readonly SettingsService _settingsService;

    public SettingsWindow(AppSettings settings, SettingsService settingsService)
    {
        InitializeComponent();
        _settings = settings;
        _settingsService = settingsService;

        StartupCheck.IsChecked = _settings.LaunchAtStartup;
        HoverExpandCheck.IsChecked = _settings.ExpandOnHover;
        MediaWidgetCheck.IsChecked = _settings.ShowMediaWidget;
        CalendarWidgetCheck.IsChecked = _settings.ShowCalendarWidget;

        ExpandedWidthSlider.Value = _settings.ExpandedWidth;
        CollapsedWidthSlider.Value = _settings.CollapsedWidth;
        CollapsedHeightSlider.Value = _settings.CollapsedHeight;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _settings.LaunchAtStartup = StartupCheck.IsChecked == true;
            _settings.ExpandOnHover = HoverExpandCheck.IsChecked == true;
            _settings.ShowMediaWidget = MediaWidgetCheck.IsChecked == true;
            _settings.ShowCalendarWidget = CalendarWidgetCheck.IsChecked == true;

            _settings.ExpandedWidth = ExpandedWidthSlider.Value;
            _settings.CollapsedWidth = CollapsedWidthSlider.Value;
            _settings.CollapsedHeight = CollapsedHeightSlider.Value;

            await _settingsService.SaveAsync(_settings);
            App.StartupService.SetEnabled(_settings.LaunchAtStartup);

            Close();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(this,
                $"Could not save settings.\n\n{ex.Message}",
                "LumaIsland",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}