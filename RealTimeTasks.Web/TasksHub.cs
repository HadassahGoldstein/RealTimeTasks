using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using RealTimeTasks.Data;
using RealTimeTasks.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeTasks.Web
{
    [Authorize]
    public class TasksHub: Hub
    {
        private readonly string _connectionString;
        public TasksHub(IConfiguration configuration)
        {
            _connectionString=configuration.GetConnectionString("ConStr");
        }
        public void AddTask(NewTaskViewModel vm)
        {           
            var task = new TaskItem() { Status = TaskItemStatus.Incomplete, TaskDescription = vm.TaskDesc };
            var taskRepo = new TasksRepository(_connectionString);
            taskRepo.AddItem(task);
            GetAllTasks();
        }
        public void TookTask(ChangeStatusViewModel vm)
        {
            var userRepo = new UserRepository(_connectionString);
            int currentUserId = userRepo.GetByEmail(Context.User.Identity.Name).Id;
            var repo = new TasksRepository(_connectionString);
            repo.WorkingOnItem(vm.TaskId,currentUserId);
            GetAllTasks();
        }
        public void CompletedTask(ChangeStatusViewModel vm)
        {
            var repo = new TasksRepository(_connectionString);
            repo.CompletedItem(vm.TaskId);
            GetAllTasks();
        }
        public void GetAllTasks()
        {
            var repo = new TasksRepository(_connectionString);
            var tasks=repo.GetAllTaskItems();
            Clients.All.SendAsync("getAllTasks", tasks);
        }                
    }
}
