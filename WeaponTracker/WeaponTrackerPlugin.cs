using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.IO;
using System.Windows.Input;

using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

using WeaponTracker.Windows;
using WeaponTracker.Services;

using Lumina;
using Lumina.Excel;
using Lumina.Excel.Sheets;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WeaponTracker;

public class WeaponTrackerPlugin : IDalamudPlugin
{
/*public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("WeaponTracker");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }*/
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    public Configuration Configuration { get; init; }
    
    private CommandService CommandService { get; init; }
    
    public readonly WindowSystem WindowSystem = new("WeaponTracker");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }
    public WeaponTrackerPlugin() 
    {
        
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        // you might normally want to embed resources and load them from the manifest stream
        // var goatImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");

        // ConfigWindow = new ConfigWindow(this);
       //  MainWindow = new MainWindow(this, goatImagePath);
        
        CommandService = new CommandService(CommandManager, MainWindow);

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);

        PluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;

        // Add a simple message to the log with level set to information
        // Use /xllog to open the log window in-game
        // Example Output: 00:57:54.959 | INF | [WeaponTracker] ===A cool log message from Sample Plugin===
        // Log.Information($"===A cool log message from {PluginInterface.Manifest.Name}===");
        
    }
        
    private List<Type> HostedServices { get; } = new()
    {
        typeof(CommandService),
    };

    public List<Type> GetHostedServices()
    {
        var hostedServices = HostedServices.ToList();
        Dictionary<Type, Type> replacements = new();
        ReplaceHostedServices(replacements);
        foreach (var replacement in replacements)
        {
            hostedServices.Remove(replacement.Key);
            hostedServices.Add(replacement.Value);       
        }
        return hostedServices;
    }

    private void ReplaceHostedServices(Dictionary<Type, Type> replacements)
    {
        // Add replacements here
    }
    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();
    private void ConfigureServices(IServiceCollection services)
    {
    }
}
