using Microsoft.AspNetCore.Mvc;
using crm.Backend.Services;
using crm.Backend.Models;
using crm.Backend.Extensions;
using Microsoft.AspNetCore.Authorization;



namespace crm.Backend.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	public class TaskController(ILogger<TaskController> _logger, ITaskService _taskService) : Controller
	{

		[HttpGet]
		public async Task<IActionResult> GetTask([FromQuery] Guid id, CancellationToken cancellationToken)
		{
			var task = await _taskService.GetByIdAsync(id);

			return Ok(task);
		}

		[HttpGet("getTasks")]
		public async Task<IActionResult> GetTasks(CancellationToken cancellationToken)
		{
			var tasks = await _taskService.GetTasksAsync();

			return Ok(tasks);
		}

		[HttpPost("createTask")]
		public async Task<IActionResult> CreateTask([FromBody] TaskModel task, CancellationToken cancellationToken)
		{
			var res = await _taskService.CreateAsync(task.ToTask());

			return Ok(res);
		}

		[HttpPost("update")]
		public async Task<IActionResult> UpdateTask([FromBody] TaskModel task, CancellationToken cancellationToken)
		{

			var res = await _taskService.UpdateAsync(task.ToTask());

			return Ok(res);
		}


		[HttpPost("updateState")]
		public async Task<IActionResult> UpdateTaskState([FromBody] TaskModel task, CancellationToken cancellationToken)
		{

			var res = await _taskService.UpdateStateAsync(task.ToTask());

			return Ok(res);
		}


		[HttpPost("delete")]
		public async Task<IActionResult> DeleteTask([FromBody] TaskModel task, CancellationToken cancellationToken)
		{
			await _taskService.DeleteAsync(task.Id);

			return Ok();
		}
	}

}