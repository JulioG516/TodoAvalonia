using Microsoft.Extensions.DependencyInjection;
using TodoAvalonia.ViewModels;

namespace TodoAvalonia.Services;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddTransient<ToDoListFileService>();
        collection.AddTransient<MainWindowViewModel>();
    }
}