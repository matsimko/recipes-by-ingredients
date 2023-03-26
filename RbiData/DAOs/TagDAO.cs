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
public class TagDAO
{
	private readonly IManagedTransaction _mt;

	public TagDAO(IManagedTransaction mt)
	{
		_mt = mt;
	}

	public Task<IEnumerable<Tag>> GetTagsForUser(User user)
	{
		return _mt.Connection.QueryAsync<Tag>(
            "sp_GetTagsForUser",
			new { UserId = user.Id },
			_mt.Transaction,
			commandType: CommandType.StoredProcedure);
	}
}
