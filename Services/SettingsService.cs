using System.IO;
using System.Text.Json;
using LumaIsland.Models;

namespace LumaIsland.Services;

public class SettingsService
{
    private readonly string _settingsDir;
    private readonly string _settingsPath;

    public SettingsService()
    {
        _settingsDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "LumaIsland");

        _settingsPath = Path.Combine(_settingsDir, "settings.json");
    }

    public async Task<AppSettings> LoadAsync()
    {
        try
        {
            if (!Directory.Exists(_settingsDir))
                Directory.CreateDirectory(_settingsDir);

            if (!File.Exists(_settingsPath))
            {
                var defaults = new AppSettings();
                await SaveAsync(defaults);
                return defaults;
            }

            var json = await File.ReadAllTextAsync(_settingsPath);
            return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public async Task SaveAsync(AppSettings settings)
    {
        if (!Directory.Exists(_settingsDir))
            Directory.CreateDirectory(_settingsDir);

        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(_settingsPath, json);
    }
}