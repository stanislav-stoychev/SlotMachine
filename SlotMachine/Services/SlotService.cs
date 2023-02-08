using Microsoft.Extensions.Logging;
using SlotMachine.Models;
using System.Text;

namespace SlotMachine.Services;

public interface ISlotService
{
    /// <summary>
    /// Simulates a spin on a slot machine
    /// </summary>
    /// <param name="bet">Bet amount</param>
    /// <returns>The return of the placed wager</returns>
    decimal Spin(
        decimal bet
    );
}

public class SlotService
    : ISlotService
{
    private readonly ICombosGenerator _symbolsGeneratorService;
    private readonly ILogger<SlotService> _logger;

    public SlotService(
        ICombosGenerator symbolsGeneratorService,
        ILogger<SlotService> logger
    )
    {
        _symbolsGeneratorService = symbolsGeneratorService ?? throw new ArgumentNullException(nameof(symbolsGeneratorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Simulates a spin on a slot machine
    /// </summary>
    /// <param name="bet">Bet amount</param>
    /// <returns>The return of the placed wager</returns>
    public decimal Spin(
        decimal bet
    )
    {
        StringBuilder builder = new(Environment.NewLine);
        decimal multiplier = 0;

        foreach(var line in _symbolsGeneratorService.GetLines())
        {
            // Due to the fact that randomized matrix with combos is not stored in
            // memory like a 2D array for example, iteration of generated result as
            // a whole is not possible, thats why a separate cycle is iterated for
            // collectiong each line
            CollectLineInfo(builder, line);
            multiplier += GetLineMultiplier(line);
        }

        _logger.LogInformation("Resulted combinations: {combinations}", builder.ToString());

        return multiplier * bet;
    }

    private static void CollectLineInfo(
        StringBuilder builder, 
        IEnumerable<Symbol> line
    )
    {
        foreach (var symbol in line)
            builder.Append($"{symbol.Alias} ");

        builder.Append(Environment.NewLine);
    }

    private static decimal GetLineMultiplier(
        IEnumerable<Symbol> line
    )
    {
        decimal multiplier = 0;
        string curSymbol = string.Empty;
        foreach(var symbol in line)
        {
            // Wildcards are always welcome :)
            if(symbol.IsWildcard)
            {
                multiplier += symbol.Coefficient;
                continue;
            }

            // A non wildcard was found (curSymbol is string.Empty) in previous iteration and
            // current symbol is different than the one in the current iteration => sorry, no combo for you
            if (curSymbol != string.Empty && curSymbol != symbol.Alias)
                return 0;

            curSymbol = symbol.Alias;
            multiplier += symbol.Coefficient;
        }

        return multiplier;
    }
}
