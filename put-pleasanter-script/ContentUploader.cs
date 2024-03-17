using PutPleasanterScript.Models;
using PutPleasanterScript.Service.JSONProcessing;
using System.Text.Json;
using PutPleasanterScript.Configuration;
using NLog;
using PutPleasanterScript.Utility;

namespace PutPleasanterScript
{
    public class ContentUploader
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly RunConfiguration configuration;

        public ContentUploader(RunConfiguration ref_configuration)
        {
            this.configuration = ref_configuration;
        }

        public async Task doUpdate()
        {
            // HttpClientのインスタンスを作成
            using (var client = new HttpClient())
            {

                try
                {

                    using (var tdm = new TempDirectoryManager())
                    {
                        logger.Info($"Pleasanter({this.configuration.PleasanterURL})から現在のスクリプト設定を読み込み開始");
                        var settingsBackupManager = new SettingsBackupManager(tdm, configuration);

                        // 現在の情報を取得
                        var response = await client.GetPleasanterSiteInfo(configuration);

                        // レスポンスが成功かどうかを確認
                        if (response.IsSuccessStatusCode)
                        {
                            // レスポンスボディを非同期で読み取り
                            var responseString = await response.Content.ReadAsStringAsync();
                            var responseObject = JsonSerializer.Deserialize<PleasanterApiResponse>(responseString);
                            settingsBackupManager.SaveFile("before.json", responseString);

                            if (responseObject?.StatusCode == 200)
                            {
                                logger.Info($"Pleasanter({this.configuration.PleasanterURL})から現在のスクリプト設定を読み込み成功");

                                // 置換対象のスクリプト情報を指定
                                var up = new List<UpdateInformation> { this.configuration.ToUpdateInformation() };
                                var modifiedJsonString = new SiteJSONProcessing(up).Processing(responseString);

                                logger.Debug(modifiedJsonString);

                                //送信する
                                {
                                    logger.Info($"Pleasanter({this.configuration.PleasanterURL})に対して更新スクリプトを送信開始");
                                    var response2 = await client.PutPleasanterSiteInfo(configuration!, modifiedJsonString);
                                    if (response2.IsSuccessStatusCode)
                                    {
                                        var responseContent = await response2.Content.ReadAsStringAsync(); // レスポンスボディを文字列で取得
                                        logger.Debug("Response: " + responseContent);
                                    }
                                    else
                                    {
                                        logger.Error("Error: " + response.StatusCode);
                                    }
                                    logger.Info($"Pleasanter({this.configuration.PleasanterURL})に対して更新スクリプトを送信完了");

                                }
                            }
                        }
                        else
                        {
                            logger.Error("Error: " + response.StatusCode);
                        }


                        //実行結果を取得してバックアップファイルを作る
                        {

                            // 現在の情報を取得
                            var responseAfter = await client.GetPleasanterSiteInfo(configuration);

                            // レスポンスが成功かどうかを確認
                            if (responseAfter.IsSuccessStatusCode)
                            {
                                // レスポンスボディを非同期で読み取り
                                var responseString = await responseAfter.Content.ReadAsStringAsync();
                                settingsBackupManager.SaveFile("after.json", responseString);
                            }

                        }

                        // 指定されている時はコードをバックアップする
                        if (this.configuration?.BackupInfo?.DoBackup == true)
                        {
                            settingsBackupManager.ExtractAsZip();
                            settingsBackupManager.KeepLatestFiles();
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.Error("Exception caught: " + e.Message);
                }
            }


        }
    }
}

