using crm.Backend.Entities;
using crm.Backend.Attributes;
using Microsoft.EntityFrameworkCore;

namespace crm.Backend.Services
{
    public interface ITaskService
    {
        Task<IBaseResponse<Entities.Task>> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task<Guid> CreateAsync(Entities.Task task);
        System.Threading.Tasks.Task<Guid> UpdateAsync(Entities.Task task);
        System.Threading.Tasks.Task DeleteAsync(Guid id);
        System.Threading.Tasks.Task<Guid> UpdateStateAsync(Entities.Task task);
        Task<IBaseResponse<List<Entities.Task>>> GetTasksAsync();

    }

    [DITransient]
    public class TaskService : ITaskService
    {

        private readonly CrmDbContext _dbContext;
        public TaskService(
            CrmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async System.Threading.Tasks.Task<Guid> CreateAsync(Entities.Task task)
        {
            var newTask = await _dbContext.AddAsync(task);
            await _dbContext.SaveChangesAsync();
            return newTask.Entity.Id;
        }

        public async System.Threading.Tasks.Task<Guid> UpdateAsync(Entities.Task task)
        {
            var oldTask = await _dbContext.Tasks.FirstAsync(x => x.Id == task.Id);


            oldTask.Title = task.Title;
            oldTask.Description = task.Description;
            oldTask.Type = task.Type;
            oldTask.Priority = task.Priority;
            oldTask.ExecutionDate = task.ExecutionDate;
            oldTask.AssignedTo = task.AssignedTo;
            oldTask.Location = task.Location;
            oldTask.State = task.State;
            oldTask.AssignedToCompany = task.AssignedToCompany;

            _dbContext.Update(oldTask);

            await _dbContext.SaveChangesAsync();
            return oldTask.Id;
        }

        public async System.Threading.Tasks.Task<Guid> UpdateStateAsync(Entities.Task task)
        {
            var oldTask = await _dbContext.Tasks.FirstAsync(x => x.Id == task.Id);

            oldTask.State = task.State;

            _dbContext.Update(oldTask);

            await _dbContext.SaveChangesAsync();
            return oldTask.Id;
        }

        public async Task<IBaseResponse<Entities.Task>> GetByIdAsync(Guid id)
        {
            var baseResponse = new BaseResponse<Entities.Task>() { Description = "" };
            try
            {
                baseResponse.Description = "Hello from Task.GetById";
                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = await _dbContext.Tasks.FirstAsync(x => x.Id == id);
                return baseResponse;
            }
            catch (Exception)
            {
                baseResponse.Description = "Fuck u no data";
                baseResponse.StatusCode = StatusCode.InternalServerError;
                return baseResponse;
            }
        }

        public async Task<IBaseResponse<List<Entities.Task>>> GetTasksAsync()
        {
            var baseResponse = new BaseResponse<List<Entities.Task>>() { Description = "" };
            try
            {
                baseResponse.Description = "Hello from Task.GetById";
                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = await _dbContext.Tasks.ToListAsync();
                return baseResponse;
            }
            catch (Exception)
            {
                baseResponse.Description = "Fuck u no data";
                baseResponse.StatusCode = StatusCode.InternalServerError;
                return baseResponse;
            }
        }

        public async System.Threading.Tasks.Task DeleteAsync(Guid id)
        {
            var taskToDelete = await _dbContext.Tasks.FirstAsync(x => x.Id == id);
            _dbContext.Tasks.Remove(taskToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }

}
