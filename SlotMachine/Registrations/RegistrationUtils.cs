using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlotMachine.Models;
using SlotMachine.Services;
using SlotMachine.Validators;

namespace SlotMachine.Registrations;

public static class RegistrationUtils
{
    /// <summary>
    /// Registers needed components for the game
    /// </summary>
    /// <param name="ctx">Host builder</param>
    /// <param name="services">Services</param>
    public static void RegisterDependencies(
        HostBuilderContext ctx, 
        IServiceCollection services
    )
    {
        var cfg = ctx.Configuration
            .GetSection(nameof(SlotConfiguration))
            .Get<SlotConfiguration>();

        ConfigValidator.Validate(cfg);

        services.AddSingleton(cfg)
            .AddSingleton<ISlotService, SlotService>()
            .AddSingleton<ICombosGenerator, CombosGenerator>()
            .AddSingleton<IGame, Game>();
    }
}
