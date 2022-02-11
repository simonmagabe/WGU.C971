using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGU.C971.Models;
using WGU.C971.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGU.C971
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void AddNewTermToolBarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection connection = new SQLiteConnection(App.FilePath))
            {
                connection.CreateTable<Term>();
                var termList = connection.Table<Term>().ToList();

                DegreePlanListView.ItemsSource = termList;
            }
        }
    }
}