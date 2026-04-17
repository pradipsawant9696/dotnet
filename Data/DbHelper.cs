using MySql.Data.MySqlClient;
using System.Data;

public class DbHelper
{
    private readonly string connectionString;

    public DbHelper(string conn)
    {
        connectionString = conn;
    }

    public DataTable GetStudents()
    {
        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM Students";
            MySqlDataAdapter da = new MySqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    public void AddStudent(Student s)
    {
        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            string query = "INSERT INTO Students(Name, RollNo, Class) VALUES(@Name,@RollNo,@Class)";
            MySqlCommand cmd = new MySqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Name", s.Name);
            cmd.Parameters.AddWithValue("@RollNo", s.RollNo);
            cmd.Parameters.AddWithValue("@Class", s.Class);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public void AddMarks(Marks m)
{
    using (MySqlConnection con = new MySqlConnection(connectionString))
    {
        string query = "INSERT INTO Marks(StudentID, SubjectName, MarksObtained, MaxMarks) VALUES(@StudentID,@SubjectName,@MarksObtained,@MaxMarks)";
        MySqlCommand cmd = new MySqlCommand(query, con);

        cmd.Parameters.AddWithValue("@StudentID", m.StudentID);
        cmd.Parameters.AddWithValue("@SubjectName", m.SubjectName);
        cmd.Parameters.AddWithValue("@MarksObtained", m.MarksObtained);
        cmd.Parameters.AddWithValue("@MaxMarks", m.MaxMarks);

        con.Open();
        cmd.ExecuteNonQuery();
    }
}

public DataTable GetMarksByStudent(int studentId)
{
    using (MySqlConnection con = new MySqlConnection(connectionString))
    {
        string query = "SELECT * FROM Marks WHERE StudentID=@StudentID";
        MySqlCommand cmd = new MySqlCommand(query, con);
        cmd.Parameters.AddWithValue("@StudentID", studentId);

        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }
}

public Student GetStudentById(int id)
{
    Student s = new Student();

    using (MySqlConnection con = new MySqlConnection(connectionString))
    {
        string query = "SELECT * FROM Students WHERE ID=@ID";
        MySqlCommand cmd = new MySqlCommand(query, con);
        cmd.Parameters.AddWithValue("@ID", id);

        con.Open();
        var dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            s.ID = Convert.ToInt32(dr["ID"]);
            s.Name = dr["Name"].ToString();
            s.RollNo = dr["RollNo"].ToString();
            s.Class = dr["Class"].ToString();
        }
    }
    return s;
}

public void UpdateStudent(Student s)
{
    using (MySqlConnection con = new MySqlConnection(connectionString))
    {
        string query = "UPDATE Students SET Name=@Name, RollNo=@RollNo, Class=@Class WHERE ID=@ID";

        MySqlCommand cmd = new MySqlCommand(query, con);

        cmd.Parameters.AddWithValue("@Name", s.Name);
        cmd.Parameters.AddWithValue("@RollNo", s.RollNo);
        cmd.Parameters.AddWithValue("@Class", s.Class);
        cmd.Parameters.AddWithValue("@ID", s.ID);

        con.Open();
        cmd.ExecuteNonQuery();
    }
}
public void DeleteStudent(int id)
{
    using (MySqlConnection con = new MySqlConnection(connectionString))
    {
        string query = "DELETE FROM Students WHERE ID=@ID";
        MySqlCommand cmd = new MySqlCommand(query, con);

        cmd.Parameters.AddWithValue("@ID", id);

        con.Open();
        cmd.ExecuteNonQuery();
    }
}
public bool IsRollNoExists(string rollNo)
{
    using (MySqlConnection con = new MySqlConnection(connectionString))
    {
        string query = "SELECT COUNT(*) FROM Students WHERE RollNo=@RollNo";
        MySqlCommand cmd = new MySqlCommand(query, con);

        cmd.Parameters.AddWithValue("@RollNo", rollNo);

        con.Open();
        int count = Convert.ToInt32(cmd.ExecuteScalar());

        return count > 0;
    }
}
public Marks GetMarkById(int id)
{
    Marks m = new Marks();

    using (MySqlConnection con = new MySqlConnection(connectionString))
    {
        string query = "SELECT * FROM Marks WHERE MarkID=@id";
        MySqlCommand cmd = new MySqlCommand(query, con);
        cmd.Parameters.AddWithValue("@id", id);

        con.Open();
        var dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            m.MarkID = Convert.ToInt32(dr["MarkID"]);
            m.StudentID = Convert.ToInt32(dr["StudentID"]);
            m.SubjectName = dr["SubjectName"].ToString();
            m.MarksObtained = Convert.ToInt32(dr["MarksObtained"]);
            m.MaxMarks = Convert.ToInt32(dr["MaxMarks"]);
        }
    }

    return m;
}

public void UpdateMarks(Marks m)
{
    using (MySqlConnection con = new MySqlConnection(connectionString))
    {
        string query = "UPDATE Marks SET SubjectName=@sub, MarksObtained=@marks, MaxMarks=@max WHERE MarkID=@id";

        MySqlCommand cmd = new MySqlCommand(query, con);

        cmd.Parameters.AddWithValue("@sub", m.SubjectName);
        cmd.Parameters.AddWithValue("@marks", m.MarksObtained);
        cmd.Parameters.AddWithValue("@max", m.MaxMarks);
        cmd.Parameters.AddWithValue("@id", m.MarkID);

        con.Open();
        cmd.ExecuteNonQuery();
    }
}
}