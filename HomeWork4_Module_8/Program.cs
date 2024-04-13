using System;
using System.Collections.Generic;
using System.IO;

class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal AverageScore { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string inputFilePath = "D:\\downloads\\BinaryReadWrite-master";
        string outputDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Students";

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        List<Student> students = LoadStudentsFromBinaryFile(inputFilePath);

        if (students != null)
        {
            DistributeStudentsToGroups(students, outputDirectory);
        }
    }

    static List<Student> LoadStudentsFromBinaryFile(string filePath)
    {
        List<Student> students = new List<Student>();

        try
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    Student student = new Student();
                    student.Name = reader.ReadString();
                    student.Group = reader.ReadString();
                    student.DateOfBirth = DateTime.FromBinary(reader.ReadInt64());
                    student.AverageScore = reader.ReadDecimal();

                    students.Add(student);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
            return null;
        }

        return students;
    }

    static void DistributeStudentsToGroups(List<Student> students, string outputDirectory)
    {
        Dictionary<string, List<Student>> groups = new Dictionary<string, List<Student>>();

        foreach (var student in students)
        {
            if (!groups.ContainsKey(student.Group))
            {
                groups[student.Group] = new List<Student>();
            }
            groups[student.Group].Add(student);
        }

        foreach (var group in groups)
        {
            string groupFilePath = Path.Combine(outputDirectory, $"{group.Key}.txt");
            using (StreamWriter writer = new StreamWriter(groupFilePath))
            {
                foreach (var student in group.Value)
                {
                    writer.WriteLine($"{student.Name}, {student.DateOfBirth}, {student.AverageScore}");
                }
            }
        }

        Console.WriteLine("Данные успешно загружены и разложены по группам.");
    }
}
