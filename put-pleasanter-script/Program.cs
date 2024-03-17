using NLog;
using System.CommandLine;
using PutPleasanterScript.CallableCommand;


class Program
{
    /// <summary>
    /// ロガー
    /// </summary>
    private static Logger logger = LogManager.GetCurrentClassLogger();


    static void Main(string[] args)
    {
        // テスト用にコマンドを残しておく
        //args = new string[] {  "DefaultConfiguration", "out.xml"};
        //args = new string[] { "PutScript", "out.xml" };
        //args = new string[] {  };

        logger.Info($"Pleasanter スクリプト送信 コマンド　起動!!! ");
        if (null != args && 0 != args.Length)
        {
            logger.Debug($"arg : {args?.Aggregate((x, y) => x + ", " + y)}");
        }

        // [Memo]
        // Argument , Optionの引数名とCommandHandler.Create関数で指定する
        // 関数パラメータの引数名は合致していないと正しく動作しないので、
        // 注意が必要
        //
        // 参考
        // https://qiita.com/TsuyoshiUshio@github/items/02902f4f46f0aa37e4b1

        // Create a root command with some options
        var rootCommand = new RootCommand();

        rootCommand.Description = "Pleasanter スクリプト送信";

        // 送信コマンドを追加
        rootCommand.Add(OnPutScript.MakeCommand());

        // デフオルトの設定ファイルを生成する
        rootCommand.Add(OnDefaultConfiguration.MakeCommand());

        // 生成処理を開始
        logger.Debug("Start Invoke!");

        // Parse the incoming args and invoke the handler
        var x = rootCommand.Invoke(args!);

        logger.Debug("End Invoke!");

        logger.Info($"Pleasanter スクリプト送信 コマンド　終了!!! ");

    }

}


