using System;
using System.Collections.Generic;

#nullable disable

namespace Goal_Achievement_Control_Windows_App.EF.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public string TelegramId { get; set; }
        public string ChatId { get; set; }
        public string OperatingMode { get; set; }
    }
}
