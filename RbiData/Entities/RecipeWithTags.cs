using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Entities;

[Table("Recipe")]
public class RecipeWithTags : Recipe
{
    public IEnumerable<Tag> Tags { get; set; }
}