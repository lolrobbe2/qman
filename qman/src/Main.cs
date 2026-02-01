using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using SkiaSharp;
using System;

AppBuilder
           .Configure<App>()
           .UsePlatformDetect()
           .UseSkia()
           .LogToTrace().StartWithClassicDesktopLifetime(args);

Console.WriteLine("hello");
