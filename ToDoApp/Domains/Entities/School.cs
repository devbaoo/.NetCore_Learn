using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDoApp.Domains.Entities;

public class School
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    
    [NotMapped]
    [JsonIgnore]
    public virtual ICollection<Student> Students { get; set; }
}