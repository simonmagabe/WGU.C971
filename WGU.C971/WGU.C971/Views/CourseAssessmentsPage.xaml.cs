using SQLite;
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
    public partial class CourseAssessmentsPage : ContentPage
    {
        public MainPage MainPage;
        public Course Course;
        public CourseDetailPage CourseDetailPage;

        public CourseAssessmentsPage(MainPage mainPage, Course course, CourseDetailPage courseDetailPage)
        {
            InitializeComponent();
            CourseDetailPage = courseDetailPage;
            MainPage = mainPage;
            Course = course;
            Title = course.Name;

            ListViewAssessments.ItemTapped += new EventHandler<ItemTappedEventArgs>(CourseItemTapped);
        }

        private async void CourseItemTapped(object sender, ItemTappedEventArgs e)
        {
            Assessment assessment = (Assessment)e.Item;
            await Navigation.PushModalAsync(new EditAssessmentPage(MainPage, Course, assessment));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.CreateTable<Assessment>();
                List<Assessment> assessments = connection.Query<Assessment>($"SELECT * FROM Assessment WHERE CourseId = { Course.Id };");
                ListViewAssessments.ItemsSource = assessments;
            }
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void BtnAddNewAssessment_Clicked(object sender, EventArgs e)
        {

        }
    }
}