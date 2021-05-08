using System;
using System.Collections.Generic;

#nullable disable

namespace Goal_Achievement_Control_Windows_App.EF.Models
{
    public partial class Goal
    {
        public long Id { get; set; }
        public string Goal1 { get; set; }
        public long UserId { get; set; }
        public byte[] IsMarked { get; set; }
    }
}
