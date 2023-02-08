using SlotMachine.Models;

namespace SlotMachine.Validators;

public static class ConfigValidator
{
    /// <summary>
    /// Validate configuration
    /// </summary>
    /// <param name="configuration">Config model</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void Validate(
        SlotConfiguration configuration
    )
    {
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        PropertyShouldBeGreaterThanZero(nameof(configuration.LineWidth), configuration.LineWidth);
        PropertyShouldBeGreaterThanZero(nameof(configuration.NumberOfLines), configuration.NumberOfLines);

        ValidateSymbols(configuration.Symbols);
    }

    private static void PropertyShouldBeGreaterThanZero(
        string name,
        short value
    )
    {
        if (value <= 0)
            throw new Exception($"Invalid {name} : {value}.");
    }

    private static void ValidateSymbols(
        List<Symbol> symbols
    )
    {
        if (!symbols?.Any() ?? false)
            throw new Exception("Symbols must be defined.");

        if (symbols.GroupBy(g => g.Alias).Count() < symbols.Count)
            throw new Exception("Symbol aliases must be unique.");

        if (symbols.Any(s => s.Odds < 0))
            throw new Exception("Odds must be greater than 0.");

        if (symbols.Sum(s => s.Odds) != 1)
            throw new Exception("Sum of symbol odds must be equal to 1");

        if (symbols.Any(s => s.Coefficient < 0))
            throw new Exception("Coefficient should be equal or greater than 0");
    }
}