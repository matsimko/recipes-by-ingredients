using RbiShared.Units;

namespace RbiFrontend.Services;

public class UnitService
{
    private readonly Unit[] _units;
    public string[] AvailableUnitSymbols { get; }

    public UnitService()
    {
        var kilogram = new Unit(new Unit.Form[] 
        {
            new Unit.Form("g", null),
            new Unit.Form("kg", 1000) 
        });
		var litre = new Unit(new Unit.Form[] 
        {
            new Unit.Form("ml", null), 
            new Unit.Form("l", 1000) 
        });
		var cup = new Unit(new Unit.Form[] 
        {
            new Unit.Form("tsp.", null),
            new Unit.Form("tbsp.", 3),
            new Unit.Form("cup", 16) 
        });
       
        _units = new Unit[] { kilogram, litre, cup };
        AvailableUnitSymbols = _units.SelectMany(u => u.ValidSymbols).ToArray();
	}
    
	public (float, string) GetSuitableAmount(float amount, string symbol)
    {
        foreach(var unit in _units)
        {
            if (unit.ValidSymbols.Contains(symbol))
            {
                return unit.GetSuitableAmount(amount, symbol);
            }
        }
        return (amount, symbol);
    }
}
