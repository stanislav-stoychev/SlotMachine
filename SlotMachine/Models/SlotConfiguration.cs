namespace SlotMachine.Models;

// Ideally poperties will be init, but for ease of testing they are set
public class SlotConfiguration
{
    public short LineWidth { get; set; }
    
    public short NumberOfLines { get; set; }

    public List<Symbol> Symbols { get; set; }
}
