using crm.Backend.Models;

namespace crm.Backend.Extensions
{
    public static class TaskExtensions
    {
        public static Entities.Task ToTask(this TaskModel task)
        {
            return new Entities.Task()
            {
                Id = task.Id,
                AssignedTo = task.AssignedTo,
                AssignedToCompany = task.AssignedToCompany,
                Description = task.Description,
                Title = task.Title,
                Type = task.Type,
                Priority = task.Priority,
                State = task.State,
                ExecutionDate = task.ExecutionDate,
                Location = task.Location
            };
        }
    }
}