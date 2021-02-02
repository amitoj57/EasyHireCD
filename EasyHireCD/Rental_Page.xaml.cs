using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Rental_Page.xaml
    /// </summary>
    public partial class Rental_Page : Window
    {
        public string UserName,UserType;
        DatabaseManager objDB = new DatabaseManager();
       
        public Rental_Page(String UName,String Utype)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            UserName = UName;
            UserType = Utype;
            lblDUser.Content = UserName;
            if (UserType == "Admin") { btnCancelTransaction.IsEnabled = true; }
            if (UserType == "User") { btnCancelTransaction.IsEnabled =false; }
            
        }


        private void dgMovies_Loaded(object sender, RoutedEventArgs e) 
        {
            dgMovies.ItemsSource = objDB.ListMovies("%").DefaultView;
            dgCustomers.ItemsSource = objDB.ListCustomers("%").DefaultView;
            dgRental.ItemsSource = objDB.ListRental("%").DefaultView;
        }

        private void dgMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            
            if (dgMovies.SelectedIndex > -1)
            {
                lblIRentalCost.Visibility = Visibility.Visible;
                lblDRentalCost.Visibility = Visibility.Visible;
                lblDYear.Visibility = Visibility.Visible; 
                lblIYear.Visibility = Visibility.Visible;
                lblDCopies.Visibility = Visibility.Visible;
                lblCopies.Visibility = Visibility.Visible;

                Double MRentalCost;
                DataRowView movierow = (DataRowView)dgMovies.SelectedItems[0];
                string MID = Convert.ToString(movierow["MovieID"]);
                String MRating = Convert.ToString(movierow["Rating"]);
                string MTitle = Convert.ToString(movierow["Title"]);
                string MYear = Convert.ToString(movierow["Year"]);
                if(MYear=="" || MYear== null) { MYear = "0"; }
                string MRental_Cost = Convert.ToString(movierow["Rental_Cost"]);
                if (MRental_Cost == "") { MRental_Cost = "0"; }
                lblDRentalCost.Content = "NZD " + MRental_Cost;
                
                MRentalCost = Convert.ToDouble(MRental_Cost); 
                string MCopies = Convert.ToString(movierow["Copies"]);
                string MPlot = Convert.ToString(movierow["Plot"]);
                string MGenre = Convert.ToString(movierow["Genre"]);
                if (MCopies == "") { MCopies = "0"; }
                if (Convert.ToInt16(MCopies) !=0)
                {
                    int RentOut_Copies = objDB.GetCopiesOut(Convert.ToInt16(MID));//To Count how many copies been rented out checking RentedMovies Table
                    //MessageBox.Show("Copies" + RentOut_Copies.ToString());
                    int Available_Copies = Convert.ToInt16(MCopies) - RentOut_Copies;
                    lblDCopies.Content = Available_Copies;
                }
                else
                {
                    MessageBox.Show(" Movie Copies are not available for Rental ");
                    lblDCopies.Content = "0";
                }
                lblDTitle.Content = MTitle;
                lblDYear.Content = MYear;

                lblDdate.Content = DateTime.Now.ToShortDateString();

                if (MYear != "" && MYear != "0")
                {

                    int Movie_Year = Convert.ToInt16(MYear);
                    int Current_Year = Convert.ToUInt16(DateTime.Now.Year.ToString());
                    int Year_Diff = Current_Year - Movie_Year;

                    if (Year_Diff <= 1)
                    {
                        if ( MRentalCost != 5.00) { 
                            MRentalCost = 5;
                            //Update the movie table for rental cost
                            bool CostUpdated = objDB.UpdateMovie(MTitle, MRentalCost.ToString(), MPlot, MYear, MCopies, MGenre, MRating, Convert.ToInt16(MID));
                            lblDRentalCost.Content = "NZD " + MRentalCost.ToString();
                            if (CostUpdated == true) { dgMovies.ItemsSource = objDB.ListMovies("%").DefaultView; }
                        }

                    }
                    else
                    {
                       if (MRentalCost != 2.00)
                        {
                            MRentalCost = 2.00;
                            bool CostUpdated = objDB.UpdateMovie(MTitle, MRentalCost.ToString(), MPlot, MYear, MCopies, MGenre, MRating, Convert.ToInt16(MID));
                            lblDRentalCost.Content = "NZD " + MRentalCost.ToString();
                            if (CostUpdated == true) { dgMovies.ItemsSource = objDB.ListMovies("%").DefaultView; ; }
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Exception in Grid Value :" + ex.Message);
            }
        }






        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {


                if (dgCustomers.SelectedIndex > -1)
                {

                    lblIMoviesRented.Visibility = Visibility.Visible;
                    lblDMoviesRented.Visibility = Visibility.Visible;
                    lblDPhone.Visibility = Visibility.Visible;
                    lblIPhone.Visibility = Visibility.Visible;

                    DataRowView customerrow = (DataRowView)dgCustomers.SelectedItems[0];
                    string CID = Convert.ToString(customerrow["CustID"]);
                    string CFirstName = Convert.ToString(customerrow["FirstName"]);
                    string CLastName = Convert.ToString(customerrow["LastName"]);
                    string CAddress = Convert.ToString(customerrow["Address"]);
                    string CPhone = Convert.ToString(customerrow["Phone"]);

                    lblDCustomerName.Content = CFirstName + " " + CLastName;
                    lblDAddress.Content = CAddress;
                    lblDPhone.Content = CPhone;

                    //To Count how many movies the customer rented so far
                    Int32 Cust_Rented_No = objDB.GetCopiesOut(Convert.ToInt16(CID));
                    lblDMoviesRented.Content = Cust_Rented_No.ToString();

                }
            }
            catch (Exception Ex)
            {

                MessageBox.Show("Exception : " + Ex.Message);
            }
        
        }

       private void btnCancelTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {

           
            String ReturnDate = DateTime.Now.ToString();
            if (dgRental.SelectedIndex > -1)
            {
                    MessageBoxResult dialogResult = MessageBox.Show("Are You Sure want to delete the Transaction? ", "Easy Hire", MessageBoxButton.YesNo);
                    if (dialogResult.ToString() == "Yes")
                    {
                         DataRowView rentalrow = (DataRowView)dgRental.SelectedItems[0];
                        int rmID = Convert.ToInt32(rentalrow["RMID"]);
                       bool movieReturned = objDB.ReturnMovie(rmID, Convert.ToDateTime(ReturnDate));
                        if (movieReturned == true)
                         {
                        MessageBox.Show("Movie Returned");
                        dgRental.ItemsSource = objDB.ListRental("%").DefaultView;
                          }
                      
                           objDB.DeleteRental(rmID);
                            MessageBox.Show("Transaction Cancelled");
                           dgRental.ItemsSource = objDB.ListRental("%").DefaultView;
    
                }
                    
                    else
                    {
                    dgRental.ItemsSource = objDB.ListRental("%").DefaultView;
                    }

                }
            else
             {
                    MessageBox.Show("Select a Rental Transaction  from the List ");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgRental_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            
            if (dgRental.SelectedIndex > -1)
            {
                lblIRentalCost.Visibility = Visibility.Hidden;
                lblDRentalCost.Visibility = Visibility.Hidden;
                lblDYear.Visibility = Visibility.Hidden;
                lblIYear.Visibility = Visibility.Hidden;
                lblDCopies.Visibility= Visibility.Hidden;
                lblCopies.Visibility = Visibility.Hidden;
                lblIMoviesRented.Visibility = Visibility.Hidden;
                lblDMoviesRented.Visibility = Visibility.Hidden;
                lblDPhone.Visibility = Visibility.Hidden;
                lblIPhone.Visibility = Visibility.Hidden;

                DataRowView rentedrow = (DataRowView)dgRental.SelectedItems[0];
                //string CID = Convert.ToString(rentedrow["CustID"]);
                string CFirstName = Convert.ToString(rentedrow["FirstName"]);
                string CLastName = Convert.ToString(rentedrow["LastName"]);
                string CAddress = Convert.ToString(rentedrow["Address"]);
                string MTitle = Convert.ToString(rentedrow["Title"]);
                String DRented= Convert.ToString(rentedrow["DateRented"]);
               
                String DToday = DateTime.Now.ToShortDateString();
                DateTime d1 = Convert.ToDateTime(DRented);
                DateTime d2 = Convert.ToDateTime(DToday);

                //To calculate how many days customer kept the movie with him
                TimeSpan ts = d2 - d1;
                int differenceInDays = ts.Days;
                string differenceAsString = differenceInDays.ToString();
                lblDDaysRented.Content= differenceAsString;
                         
                
                lblDCustomerName.Content = CFirstName + " " + CLastName;
                lblDAddress.Content = CAddress;
                
                lblDPhone.Content="";
                lblDTitle.Content = MTitle;          
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Exception Grid Value :" + ex.Message);
            }
        }

        private void txtSearchMovie_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgMovies.ItemsSource = objDB.ListMovies(txtSearchMovie.Text).DefaultView;
        }

        private void txtSearchCustomer_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgCustomers.ItemsSource = objDB.ListCustomers(txtSearchCustomer.Text).DefaultView;
        }

        private void txtRentedSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgRental.ItemsSource = objDB.ListRental(txtRentedSearch.Text).DefaultView;
        }

        private void btnReturnMovie_Click(object sender, RoutedEventArgs e)
        {
            try
            {

           
            String ReturnDate = DateTime.Now.ToString();
            if (dgRental.SelectedIndex > -1)
            {
                DataRowView rentalrow = (DataRowView)dgRental.SelectedItems[0];
                                            
                int rmID = Convert.ToInt32(rentalrow["RMID"]);
                bool movieReturned =objDB.ReturnMovie(rmID, Convert.ToDateTime(ReturnDate));
                if (movieReturned == true)
                    {
                        MessageBox.Show("Movie Returned");
                    dgRental.ItemsSource = objDB.ListRental("%").DefaultView;
                    txtRentedSearch.Clear();
                    dgMovies.ItemsSource = objDB.ListMovies("%").DefaultView;
                    dgCustomers.ItemsSource = objDB.ListCustomers("%").DefaultView;

                    }

                txtRentedSearch.Clear();
            }
            else
            {
                MessageBox.Show("Select a Movie to Return");
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnIssueMovie_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            int Avail_Copies;
            if (dgMovies.SelectedIndex > -1 && dgCustomers.SelectedIndex > -1)
            {
                DataRowView movierow = (DataRowView)dgMovies.SelectedItems[0];
                DataRowView customerrow= (DataRowView)dgCustomers.SelectedItems[0];
                int MovieID = Convert.ToInt32(movierow["MovieID"]);
                int CustID= Convert.ToInt32(customerrow["CustID"]);

                String  Movie_Copies = Convert.ToString(movierow["Copies"]);
                if (Movie_Copies == "") { Movie_Copies = "0"; }

                Avail_Copies = Convert.ToInt16(Movie_Copies);

                if ( Avail_Copies != 0)
                {
                    //MessageBox.Show("Available"+Avail_Copies.ToString());

                    Int32 RCopies = objDB.GetCopiesOut(MovieID);
                    //MessageBox.Show("Rented"+RCopies.ToString());
                    //To get Rented Copies
                    Int32 Cust_Rented_Copies = objDB.GetCopiesOut(CustID);
                    //MessageBox.Show("Customer Rented" + Cust_Rented_Copies.ToString());
                    if (Cust_Rented_Copies >= 2)
                    {
                        MessageBox.Show("Customer can Rent only 2 Movies at a time");
                    }
                    else
                    {
                        if (Avail_Copies > RCopies && Cust_Rented_Copies < 2)
                        {

                            String DateRented = DateTime.Now.ToShortDateString();

                            bool movieIssued = objDB.IssueMovie(MovieID, CustID, Convert.ToDateTime(DateRented));
                                if (movieIssued == true)
                              {
                                MessageBox.Show("Movie Issued");
                                dgMovies.ItemsSource = objDB.ListMovies("%").DefaultView;
                                dgRental.ItemsSource = objDB.ListRental("%").DefaultView;
                                txtSearchMovie.Clear();
                                txtSearchCustomer.Clear();

                             }
                        }
                        else
                        {
                            MessageBox.Show("Unable to issue, Movie currently out of Stock or Customer Limit of 2 Exceeded");

                        }
                    }
                }
                else
                { MessageBox.Show(" Movie Copies are not Available. Zero Copies in the System. "); }

                
                txtSearchMovie.Clear();
                txtSearchCustomer.Clear();
            }
            else
            {
                MessageBox.Show("Select a Movie and a Customer to issue");

            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
                        
        }

        private void btnAddMovie_Click(object sender, RoutedEventArgs e)
        {
            Admin ad = new Admin(UserName,UserType);
            Hide();
            ad.tcAdmin.SelectedIndex = 1;
            ad.ShowDialog();
           
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            Admin ad = new Admin(UserName, UserType);
            Hide();
            ad.tcAdmin.SelectedIndex = 0;
            ad.ShowDialog();

        }

        private void btnAdminPage_Click(object sender, RoutedEventArgs e)
        {
            Admin ad = new Admin(UserName, UserType);
           
            Hide();
            ad.tcAdmin.SelectedIndex = 0;
            ad.ShowDialog();
        }

        private void btnStatistics_Click(object sender, RoutedEventArgs e)
        {
            Statistics stat = new Statistics(UserName,UserType);
            Hide();
            stat.Show();
        }

        private void btnSRClear_Click(object sender, RoutedEventArgs e)
        {
            txtRentedSearch.Clear();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {

          MainWindow login = new MainWindow();
            login.Show();
            Close() ;

        }
       
    }
}
