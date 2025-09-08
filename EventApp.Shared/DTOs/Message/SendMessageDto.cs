using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Shared.DTOs.Message
{
    public class SendMessageDto
    {
        public int ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
