using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RbiData.Entities;

namespace RbiData.Services;
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string name, long id) : this($"{name} with id {id} was not found")
    {
    }

    public EntityNotFoundException(string? message) : base(message)
    {
    }
}
