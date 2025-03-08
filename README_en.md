# TaskBarSwitch

<p align="center">
  <img src="./TaskBarSwitch/asset/icon.ico" alt="TaskBarSwitch Icon">
</p>

[Japanese ver.](README.md)

TaskBarSwitch is a tool for toggling the visibility of the Windows taskbar. This tool allows you to easily switch between always-on-top and auto-hide modes for the taskbar.

## Features

<p align="center">
  <img src="./doc/img/use.gif" alt="use" width="640">
</p>

- Toggle between always-on-top and auto-hide modes for the taskbar
- Display icons using Windows accent colors
- Operate from the context menu

## Requirements

- .NET 9.0 or later
- Visual Studio Code
- `Svg.Skia` library

## Setup

1. Clone the repository.

    ```sh
    git clone https://github.com/yourusername/TaskBarSwitch.git
    cd TaskBarSwitch
    ```

2. Install the required library.

    ```sh
    dotnet add package Svg.Skia
    ```

3. Open the project in Visual Studio Code.

    ```sh
    code .
    ```

## Build

Build the project with the following command.

```sh
dotnet build
```

Alternatively, you can build the project from Visual Studio Code tasks. To run the tasks, follow these steps:

1. From the menu, select "Terminal" > "Run Task".
2. Select "Debug" or "Release".

## Usage

1. When you launch the application, an icon will be added to the taskbar.
2. You can toggle between always-on-top and auto-hide modes by double-clicking the icon or using the context menu.

### Command Line

You can specify always-on-top or auto-hide mode at startup using command line options.

- Always-on-top
```dos
TaskBarSwitch.exe --show
```

- Auto-hide
```dos
TaskBarSwitch.exe --hide
```