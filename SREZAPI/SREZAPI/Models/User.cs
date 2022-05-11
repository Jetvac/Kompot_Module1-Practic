using System;
using System.Collections.Generic;

namespace SREZAPI.Models
{
    public partial class User
    {
        public User()
        {
            Clients = new HashSet<Client>();
        }

        public int UserId { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<Client> Clients { get; set; }
    }
}
