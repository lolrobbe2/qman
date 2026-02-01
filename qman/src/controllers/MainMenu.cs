using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using qman.src.lib;
using qmanlib.src.storage.qdb;
using qmanlib.src.storage.repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace qman.src.controllers
{
    /// <summary>
    /// Control class for all the main menu bar logic
    /// </summary>
    public class MainMenu : Controller
    {
        public override Control? GetContent(ref DockPanel panel)
        {
            // Create the menu
            Menu menu = GetMenu();
            // Dock the menu to the top
            DockPanel.SetDock(menu, Dock.Top);
            return menu;
        }
        private Menu GetMenu()
        {
            MenuBarBuilder menuBarBuilder = MenuBarBuilder.Create();
            menuBarBuilder
                .AddMenu("_File")
                .AddItem("_Open", OpenQDB)
                .AddItem("_Save")
                .EndMenu();
            return menuBarBuilder.Build();
        }
        public async void OpenQDB(object? sender, RoutedEventArgs args)
        {
            var options = new FilePickerOpenOptions()
            {
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("QBUS Database")
                    {
                        Patterns = new[] { "*.qdb" }
                    }
                }
            };
            IReadOnlyList<IStorageFile> files = await topLevel?.StorageProvider.OpenFilePickerAsync(options)!;
            QbusDatabase db = QbusDatbaseReader.Open(files[0].TryGetLocalPath()!);
            GlobalState.repositories = new CommonRepostories(db);
            UpdateAllContent();
        }

     
    }
}
