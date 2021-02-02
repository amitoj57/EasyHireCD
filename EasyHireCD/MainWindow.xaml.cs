using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyHireCD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseManager objDB = new DatabaseManager();
        public String First_Name, User_Type;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtUserName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            {
                e.Handled = new Regex("[^a-zA-Z0-9]+").IsMatch(e.Text); //Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
            }
        }

        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtUserName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
         
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text != "" && pbPassword.Password != "")
            {
                DataTable DT = objDB.Login(txtUserName.Text, pbPassword.Password);
                if (DT != null && DT.Rows.Count > 0)
                {


                    DataRow row = DT.Rows[0];
                    String First_Name = row["FirstName"].ToString();
                    String Last_Name = row["LastName"].ToString();
                    String User_Type = row["UserType"].ToString();
                    //MessageBox.Show(First_Name + Last_Name + User_Type);
                    txtUserName.Clear();
                    pbPassword.Clear();
                    txtUserName.Focus();
                                                           
                    Rental_Page rent = new Rental_Page(First_Name, User_Type);
                    this.Hide();
                    rent.ShowDialog();
                                          
                }
                else
                {
                    MessageBox.Show(" Login Failed. Try Again");
                    txtUserName.Clear();
                    pbPassword.Clear();
                    txtUserName.Focus();
                }
            }
            else
            {
                MessageBox.Show("Enter User Name or Password");
                txtUserName.Focus();
            }
           
        }
    }
}


