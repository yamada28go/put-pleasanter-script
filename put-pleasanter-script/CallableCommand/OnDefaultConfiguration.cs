using System;
using NLog;
using System.CommandLine;
using System.CommandLine.Invocation;


namespace PutPleasanterScript.CallableCommand
{
    public class OnDefaultConfiguration
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static string GetCommandName()
        {
            return "DefaultConfiguration";
        }

        /// <summary>
        /// 処理対象のコマンドを設定する
        /// </summary>
        /// <returns></returns>
        public static Command MakeCommand()
        {
            var cmd = new Command(OnDefaultConfiguration.GetCommandName());
            cmd.AddArgument(new Argument<FileInfo>(
            "OutFileName",
            description: "出力されるデフォルトの設定ファイル名称"
            ));
            cmd.Description = "デフォルトの設定ファイルを取得します。";

            cmd.Handler = CommandHandler.Create<FileInfo>((OutFileName) =>
            {
                logger.Debug($"On {OnDefaultConfiguration.GetCommandName()} Start!");
                logger.Debug($"On {OnDefaultConfiguration.GetCommandName()} OutFileName: {OutFileName}");

                var x = new OnDefaultConfiguration();
                x.On(OutFileName);
                logger.Debug("On OnDefaultConfiguration End!");
            });

            return cmd;
        }

        /// <summary>
        /// コマンド実行時のパラメータ
        /// </summary>
        /// <param name="workDir"></param>
        /// <param name="outFile"></param>
        /// <returns></returns>
        private int On(FileInfo outFile)
        {
            // デフォルト設定
            var c = new PutPleasanterScript.Configuration.RunConfiguration
            {
                ApiKey = "PleasanterのAPIキーを指定してください。",
                PleasanterURL = "PleasanterのURLを指定してください。",
                SiteID = -1,
                WebScript = new Configuration.WebScript
                {
                    ScriptPath = "送信対象のスクリプトパスを指定してください。",
                    ScriptId = -1
                },
                BackupInfo = new Configuration.BackupInfo
                {
                    DoBackup = true,
                    Keep = 5
                }
            };

            // XML形式としてデフォルト設定を生成する
            var file = Path.Combine(outFile.Name);
            logger.Info($"出力されるデフォルト設定ファイル : {file}");
            PutPleasanterScript.Utility.XMLSerialize.Serialize(c, file);

            return 0;

        }
    }
}

