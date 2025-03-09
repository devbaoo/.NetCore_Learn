using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Domains.Entities;

[Table("Students")]
public class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    [Column("Surname")]
    [MaxLength(50)]
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    // [Timestamp]
    // public byte[] RowVersion { get; set; }
    
    [ConcurrencyCheck]  
    public decimal balance { get; set; }
    
    public string Address1 { get; set; }
    
    public string? Address2 { get; set; }
    
    [NotMapped]
    public int Age { get; set; }
    
    [ForeignKey("School")]
    public int SId { get; set; }
    
    public virtual School School { get; set; }
    
    public virtual ICollection<CourseStudent> CourseStudents { get; set; }

}