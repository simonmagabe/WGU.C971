using Plugin.LocalNotifications;
using SQLite;
using System;
using System.Collections.Generic;
using WGU.C971.Models;
using WGU.C971.Services;
using WGU.C971.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGU.C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage MainHomePage;
        public List<Term> TermList = new List<Term>();
        public List<Course> CoursesList = new List<Course>();
        public List<Assessment> AssessmentList = new List<Assessment>();

        public bool NeedMockData = true;

        public MainPage()
        {
            InitializeComponent();
            DegreePlanListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(ItemTapped);
            MainHomePage = this;
        }

        private async void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Term term = (Term)e.Item;
            await Navigation.PushAsync(new TermDetailPage(term, MainHomePage));
        }

        private void AddNewTermToolBarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AddNewTermPage(MainHomePage));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.CreateTable<Term>();
                TermList = connection.Table<Term>().ToList();
                connection.CreateTable<Course>();
                connection.CreateTable<Assessment>();
            }

            if (TermList.Count > 0 && NeedMockData)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
                {
                    connection.DropTable<Term>();
                    connection.DropTable<Course>();
                    connection.DropTable<Assessment>();

                    connection.CreateTable<Term>();
                    connection.CreateTable<Course>();
                    connection.CreateTable<Assessment>();

                    MockDataFactory.GenerateSampleMockData(1);
                }

                NeedMockData = false;
                DisplayNotifications();
            }
            else if (NeedMockData)
            {
                MockDataFactory.GenerateSampleMockData(3);
                DisplayNotifications();
                NeedMockData=false;
            }

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                TermList = connection.Table<Term>().ToList();
                DegreePlanListView.ItemsSource = TermList;
            }
        }

        private void DisplayNotifications()
        {
            foreach (Term term in TermList)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
                {
                    string courseQueryString = $"SELECT * FROM Course WHERE TermId = '{ term.Id }';";
                    List<Course> courses = connection.Query<Course>(courseQueryString);

                    foreach (Course course in courses)
                    {
                        double daysToStartDate = (course.StartDate - DateTime.Now).TotalDays;
                        double daysToEndDate = (course.EndDate - DateTime.Now).TotalDays;

                        if (daysToStartDate <= 7)
                        {
                            string title = "Course Due Soon Notification";
                            string message = $"{course.Name} Will Start Soon. \nStart Date: {course.StartDate}";
                            CrossLocalNotifications.Current.Show(title, message);
                        }

                        if (daysToEndDate <= 7)
                        {
                            string title = "Course End Soon Notification";
                            string message = $"{course.Name} Will End Soon. \nEnd Date: {course.EndDate}";
                            CrossLocalNotifications.Current.Show(title, message);
                        }

                        string assessmentQueryString = $"SELECT * FROM Assessment WHERE CourseId = '{ course.Id }'";
                        List<Assessment> assessments = connection.Query<Assessment>(assessmentQueryString);
                        foreach (Assessment assessment in assessments)
                        {
                            double daysToStartAssessment = (assessment.StartDate - DateTime.Now).TotalDays;
                            if (daysToStartAssessment <= 5)
                            {
                                string title = "Assessment Due Soon Notification";
                                string message = $"{ assessment.Name } is Due Soon. \nDate Due: {assessment.EndDate}";
                                CrossLocalNotifications.Current.Show(title, message);
                            }
                        }
                    }
                }
            }
        }
    }
}