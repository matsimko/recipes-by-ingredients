using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Entities;

public class RecipeWithTags : Recipe
{
    public List<Tag> Tags { get; set; } = new List<Tag>();
}