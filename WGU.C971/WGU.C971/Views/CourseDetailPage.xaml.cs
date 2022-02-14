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
    }
}