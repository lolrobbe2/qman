using Avalonia;
using Avalonia.Controls;
using Avalonia.Themes;
using Avalonia.Controls.ApplicationLifetimes;
using qman.src.lib;
using Avalonia.Themes.Simple;
using System;

namespace qman.src.Windows.main
{
    internal class MainWindow : Window
    {
        public MainWindow()
        {
            this.Styles.Add(new SimpleTheme());
            // Set the window title
            this.Title = "QMan";

            // Optional: start maximized / full screen
            this.WindowState = WindowState.Maximized;

            // Optional: remove decorations for true fullscreen
            // this.SystemDecorations = SystemDecorations.None;
            // this.CanResize = false;

            // Set empty content
            this.Content = GetContent();
        }
        private object? GetContent()
        {
            var dockPanel = new DockPanel();

            // Create the menu
            Menu menu = GetMenu();

            // Dock the menu to the top
            DockPanel.SetDock(menu, Dock.Top);

            // Add the menu first
            dockPanel.Children.Add(menu);

            // Add empty content below
            var emptyContent = new Grid();
            dockPanel.Children.Add(emptyContent);

            return dockPanel;
        }

        private Menu GetMenu()
        {
            MenuBarBuilder menuBarBuilder =  MenuBarBuilder.Create();
            menuBarBuilder
                .AddMenu("_File")
                .AddItem("_Open")
                .AddItem("_Save")
                .EndMenu();
            return menuBarBuilder.Build();
        }
    }
}
