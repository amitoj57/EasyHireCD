using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyHireCD
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        String UName, UserType;
        DatabaseManager objDB = new DatabaseManager();
        public Statistics(String Uname,String Utype)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbRentedMovies_Checked(object sender, RoutedEventArgs e)
        {
           // bool Rental = objDB.ListRental("%");a
            dgStat.ItemsSource = objDB.ListRental("%").DefaultView;
        }

        private void rbRegularCustomers_Checked(object sender, RoutedEventArgs e)
        {
            String SQLStatement = "Select Count(CustId) AS Rented_Movies, FirstName from Customer inner join RentedMovies R on R.CustIDFK = CustID Group by FirstName Order by Rented_Movies DESC";
            dgStat.ItemsSource = objDB.ListPopular(SQLStatement).DefaultView;

        }

        private void rbPopularVideos_Checked(object sender, RoutedEventArgs e)
        {
            String SQLStatement = "select Count(MovieId)AS POPULARITY, Title,Genre from Movies inner join RentedMovies R on R.MovieIDFK = MovieID Group by Title,Genre Order by POPULARITY DESC";
            dgStat.ItemsSource = objDB.ListPopular(SQLStatement).DefaultView;

        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Rental_Page RP = new Rental_Page(UName, UserType);

            this.Close();
            RP.Show();

           

        }

        private void rbListAll_Checked(object sender, RoutedEventArgs e)
        {
            String SQLStatement = " Select R.RMID,C.FirstName,C.LastName,C.Address, M.Title,M.Genre,R.dateRented, R.DateReturned,M.Rental_Cost from RentedMovies R inner join Movies M on R.MovieIDFK = M.MovieID inner join Customer C on R.CustIDFK = c.CustID ";
            dgStat.ItemsSource = objDB.ListPopular(SQLStatement).DefaultView;
        }

        private void radioButton_Copy_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void rbLatestMovies_Checked(object sender, RoutedEventArgs e)
        {
            String SQLStatement = "select Max(Year)AS Latest ,Title,Genre,Year from Movies Group by Title,Genre,Year order by Latest DESC";
            dgStat.ItemsSource = objDB.ListPopular(SQLStatement).DefaultView;
        }
    }
}
