using Dapper;
using Dapper.Contrib.Extensions;
using RbiData.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.DAOs;
public class IngredientDAO
{
	private readonly IManagedTransaction _mt;

	public IngredientDAO(IManagedTransaction mt)
	{
		_mt = mt;
	}

	public async Task<Ingredient> Insert(Ingredient ingredient)
	{
		var id = await _mt.Connection.InsertAsync(ingredient, _mt.Transaction);
		ingredient.Id = id;
		return ingredient;
	}

	public async Task Delete(Ingredient ingredient)
	{
		await _mt.Connection.DeleteAsync(ingredient, _mt.Transaction);
	}

	public Task<IEnumerable<Ingredient>> GetIngredientsForUser(User user)
	{
		return _mt.Connection.QueryAsync<Ingredient>(
			"sp_GetIngredientsForUser",
			new { UserId = user.Id },
			_mt.Transaction,
			commandType: CommandType.StoredProcedure);
	}
}
