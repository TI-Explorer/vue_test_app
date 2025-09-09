namespace backend;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "ToDo"; //ToDo, InProgress, Done
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int Priority { get; set; } = 0; //low = 0 med = 1 high = 2 
}