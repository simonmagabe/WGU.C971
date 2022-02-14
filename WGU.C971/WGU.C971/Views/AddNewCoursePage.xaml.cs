﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGU.C971.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGU.C971.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewCoursePage : ContentPage
    {
        public Term Term;
        public MainPage MainPage;

        public AddNewCoursePage(Term term, MainPage mainPage)
        {
            InitializeComponent();
            Term = term;
            MainPage = mainPage;
        }

        private async void BtnSaveCourse_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (RequiredCourseEntryPopulated())
                {
                    ValidateStartAndEndDates();

                    Course course = new Course()
                    {
                        Name = TxtCourseName.Text,
                        Status = PickerCourseStatus.SelectedItem.ToString().Trim(),
                        StartDate = DatePickerStartDate.Date,
                        EndDate = DatePickerEndDate.Date,
                        InstructorName = TxtInstructorName.Text,
                        InstructorEmail = TxtInstructorEmail.Text,
                        InstructorPhone = TxtInstructorPhone.Text,
                        Note = TxtNotes.ToString().Trim(),
                        TermId = Term.Id
                    };

                    using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
                    {
                        connection.Insert(course);
                        MainPage.CoursesList.Add(course);
                        await Navigation.PopModalAsync();
                    }
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(TxtCourseName.Text))
                    {
                        throw new ApplicationException("Course Name is REQUIRED");
                    }

                    if (String.IsNullOrWhiteSpace(DatePickerStartDate.Date.ToString()))
                    {
                        throw new ApplicationException("Start Date is REQUIRED");
                    }

                    if (String.IsNullOrWhiteSpace(DatePickerEndDate.Date.ToString()))
                    {
                        throw new ApplicationException("End Date is REQUIRED");
                    }

                    if (String.IsNullOrWhiteSpace(TxtInstructorName.Text))
                    {
                        throw new ApplicationException("Instructor Name is REQUIRED");
                    }

                    if (String.IsNullOrWhiteSpace(TxtInstructorEmail.Text))
                    {
                        throw new ApplicationException("Instructor Email is REQUIRED");
                    }

                    if (String.IsNullOrWhiteSpace(TxtInstructorPhone.Text))
                    {
                        throw new ApplicationException("Instructor Phone is REQUIRED");
                    }
                }
            }
            catch (ApplicationException ex)
            {
                await DisplayAlert(Title, ex.Message, "OK");
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        private bool RequiredCourseEntryPopulated()
        {
            return !String.IsNullOrWhiteSpace(TxtCourseName.Text) &&
                !String.IsNullOrWhiteSpace(PickerCourseStatus.SelectedItem.ToString()) &&
                !String.IsNullOrWhiteSpace(DatePickerStartDate.Date.ToString()) && 
                !String.IsNullOrWhiteSpace(DatePickerEndDate.Date.ToString()) &&
                !String.IsNullOrWhiteSpace(TxtInstructorName.Text) &&
                !String.IsNullOrWhiteSpace(TxtInstructorEmail.Text) && 
                !String.IsNullOrWhiteSpace(TxtInstructorPhone.Text);
        }

        private void ValidateStartAndEndDates()
        {
            if (DatePickerStartDate.Date > DatePickerEndDate.Date)
            {
                string message = "Please Check the Start Date and End Date!\nSelected Start Date is Greater Than End Date";
                throw new ApplicationException(message);
            }

            if (DatePickerStartDate.Date < DateTime.Now)
            {
                string message = "Start Date Cannot Be a Past Date.\nPlease Select a Future Date.";
                throw new ApplicationException(message);
            }

            if (DatePickerStartDate.Date < Term.StartDate || 
                DatePickerStartDate.Date > Term.EndDate || 
                DatePickerEndDate.Date > Term.EndDate)
            {
                string message = "Please Check the Start Date and End Date!\n" +
                    "The course Must Start and End within the Term's Start and End Date.";
                throw new ApplicationException(message);
            }
        }
    }
}