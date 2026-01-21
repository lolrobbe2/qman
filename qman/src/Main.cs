using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
AppBuilder
           .Configure<App>()
           .UsePlatformDetect()
           .LogToTrace().StartWithClassicDesktopLifetime(args);
Console.WriteLine("hello");
