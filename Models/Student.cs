using System.ComponentModel.DataAnnotations;

public class Student
{
    public int ID { get; set; }

    [Required(ErrorMessage = "Name is required")]
   [RegularExpression(@"^[A-Za-z]+(\s[A-Za-z]+)+$", ErrorMessage = "Enter full name (First and Last)")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    public string Name { get; set; }

[Required(ErrorMessage = "Roll No is required")]
[RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numbers allowed")]
public string RollNo { get; set; }


    [Required(ErrorMessage = "Class is required")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Only letters allowed")]
    public string Class { get; set; }
}