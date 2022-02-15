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
    public partial class CourseDetailPage : ContentPage
    {
        public Course Course;
        public Term Term;
        public MainPage MainPage;

        public CourseDetailPage(Term term, Course course, MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
            Term = term;
            Course = course;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            TxtCourseName.Text = Course.Name;
            PickerCourseStatus.SelectedItem = Course.Status;
            DatePickerStartDate.Date = Course.StartDate;
            DatePickerEndDate.Date = Course.EndDate;
            TxtInstructorName.Text = Course.InstructorName;
            TxtInstructorEmail.Text = Course.InstructorEmail;
            TxtInstructorPhone.Text = Course.InstructorPhone;
            TxtNotes.Text = Course.Note;
        }

        private void BtnShareNote_Clicked(object sender, EventArgs e)
        {

        }

        private async void BtnSaveCourse_Clicked(object sender, EventArgs e)
        {
            // Validate the inputs
            // Save new course values to the course table
            await Navigation.PopModalAsync();
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void BtnAssessments_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnDelete_Clicked(object sender, EventArgs e)
        {

        }
    }
}