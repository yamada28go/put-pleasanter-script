using System;
namespace PutPleasanterScript.Utility
{
    public class TempDirectoryManager : IDisposable
    {

        public string TempDirectoryPath { get; private set; }

        public TempDirectoryManager()
        {
            // 一時ディレクトリのパスを生成
            TempDirectoryPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            // 一時ディレクトリを作成
            Directory.CreateDirectory(TempDirectoryPath);
        }

        // IDisposableの実装
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // マネージリソースの解放処理が必要な場合はここに記述

            }

            // 一時ディレクトリの削除
            if (Directory.Exists(TempDirectoryPath))
            {
                Directory.Delete(TempDirectoryPath, true);
            }
        }

        // デストラクタ
        ~TempDirectoryManager()
        {
            Dispose(false);
        }
    }
}

