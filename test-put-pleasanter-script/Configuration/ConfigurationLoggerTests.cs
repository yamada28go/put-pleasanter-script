using System;
using PutPleasanterScript.Configuration;

namespace test_PutPleasanterScript.Configuration
{

    [TestClass]
    public class ConfigurationLoggerTests
    {
        [TestMethod]
        public void ToStringForLogging_ValidConfiguration_ReturnsCorrectString()
        {
            // Arrange
            var runConfig = new RunConfiguration
            {
                ApiKey = "123456789",
                PleasanterURL = "https://example.com",
                SiteID = 100,
                WebScript = new WebScript
                {
                    ScriptPath = "/path/to/script",
                    ScriptId = 10
                }
            };

            var expected = "RunConfiguration:" + Environment.NewLine +
                           "- ApiKey: 123456789" + Environment.NewLine +
                           "- PleasanterURL: https://example.com" + Environment.NewLine +
                           "- SiteID: 100" + Environment.NewLine +
                           "  WebScript:" + Environment.NewLine +
                           "  - ScriptPath: /path/to/script" + Environment.NewLine +
                           "  - ScriptId: 10" + Environment.NewLine;

            // Act
            var actual = ConfigurationLogger.ToStringForLogging(runConfig);

            // Assert
            Assert.AreEqual(expected, actual, "The ToStringForLogging method did not return the expected string.");
        }

        [TestMethod]
        public void ToStringForLogging_NullWebScript_ReturnsCorrectStringWithNullWebScript()
        {
            // Arrange
            var runConfig = new RunConfiguration
            {
                ApiKey = "123456789",
                PleasanterURL = "https://example.com",
                SiteID = 100,
                WebScript = null // WebScript is null
            };

            var expected = "RunConfiguration:" + Environment.NewLine +
                           "- ApiKey: 123456789" + Environment.NewLine +
                           "- PleasanterURL: https://example.com" + Environment.NewLine +
                           "- SiteID: 100" + Environment.NewLine +
                           "  WebScript: null" + Environment.NewLine;

            // Act
            var actual = ConfigurationLogger.ToStringForLogging(runConfig);

            // Assert
            Assert.AreEqual(expected, actual, "The ToStringForLogging method did not return the expected string when WebScript is null.");
        }
    }
}