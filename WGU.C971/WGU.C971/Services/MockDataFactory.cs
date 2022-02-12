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
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(4)
            };
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.Insert(term);
            }

            Course course = new Course()
            {
                Name = "Software 2",
                TermId = term.Id,
                Status = "Plan To Take",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(4),
                InstructorName = "Jane Doe",
                InstructorEmail = "jdoe@wgu.edu",
                InstructorPhone = "555-555-5555",
                Note = "Welcome to Western Governors University. This is Software 2"
            };
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.Insert(course);
            }

            Assessment performanceAssessment = new Assessment()
            {
                Name = "PAG1",
                CourseId = course.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(2.5),
                Type = "Performance"
            };
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.Insert(performanceAssessment);
            }

            Assessment objectiveAssessment = new Assessment()
            {
                Name = "PAG1",
                CourseId = course.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(2.5),
                Type = "Performance"
            };
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.Insert(objectiveAssessment);
            }
        }
    }
}
