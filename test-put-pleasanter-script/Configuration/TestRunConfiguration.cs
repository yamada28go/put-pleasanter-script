using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PutPleasanterScript.Configuration;

namespace test_PutPleasanterScript.Configuration
{

    [TestClass]
    public class ConfigurationValidatorTests
    {
        [TestMethod]
        public void Validate_ValidConfiguration_ReturnsNull()
        {
            // Arrange
            var config = new RunConfiguration
            {
                ApiKey = "validApiKey",
                PleasanterURL = "https://example.com",
                SiteID = 1,
                WebScript = new WebScript
                {
                    ScriptPath = "valid/path",
                    ScriptId = 1
                }
            };

            // Act
            var result = ConfigurationValidator.Validate(config);

            // Assert
            Assert.IsNull(result, "Validation should pass for valid configuration.");
        }

        [TestMethod]
        public void Validate_MissingApiKey_ReturnsErrorMessage()
        {
            // Arrange
            var config = new RunConfiguration
            {
                ApiKey = "",
                PleasanterURL = "https://example.com",
                SiteID = 1,
                WebScript = new WebScript
                {
                    ScriptPath = "valid/path",
                    ScriptId = 1
                }
            };

            // Act
            var result = ConfigurationValidator.Validate(config);

            // Assert
            Assert.AreEqual("APIキーは必須です。", result, "Validation should fail due to missing API key.");
        }

        [TestMethod]
        public void Validate_InvalidPleasanterURL_ReturnsErrorMessage()
        {
            // Arrange
            var config = new RunConfiguration
            {
                ApiKey = "validApiKey",
                PleasanterURL = "invalid-url",
                SiteID = 1,
                WebScript = new WebScript
                {
                    ScriptPath = "valid/path",
                    ScriptId = 1
                }
            };

            // Act
            var result = ConfigurationValidator.Validate(config);

            // Assert
            Assert.AreEqual("プリザンターのURLは有効なURLである必要があります。", result, "Validation should fail due to invalid Pleasanter URL.");
        }

        [TestMethod]
        public void Validate_NegativeSiteID_ReturnsErrorMessage()
        {
            // Arrange
            var config = new RunConfiguration
            {
                ApiKey = "validApiKey",
                PleasanterURL = "https://example.com",
                SiteID = -1,
                WebScript = new WebScript
                {
                    ScriptPath = "valid/path",
                    ScriptId = 1
                }
            };

            // Act
            var result = ConfigurationValidator.Validate(config);

            // Assert
            Assert.AreEqual("サイトIDは0以上である必要があります。", result, "Validation should fail due to negative SiteID.");
        }

        [TestMethod]
        public void Validate_NullWebScript_ReturnsErrorMessage()
        {
            // Arrange
            var config = new RunConfiguration
            {
                ApiKey = "validApiKey",
                PleasanterURL = "https://example.com",
                SiteID = 1,
                WebScript = null
            };

            // Act
            var result = ConfigurationValidator.Validate(config);

            // Assert
            Assert.AreEqual("設定ファイルでWebScripのパラメータを設定してください。", result, "Validation should fail due to null WebScript.");
        }

        [TestMethod]
        public void Validate_InvalidScriptPathInWebScript_ReturnsErrorMessage()
        {
            // Arrange
            var config = new RunConfiguration
            {
                ApiKey = "validApiKey",
                PleasanterURL = "https://example.com",
                SiteID = 1,
                WebScript = new WebScript
                {
                    ScriptPath = "",
                    ScriptId = 1
                }
            };

            // Act
            var result = ConfigurationValidator.Validate(config);

            // Assert
            Assert.AreEqual("スクリプトのパスは必須です。", result, "Validation should fail due to invalid script path in WebScript.");
        }
    }

}

