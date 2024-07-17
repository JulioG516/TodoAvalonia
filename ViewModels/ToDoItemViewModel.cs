using CommunityToolkit.Mvvm.ComponentModel;
using TodoAvalonia.Models;

namespace TodoAvalonia.ViewModels;

// Needs to be partial to use SourceGenerator
public partial class ToDoItemViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isChecked;

    [ObservableProperty] private string? _content;

    /// <summary>
    /// Creates a new blank ToDoItemViewModel
    /// </summary>
    public ToDoItemViewModel()
    {
    }

    /// <summary>
    /// Creates a new ToDoItemViewModel for the given <see cref="TodoAvalonia.Models.ToDoItem"/>
    /// </summary>
    /// <param name="item">The item to load</param>
    public ToDoItemViewModel(ToDoItem item)
    {
        IsChecked = item.IsChecked;
        Content = item.Content;
    }
    
    /// <summary>
    /// Gets a ToDoItem of this ViewModel
    /// </summary>
    /// <returns>The ToDoItem</returns>
    public ToDoItem GetToDoItem()
    {
        return new ToDoItem()
        {
            IsChecked = this.IsChecked,
            Content = this.Content
        };
    }
}