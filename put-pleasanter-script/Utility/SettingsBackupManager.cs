using System;
using System.IO.Compression;
using NLog;
using PutPleasanterScript.Configuration;

namespace PutPleasanterScript.Utility
{
    public class SettingsBackupManager
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 作業用パス
        /// </summary>
        private readonly string path;

        private readonly TempDirectoryManager tempDirectoryManager;
        private readonly RunConfiguration runConfiguration;


        public SettingsBackupManager(TempDirectoryManager tempDirectoryManager, RunConfiguration runConfiguration)
        {
            this.tempDirectoryManager = tempDirectoryManager;
            this.runConfiguration = runConfiguration;


            path = CreateDirectoryWithCurrentTime(tempDirectoryManager.TempDirectoryPath);

        }

        /// <summary>
        /// 読み取ったテキストコンテンツをファイルに書き出す
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public void SaveFile(string fileName, string content)
        {
            var p = Path.Combine(path, fileName);
            File.WriteAllText(p, content);
        }

        /// <summary>
        /// バックアップしたファイルをzipで出力する
        /// </summary>
        public void ExtractAsZip()
        {
            var extractPath = BackupDirName;
            Directory.CreateDirectory(extractPath);

            var conpFileName = CompressDirectory(path, extractPath);
            logger.Info($"{conpFileName}にスクリプトのバックアップが作成しれました。");

        }

        /// <summary>
        /// 古いログを自動的に削除する
        /// </summary>
        public void KeepLatestFiles()
        {

            var extractPath = BackupDirName;
            KeepLatestFiles(extractPath, this.runConfiguration?.BackupInfo?.Keep ?? Int32.MaxValue);

        }


        /// <summary>
        /// バックアップ対象のディレクトリ名
        /// </summary>
        private static string BackupDirName = "scrip-backup";


        #region 補助関数

        /// <summary>
        /// バックアップファイルのスクリプト名
        /// </summary>
        private static string PrefixFileName = "ScriptBackup";

        /// <summary>
        /// 古いログは自動的に削除する
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="numberOfFilesToKeep"></param>
        public static void KeepLatestFiles(string directoryPath, int numberOfFilesToKeep)
        {
            try
            {
                // ディレクトリ内のすべてのファイルを取得
                var files = new DirectoryInfo(directoryPath).GetFiles();

                // ファイルを最終更新日時で降順に並べ替え
                var filesToDelete = files
                    .Where(x => x.Name.Contains("zip"))
                    .Where(x => x.Name.Contains(PrefixFileName))
                    .OrderByDescending(f => f.LastWriteTime)
                                         .Skip(numberOfFilesToKeep) // 最新のn個のファイルを除外
                                         .ToList();

                // 除外された古いファイルを削除
                foreach (var file in filesToDelete)
                {
                    logger.Debug($"Deleting: {file.FullName}");
                    logger.Info($"{file.FullName}は保存期限が切れているので削除されます。");
                    file.Delete();
                }

                logger.Debug("Cleanup complete. Only the latest files have been kept.");
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// 指定されたディレクトリ配下のファイルとサブディレクトリをZIP形式で圧縮します。
        /// </summary>
        /// <param name="sourceDirectory">圧縮するファイルが含まれるディレクトリのパス。</param>
        /// <param name="destinationZipFilePath">生成されるZIPファイルのパス。</param>
        public static string CompressDirectory(string sourceDirectory, string destinationZipFilePath)
        {
            // 指定されたディレクトリが存在しない場合、例外を投げる
            if (!Directory.Exists(sourceDirectory))
            {
                throw new DirectoryNotFoundException($"指定されたディレクトリ '{sourceDirectory}' が見つかりません。");
            }

            var destinationZipName = Path.GetFileName(sourceDirectory) + ".zip";
            var dest = Path.Combine(destinationZipFilePath, destinationZipName);

            // ZIPファイルを作成する
            ZipFile.CreateFromDirectory(sourceDirectory, dest);

            logger.Debug($"'{sourceDirectory}' が '{dest}' に圧縮されました。");

            return dest;
        }

        /// <summary>
        /// 時刻指定で作業ディレクトリを作成する
        /// </summary>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static string CreateDirectoryWithCurrentTime(string basePath)
        {
            // 現在時刻を yyyyMMddHHmmss 形式で取得
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            // ベースパスとタイムスタンプを組み合わせて完全なディレクトリパスを生成
            string directoryPath = Path.Combine(basePath, $"{PrefixFileName}_{timestamp}");

            // 指定したパスにディレクトリを作成
            Directory.CreateDirectory(directoryPath);

            logger.Debug($"ディレクトリ '{directoryPath}' を作成しました。");

            return directoryPath;
        }

        #endregion
    }
}

