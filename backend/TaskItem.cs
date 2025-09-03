namespace backend;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "ToDo"; //ToDo, InProgress, Done

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}