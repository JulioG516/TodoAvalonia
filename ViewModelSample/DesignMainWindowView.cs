﻿using TodoAvalonia.ViewModels;

namespace TodoAvalonia.ViewModelSample;

public class DesignMainWindowView : MainWindowViewModel
{
    public DesignMainWindowView()
    {
        ToDoItems.Add(new ToDoItemViewModel()
        {
            Content = "Buy some coffee",
            IsChecked = false
        });
        
        ToDoItems.Add(new ToDoItemViewModel()
        {
            Content = "Learn Avalonia UI",
            IsChecked = true
        });
    }
}