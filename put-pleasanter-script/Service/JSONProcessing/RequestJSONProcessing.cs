using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Linq;
using PutPleasanterScript.Models;

namespace PutPleasanterScript.Service.JSONProcessing
{

    /// <summary>
    /// JSON データを処理するクラス。
    /// </summary>
    public class RequestJSONProcessing
    {

        private readonly ApiRequest request;

        /// <summary>
        /// JSONProcessing の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="request">更新情報のリスト。</param>
        public RequestJSONProcessing(ApiRequest request)
        {
            this.request = request;
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

            {
                writer.WriteStartObject();

                {
                    writer.WritePropertyName("ApiVersion");
                    writer.WriteStringValue(this.request.ApiVersion);

                    writer.WritePropertyName("ApiKey");
                    writer.WriteStringValue(this.request.ApiKey);
                }

                WriteJsonWithModifiedScriptsBody(doc.RootElement, writer);
                writer.WriteEndObject();

            }

            writer.Flush();

            return System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
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

                    foreach (var property in element.EnumerateObject())
                    {
                        // コメント行は、「追加」扱いとなってしまうので、無視する
                        if (property.Name != "Comments")
                        {
                            writer.WritePropertyName(property.Name);
                            property.Value.WriteTo(writer);
                        }
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

}
