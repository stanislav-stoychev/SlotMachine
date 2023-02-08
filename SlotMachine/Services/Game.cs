using Microsoft.Extensions.Logging;

namespace SlotMachine.Services;

public interface IGame
{
    /// <summary>
    /// Starts a game
    /// </summary>
    public void Play();
}

public class Game
    : IGame
{
    private readonly ISlotService _slotService;
    private readonly ILogger<Game> _logger;
    private decimal _balance;

    public Game(
        ISlotService slotService,
        ILogger<Game> logger
    )
    {
        _slotService= slotService ?? throw new ArgumentNullException(nameof(slotService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Starts a game
    /// </summary>
    public void Play()
    {
        _balance = ReadAmounFromKeyboard("Please deposit money you would like to play with:");

        while(_balance > 0)
        {
            var bet = ReadAmounFromKeyboard("Enter stake amount:");
            var wonAmount = _slotService.Spin(bet);
            _balance += wonAmount - bet;

            _logger.LogInformation("You have won: {amount}", wonAmount);
            _logger.LogInformation("Current balance is: {balance}", _balance);
        }
    }

    private decimal ReadAmounFromKeyboard(
        string message
    )
    {
        decimal result;
        while (true)
        {
            _logger.LogInformation("{message}", message);

            var amountStr = Console.ReadLine();
            if (decimal.TryParse(amountStr, out result))
                break;

            _logger.LogError("Please enter valid amount.");
        }

        return result;
    }
}
