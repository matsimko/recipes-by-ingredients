﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Entities;


public class Recipe
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsPublic { get; set; }
    public User? User { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? CreationDate { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}