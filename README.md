<p align="center">
  <img src="./TaskBarSwitch/asset/icon.ico" alt="TaskBarSwitch Icon">
</p>
<h1 align="center">TaskBarSwitch</h1>

[English ver.](README_en.md)

TaskBarSwitchは、Windowsのタスクバーの表示/非表示を切り替えるためのツールです。タスクバーの常時表示と自動非表示を簡単に切り替えることができます。

## 機能

<p align="center">
  <img src="./doc/img/use.gif" alt="use" width="640">
</p>

- タスクバーの常時表示と自動非表示の切り替え
- Windowsのアクセントカラーを使用したアイコン表示
- コンテキストメニューからの操作

## 必要条件

- .NET 9.0以降
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

## ビルド

以下のコマンドでプロジェクトをビルドします。

```sh
dotnet build
```

または、Visual Studio Codeのタスクからビルドすることもできます。タスクの実行方法は以下の通りです。

1. メニューから「ターミナル」 > 「タスクの実行」を選択します。
2. 「Debug」または「Release」を選択します。

## 使い方
1. アプリケーションを起動するとタスクバーにアイコンが追加されます。
2. アイコンをダブルクリックもしくは右クリックのコンテキストメニューからタスクバーの常時表示/自動非表示を切り替えることができます。

### コマンドライン
コマンドラインオプションを使用して、起動時に常時表示/自動非表示を指定できます。

- 常時表示
```dos
TaskBarSwitch.exe --show
```

- 自動非表示
```dos
TaskBarSwitch.exe --hide
```
