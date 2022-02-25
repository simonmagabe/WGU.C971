using Plugin.LocalNotification;
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
                CoursesList = connection.Table<Course>().ToList();
                connection.CreateTable<Assessment>();
                AssessmentList = connection.Table<Assessment>().ToList();
            }

            if (TermList.Count < 1)
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
            }

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                TermList = connection.Table<Term>().ToList();
                DegreePlanListView.ItemsSource = TermList;
                DisplayNotifications();
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
                        string courseStartTitle = "Course Start Notification";
                        string courseStartMessage = course.StartDate >= DateTime.Today ? $"{course.Name} is Scheduled to Start. \nStart Date: {course.StartDate}" : 
                            $"{course.Name} is Already Started. \nStart Date Was: {course.StartDate}";
                        
                        var courseStartNotification = new NotificationRequest
                        {
                            Title = courseStartTitle,
                            Description = courseStartMessage,
                            ReturningData = "Test Data",
                            NotificationId = course.Id,
                        };
                        NotificationCenter.Current.Show(courseStartNotification);

                        string courseEndTitle = "Course End Notification";
                        string courseEndMessage = course.EndDate >= DateTime.Today ? $"{course.Name} Ends On: {course.EndDate}" : 
                            $"{course.Name} Has Ended. \nEnd Date Was On: {course.EndDate}";

                        var courseEndNotification = new NotificationRequest
                        {
                            Title = courseEndTitle,
                            Description = courseEndMessage,
                        };
                        NotificationCenter.Current.Show(courseEndNotification);

                        // Assessments Start/End Date Notifications
                        string assessmentQueryString = $"SELECT * FROM Assessment WHERE CourseId = { course.Id };";
                        List<Assessment> assessments = connection.Query<Assessment>(assessmentQueryString);

                        foreach (Assessment assessment in assessments)
                        {
                            string assessmentStartTitle = "Assessment Start Notification";
                            string assessmentStartMessage = assessment.StartDate >= DateTime.Now ? $"{assessment.Name} is Scheduled to Start. \nStart Date: {assessment.StartDate}" :
                                $"{assessment.Name} is Already Started. \nStart Date Was: {assessment.StartDate}";

                            var assessmentStartNotification = new NotificationRequest
                            {
                                Title = assessmentStartTitle,
                                Description = assessmentStartMessage
                            };
                            NotificationCenter.Current.Show(assessmentStartNotification);

                            string assessmentEndTitle = "Assessment End Notification";
                            string assessmentEndMessage = assessment.EndDate >= DateTime.Now ? $"{assessment.Name} Ends On: {assessment.EndDate}" :
                                $"{assessment.Name} Has Ended. \nEnd Date Was On: {assessment.EndDate}";

                            var assessmentEndNotification = new NotificationRequest
                            {
                                Title = assessmentEndTitle,
                                Description = assessmentEndMessage,
                            };
                            NotificationCenter.Current.Show(assessmentEndNotification);
                        }
                    }
                }
            }
        }
    }
}