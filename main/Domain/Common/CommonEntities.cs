using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public abstract class CommonEntities
    {
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
