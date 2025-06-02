using System.Threading;
using System.Threading.Tasks;
using Dalamud.Game.Command;
using Dalamud.Plugin.Services;
using WeaponTracker.Windows;

using Microsoft.Extensions.Hosting;


namespace WeaponTracker.Services;

public class CommandService(ICommandManager commandManager, MainWindow mainWindow) : IHostedService
{
    private readonly string[] commandNames = { "/weapontracker", "/wt" };
    public ICommandManager CommandManager { get; } = commandManager;
    
    public MainWindow MainWindow { get; } = mainWindow;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var commandName in commandNames)
        {
            CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Open the Weapon Tracker.",
            });
        }
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    private void OnCommand(string command, string args)
    {
        mainWindow.Toggle();
    }
}
