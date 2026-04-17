using System.Collections.Generic;

public class StudentReportViewModel
{
    public Student Student { get; set; }
    public List<Marks> Marks { get; set; }
    public double Average { get; set; }
}