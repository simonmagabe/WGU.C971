using System;
using SQLite;
using System.Collections.Generic;
using System.Text;
using WGU.C971.Models;

namespace WGU.C971.Services
{
    internal class MockDataFactory
    {
        public static void GenerateSampleMockData(int termId)
        {
            Term term = new Term()
            {
                Name = $"Term {termId}",
                StartDate = DateTime.Today.AddDays(4),
                EndDate = DateTime.Today.AddMonths(4).AddDays(4)
            };
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.Insert(term);
            }

            List<Course> courses = new List<Course>();

            for (int i = 1; i <= 2; i++)
            {
                string name = $"Test Course {i}";

                Course course = new Course()
                {
                    Name = name,
                    TermId = term.Id,
                    Status = "Anticipate To Take",
                    StartDate = DateTime.Today.AddDays(4),
                    EndDate = DateTime.Today.AddMonths(4).AddDays(4),
                    InstructorName = "Simon Magabe",
                    InstructorEmail = "smagabe@wgu.edu",
                    InstructorPhone = "555-555-5555",
                    Note = $"Welcome to WGU. This is Test Course {i}"
                };
                courses.Add(course);
            }

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.InsertAll(courses);
            }
            
            List<Assessment> perfAssessments = new List<Assessment>();

            for (int i = 0; i < 2; i++)
            {
                Assessment performanceAssessment = new Assessment()
                {
                    Name = "PAG1",
                    CourseId = courses[i].Id,
                    StartDate = DateTime.Today.AddDays(4),
                    EndDate = DateTime.Today.AddDays(4).AddHours(2.5),
                    Type = "Performance"
                };
                perfAssessments.Add(performanceAssessment);
            }

            
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.InsertAll(perfAssessments);
            }

            List<Assessment> objectiveAssessments = new List<Assessment>();
            for (int i = 0; i < 2; i++)
            {
                Assessment objectiveAssessment = new Assessment()
                {
                    Name = "OBJ1",
                    CourseId = courses[i].Id,
                    StartDate = DateTime.Today.AddDays(4),
                    EndDate = DateTime.Today.AddDays(4).AddHours(2.5),
                    Type = "Objective"
                };
                objectiveAssessments.Add(objectiveAssessment);

            }
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.InsertAll(objectiveAssessments);
            }
        }
    }
}
