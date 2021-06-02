using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RealTimeTasks.Data
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string TaskDescription { get; set; }
        public TaskItemStatus Status { get; set; }
        public int UserId { get; set; }      
    }
}
