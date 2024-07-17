using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TodoAvalonia.Models;

namespace TodoAvalonia.Services;

// TODO: Make that classes non-static and uses Dependency Injection
public static class ToDoListFileService
{
    // TODO: use an universal file name that's not hard coded
    // This is a hard coded path to the file. It may not be available on every platform. In your real world App you may
    // want to make this configurable
    private static string _jsonFileName =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Achilles.Todo.Avalonia", "MyToDoList.txt");


    /// <summary>
    /// Stores the given items into file on disc
    /// </summary>
    /// <param name="itemsToSave"></param>
    public static async Task SaveToFileAsync(IEnumerable<ToDoItem> itemsToSave)
    {
        // Ensure all directories exist
        Directory.CreateDirectory(Path.GetDirectoryName(_jsonFileName));

        // use a FileStream to write all items to disc
        using (var fs = File.Create(_jsonFileName))
        {
            await JsonSerializer.SerializeAsync(fs, itemsToSave);
        }
    }

    /// <summary>
    /// Loads the file from disc and returns the items stored inside
    /// </summary>
    /// <returns>An Ienumerable of items loaded or null in case the file was not found</returns>
    public static async Task<IEnumerable<ToDoItem>?> LoadFileAsync()
    {
        try
        {
            await using (var fs = File.OpenRead(_jsonFileName))
            {
                return await JsonSerializer.DeserializeAsync<IEnumerable<ToDoItem>>(fs);
            }
        }
        catch (Exception e) when (e is FileNotFoundException || e is DirectoryNotFoundException)
        {
            // if file or directory not found, just return null
            return null;
        }
    }
}