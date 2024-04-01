namespace Training2._1.Models
{
    public class Student
    {
        public int Id { get; set; } 
        public string Name { get; set; }
         public int DptId { get; set; }
        public Department? Dpt { get; set; }
    }
}
