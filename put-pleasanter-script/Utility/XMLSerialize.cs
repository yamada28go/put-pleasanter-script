using System;
using System.Text;
using System.Xml.Serialization;

namespace PutPleasanterScript.Utility
{
    /// <summary>
    /// XML形式でシリアライズするユーティリティクラス
    /// </summary>
    public class XMLSerialize
    {
        /// <summary>
        /// 指定されたXML定義をシリアライズする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="xmlFile"></param>
        public static void Serialize<T>(T obj, string xmlFile)
        {
            var xmlSerializer1 = new XmlSerializer(typeof(T));
            using (var streamWriter = new StreamWriter(xmlFile, false, Encoding.UTF8))
            {
                xmlSerializer1.Serialize(streamWriter, obj);
                streamWriter.Flush();
            }
        }

        /// <summary>
        /// 指定されたXML定義をデシリアライズ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="xmlFile"></param>
        public static T Deserialize<T>(string xmlFile) where T : new()
        {
            var xmlSerializer2 = new XmlSerializer(typeof(T));
            var xmlSettings = new System.Xml.XmlReaderSettings()
            {
                CheckCharacters = false,
            };
            using (var streamReader = new StreamReader(xmlFile, Encoding.UTF8))
            using (var xmlReader
                    = System.Xml.XmlReader.Create(streamReader, xmlSettings))
            {
                var result = (T?)xmlSerializer2.Deserialize(xmlReader);
                if (result is not null)
                {
                    return result;
                }
            }

            throw new ApplicationException("ファイルの読み込みに失敗しました。");

        }

    }
}

