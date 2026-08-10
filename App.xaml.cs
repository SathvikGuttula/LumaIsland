using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using LumaIsland.Models;
using LumaIsland.Services;
using LumaIsland.ViewModels;
using LumaIsland.Views;
using Application = System.Windows.Application;

namespace LumaIsland;

public partial class App : Application
{
    public static AppSettings Settings { get; private set; } = new();
    public static SettingsService SettingsService { get; private set; } = null!;
    public static MediaSessionService MediaSessionService { get; private set; } = null!;
    public static StartupService StartupService { get; private set; } = null!;

    private NotifyIcon? _trayIcon;
    private IslandWindow? _islandWindow;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        try
        {
            SettingsService = new SettingsService();
            MediaSessionService = new MediaSessionService();
            StartupService = new StartupService();

            Settings = await SettingsService.LoadAsync();
            StartupService.SetEnabled(Settings.LaunchAtStartup);

            await MediaSessionService.StartAsync();

            CreateTrayIcon();

            if (Settings.IsFirstRun)
            {
                var onboarding = new OnboardingWindow(Settings, SettingsService);
                onboarding.ShowDialog();

                Settings.IsFirstRun = false;
                await SettingsService.SaveAsync(Settings);
            }

            var vm = new IslandViewModel(Settings, SettingsService, MediaSessionService);
            _islandWindow = new IslandWindow(vm);
            MainWindow = _islandWindow;
            _islandWindow.Show();
        }
        catch (Exception ex)
        {
            LogStartupCrash(ex);

            System.Windows.MessageBox.Show(
                $"LumaIsland failed to start.\n\n{ex.Message}\n\nSee crash.log in %AppData%\\LumaIsland for details.",
                "LumaIsland Startup Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Shutdown(-1);
        }
    }

    private static void LogStartupCrash(Exception ex)
    {
        try
        {
            var settingsDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "LumaIsland");
            Directory.CreateDirectory(settingsDir);

            var path = Path.Combine(settingsDir, "crash.log");
            var sb = new StringBuilder();
            sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Startup crash");
            sb.AppendLine(ex.ToString());
            sb.AppendLine();

            File.AppendAllText(path, sb.ToString());
        }
        catch
        {
            // Avoid throwing while handling startup failures.
        }
    }

    private void CreateTrayIcon()
    {
        var icon = GetTrayIconSafe();

        _trayIcon = new NotifyIcon
        {
            Text = "LumaIsland",
            Visible = true,
            Icon = icon
        };

        var menu = new ContextMenuStrip();
        menu.Items.Add("Open Settings", null, (_, _) => OpenSettings());
        menu.Items.Add("Toggle Island", null, (_, _) => ToggleIslandVisibility());
        menu.Items.Add("Exit", null, (_, _) => Shutdown());

        _trayIcon.ContextMenuStrip = menu;
        _trayIcon.DoubleClick += (_, _) => ToggleIslandVisibility();
    }

    private static Icon GetTrayIconSafe()
    {
        var appDir = AppContext.BaseDirectory;
        var iconPath = Path.Combine(appDir, "Assets", "tray.ico");

        if (File.Exists(iconPath))
        {
            try
            {
                // Load from stream so invalid icon data can be handled gracefully.
                using var stream = File.OpenRead(iconPath);
                return new Icon(stream);
            }
            catch
            {
                // Fall back to a known-good icon instead of crashing startup.
            }
        }

        return SystemIcons.Application;
    }

    private void ToggleIslandVisibility()
    {
        if (_islandWindow == null) return;

        if (_islandWindow.IsVisible)
            _islandWindow.Hide();
        else
            _islandWindow.Show();
    }

    private void OpenSettings()
    {
        Current.Dispatcher.Invoke(() =>
        {
            var settingsWindow = new SettingsWindow(Settings, SettingsService);
            settingsWindow.Owner = _islandWindow;
            settingsWindow.ShowDialog();
        });
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _trayIcon?.Dispose();
        base.OnExit(e);
    }
}