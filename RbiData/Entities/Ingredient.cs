using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Entities;

public class Ingredient
{
    public long Id { get; set; }
    public string? Name { get; set; }

    //ingredients that don't exist in the global list can be created by users,
    //but we need to differentiate them from each other so that other users
    //don't get ingredient suggestions with arbitrary names that others created
    public User? User { get; set; }  
}
