using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using NLog;
using PutPleasanterScript.Configuration;

namespace PutPleasanterScript.Service.JSONProcessing
{
    /// <summary>
    /// 更新情報を格納するためのレコード。
    /// </summary>
    public record UpdateInformation(int ScriptId, string Body);

    /// <summary>
    /// JSON データを処理するクラス。
    /// </summary>
    public class SiteJSONProcessing
    {
        private readonly List<UpdateInformation> info;

        /// <summary>
        /// JSONProcessing の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="info">更新情報のリスト。</param>
        public SiteJSONProcessing(List<UpdateInformation> info)
        {
            this.info = info;
        }

        /// <summary>
        /// 応答文字列を処理し、更新情報に基づいて JSON データを変更します。
        /// </summary>
        /// <param name="responseString">処理する JSON 文字列。</param>
        /// <returns>変更された JSON 文字列。</returns>
        public string Processing(string responseString)
        {
            using JsonDocument doc = JsonDocument.Parse(responseString);
            using MemoryStream memoryStream = new MemoryStream();
            using Utf8JsonWriter writer = new Utf8JsonWriter(memoryStream, new JsonWriterOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Indented = true
            });

            var rr = doc.RootElement.GetProperty("Response").GetProperty("Data");

            WriteJsonWithModifiedScriptsBody(rr, writer);
            writer.Flush();

            return System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        /// <summary>
        /// "Scripts" セクションを処理し、必要に応じて "Body" プロパティを更新します。
        /// </summary>
        /// <param name="element">処理する JSON 要素。</param>
        /// <param name="writer">変更を書き込む Utf8JsonWriter オブジェクト。</param>
        private void OnScripts(JsonElement element, Utf8JsonWriter writer)
        {
            writer.WritePropertyName("Scripts");
            writer.WriteStartArray();

            var arrayElement = element.EnumerateArray();
            for (int i = 0; i < arrayElement.Count(); i++)
            {
                JsonElement item = arrayElement.ElementAt(i);
                writer.WriteStartObject();

                // Bodyは書き換えるかもしれないので、
                // 保持しておいて後回しで対応する
                JsonProperty? bodyP = null;
                JsonProperty? idP = null;

                foreach (var subProperty in item.EnumerateObject())
                {
                    if (subProperty.Name == "Body")
                    {
                        bodyP = subProperty;
                        continue;
                    }
                    else if (subProperty.Name == "Id")
                    {
                        idP = subProperty;
                        continue;
                    }
                    writer.WritePropertyName(subProperty.Name);
                    subProperty.Value.WriteTo(writer);
                }

                if (bodyP is null || idP is null)
                {
                    throw new ApplicationException("body要素またはid要素がありません。apiの構造が想定されたものから変更されています。");
                }
                else
                {
                    // Body要素を書き出す
                    var id = idP.Value.Value.GetInt32();
                    var up = this.info.FirstOrDefault(x => x.ScriptId == id);
                    if (up is not null)
                    {
                        writer.WritePropertyName("Body");
                        writer.WriteStringValue(up.Body);
                    }
                    else
                    {
                        //writer.WritePropertyName("Body");
                        bodyP.Value.WriteTo(writer);
                    }

                    //ID要素を書き出す
                    idP.Value.WriteTo(writer);
                }

                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }

        /// <summary>
        /// JSON 要素を再帰的に処理し、"Scripts" セクションがある場合は特別に処理します。
        /// </summary>
        /// <param name="element">処理する JSON 要素。</param>
        /// <param name="writer">変更を書き込む Utf8JsonWriter オブジェクト。</param>
        /// <param name="onSiteSettings">現在 "SiteSettings" セクション内にいるかどうか。</param>
        private void WriteJsonWithModifiedScriptsBody(JsonElement element, Utf8JsonWriter writer, bool onSiteSettings = false)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    if (!onSiteSettings)
                    {
                        writer.WriteStartObject();
                    }

                    foreach (var property in element.EnumerateObject())
                    {
                        if (onSiteSettings && property.Name == "Scripts")
                        {
                            OnScripts(property.Value, writer);
                        }
                        else
                        {
                            writer.WritePropertyName(property.Name);
                            if (property.Name == "SiteSettings")
                            {
                                writer.WriteStartObject();
                                WriteJsonWithModifiedScriptsBody(property.Value, writer, true);
                                writer.WriteEndObject();
                            }
                            else
                            {
                                property.Value.WriteTo(writer);
                            }
                        }
                    }

                    if (!onSiteSettings)
                    {
                        writer.WriteEndObject();
                    }
                    break;
                case JsonValueKind.Array:
                    writer.WriteStartArray();
                    foreach (var item in element.EnumerateArray())
                    {
                        WriteJsonWithModifiedScriptsBody(item, writer);
                    }
                    writer.WriteEndArray();
                    break;
                default:
                    element.WriteTo(writer);
                    break;
            }
        }
    }


    public static class Helper
    {

        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public static UpdateInformation ToUpdateInformation(this RunConfiguration src)
        {

            if (true == string.IsNullOrWhiteSpace(src?.WebScript?.ScriptPath) ||
                 src.WebScript.ScriptPath is null)
            {
                throw new ApplicationException("WebScriptパラメータを設定してください。");
            }
            else
            {
                logger.Info($"アップロード対象のスクリプトファイル : {src.WebScript.ScriptPath}");
                string contentRet = File.ReadAllText(src.WebScript.ScriptPath);
                return new UpdateInformation(src.WebScript.ScriptId, contentRet);
            }
        }

    }


}
