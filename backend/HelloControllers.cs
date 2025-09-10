using Microsoft.AspNetCore.Mvc;

namespace backend;

[ApiController]
[Route("api/[controller]")]

public class TasksController : ControllerBase
{
    private static List<TaskItem> tasks = new();
    private static int nextId = 1;

    [HttpGet]
    public IActionResult Get() => Ok(tasks);

    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
        try
        {
            task.Id = nextId++;
            task.CreatedDate = DateTime.UtcNow;
            tasks.Add(task);
            string formattedTaskInfo = $"----------------\nThere has been a new task added\nTask id: {task.Id}\nTask Title: {task.Title}\nTask timestamp: {task.CreatedDate}\nPriority: {task.Priority}\n\n";
            Console.WriteLine(formattedTaskInfo);
            return Ok(task);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in Creating task: {e.Message}");
            return StatusCode(500, "An Error occurred while creating the task");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, TaskItem updated)
    {

        try
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null) return NotFound();
            var oldTitle = task.Title;
            var oldStatus = task.Status;

            task.Title = updated.Title;
            task.Status = updated.Status;
            string formattedTaskString = $"----------------\nTask Updated\nTask id: {task.Id}\nTask Title: {task.Title}\nTask Status: {task.Status}\nTask timestamp: {task.CreatedDate}\nPriority: {task.Priority}\nOld Title: {oldTitle}\nOld Status: {oldStatus}\n\n";
            Console.WriteLine(formattedTaskString);
            return Ok(task);
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nThere was an error updating task {id}\nError: {e.Message}\n");
            return StatusCode(500, "An error occured updating a task\n");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            string formattedDeleteTask = $"----------------\nTask being Deleted: {task.Id}\n\n";
            Console.WriteLine(formattedDeleteTask);
            tasks.Remove(task);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nThere was an error deleting task: {id}\nError: {e.Message}\n");
            return StatusCode(500, "An error occurred when trying to delete the task\n");
        }
    }
}
