using SlotMachine.Models;

namespace SlotMachine.Services;

public interface ICombosGenerator
{
    /// <summary>
    /// Iterates over randomized lines
    /// </summary>
    /// <returns>Stream of lines</returns>
    public IEnumerable<IEnumerable<Symbol>> GetLines();
}

public class CombosGenerator
    : ICombosGenerator
{
    private readonly SlotConfiguration _configuration;
    /// Pool which holds the ocurrances of the symbols whith respect of the specified percentages
    private readonly string[] _symbolsPool;
    /// Size of the pool. This may vary because for example if you have 0.1%, you will need
    /// an array of 1000 element to express the distribution
    private readonly int _symbolsPoolSize;
    private readonly Random _rng = new();

    public CombosGenerator(
        SlotConfiguration configuration
    )
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _symbolsPoolSize = GetSizeOfSymbolsPool();
        _symbolsPool = InitializeSymbolsPool();
    }

    /// <summary>
    /// Iterates over randomized lines
    /// </summary>
    /// <returns>Stream of lines</returns>
    public IEnumerable<IEnumerable<Symbol>> GetLines()
    {
        for (int i = 0; i < _configuration.NumberOfLines; i++)
        {
            var result = new List<Symbol>();
            for (int j = 0; j < _configuration.LineWidth; j++)
            {
                var a = GetRandomSymbol();
                result.Add(_configuration.Symbols.First(s => s.Alias.Equals(a)));
            }
            
            yield return result;
        }
    }

    /// <summary>
    /// Gets "random" symbol
    /// </summary>
    /// <returns>Symbol</returns>
    private string GetRandomSymbol()
        => _symbolsPool[_rng.Next(0, _symbolsPoolSize)];

    /// <summary>
    /// Gets the size which the pool needs to be so even lowest percentages can be represented in the distribution
    /// </summary>
    /// <returns>Size</returns>
    private int GetSizeOfSymbolsPool()
        => (int)_configuration.Symbols.Max(s => Math.Pow(10, BitConverter.GetBytes(decimal.GetBits(s.Odds)[3])[2]));

    /// <summary>
    /// Initializes the pool
    /// </summary>
    /// <returns>Pool with fairly distributed symbols with respect to the configuration</returns>
    private string[] InitializeSymbolsPool()
        => _configuration.Symbols.SelectMany(s => Enumerable.Repeat(s.Alias, (int)(s.Odds * _symbolsPoolSize)))
            .ToArray();
}
