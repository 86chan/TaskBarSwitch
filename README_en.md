# TaskBarSwitch

<p align="center">
  <img src="./TaskBarSwitch/asset/icon.ico" alt="TaskBarSwitch Icon">
</p>

[Japanese ver.](README.md)

TaskBarSwitch is a tool for toggling the visibility of the Windows taskbar. This tool allows you to easily switch between auto-hide and always-on-top modes for the taskbar.

## Features

- Toggle between auto-hide and always-on-top modes for the taskbar
- Display icons using Windows accent colors
- Operate from the context menu

## Requirements

- .NET 5.0 or later
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

## Build and Run

### Build

Build the project with the following command.

```sh
dotnet build