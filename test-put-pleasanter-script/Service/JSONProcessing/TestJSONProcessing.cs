using System.Text.Json;
using PutPleasanterScript.Service;
using PutPleasanterScript.Service.JSONProcessing;

namespace test_PutPleasanterScript;

[TestClass]
public class TestJSONProcessing
{

    [TestMethod]
    public void スクリプト置換試験()
    {

        // 実行中のディレクトリのフルパスを取得
        string currentDirectory = Directory.GetCurrentDirectory();

        // ファイルの内容を一括で読み込み
        string content = File.ReadAllText(Path.Combine(currentDirectory, "Data", "TestJSONProcessing", "スクリプト置換試験.json"));

        // スクリプトを置換する
        var up = new List<UpdateInformation> { new UpdateInformation(1, "Hoge") };
        var modifiedJsonString = new SiteJSONProcessing(up).Processing(content);


        Console.WriteLine(modifiedJsonString);

        // 結果判定

        // responseStringを特定の型にデシリアライズ
        var responseObject = JsonSerializer.Deserialize<PutPleasanterScript.Models.Data>(modifiedJsonString);
        Assert.AreEqual(responseObject?.SiteSettings?.Scripts?[0]?.Body, up[0].Body);

    }

    [TestMethod]
    public void スクリプト置換試験_コードが複雑()
    {

        // 実行中のディレクトリのフルパスを取得
        string currentDirectory = Directory.GetCurrentDirectory();

        // ファイルの内容を一括で読み込み
        string content = File.ReadAllText(Path.Combine(currentDirectory, "Data", "TestJSONProcessing", "スクリプト置換試験.json"));

        // スクリプトを置換する
        var up = new List<UpdateInformation> { new UpdateInformation(1, "const anExampleVariable = \"Hello World\";\nconsole.log(anExampleVariable);\n") };
        var modifiedJsonString = new SiteJSONProcessing(up).Processing(content);


        Console.WriteLine(modifiedJsonString);

        // 結果判定

        // responseStringを特定の型にデシリアライズ
        var responseObject = JsonSerializer.Deserialize<PutPleasanterScript.Models.Data>(modifiedJsonString);
        Assert.AreEqual(responseObject?.SiteSettings?.Scripts?[0]?.Body, up[0].Body);

    }



    [TestMethod]
    public void 置換しなければ同じデータとなるか()
    {

        // 実行中のディレクトリのフルパスを取得
        string currentDirectory = Directory.GetCurrentDirectory();

        // ファイルの内容を一括で読み込み
        string content = File.ReadAllText(Path.Combine(currentDirectory, "Data", "TestJSONProcessing", "スクリプト置換試験.json"));

        // スクリプトを置換する
        var up = new List<UpdateInformation> { };
        var modifiedJsonString = new SiteJSONProcessing(up).Processing(content);
        var mjs = NormalizeJsonString(modifiedJsonString);

        string contentRet = File.ReadAllText(Path.Combine(currentDirectory, "Data", "TestJSONProcessing", "置換しなければ同じデータとなるか_ 変更結果データ.json"));
        var mc = NormalizeJsonString(contentRet);

        Console.WriteLine(modifiedJsonString);
        Console.WriteLine("---");
        Console.WriteLine(contentRet);

        Assert.AreEqual(mjs, mc);

    }


    [TestMethod]
    public void 対象スクリプトのindex指定()
    {

        // 実行中のディレクトリのフルパスを取得
        string currentDirectory = Directory.GetCurrentDirectory();

        // ファイルの内容を一括で読み込み
        string content = File.ReadAllText(Path.Combine(currentDirectory, "Data", "TestJSONProcessing", "対象スクリプトのindex指定.json"));

        // スクリプトを置換する
        var up = new List<UpdateInformation> { new UpdateInformation(3, "Hoge") };
        var modifiedJsonString = new SiteJSONProcessing(up).Processing(content);


        Console.WriteLine(modifiedJsonString);

        // 結果判定

        // responseStringを特定の型にデシリアライズ
        var responseObject = JsonSerializer.Deserialize<PutPleasanterScript.Models.Data>(modifiedJsonString);
        Assert.AreEqual(responseObject?.SiteSettings?.Scripts?[2]?.Body, up[0].Body);


    }



    [TestMethod]
    public void 更新情報が空のリストの場合の処理()
    {
        // Arrange
        var updates = new List<UpdateInformation>(); // 空の更新情報リスト
        var jsonProcessing = new SiteJSONProcessing(updates);
        var inputJson = @"
            {
                ""Response"": {
                    ""Data"": {
                        ""Scripts"": [
                            {
                                ""Title"": ""test"",
                                ""Body"": ""console.log(\""A\"")"",
                                ""Id"": 1
                            }
                        ]
                    }
                }
            }";

        // Act
        var resultJson = jsonProcessing.Processing(inputJson);

        var 正解 = @"
            {
                        ""Scripts"": [
                            {
                                ""Title"": ""test"",
                                ""Body"": ""console.log(\""A\"")"",
                                ""Id"": 1
                            }
                        ]
            }";


        // Assert
        Assert.AreEqual(NormalizeJsonString(正解), NormalizeJsonString(resultJson), "JSON should not be modified when update list is empty.");
    }

    /// <summary>
    /// 2個のjsonを比較するために整形する
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    private static string NormalizeJsonString(string json)
    {
        // Utility method to remove insignificant whitespace and standardize JSON string format
        // for comparison purposes.
        return System.Text.Json.JsonSerializer.Serialize(
            System.Text.Json.JsonSerializer.Deserialize<object>(json),
            new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
    }


}
