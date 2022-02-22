using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using WGU.C971.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGU.C971.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermDetailPage : ContentPage
    {
        public Term Term;
        public MainPage MainPage;

        public TermDetailPage()
        {
            InitializeComponent();
        }

        public TermDetailPage(Term term, MainPage mainPage)
        {
            InitializeComponent();
            MainPage = mainPage;
            Term = term;
            Title = term.Name;
            CourseListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(CourseItemTapped);
        }

        private async void CourseItemTapped(object sender, ItemTappedEventArgs e)
        {
            Course selectedCourse = (Course)e.Item;
            await Navigation.PushModalAsync(new CourseDetailPage(MainPage, Term, selectedCourse));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LblTermStartDate.Text = Term.StartDate.Date.ToString("MM/dd/yyyy");
            LblTermEndDate.Text = Term.EndDate.ToString("MM/dd/yyyy");

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.CreateTable<Course>();
                string coursesQueryString = $"SELECT * FROM Course WHERE TermId = '{ Term.Id }'";
                List<Course> courses = connection.Query<Course>(coursesQueryString);
                CourseListView.ItemsSource = courses;
            }
        }

        private async void BtnAddNewCourse_Clicked(object sender, EventArgs e)
        {
            if (GetCourseCount() < 6)
            {
                await Navigation.PushModalAsync(new AddNewCoursePage(Term, MainPage));
            }
            else
            {
                string title = "Course Maximum Warning!";
                string message = "You cannot add more courses.\nA Maximum number of courses per Term reached.";
                await DisplayAlert(title, message, "OK");
            }
        }

        private async void BtnEditTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditTermPage(Term, MainPage));
        }

        private async void BtnDeleteTerm_Clicked(object sender, EventArgs e)
        {
            // Cascade Delete associated courses and their associated assessments
            try
            {
                string message = $"Are you sure you want to delete { Term.Name }?";
                var response = await DisplayAlert("Warning!", message, "Yes", "No");
                if (response)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
                    {
                        List<Course> courses = connection.Query<Course>($"SELECT * FROM Course WHERE TermId = '{ Term.Id }';");

                        foreach (Course course in courses)
                        {
                            List<Assessment> assessments = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE CourseId = '{ course.Id }';");
                            foreach (Assessment assessment in assessments)
                            {
                                connection.Delete(assessment);
                            }
                            connection.Delete(course);
                        }

                        connection.Delete(Term);
                        await Navigation.PopToRootAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private int GetCourseCount()
        {
            int count = 0;

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                string queryString = $"SELECT * FROM Course WHERE TermId = '{ Term.Id }';";
                List<Course> courses = connection.Query<Course>(queryString);
                count = courses.Count();
            }
                return count;
        }
    }
}