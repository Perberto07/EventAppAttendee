using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Shared.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        [Required]
        public Conversation Conversation { get; set; } = null!;
        public Guid SenderId { get; set; }
        public User? User { get; set; }
        public string MessageText { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
