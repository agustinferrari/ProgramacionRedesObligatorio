using System;
namespace ServerLogs.Models
{
    public class FilterModel
    {
        public string User { get; set; }
        public string Game { get; set; }
        public DateTime? Date { get; set; }
    }
}