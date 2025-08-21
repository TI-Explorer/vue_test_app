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
        task.Id = nextId++;
        tasks.Add(task);
        Console.WriteLine("Task added!" + task.Id + "\n" + task.Title);
        return Ok(task);
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
