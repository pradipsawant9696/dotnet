using Microsoft.AspNetCore.Mvc;
using System.Data;

public class StudentController : Controller
{
    private readonly IConfiguration _config;

    public StudentController(IConfiguration config)
    {
        _config = config;
    }

public IActionResult Index()
{
    DbHelper db = new DbHelper(_config.GetConnectionString("DefaultConnection"));
    DataTable dt = db.GetStudents();

    // 👉 Add new column for Average
    dt.Columns.Add("Average");

    int total = dt.Rows.Count;
    int pass = 0;
    int fail = 0;

    foreach (DataRow row in dt.Rows)
    {
        int id = Convert.ToInt32(row["ID"]);
        var marks = db.GetMarksByStudent(id);

        double totalMarks = 0;

        foreach (DataRow m in marks.Rows)
        {
            totalMarks += Convert.ToInt32(m["MarksObtained"]);
        }

        // ✅ FIXED LOGIC
        if (marks.Rows.Count > 0)
        {
            double avg = totalMarks / marks.Rows.Count;

            row["Average"] = avg;   // store average

            if (avg >= 40)
                pass++;
            else
                fail++;
        }
        else
        {
            row["Average"] = "Not Assigned"; // ✅ no marks case
        }
    }

    ViewBag.Total = total;
    ViewBag.Pass = pass;
    ViewBag.Fail = fail;

    return View(dt);
}

    public IActionResult Create()
    {
        return View();
    }

[HttpPost]
public IActionResult Create(Student s)
{
    DbHelper db = new DbHelper(_config.GetConnectionString("DefaultConnection"));

        s.RollNo = db.GetNextRollNo(s.Class).ToString();


    // // ✅ Check duplicate FIRST
    // if (db.IsRollNoExists(s.RollNo))
    // {
    //     ModelState.AddModelError("RollNo", $"Roll Number {s.RollNo} already exists!");
    // }
    

    if (ModelState.IsValid)
    {
        db.AddStudent(s);
        return RedirectToAction("Index");
    }

    return View(s);
}

     public IActionResult AddMarks(int id)
     {
         Marks m = new Marks();
         m.StudentID = id;   // ✅ VERY IMPORTANT
     
         return View(m);
     }

[HttpPost]
public IActionResult AddMarks(Marks m)
{
    if (ModelState.IsValid)
    {
        DbHelper db = new DbHelper(_config.GetConnectionString("DefaultConnection"));
        db.AddMarks(m);
        return RedirectToAction("Index");
        Console.WriteLine("StudentID: " + m.StudentID);
Console.WriteLine("Marks: " + m.MarksObtained);
    }
    return View(m);
}

public IActionResult Report(int id)
{
    DbHelper db = new DbHelper(_config.GetConnectionString("DefaultConnection"));

    var student = db.GetStudentById(id);
    var marksTable = db.GetMarksByStudent(id);

    List<Marks> marksList = new List<Marks>();
    double total = 0;

    foreach (DataRow row in marksTable.Rows)
    {
        Marks m = new Marks
        {
             MarkID = Convert.ToInt32(row["MarkID"]),   // ✅ ADD THIS
             StudentID = id,                            // ✅ ALSO ADD THIS
            SubjectName = row["SubjectName"].ToString(),
            MarksObtained = Convert.ToInt32(row["MarksObtained"]),
            MaxMarks = Convert.ToInt32(row["MaxMarks"])
        };

        total += m.MarksObtained;
        marksList.Add(m);
    }

    double avg = marksList.Count > 0 ? total / marksList.Count : 0;

    StudentReportViewModel vm = new StudentReportViewModel
    {
        Student = student,
        Marks = marksList,
        Average = avg
    };

    return View(vm);
}

public IActionResult Edit(int id)
{
    DbHelper db = new DbHelper(_config.GetConnectionString("DefaultConnection"));
    var student = db.GetStudentById(id);
    return View(student);
}

[HttpPost]
public IActionResult Edit(Student s)
{
    if (ModelState.IsValid)
    {
        DbHelper db = new DbHelper(_config.GetConnectionString("DefaultConnection"));
        db.UpdateStudent(s);
        return RedirectToAction("Index");
    }
    return View(s);
}

public IActionResult Delete(int id)
{
    DbHelper db = new DbHelper(_config.GetConnectionString("DefaultConnection"));
    db.DeleteStudent(id);

    return RedirectToAction("Index");
}

public IActionResult EditMarks(int id)
{
    DbHelper db = new DbHelper(_config.GetConnectionString("DefaultConnection"));
    var mark = db.GetMarkById(id);

    return View(mark);
}

[HttpPost]
public IActionResult EditMarks(Marks m)
{
    if (ModelState.IsValid)
    {
        DbHelper db = new DbHelper(_config.GetConnectionString("DefaultConnection"));
        db.UpdateMarks(m);

        return RedirectToAction("Report", new { id = m.StudentID });
    }

    return View(m);
}

[HttpGet]
public JsonResult GetRollNo(string studentClass)
{
    DbHelper db = new DbHelper(_config.GetConnectionString("DefaultConnection"));
    int rollNo = db.GetNextRollNo(studentClass);

    return Json(rollNo);
}

}