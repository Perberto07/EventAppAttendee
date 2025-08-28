using EventApp.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventApp.Shared.DTOs.Event
{
    public class UpdateEventStatusDto
    {
        public EventStatus Status { get; set; }
    }
}
