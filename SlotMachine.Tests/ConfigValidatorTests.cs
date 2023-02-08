using Newtonsoft.Json;
using SlotMachine.Models;
using SlotMachine.Validators;

namespace SlotMachine.Tests;

public class ConfigValidatorTests
{
    private SlotConfiguration _validCfg;

    [SetUp]
    public void SetUp()
        => _validCfg = JsonConvert.DeserializeObject<SlotConfiguration>(File.ReadAllText(@"ValidConfig.json"));

    [Test]
    public void ConfigValidator_behaves_as_expected_with_valid_config()
        => Assert.DoesNotThrow(() => ConfigValidator.Validate(_validCfg));

    [Test]
    public void ConfigValidator_throws_when_line_width_is_not_ok()
    {
        // Arrange
        _validCfg.LineWidth = 0;

        // Act && Assert
        Assert.Throws<Exception>(() => ConfigValidator.Validate(_validCfg));
    }

    [Test]
    public void ConfigValidator_throws_when_line_number_is_not_ok()
    {
        // Arrange
        _validCfg.NumberOfLines = 0;

        // Act && Assert
        Assert.Throws<Exception>(() => ConfigValidator.Validate(_validCfg));
    }

    [Test]
    public void ConfigValidator_throws_when_odds_sum_is_above_1()
    {
        // Arrange
        _validCfg.Symbols.First().Odds = 1M;

        // Act && Assert
        Assert.Throws<Exception>(() => ConfigValidator.Validate(_validCfg));
    }

    [Test]
    public void ConfigValidator_throws_when_odds_contain_negative_coefficient()
    {
        // Arrange
        _validCfg.Symbols.First().Coefficient = -1;

        // Act && Assert
        Assert.Throws<Exception>(() => ConfigValidator.Validate(_validCfg));
    }

    [Test]
    public void ConfigValidator_throws_when_aliases_are_not_unique()
    {
        // Arrange
        foreach (var symbol in _validCfg.Symbols)
            symbol.Alias = "test";

        // Act && Assert
        Assert.Throws<Exception>(() => ConfigValidator.Validate(_validCfg));
    }
}
