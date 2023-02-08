namespace SlotMachine.Models;

// Ideally poperties will be init, but for ease of testing they are set
public class Symbol
{
    public string Name { get; set; }

    public string Alias { get; set; }

    public decimal Odds { get; set; }

    public decimal Coefficient { get; set; }

    public bool IsWildcard { get; set; }
}