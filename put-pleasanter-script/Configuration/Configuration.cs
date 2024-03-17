using System;
using PutPleasanterScript.Models;
using System.Text;
using System.Text.Json;
using PutPleasanterScript.Service.JSONProcessing;

namespace PutPleasanterScript.Configuration
{

    public class WebScript
    {

        /// <summary>
        /// 送信用のスクリプトのパス
        /// </summary>
        public string ScriptPath { get; set; } = string.Empty;

        /// <summary>
        /// 更新対象のスクリプトID
        /// </summary>
        public int ScriptId { get; set; } = -1;

    }

    /// <summary>
    /// バックアップ動作設定
    /// </summary>
    public class BackupInfo
    {
        /// <summary>
        /// 設定値を自動バックアップするか?
        /// </summary>
        public bool DoBackup = true;

        /// <summary>
        /// 何個までバックアップファイルをキープするか
        /// </summary>
        public int Keep = Int32.MaxValue;
    }

    public class RunConfiguration
    {
        /// <summary>
        ///  接続用のAPIキー
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;


        /// <summary>
        /// プリザンターのURL
        /// </summary>
        public string PleasanterURL { get; set; } = string.Empty;


        /// <summary>
        /// 更新対象のサイトID
        /// </summary>
        public long SiteID { get; set; } = -1;

        /// <summary>
        /// 更新対象のWebスクリプト名
        /// </summary>
        public WebScript? WebScript = null;

        /// <summary>
        /// バックアップディレクトリ
        /// </summary>
        public BackupInfo? BackupInfo = null;

    }



    public static class ConfigurationLogger
    {
        public static string ToStringForLogging(this RunConfiguration config)
        {
            var sb = new StringBuilder();

            sb.AppendLine("RunConfiguration:");
            sb.AppendLine($"- ApiKey: {config.ApiKey}");
            sb.AppendLine($"- PleasanterURL: {config.PleasanterURL}");
            sb.AppendLine($"- SiteID: {config.SiteID}");

            if (config.WebScript != null)
            {
                sb.AppendLine("  WebScript:");
                sb.AppendLine($"  - ScriptPath: {config.WebScript.ScriptPath}");
                sb.AppendLine($"  - ScriptId: {config.WebScript.ScriptId}");
            }
            else
            {
                sb.AppendLine("  WebScript: null");
            }

            return sb.ToString();
        }
    }


    public static class ConfigurationValidator
    {
        /// <summary>
        /// Configurationのプロパティが適切に設定されているか検証します。
        /// </summary>
        /// <param name="config">検証するConfigurationオブジェクト</param>
        public static string? Validate(this RunConfiguration config)
        {
            string? errorMessage = null;
            // ApiKeyのチェック
            if (string.IsNullOrWhiteSpace(config.ApiKey))
            {
                errorMessage = "APIキーは必須です。";
                return errorMessage;
            }

            // PleasanterURLのチェック
            if (string.IsNullOrWhiteSpace(config.PleasanterURL) || !Uri.IsWellFormedUriString(config.PleasanterURL, UriKind.Absolute))
            {
                errorMessage = "プリザンターのURLは有効なURLである必要があります。";
                return errorMessage;
            }

            // SiteIDのチェック
            if (config.SiteID < 0)
            {
                errorMessage = "サイトIDは0以上である必要があります。";
                return errorMessage;
            }

            // WebScriptの存在チェック
            if (config.WebScript != null)
            {
                // ScriptPathのチェック
                if (string.IsNullOrWhiteSpace(config.WebScript.ScriptPath))
                {
                    errorMessage = "スクリプトのパスは必須です。";
                    return errorMessage;
                }

                // ScriptIdのチェック
                if (config.WebScript.ScriptId < 0)
                {
                    errorMessage = "スクリプトIDは0以上である必要があります。";
                    return errorMessage;
                }
            }
            else
            {
                errorMessage = "設定ファイルでWebScripのパラメータを設定してください。";
                return errorMessage;
            }

            return errorMessage;
        }
    }


    public static class PleasanterURLHelper
    {

        public static string ToGetsiteURL(this RunConfiguration configuration)
        {

            // POSTリクエストを送信するURL
            var url = $"{configuration.PleasanterURL}/api/items/{configuration.SiteID}/getsite";
            return url;
        }

        public static string ToUpdatesiteURL(this RunConfiguration configuration)
        {

            // POSTリクエストを送信するURL
            var url = $"{configuration.PleasanterURL}/api/items/{configuration.SiteID}/updatesite";
            return url;
        }


    }


    public static class HttpClientHelper
    {

        public static async Task<HttpResponseMessage> GetPleasanterSiteInfo(this HttpClient client, RunConfiguration configuration)
        {

            // POSTリクエストを送信するURL
            //var url = "http://localhost:8081/api/items/2/getsite";

            // ApiRequestオブジェクトを作成し、プロパティを設定
            var requestObj = new ApiRequest
            {
                ApiKey = configuration.ApiKey
            };

            // オブジェクトをJSONにシリアライズ
            var jsonData = JsonSerializer.Serialize(requestObj);

            // StringContentを使用して、HTTPコンテンツを作成
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // POSTリクエストを非同期で送信し、レスポンスを待機
            var response = await client.PostAsync(configuration.ToGetsiteURL(), content);


            return response;
        }


        public static async Task<HttpResponseMessage> PutPleasanterSiteInfo(this HttpClient client, RunConfiguration configuration, string dataString)
        {
            var requestObj = new ApiRequest
            {
                ApiKey = configuration.ApiKey
            };

            var upData = new RequestJSONProcessing(requestObj).Processing(dataString);


            //var json = JsonConvert.SerializeObject(postData); // オブジェクトをJSON文字列にシリアライズ
            var content = new StringContent(upData, Encoding.UTF8, "application/json"); // コンテントタイプを指定

            //            var url2 = "http://localhost:8081/api/items/2/updatesite";
            var response2 = await client.PostAsync(configuration.ToUpdatesiteURL(), content); // POSTリクエストを非同期で実行


            return response2;
        }

    }
}

