using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Shared.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public Guid? AdminId { get; set; }
        public User? Admin {  get; set; }
        public Guid? OrganizerId { get; set; }
        public User? Organizer { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
