using System;
using System.Collections.Generic;

namespace SREZAPI.Models
{
    public partial class Client
    {
        public int ClientId { get; set; }
        public int? UserId { get; set; }
        public string FullName { get; set; } = null!;
        public byte[]? Photo { get; set; }

        public virtual User? User { get; set; }
    }
}
