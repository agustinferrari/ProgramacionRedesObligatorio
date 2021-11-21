using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerLogs.Models
{
    public class FilterModel
    {
        public string User { get; set; }
        public string Game { get; set; }
        public DateTime Date { get; set; }
    }
}
