using System;
using System.Collections.Generic;
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

            List<Course> courseList = new List<Course>();

            for (int i = 1; i <= 6; i++)
            {
                Course course = new Course()
                {
                    Name = $"Test Course {i}",
                    TermId = term.Id,
                    Status = "Anticipate To Take",
                    StartDate = DateTime.Today.AddDays(4),
                    EndDate = DateTime.Today.AddMonths(4).AddDays(4),
                    InstructorName = "Simon Magabe",
                    InstructorEmail = "smagabe@wgu.edu",
                    InstructorPhone = "555-555-5555",
                    Note = $"Welcome to WGU. This is Test Course {i}."
                };
                courseList.Add(course);
            }

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.InsertAll(courseList);
            }

            List<Assessment> perfAssessmentList = new List<Assessment>();

            for (int i = 1; i <= courseList.Count; i++)
            {
                Assessment performanceAssessment = new Assessment()
                {
                    Name = "PAG1",
                    CourseId = courseList[i].Id,
                    StartDate = DateTime.Today.AddDays(3),
                    EndDate = DateTime.Today.AddDays(3).AddHours(2.5),
                    Type = "Performance"
                };
                perfAssessmentList.Add(performanceAssessment);
            }
            
            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.InsertAll(perfAssessmentList);
            }

            List<Assessment> objAssessmentList = new List<Assessment>();
            for (int i = 1; i <= courseList.Count; i++)
            {
                Assessment objectiveAssessment = new Assessment()
                {
                    Name = "OBJ1",
                    CourseId = courseList[i].Id,
                    StartDate = DateTime.Today.AddDays(2),
                    EndDate = DateTime.Today.AddDays(2).AddHours(2.5),
                    Type = "Objective"
                };
                objAssessmentList.Add(objectiveAssessment);
            }

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.InsertAll(objAssessmentList);
            }
        }
    }
}
