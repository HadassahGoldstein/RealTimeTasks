using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RealTimeTasks.Data
{
    public class TasksRepository
    {
        private readonly string _connectionString;
        public TasksRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<TaskItem> GetAllTaskItems()
        {
            using var context = new TasksDbContext(_connectionString);
            return context.TaskItems.Where(t => t.Status != TaskItemStatus.Completed).ToList();
        }
        public void CompletedItem(int id)
        {
            using var context = new TasksDbContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"UPDATE TaskItems SET Status={TaskItemStatus.Completed} WHERE Id={id}");
        }
        public void WorkingOnItem(int id,int userId)
        {
            using var context = new TasksDbContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"UPDATE TaskItems SET Status={TaskItemStatus.InProgress}, UserId={userId} WHERE Id={id}");
        }
        public void AddItem(TaskItem task)
        {
            using var context = new TasksDbContext(_connectionString);
            context.TaskItems.Add(task);
            context.SaveChanges();
        }
        public User GetUserById(int id)
        {
            using var context = new TasksDbContext(_connectionString);
            return context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
