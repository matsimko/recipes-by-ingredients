using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiShared.Units;
public class Unit
{
	public class Form
	{
		public string Symbol { get; }
		public float? BackConversionRate { get; }

		public Form(string symbol, float? conversionRate)
		{
			Symbol = symbol;
			BackConversionRate = conversionRate;
		}
	}

	public string[] ValidSymbols { get; }
	private readonly Form[] _forms;

	public Unit(Form[] forms)
	{
		_forms = forms;
		ValidSymbols = forms.Select(f => f.Symbol).ToArray();
	}

	public (float, string) GetSuitableAmount(float amount, string symbol)
	{
		try
		{
			int i = Array.FindIndex(_forms, f => f.Symbol == symbol);
			return GetSuitableAmount(amount, i);
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException("Unknown unit symbol, or unit forms were not set up properly", ex);
		}
	}

	private (float, string) GetSuitableAmount(float amount, int i)
	{
		var form = _forms[i];
		if (amount < 1 && form.BackConversionRate != null)
		{
			return GetSuitableAmount(amount * (float)form.BackConversionRate, i - 1);
		}
		else if (i < _forms.Count() - 1 && amount >= _forms[i + 1].BackConversionRate)
		{
			return GetSuitableAmount(amount / (float)_forms[i + 1].BackConversionRate, i + 1);
		}

		return (amount, form.Symbol);
	}
}
