using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Shared.DTOs.Message
{
    public class CreateConversationDto
    {
        public Guid AdminId { get; set; }
        public Guid OrganizerId { get; set; }
    }
}
