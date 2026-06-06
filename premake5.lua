require "vstudio"
function platformsElement(cfg)
   _p(2,'<Platforms>x64;AnyCpu</Platforms>')
end

premake.override(premake.vstudio.cs2005.elements, "projectProperties", function (oldfn, cfg)
   return table.join(oldfn(cfg), {
   platformsElement,
   })
end)


workspace "qman"
architecture "x86_64"
   configurations { "Debug", "Release" }
   startproject "qman"

   project "mdbreader"
      clr "Unsafe"
      kind "SharedLib" -- CLI application
      dotnetframework "net10.0" -- Targeting .NET 9.0
      location "mdbreader"
      language "C#"
      targetdir "bin/%{cfg.buildcfg}"
      files { "%{prj.name}/src/MdbReader/**.cs" } -- Include all C# source files
      vsprops {
         PublishSingleFile = "true",
         SelfContained = "true",
         IncludeNativeLibrariesForSelfExtract = "true",
         PublishTrimmed =  "true",
         Nullable = "enable",
         AllowUnsafeBlocks = "true",

      }
      filter "configurations:Debug"
         defines { "DEBUG" }
         optimize "Off"
      
      filter "configurations:Release"
         symbols "Off"
         defines { "NDEBUG" }
         optimize "On"

   project "qman-lib"
      kind "SharedLib" -- CLI application
      dotnetframework "net10.0" -- Targeting .NET 9.0
      location "qman-lib"
      language "C#"
      targetdir "bin/%{cfg.buildcfg}"
      files { "%{prj.name}/src/**.cs" } -- Include all C# source files
      nuget { "Spectre.Console:0.50.0", "YamlDotNet:16.3.0"}
      vsprops {
         PublishSingleFile = "true",
         SelfContained = "true",
         IncludeNativeLibrariesForSelfExtract = "true",
         PublishTrimmed =  "true",
         Nullable = "enable",

      }
      links {"qman-common"}

      filter "configurations:Debug"
         defines { "DEBUG" }
         optimize "Off"
      
      filter "configurations:Release"
         symbols "Off"
         defines { "NDEBUG" }
         optimize "On"
   project "qman-common"
      kind "SharedLib" -- CLI application
      dotnetframework "net10.0" -- Targeting .NET 9.0
      location "qman-common"
      language "C#"
      targetdir "bin/%{cfg.buildcfg}"
      files { "%{prj.name}/src/**.cs" } -- Include all C# source files
      nuget { "Spectre.Console:0.50.0", "YamlDotNet:16.3.0"}
      vsprops {
         PublishSingleFile = "true",
         SelfContained = "true",
         IncludeNativeLibrariesForSelfExtract = "true",
         PublishTrimmed =  "true",
         Nullable = "enable",

      }
      links {"mdbreader"}

      filter "configurations:Debug"
         defines { "DEBUG" }
         optimize "Off"
      
      filter "configurations:Release"
         symbols "Off"
         defines { "NDEBUG" }
         optimize "On"

   project "qman-cli"
      kind "ConsoleApp" -- CLI application
      dotnetframework "net10.0" -- Targeting .NET 9.0
      location "qman-cli"
      language "C#"
      targetdir "bin/%{cfg.buildcfg}"
      files { "%{prj.name}/src/**.cs" } -- Include all C# source files
      nuget { "Spectre.Console.Cli:0.50.0", "StreamJsonRpc:2.22.11" }
      links {"qman-lib"}
      vsprops {
         PublishSingleFile = "true",
         SelfContained = "true",
         IncludeNativeLibrariesForSelfExtract = "true",
         PublishTrimmed =  "true",
         Nullable = "enable"
      }
      filter "configurations:Debug"
         defines { "DEBUG" }
         optimize "Off"
      
      filter "configurations:Release"
         symbols "Off"
         defines { "NDEBUG" }
         optimize "On"
   
   project "qman"
      kind "ConsoleApp" -- CLI application
      dotnetframework "net10.0" -- Targeting .NET 9.0
      location "qman"
      language "C#"
      targetdir "bin/%{cfg.buildcfg}"
      files { "%{prj.name}/src/**.cs" } -- Include all C# source files
      nuget { "Avalonia:11.3.11","Avalonia.Desktop:11.3.11","Avalonia.Themes.Simple:11.3.11","TextCopy:6.2.1" }
      vsprops {
         PublishSingleFile = "true",
         SelfContained = "true",
         IncludeNativeLibrariesForSelfExtract = "true",
         PublishTrimmed =  "true",
         Nullable = "enable",
      }
      links {"qman-lib"}

      filter "configurations:Debug"
         defines { "DEBUG" }
         optimize "Off"
      
      filter "configurations:Release"
         symbols "Off"
         defines { "NDEBUG" }
         optimize "On"

project "qman-controller"
      kind "ConsoleApp" -- CLI application
      dotnetframework "net10.0" -- Targeting .NET 9.0
      location "qman-controller"
      language "C#"
      targetdir "bin/%{cfg.buildcfg}"
      files { "%{prj.name}/src/**.cs" } -- Include all C# source files
      nuget { "Spectre.Console.Cli:0.50.0", "StreamJsonRpc:2.22.11" }
      links {"qman-common"}
      vsprops {
         PublishSingleFile = "true",
         SelfContained = "true",
         IncludeNativeLibrariesForSelfExtract = "true",
         PublishTrimmed =  "true",
         Nullable = "enable"
      }
      filter "configurations:Debug"
         defines { "DEBUG" }
         optimize "Off"
      
      filter "configurations:Release"
         symbols "Off"
         defines { "NDEBUG" }
         optimize "On"

   externalproject "qman_native"
      location "qman_native"
      kind "WindowedApp"
      language "C#"
      filename "qman_native"