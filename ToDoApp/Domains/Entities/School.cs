using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Domains.Entities;

public class School
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    
    [NotMapped]
    public virtual ICollection<Student> Students { get; set; }
}