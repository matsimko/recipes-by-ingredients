using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Entities;

public class User
{
    public long Id { get; set; }

    public string? Username { get; set; }

    //If a user is anonymous, their public recipes won't display their username to others
    public string? IsAnonymous { get; set; }


}
