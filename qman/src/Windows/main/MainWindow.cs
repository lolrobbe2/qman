using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Themes;
using Avalonia.Themes.Simple;
using qman.src.lib;
using System;
using System.Collections.Generic;

namespace qman.src.Windows.main
{
    internal class MainWindow : Window
    {
        private readonly Dictionary<string, Control> ControllerControls = new();
        private Dictionary<string,Controller> Controllers { get; init; }
        public MainWindow()
        {
            this.Styles.Add(new SimpleTheme());
            // Set the window title
            this.Title = "QMan";

            // Optional: start maximized / full screen
            this.WindowState = WindowState.Maximized;
            this.Controllers = new Dictionary<string, Controller>();
            // Optional: remove decorations for true fullscreen
            // this.SystemDecorations = SystemDecorations.None;
            // this.CanResize = false;
            // Set empty content
            this.Content = GetContent();
            
        }
        public MainWindow AddController<TController>(string name)
            where TController: Controller, new()
        {

            var control = new TController() { Name = name, RequestUpdate = UpdateController };
            control.topLevel = GetTopLevel(this);
            control.Init();
            Controllers.Add(name, control); return this;
        }
        public MainWindow AddController<TController>(string name, TController instance)
           where TController : Controller, new()
        {
            instance.topLevel = GetTopLevel(this);
            instance.Name = name;
            instance.RequestUpdate = UpdateController;
            instance.Init();
            Controllers.Add(name, instance);
            return this;
        }
        public void Build()
        {
            this.Content = GetContent();
        }
        private object? GetContent()
        {
            ControllerControls.Clear();
            var dockPanel = new DockPanel();

            foreach (Controller item in Controllers.Values)
            {
                Control? control = item.GetContent(ref dockPanel);
                if (control != null)
                {
                    dockPanel.Children.Add(control);
                    ControllerControls[item.Name!] = control;
                }
            }
            
            return dockPanel;
        }
        private void UpdateController(string name)
        {
            if (name == "")
            {
                foreach (string control in Controllers.Keys)
                {
                    UpdateController(control);
                }
            }

            if (!Controllers.TryGetValue(name, out var controller) || !ControllerControls.TryGetValue(name, out var oldControl))
            {
                return;
            }

            // 2. Get the parent (the DockPanel)
            if (oldControl.Parent is DockPanel parentPanel)
            {
                // 3. Get the updated content from the controller
                // Note: You might need to pass the parent ref if your GetContent logic requires it
                Control? newControl = controller.GetContent(ref parentPanel);

                if (newControl != null)
                {
                    // 4. Find the index of the old control to maintain layout order
                    int index = parentPanel.Children.IndexOf(oldControl);

                    // 5. Swap them
                    parentPanel.Children[index] = newControl;

                    // 6. Update our tracking dictionary
                    ControllerControls[name] = newControl;
                }
                else
                {
                    // If the update results in null, just remove it
                    parentPanel.Children.Remove(oldControl);
                    ControllerControls.Remove(name);
                }
            }
        }
    }
}
