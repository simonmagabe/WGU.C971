using System;
using SQLite;
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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LblTermStartDate.Text = Term.StartDate.ToString("MM/dd/yyyy");
            LblTermEndDate.Text = Term.EndDate.ToString("MM/dd/yyyy");

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.CreateTable<Course>();
                string coursesQueryString = $"SELECT * FROM Course WHERE TermId = '{ Term.Id }'";
                List<Course> courses = connection.Query<Course>(coursesQueryString);
                CourseListView.ItemsSource = courses;
            }
        }

        private void BtnAddNewCourse_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnEditTerm_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnDeleteTerm_Clicked(object sender, EventArgs e)
        {

        }

        private void CourseCell_Tapped(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}