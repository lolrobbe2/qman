using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using qman.src.controllers;
using qman.src.Windows;
using qman.src.Windows.main;
using System.IO;
using System.Threading.Tasks;

public class App : Application
{
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindow window = new MainWindow();

            #region Controllers
            window.AddController<MainMenu>("main_menu");
            window.AddController<ModulesController>("modules_controller");
            #endregion
            window.Build();
            desktop.MainWindow = window;
            
        }

        base.OnFrameworkInitializationCompleted();
    }
}
