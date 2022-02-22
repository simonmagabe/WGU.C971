using System;
using SQLite;
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

            Course course = new Course()
            {
                Name = "Test Course",
                TermId = term.Id,
                Status = "Anticipate To Take",
                StartDate = DateTime.Today.AddDays(4),
                EndDate = DateTime.Today.AddMonths(4).AddDays(4),
                InstructorName = "Simon Magabe",
                InstructorEmail = "smagabe@wgu.edu",
                InstructorPhone = "555-555-5555",
                Note = $"Welcome to WGU. This is Test Course."
            };

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.Insert(course);
            }

            Assessment performanceAssessment = new Assessment()
            {
                Name = "PAG1",
                CourseId = course.Id,
                StartDate = DateTime.Today.AddDays(10),
                EndDate = DateTime.Today.AddDays(10).AddHours(2.5),
                Type = "Performance"
            };


            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.Insert(performanceAssessment);
            }

            Assessment objectiveAssessment = new Assessment()
            {
                Name = "OBJ1",
                CourseId = course.Id,
                StartDate = DateTime.Today.AddDays(20),
                EndDate = DateTime.Today.AddDays(20).AddHours(2.5),
                Type = "Objective"
            };

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.Insert(objectiveAssessment);
            }
        }
    }
}
