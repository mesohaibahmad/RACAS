using System;
using System.Collections.Generic;

#nullable disable

namespace RACAS.Models
{
    public partial class TaskType
    {
        public long Id { get; set; }
        public long TaskName { get; set; }
        public long TaskDescription { get; set; }
        public long ComplexityLevel { get; set; }
        public long DefaultNecessaryTime { get; set; }
    }
}
