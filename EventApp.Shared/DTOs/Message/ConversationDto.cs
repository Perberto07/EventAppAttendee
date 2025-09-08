using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Shared.DTOs.Message
{
    public class ConversationDto
    {
        public int Id { get; set; }
        public Guid? AdminId { get; set; }
        public string? AdminUsername { get; set; }
        public Guid? OrganizerId { get; set; }
        public string? OrganizerUsername { get; set; }
        public DateTime Created { get; set; }
        public List<MessageDto> Messages { get; set; } = new();
    }
}
