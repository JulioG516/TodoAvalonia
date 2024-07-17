using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TodoAvalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Gets a collection of <see cref="TodoAvalonia.Models.ToDoItem"/> TodoItem which allows adding and removing items
    /// </summary>
    /// <returns></returns>
    public ObservableCollection<ToDoItemViewModel> ToDoItems { get; } = new();

    /// <summary>
    /// Gets or set the content for new Items to add. If this string is not empty, the AddItemCommand will be enabled automatically
    /// </summary>
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    private string? _newItemContent;

    /// <summary>
    /// Returns if a new Item can be added. We require to have the NewItem some Text
    /// </summary>
    private bool CanAddItem() => !string.IsNullOrEmpty(NewItemContent);


    // If we annotate a void or a Task with the [RelayCommand-attribute], a new RelayCommand will be generated for us

    /// <summary>
    /// This command is used to add a new Item to the List
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanAddItem))]
    private void AddItem()
    {
        // Add a new item to the list
        ToDoItems.Add(new ToDoItemViewModel() { Content = NewItemContent });

        // Reset the New Item Content
        NewItemContent = null;
    }

    /// <summary>
    /// Removes the given item from the list
    /// </summary>
    /// <param name="item"></param>
    [RelayCommand]
    private void RemoveItem(ToDoItemViewModel item)
    {
        // Remove the given item from the list
        ToDoItems.Remove(item);
    }
}