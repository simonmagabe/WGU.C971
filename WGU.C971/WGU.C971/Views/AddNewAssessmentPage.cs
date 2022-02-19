using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace WGU.C971.Views
{
    public class AddNewAssessmentPage : ContentPage
    {
        public AddNewAssessmentPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Add New Assessment Page" }
                }
            };
        }
    }
}