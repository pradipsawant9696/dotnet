using System.ComponentModel.DataAnnotations;

public class Marks
{
    public int MarkID { get; set; }
    public int StudentID { get; set; }

    [Required(ErrorMessage = "Subject is required")]
    [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only alphabets allowed")]
    public string SubjectName { get; set; }

    [Required(ErrorMessage = "Marks required")]
    [Range(0, 100, ErrorMessage = "Marks must be between 0 and 100")]
    public int MarksObtained { get; set; }

    [Required]
    [Range(1, 100)]
    public int MaxMarks { get; set; }
}