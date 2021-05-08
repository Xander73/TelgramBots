using System;
using System.Collections.Generic;

#nullable disable

namespace Goal_Achievement_Control_Windows_App.EF.Models
{
    public partial class Mark
    {
        public long Id { get; set; }
        public string Date { get; set; }
        public string Mark1 { get; set; }
        public long GoalId { get; set; }
    }
}
