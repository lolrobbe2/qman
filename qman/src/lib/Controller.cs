using Avalonia.Controls;
using qman.src.Windows.main;
using System;
using System.Collections.Generic;
using System.Text;

namespace qman.src.lib
{
    public delegate void UpdateControllersHandler(string name);

    public abstract class Controller
    {
        public TopLevel? topLevel { get; set; }
        public string? Name { get; set; }
        public UpdateControllersHandler? RequestUpdate { get; set; }
        public abstract Control? GetContent(ref DockPanel panel);
        public virtual void UpdateContent()
        {
            if (RequestUpdate != null)
            {
                RequestUpdate.Invoke(Name ?? "");
            }
        }
        public virtual void UpdateAllContent()
        {
            if (RequestUpdate != null)
            {
                RequestUpdate.Invoke("");
            }
        }
        public virtual void Init()
        {

        }
    }

    public abstract class ContainerController : Controller
    {
        private DockPanel DockPanel = new();
        private Dictionary<string, Control> ControllerControls = new();
        private Dictionary<string, Controller> Controllers = new ();
        public ContainerController AddController<TController>(string name)
         where TController : Controller, new()
        {
            var control = new TController() { Name = name, RequestUpdate = UpdateController };
            control.topLevel = topLevel;
            control.Init();
            Controllers.Add(name, control);
            return this;
        }
        public ContainerController AddController<TController>(string name, TController instance)
           where TController : Controller, new()
        {
            instance.topLevel = topLevel;
            instance.Name = name;
            instance.RequestUpdate = UpdateController;
            instance.Init();

            Controllers.Add(name, instance);
            return this;
        }
        public override Control? GetContent(ref DockPanel panel)
        {
            ControllerControls.Clear();
            DockPanel.Children.Clear();
            foreach (Controller item in Controllers.Values)
            {
                Control? control = item.GetContent(ref DockPanel);
                if (control != null)
                {
                    
                    DockPanel.Children.Add(control);
                    ControllerControls[item.Name!] = control;
                }
            }

            return DockPanel;
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
            UpdateContent();
        }
    }
}
