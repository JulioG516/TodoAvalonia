using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using TodoAvalonia.Services;
using TodoAvalonia.ViewModels;
using TodoAvalonia.Views;

namespace TodoAvalonia;

public partial class App : Application
{
    // This is a refenrece to our MainWindowViewModel which we use to save the list
    // on app shutdown

    private MainWindowViewModel _mainViewModel;
    private IServiceProvider _services;
    private ToDoListFileService _toDoListFileService;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        // CommunityToolkit, line below is needed to remove Avalonia data validation
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        ConfigureServiceProvider();
        
        _mainViewModel = _services.GetRequiredService<MainWindowViewModel>();
        _toDoListFileService = _services.GetRequiredService<ToDoListFileService>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow
            {
                DataContext = _mainViewModel,
            };

            // Listen to the ShutdownRequested-event
            desktop.ShutdownRequested += DesktopOnShutdownRequest;

            await InitMainWindowViewModelAsnyc();
        }

        base.OnFrameworkInitializationCompleted();
    }

    // We want to save our ToDoList before we actually shutdown the App. As File I/O is async, we need to wait until file is closed
    // before we can actually close this window

    private bool _canClose; // flag used to check if window is allowed to close or not

    private async void DesktopOnShutdownRequest(object? sender, ShutdownRequestedEventArgs e)
    {
        e.Cancel = !_canClose; // cancel closing event first time

        if (!_canClose)
        {
            // To save the items, we map them to the ToDoItem-Model which is better suited for I/O operations

            var itemsToSave = _mainViewModel.ToDoItems.Select(item => item.GetToDoItem());
            await _toDoListFileService.SaveToFileAsync(itemsToSave);

            // set canClose to true and call the close window again
            _canClose = true;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }

    // Load data from disc
    private async Task InitMainWindowViewModelAsnyc()
    {
        // get the items to load
        var itemsLoaded = await _toDoListFileService.LoadFileAsync();

        if (itemsLoaded is not null)
        {
            foreach (var item in itemsLoaded)
            {
                _mainViewModel.ToDoItems.Add(new ToDoItemViewModel(item));
            }
        }
    }


    private void ConfigureServiceProvider()
    {
        var services = ConfigureServices();
        _services = services.BuildServiceProvider();
    }

    private static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddScoped<MainWindow>();
        services.AddScoped<ToDoListFileService>();
        services.AddScoped<MainWindowViewModel>();
        
        
        return services;
    }
}