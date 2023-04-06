using Dapper;
using Microsoft.Extensions.Logging;
using RbiData.DAOs;
using RbiData.Entities;
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
	private readonly IManagedTransactionFactory _transactionFactory;
	private readonly IDaoFactory<TagDao> _tagDaoFactory;

	public TagService(
		IManagedTransactionFactory transactionFactory,
		IDaoFactory<TagDao> tagDao)
	{
		_transactionFactory = transactionFactory;
		_tagDaoFactory = tagDao;
	}

	public async Task<IEnumerable<Tag>> SearchTags(string prefix)
	{
		using var transaction = _transactionFactory.Create();
		var tagDao = _tagDaoFactory.Create(transaction);
		var tags = await tagDao.SearchTags(prefix);
		transaction.Commit();
		return tags;
	}
}