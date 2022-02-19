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
    public partial class EditAssessmentPage : ContentPage
    {
        public MainPage MainPage;
        public Course Course;
        public Assessment Assessment;
        public List<string> AssessmentTypes = new List<string> { "Performance", "Objective" };

        public EditAssessmentPage(MainPage mainPage, Course course, Assessment assessment)
        {
            InitializeComponent();
            MainPage = mainPage;
            Course = course;
            Assessment = assessment;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            TxtAssessmentName.Text = Assessment.Name;
            PickerAssessmentType.ItemsSource = AssessmentTypes;
            PickerAssessmentType.SelectedItem = AssessmentTypes.Find(assessmentType => assessmentType == Assessment.Type);
            DatePickerStartDate.Date = Assessment.StartDate;
            DatePickerEndDate.Date = Assessment.EndDate;
            Title = Assessment.Name;
        }

        private async void BtnGoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}