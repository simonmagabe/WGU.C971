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

        public bool firstTime = true;

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

            DisplayDueDatesNotifications();

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                TermList = connection.Table<Term>().ToList();
                DegreePlanListView.ItemsSource = TermList;
            }
        }

        private async void DisplayDueDatesNotifications()
        {
            foreach (Term term in TermList)
            {
                var courses = CoursesList.FindAll(course => course.TermId == term.Id);

                foreach (Course course in courses)
                {
                    bool isDueInFiveDays = (course.StartDate - DateTime.Now).TotalDays <= 5;
                    bool isEdingInSevenDays = (course.EndDate - DateTime.Now).TotalDays <= 7;

                    if (isDueInFiveDays)
                    {
                        string title = "Course Start Notification";
                        string body = $"Course: {course.Name}\nStart Date: {course.StartDate}.";
                        
                        var courseStartNotification = new NotificationRequest
                        {
                            NotificationId = course.Id,
                            Title = title,
                            Description = body,
                            ReturningData = "Course is Starting Soon",
                            Schedule =
                            {
                                NotifyTime = DateTime.Now.AddSeconds(1),
                            }
                        };
                        await NotificationCenter.Current.Show(courseStartNotification);
                    }

                    if (isEdingInSevenDays)
                    {
                        var courseEndNotification = new NotificationRequest
                        {
                            NotificationId = (course.Id+1),
                            Title = "Course End Notification",
                            Description = $"Course: {course.Name}\nEnd Date: {course.EndDate}.",
                            ReturningData = "Course is Starting Soon",
                            Schedule =
                            {
                                NotifyTime = DateTime.Now.AddSeconds(1),
                            }
                        };
                        await NotificationCenter.Current.Show(courseEndNotification);
                    }

                    var assessments = AssessmentList.FindAll(assessment => assessment.CourseId == course.Id);
                    foreach (Assessment assessment in assessments)
                    {
                        bool isAssessmentInFiveDays = (assessment.StartDate - DateTime.Now).TotalDays <= 5;
                        if (isAssessmentInFiveDays)
                        {
                            string title = "Assessment Due Notification";
                            string body = $"{assessment.Name} is Due.\nStart Date: {assessment.StartDate}\nEnd Date: {assessment.EndDate}";
                            
                            var assessmentDueNotification = new NotificationRequest
                            {
                                NotificationId = (assessment.Id + 6),
                                Title = title,
                                Description = body,
                                ReturningData = "Assessment is Due Soon",
                                Schedule =
                            {
                                NotifyTime = DateTime.Now.AddSeconds(1),
                            }
                            };
                            await NotificationCenter.Current.Show(assessmentDueNotification);
                        }
                    }
                }
            }
        }
    }
}