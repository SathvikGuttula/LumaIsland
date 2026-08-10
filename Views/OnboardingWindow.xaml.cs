using System.Windows;
using LumaIsland.Models;
using LumaIsland.Services;

namespace LumaIsland.Views;

public partial class OnboardingWindow : Window
{
    private readonly AppSettings _settings;
    private readonly SettingsService _settingsService;

    public OnboardingWindow(AppSettings settings, SettingsService settingsService)
    {
        InitializeComponent();
        _settings = settings;
        _settingsService = settingsService;
    }

    private void Launch_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void Settings_Click(object sender, RoutedEventArgs e)
    {
        var settingsWindow = new SettingsWindow(_settings, _settingsService)
        {
            Owner = this
        };

        settingsWindow.ShowDialog();
    }
}