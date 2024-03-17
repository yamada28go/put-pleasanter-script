# put-pleasanter-web-script

`put-pleasanter-web-script`はPleasanterにスクリプトをアップロードするためのコマンドラインツールです。Pleasanterのウェブサイトやアプリケーションにカスタムスクリプトを容易に導入することが可能になります。

## 機能

- Pleasanterのスクリプトを送信します。
- コード生成の設定に使用するデフォルトの設定ファイルを取得します。
- スクリプトのバックアップと古いバックアップの削除機能。

## 使用方法

### スクリプト送信

Pleasanterのスクリプトを送信するには、以下のコマンドを使用します。

```sh
put-pleasanter-web-script PutScript <ConfigurationFileName>
```
ConfigurationFileNameには、アップロード設定情報が記載されたXMLファイルのパスを指定します。

デフォルト設定ファイルの取得
コード生成の設定に使用するデフォルトの設定ファイルを取得するには、以下のコマンドを使用します。


### デフォルト設定ファイルの取得
コード生成の設定に使用するデフォルトの設定ファイルを取得するには、以下のコマンドを使用します。

```
put-pleasanter-web-script DefaultConfiguration <OutFileName>
```

OutFileNameには、出力されるデフォルトの設定ファイル名称を指定します。


```
<?xml version="1.0" encoding="utf-8"?>
<RunConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <WebScript>
    <ScriptPath>送信対象のスクリプトパスを指定してください。</ScriptPath>
    <ScriptId>-1</ScriptId>
  </WebScript>
  <BackupInfo>
    <DoBackup>true</DoBackup>
    <Keep>5</Keep>
  </BackupInfo>
  <ApiKey>PleasanterのAPIキーを指定してください。</ApiKey>
  <PleasanterURL>PleasanterのURLを指定してください。</PleasanterURL>
  <SiteID>-1</SiteID>
</RunConfiguration>

```

設定ファイルにおける各種項目の詳細は以下となります。

| 要素名                   | 説明                                                                                              | 例                                                                                        |
|------------------------|-------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------|
| `WebScript`            | スクリプトの送信に関連する設定を含む要素です。                                                    | `<WebScript></WebScript>`                                                                 |
| → `ScriptPath`         | 送信するスクリプトのファイルパスを指定します。                                                     | `<ScriptPath>/path/to/script.js</ScriptPath>`                                             |
| → `ScriptId`           | Pleasanter上で更新対象のスクリプトIDを指定します。                                                | `<ScriptId>123</ScriptId>`                                                                |
| `BackupInfo`           | スクリプトのバックアップに関する設定を含む要素です。                                               | `<BackupInfo></BackupInfo>`                                                               |
| → `DoBackup`           | スクリプトの送信時にバックアップを作成するかどうかを指定します。`true` または `false`で指定します。 | `<DoBackup>true</DoBackup>`                                                               |
| → `Keep`               | 保持するバックアップの最大数を指定します。過去のバックアップはこの数を超えると削除されます。               | `<Keep>5</Keep>`                                                                          |
| `ApiKey`               | PleasanterのAPIキーを指定します。これはPleasanterへの認証に使用されます。                           | `<ApiKey>your_api_key_here</ApiKey>`                                                      |
| `PleasanterURL`        | PleasanterのベースURLを指定します。これはスクリプト送信先のURLです。                                | `<PleasanterURL>http://example.com</PleasanterURL>`                                       |
| `SiteID`               | スクリプトを送信するPleasanter上のサイトIDを指定します。                                          | `<SiteID>1</SiteID>`                                                                      |


## ライセンス

このプロジェクトはMITライセンスのもとで公開されています。