
namespace ToDoApp.Domains.Entities
{
    public class ExamResult
    {
        public int Id { get; set; }

        public int ExamId { get; set; }           // FK đến Exam
        public Exam Exam { get; set; }

        public int StudentId { get; set; }        // FK đến Student
        public Student Student { get; set; }

        public decimal Score { get; set; }
        public DateTime DateCalculated { get; set; }
    }
}

