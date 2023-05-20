using Dapper;
using Microsoft.Extensions.Logging;
using RbiData.DAOs;
using RbiData.Entities;
using RbiData.Transactions;
using RbiShared.SearchObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Services;
public class TagService : ITagService
{
	private readonly TransactionExecutor<TagDao> _transactionExecuter;

	public TagService(
		IManagedTransactionFactory transactionFactory,
		IDaoFactory<TagDao> tagDaoFactory)
	{
		_transactionExecuter = new(transactionFactory, tagDaoFactory);
	}

	public Task<IEnumerable<Tag>> SearchTags(string prefix)
	{
		return _transactionExecuter.Execute(tagDao =>
		{
			return tagDao.SearchTags(prefix);
		});
	}
}