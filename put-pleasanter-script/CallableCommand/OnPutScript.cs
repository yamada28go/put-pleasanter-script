using System;
using NLog;
using System.CommandLine;
using System.CommandLine.Invocation;
using PutPleasanterScript.Configuration;

namespace PutPleasanterScript.CallableCommand
{
    public class OnPutScript
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static string GetCommandName()
        {
            return "PutScript";
        }

        /// <summary>
        /// 処理対象のコマンドを設定する
        /// </summary>
        /// <returns></returns>
        public static Command MakeCommand()
        {
            var cmd = new Command(OnPutScript.GetCommandName());
            cmd.AddArgument(new Argument<FileInfo>(
            "ConfigurationFileName",
            description: "アップロード設定情報のファイル名称"
            ));
            cmd.Description = "Pleasanterのスクリプトを送信します。";

            cmd.Handler = CommandHandler.Create<FileInfo>((ConfigurationFileName) =>
            {
                logger.Debug($"On {OnPutScript.GetCommandName()} Start!");
                logger.Debug($"On {OnPutScript.GetCommandName()} CofigFileName: {ConfigurationFileName}");

                var x = new OnPutScript();
                x.On(ConfigurationFileName);
                logger.Debug("On OnPutScript End!");
            });

            return cmd;
        }

        /// <summary>
        /// コマンド実行時のパラメータ
        /// </summary>
        /// <param name="workDir"></param>
        /// <param name="ConfigurationFileName"></param>
        /// <returns></returns>
        private void On(FileInfo ConfigurationFileName)
        {
            // XML形式としてデフォルト設定を生成する
            var file = Path.Combine(ConfigurationFileName.Name);
            logger.Info($"読み込んだ設定ファイル名 : {file}");
            var config = PutPleasanterScript.Utility.XMLSerialize.Deserialize<RunConfiguration>(file);
            logger.Info($"設定情報 : {config.ToStringForLogging()}");

            var v = config.Validate();
            if (false == string.IsNullOrWhiteSpace(v))
            {
                throw new ApplicationException(v);
            }



            // コードを実行
            var p = new ContentUploader(config);
            p.doUpdate().Wait();



        }
    }
}

