namespace StudentManagerApp
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public double AverageGrade { get; set; }

        public void ShowInfo()
        {
            Console.WriteLine($"{Id}: {FullName}, группа {Group}, средний балл {AverageGrade}");
        }
    }
}
