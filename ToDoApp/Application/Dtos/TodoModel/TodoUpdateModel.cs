namespace TodoWeb.Domains.Dtos;

public class TodoUpdateModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}