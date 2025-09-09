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
        var task = tasks.FirstOrDefault(t => t.Id == id);

        if (task == null) return NotFound();

        task.Title = updated.Title;
        task.Status = updated.Status;
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return NotFound();

        tasks.Remove(task);
        return NoContent();
    }
}
