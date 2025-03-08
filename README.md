<p align="center">
  <img src="./TaskBarSwitch/asset/icon.ico" alt="TaskBarSwitch Icon">
</p>
<h1 align="center">TaskBarSwitch</h1>

[English ver.](README_en.md)

TaskBarSwitchは、Windowsのタスクバーの表示/非表示を切り替えるためのツールです。このツールは、タスクバーの自動非表示と常時表示を簡単に切り替えることができます。

## 機能

- タスクバーの自動非表示と常時表示の切り替え
- Windowsのアクセントカラーを使用したアイコン表示
- コンテキストメニューからの操作

## 必要条件

- .NET 5.0以降
- Visual Studio Code
- `Svg.Skia`ライブラリ

## セットアップ

1. リポジトリをクローンします。

    ```sh
    git clone https://github.com/yourusername/TaskBarSwitch.git
    cd TaskBarSwitch
    ```

2. 必要なライブラリをインストールします。

    ```sh
    dotnet add package Svg.Skia
    ```

3. Visual Studio Codeでプロジェクトを開きます。

    ```sh
    code .
    ```

## ビルドと実行

### ビルド

以下のコマンドでプロジェクトをビルドします。

```sh
dotnet build
```