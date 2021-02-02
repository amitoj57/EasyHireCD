using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;


namespace EasyHireCD
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public String UName,UserType;
        DatabaseManager objDB = new DatabaseManager();

        
        public Admin(String Uname,String UType)
        {
            InitializeComponent();
            UName = Uname;
            UserType = UType;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            if (UserType == "Admin") { btnEmpSave.IsEnabled = true;btnEmpEdit.IsEnabled = true;btnEmpDelete.IsEnabled = true;btnDelete.IsEnabled = true;btnDeleteMovie.IsEnabled = true; }
            if (UserType == "User") { btnEmpSave.IsEnabled = false ; btnEmpEdit.IsEnabled = false; btnEmpDelete.IsEnabled = false; btnDelete.IsEnabled = false; btnDeleteMovie.IsEnabled =false; }

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            
            try
            {   Load_Events(); }
            catch (Exception ex)         
            {
                MessageBox.Show("Database :" + ex.Message);
            }
         }    
                       
        private void txtSearchFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgAddCustomers.ItemsSource = objDB.ListCustomers(txtSearchFirstName.Text).DefaultView;
        }
        private void dgAddCustomers_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            
            if (dgAddCustomers.SelectedIndex > -1)
            {
                DataRowView customerrow = (DataRowView)dgAddCustomers.SelectedItems[0];
                string CID = Convert.ToString(customerrow["CustID"]);
                string CFirstName = Convert.ToString(customerrow["FirstName"]);
                string CLastName = Convert.ToString(customerrow["LastName"]);
                string CAddress = Convert.ToString(customerrow["Address"]);
                string CPhone = Convert.ToString(customerrow["Phone"]);
                txtCID.Text = CID;
                txtFirstName.Text = CFirstName;
                txtLastName.Text = CLastName;
                txtAddress.Text = CAddress;
                txtPhone.Text = CPhone;

            }
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.Message);
            }
        }
        //Add Customer
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

          
            if (dgAddCustomers.SelectedIndex > -1)
            {
                clearFieldCustomer();
                MessageBox.Show("Enter Customer Details ");
                txtFirstName.Focus();
            }
            else
            {
                  AddCustomers();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
               
        //Edit Customer 
        private void btnCustEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            if (dgAddCustomers.SelectedIndex > -1)
            {
                if (txtFirstName.Text != "" && txtLastName.Text != "" && txtAddress.Text != "" && txtPhone.Text != "")
                {
                    DataRowView customerrow = (DataRowView)dgAddCustomers.SelectedItems[0];
                    string CID = Convert.ToString(customerrow["CustID"]);
                    txtCID.Text = CID;//MessageBox.Show(CID);
                    int custID = Convert.ToInt16(txtCID.Text);
                    string CFirstName = txtFirstName.Text;
                    string CLastName = txtLastName.Text;
                    string CAddress = txtAddress.Text;
                    string CPhone = txtPhone.Text;

                    bool CustomerEdited = objDB.UpdateCustomer(txtFirstName.Text, txtLastName.Text, txtAddress.Text, txtPhone.Text, custID);
                    if (CustomerEdited == true)
                    {
                        MessageBox.Show("Customer Details Updated");
                        clearFieldCustomer();

                        dgAddCustomers.ItemsSource = objDB.ListCustomers("%").DefaultView;
                        txtSearchFirstName.Clear();
                    }

                }
                else
                {
                    MessageBox.Show("Complete All Fields");
                }
            }
            else
            {
                MessageBox.Show("Select a Customer From the List below to Update");
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        // Delete Customer
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            if (dgAddCustomers.SelectedIndex > -1)
            {
                MessageBoxResult dialogResult = MessageBox.Show("Are You Sure want to delete the Customer ? ", "Easy Hire", MessageBoxButton.YesNo);
                if (dialogResult.ToString() == "Yes")
                {
                    DataRowView row = (DataRowView)dgAddCustomers.SelectedItems[0];
                    Int32 CustID = Convert.ToInt32(row["CustID"]);

                    //To count no of movies customer rented so far
                    Int32 CustCount = objDB.GetCopiesOut(CustID); 
                    if (CustCount > 0)
                    {
                        MessageBox.Show("Can not Delete, Customer Rented  Movies");
                        clearFieldCustomer();
                        dgAddCustomers.ItemsSource = objDB.ListCustomers("%").DefaultView;
                    }
                    else
                    {

                        objDB.DeleteCustomer(CustID);
                        MessageBox.Show("Customer Deleted");
                        clearFieldCustomer();
                        dgAddCustomers.ItemsSource = objDB.ListCustomers("%").DefaultView;
                    }
                }
                else
                {
                    clearFieldCustomer();
                    dgAddCustomers.ItemsSource = objDB.ListCustomers("%").DefaultView;
                }
                           
            }
            else
            {
                MessageBox.Show("Select a Customer from the List to Delete");
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void txtSearchTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgAddMovies.ItemsSource = objDB.ListMovies(txtSearchTitle.Text).DefaultView;
        }

        private void dgAddMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

           
            if (dgAddMovies.SelectedIndex > -1)
            {
                DataRowView movierow = (DataRowView)dgAddMovies.SelectedItems[0];
                string MID = Convert.ToString(movierow["MovieID"]);
                String MRating = Convert.ToString(movierow["Rating"]);
                string MTitle = Convert.ToString(movierow["Title"]);
                string MYear = Convert.ToString(movierow["Year"]);
                string MRentalCost = Convert.ToString(movierow["Rental_Cost"]);
                string MCopies = Convert.ToString(movierow["Copies"]);
                string MPlot = Convert.ToString(movierow["Plot"]);
                string MGenre = Convert.ToString(movierow["Genre"]);
                txtMID.Text = MID;
                txtRating.Text = MRating;
                txtTitle.Text = MTitle;
                txtYear.Text = MYear;
                txtRentcost.Text = MRentalCost;
                txtCopies.Text = MCopies;
                txtPlot.Text = MPlot;
                txtGenre.Text = MGenre;
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void txtSearchFirstName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void txtSearchFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void txtSearchFirstName_KeyDown(object sender, KeyEventArgs e)
        {

        }
          

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
           
        }

       
        private void btnSaveMovie_Click(object sender, RoutedEventArgs e)
        {
            try
            {

           
            if (dgAddMovies.SelectedIndex > -1)
            {
                ClearFieldMovie();
                MessageBox.Show("Enter Movie Details ");
                txtTitle.Focus();
                dgAddMovies.SelectedIndex = -1;

            }
            else
            {
                checkMovieExist();
                
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        //Check Movie Exist or not
        private void checkMovieExist()
        {
            Int32 Title_Exist = objDB.CheckName(txtTitle.Text);
            if (Title_Exist > 0)
            {
                MessageBox.Show("Movie Name : " + txtTitle.Text+ " already Exist");
            }

            else {  AddNewMovie(); }
        }

        private void btnEditMovie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateMovies();
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.Message);
            }
           
        }

        /* FUNCTION TO ADD NEW CUSTOMER DETAILS IN TO THE DATABASE */

        private void AddCustomers()
        {
            if (txtFirstName.Text != "" && txtLastName.Text != "" && txtAddress.Text != "" && txtPhone.Text != "")
            {
                bool CustomerAdded = objDB.AddCustomer(txtFirstName.Text, txtLastName.Text, txtAddress.Text, txtPhone.Text);
                if (CustomerAdded == true)
                {
                    MessageBox.Show(" Customer Successfully Added ");
                    clearFieldCustomer();
                    dgAddCustomers.ItemsSource = objDB.ListCustomers("%").DefaultView;
                }
            }
            else
            {
                MessageBox.Show("Enter Customer Details");
            }
        }


        /* FUNCTION TO ADD NEW MOVIES IN TO THE DATABASE */

        private void AddNewMovie()
        {
            if ((txtTitle.Text.Trim()) != "" && (txtTitle.Text.Trim()) != " " && txtRentcost.Text != "" && txtPlot.Text != "" && txtYear.Text != "" && txtCopies.Text != "" && txtGenre.Text.Trim() != "" && txtRating.Text != "")
            {
                bool MovieAdded = objDB.AddMovies(txtTitle.Text.Trim(), txtRentcost.Text, txtPlot.Text, txtYear.Text, txtCopies.Text, txtGenre.Text, txtRating.Text);
                //(String title, String rentalcost, String plot, String year, String copies, String genre, String rating)
                if (MovieAdded == true)
                {
                    MessageBox.Show("Movie Successfully Added ");
                    ClearFieldMovie();
                    dgAddMovies.ItemsSource = objDB.ListMovies("%").DefaultView;
                }
            }
            else
            {
                MessageBox.Show("Enter Movie Details");
            }
        }

        private void UpdateMovies()
        {
            if (dgAddMovies.SelectedIndex > -1)
            {
                if (txtTitle.Text != "" && txtRentcost.Text != "" && txtPlot.Text != "" && txtYear.Text != "" && txtCopies.Text != "" && txtGenre.Text != "" && txtRating.Text != "")
                {
                    DataRowView movierrow = (DataRowView)dgAddMovies.SelectedItems[0];
                    int movieID = Convert.ToInt16(movierrow["MovieID"]);

                    bool MovieEdited = objDB.UpdateMovie(txtTitle.Text, txtRentcost.Text, txtPlot.Text, txtYear.Text, txtCopies.Text, txtGenre.Text, txtRating.Text, movieID);
                    if (MovieEdited == true)
                    {
                        MessageBox.Show(" Movie Details Updated ");
                        ClearFieldMovie(); //Function call to clear Textboxes
                        dgAddMovies.ItemsSource = objDB.ListMovies("%").DefaultView;
                        txtSearchTitle.Clear();
                    }

                }
                else
                {
                    MessageBox.Show("Complete All Fields");
                }
            }
            else
            {
                MessageBox.Show("Select a Movie From the List below to Update");
            }
        }

        private void AddEmployees()
        {
            if (txtEmpFirstName.Text != "" && txtEmpLastName.Text != "" && txtEmpAddress.Text != "" && txtEmpPhone.Text != ""&& pbPassword.Password != "" && cbUserType.Text!="")
            {
                txtEmpUserName.Text = txtEmpFirstName.Text;
                bool EmployeeAdded = objDB.AddEmployee(txtEmpFirstName.Text.Trim(), txtEmpLastName.Text.Trim(), txtEmpAddress.Text.Trim(), txtEmpPhone.Text.Trim(), txtEmpUserName.Text.Trim(), pbPassword.Password.Trim(), cbUserType.Text);
                if (EmployeeAdded == true)
                {
                    MessageBox.Show(" Employee Successfully Added ");
                    clearFieldEmployee();
                    dgAddEmployee.ItemsSource = objDB.ListEmployee("%").DefaultView;
                }
            }
            else
            {
                MessageBox.Show("Enter Employee Details");
            }
        }

        private void ClearFieldMovie()
        {
            txtMID.Clear();
            txtTitle.Clear();
            txtRentcost.Clear();
            txtPlot.Clear();
            txtYear.Clear();
            txtCopies.Clear();
            txtGenre.Clear();
            txtRating.Clear();
        }
        private void clearFieldCustomer()
        {
            txtCID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
        }
        private void clearFieldEmployee()
        {
            txtCID.Clear();
            txtEmpFirstName.Clear();
            txtEmpLastName .Clear();
            txtEmpAddress.Clear();
            txtEmpPhone.Clear();
            txtEmpUserName.Clear();
            pbPassword.Clear();
        }


        private void tcAdmin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btnDeleteMovie_Click(object sender, RoutedEventArgs e)
        {
            try
            {

           
            if (dgAddMovies.SelectedIndex > -1)
            {
                MessageBoxResult dialogResult = MessageBox.Show("Are You Sure want to delete the Movie? ", "Easy Hire", MessageBoxButton.YesNo);
                if (dialogResult.ToString() == "Yes")
                {
                    DataRowView movierow = (DataRowView)dgAddMovies.SelectedItems[0];
                    
                    String MRating = Convert.ToString(movierow["Rating"]);
                    string MTitle = Convert.ToString(movierow["Title"]);
                    string MYear = Convert.ToString(movierow["Year"]);
                    string MRentalCost = Convert.ToString(movierow["Rental_Cost"]);
                    string MCopies = Convert.ToString(movierow["Copies"]);
                    string MPlot = Convert.ToString(movierow["Plot"]);
                    string MGenre = Convert.ToString(movierow["Genre"]);
                    Int32 MovieID = Convert.ToInt32(movierow["MovieID"]);
                    Int32 Tot_Copies= Convert.ToInt32(movierow["Copies"]);

                    Int32 Out_Copies = objDB.GetCopiesOut(MovieID); //MessageBox.Show(Out_Copies.ToString());
                    Int32 Avail_Copies = Tot_Copies - Out_Copies;

                    if (Out_Copies>0 && Tot_Copies!=0)
                    {
                        if (Tot_Copies == Avail_Copies)     //Delete All the Copies if None of the copies are rented out

                        {
                            
                            objDB.DeleteMovie(MovieID);
                            dgAddMovies.ItemsSource = objDB.ListMovies("%").DefaultView;
                        }
                       

                            if (Avail_Copies > 0)    //Delete Only the availbale Copies ( not the movie) and Update the Table 
                            {
                            MessageBoxResult dialogAvailableCopies = MessageBox.Show("Some Copies of the Movie is Rented Out. Do you want to delete the Available Copies? ", "Easy Hire", MessageBoxButton.YesNo);
                              if(dialogAvailableCopies.ToString() == "Yes")
                                {

                                String Copies = (Tot_Copies - Avail_Copies).ToString();
                                bool MovieEdited = objDB.UpdateMovie(MTitle, MRentalCost, MPlot, MYear, Copies, MGenre, MRating, MovieID);
                                if (MovieEdited == true)
                                {

                                    MessageBox.Show(" All Available Copies the Movie is Deleted");
                                    ClearFieldMovie(); //Function call to clear Textboxes
                                    dgAddMovies.ItemsSource = objDB.ListMovies("%").DefaultView;
                                    txtSearchTitle.Clear();
                                }
                                }
                            }
                            else
                            {
                                MessageBox.Show(Out_Copies.ToString("Can not Delete the Movie  : All Copies Rented Out"));
                            }
                          
                    }
                    else
                    {
                        objDB.DeleteMovie(MovieID);
                        MessageBox.Show("  Movie  Deleted ");
                        dgAddMovies.ItemsSource = objDB.ListMovies("%").DefaultView;
                        ClearFieldMovie();

                    }
                    
              }
                else
                {
                    //MessageBox.Show("Unable To Delete, Movie currently out");
                    ClearFieldMovie();

                    dgAddMovies.ItemsSource = objDB.ListMovies("%").DefaultView;
                }
            
               
            }
            else { MessageBox.Show("Select a Movie to Delete"); }
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.Message);
            }
        }

        private void txtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            {
                TextBox textBox = sender as TextBox;
                Int32 selectionStart = textBox.SelectionStart;
                Int32 selectionLength = textBox.SelectionLength;
                String newText = String.Empty;
                int count = 0;
                foreach (Char c in textBox.Text.ToCharArray())
                {
                    if (Char.IsDigit(c) || Char.IsControl(c) || (c == '.' && count == 0))
                    {
                        newText += c;
                        if (c == '.')
                            count += 1;
                    }
                }
                textBox.Text = newText;
                textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
            }
       }

    // Validation for Textbox which accepts Phone No. 
    // This textbox will only accepts Numbers
         

        private void txtFirstName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
           /* if (!Char.IsLetter((char)KeyInterop.VirtualKeyFromKey(e.Key)) & e.Key != Key.Back & e.Key != Key.Enter & e.Key != Key.Tab | e.Key == Key.Space| Char.IsNumber((char)KeyInterop.VirtualKeyFromKey(e.Key)))
            {
                e.Handled = true;
                // MessageBox.Show("I only accept numbers, sorry. :(", "This textbox says...");
            }
            */

        }
        //Add new EMployee
        private void btnEmpSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

          
            if (dgAddEmployee.SelectedIndex > -1)
            {
                
                clearFieldEmployee();
                MessageBox.Show("Enter Employee Details ");
                txtFirstName.Focus();
                dgAddEmployee.SelectedIndex = -1;

            }
            else
            {

                AddEmployees();
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        //Edit Employee
        private void btnEmpEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

           
            if (dgAddEmployee.SelectedIndex > -1)
            {
                if (txtEmpFirstName.Text != "" && txtEmpLastName.Text != "" && txtEmpAddress.Text != "" && txtEmpPhone.Text != "" && pbPassword.Password != "" && cbUserType.Text!="")
                {
                    DataRowView emprow = (DataRowView)dgAddEmployee.SelectedItems[0];
                    string EID = Convert.ToString(emprow["EmpID"]);
                    txtempID.Text = EID;//MessageBox.Show(CID);
                    int EmpID = Convert.ToInt16(txtempID.Text);
                    string CFirstName = txtFirstName.Text;
                    string CLastName = txtLastName.Text;
                    string CAddress = txtAddress.Text;
                    string CPhone = txtPhone.Text;
                    String CUserName = txtEmpUserName.Text;
                    String CPassword = pbPassword.Password;
                    String CUserType = cbUserType.Text;

                    //objDB.UpdateCustomer(CFirstName, CLastName, CAddress, CPhone, custID);
                    bool employeeEdited = objDB.UpdateEmployee(txtEmpFirstName.Text, txtEmpLastName.Text, txtEmpAddress.Text, txtEmpPhone.Text, pbPassword.Password,cbUserType.Text, EmpID);
                   // MessageBox.Show(pbPassword.Password);
                    if (employeeEdited == true)
                    {
                        MessageBox.Show("Employee Details Updated");
                        clearFieldEmployee();

                        dgAddEmployee.ItemsSource = objDB.ListEmployee("%").DefaultView;
                        txtEmpSearchFirstName.Clear();
                    }

                }
                else
                {
                    MessageBox.Show("Complete All Fields");
                }
            }
            else
            {
                MessageBox.Show("Select an Employee From the List below to Update");
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        
        // Delete Employee
        private void btnEmpDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {

           
            if (dgAddEmployee.SelectedIndex > -1)
            {
                MessageBoxResult dialogResult = MessageBox.Show("Are You Sure want to delete the Employee ? ", "Easy Hire", MessageBoxButton.YesNo);
                if (dialogResult.ToString() == "Yes")
                {
                    DataRowView row = (DataRowView)dgAddEmployee.SelectedItems[0];
                    int EmpID = Convert.ToInt32(row["EmpID"]);
                    //if (Convert.ToString(row["Available"]) != "No")
                    // {
                    objDB.DeleteEmployee(EmpID);
                    MessageBox.Show("Employee Deleted");
                    clearFieldEmployee();
                    dgAddEmployee.ItemsSource = objDB.ListEmployee("%").DefaultView;
                }
                else
                {
                    //MessageBox.Show("Unable To Delete, book currently out");
                    clearFieldEmployee();
                    dgAddEmployee.ItemsSource = objDB.ListEmployee("%").DefaultView;
                }

                
            }
            else
            {
                MessageBox.Show("Select an Employee from the List to Delete");
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgAddEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            
            if (dgAddEmployee.SelectedIndex > -1)
            {
                
                DataRowView emprow = (DataRowView)dgAddEmployee.SelectedItems[0];
                string EID = Convert.ToString(emprow["EmpID"]);
                string CFirstName = Convert.ToString(emprow["FirstName"]);
                string CLastName = Convert.ToString(emprow["LastName"]);
                string CAddress = Convert.ToString(emprow["Address"]);
                string CPhone = Convert.ToString(emprow["Phone"]);
              
                txtempID.Text = EID;
                txtEmpFirstName.Text = CFirstName;
                txtEmpLastName.Text = CLastName;
                txtEmpAddress.Text = CAddress;
                txtEmpPhone.Text = CPhone;
             

            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

    

        private void Exit1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EmpExit_Click(object sender, RoutedEventArgs e)
        {

            Rental_Page RP = new Rental_Page(UName, UserType);

            this.Close();
            RP.Show();
        }

      
        private void txtSearchFirstName1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtEmpSearchFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            dgAddEmployee.ItemsSource = objDB.ListEmployee(txtEmpSearchFirstName.Text).DefaultView;
        }

        

        //Form Load Events
        private void Load_Events()
        {
            cbUserType.Items.Add("User");
            cbUserType.Items.Add("Admin");
            txtCID.IsEnabled = false;
            txtempID.IsEnabled = false;
            txtSearchFirstName.Focus();
            dgAddCustomers.ItemsSource = objDB.ListCustomers("%").DefaultView;
            dgAddMovies.ItemsSource = objDB.ListMovies("%").DefaultView;
            dgAddEmployee.ItemsSource = objDB.ListEmployee("%").DefaultView;
        }

        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
             TextBox textBox = sender as TextBox;
            ValidationTextOnly(sender);
        }

        private void btnCustExit_Click(object sender, RoutedEventArgs e)
        {
            Rental_Page RP = new Rental_Page(UName, UserType);
            
            this.Close();
            RP.Show();
        }

        private void Exit_Copy_Click(object sender, RoutedEventArgs e)
        {

            Rental_Page RP = new Rental_Page(UName, UserType);

            this.Close();
            RP.Show();
        }

        private void txtYear_PreviewKeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txtYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumberValidation(sender, e);
        }

        private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void txtEmpPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumberValidation(sender, e);
        }

        private void txtCopies_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumberValidation(sender, e);
        }

        private void txtEmpFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            ValidationTextOnly(sender);
        }

        //Only Accepts Numbers
        private void NumberValidation(Object sender, TextChangedEventArgs e)
        {
            {
                TextBox textBox = sender as TextBox;
                Int32 selectionStart = textBox.SelectionStart;
                Int32 selectionLength = textBox.SelectionLength;
                String newText = String.Empty;
                int count = 0;
                foreach (Char c in textBox.Text.ToCharArray())
                {
                    if (Char.IsDigit(c) || Char.IsControl(c) || (c == '.' && count == 0))
                    {
                        newText += c;
                        if (c == '.')
                            count += 1;
                    }
                }
                textBox.Text = newText;
                textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
            }
        }

        private void txtEmpLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            ValidationTextOnly(sender);
        }

        private void ValidationTextOnly(Object sender)
        {
            {
                TextBox textBox = sender as TextBox;
                Int32 selectionStart = textBox.SelectionStart;
                Int32 selectionLength = textBox.SelectionLength;
                String newText = String.Empty;
                int count = 0;
                foreach (Char c in textBox.Text.ToCharArray())
                {
                    if (Char.IsLetter(c) )
                    {
                        newText += c;
                        if (c == '.')
                            count += 1;
                    }
                }
                textBox.Text = newText;
                textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
            }
        }

        private void txtEmpAddress_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox textBox = sender as TextBox;
            ValidationTextNumber(sender);
        }

        private void txtAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            ValidationTextNumber(sender);

        }

        private void txtLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            ValidationTextOnly(sender);
        }

        private void txtRentcost_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            validationNumber(sender);
        }

        private void ValidationTextNumber(Object sender)
        {
            {
                TextBox textBox = sender as TextBox;
                Int32 selectionStart = textBox.SelectionStart;
                Int32 selectionLength = textBox.SelectionLength;
                String newText = String.Empty;
                int count = 0;
                foreach (Char c in textBox.Text.ToCharArray())
                {
                    if (Char.IsLetter(c) || Char.IsDigit(c))
                    {
                        newText += c;
                        if (c == '.')
                            count += 1;
                    }
                }
                textBox.Text = newText;
                textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
            }
        }

        private void txtGenre_TextChanged(object sender, TextChangedEventArgs e)
        {
            {
                TextBox textBox = sender as TextBox;
                Int32 selectionStart = textBox.SelectionStart;
                Int32 selectionLength = textBox.SelectionLength;
                String newText = String.Empty;
                int count = 0;
                foreach (Char c in textBox.Text.ToCharArray())
                {
                    if (Char.IsLetter(c) || Char.IsControl(c) || (c == ',' && count == 0))
                    {
                        newText += c;
                        if (c == '.')
                            count += 1;
                    }
                }
                textBox.Text = newText;
                textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
            }
        }

        private void txtRating_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void validationNumber(Object sender)
        {
            {
                TextBox textBox = sender as TextBox;
                Int32 selectionStart = textBox.SelectionStart;
                Int32 selectionLength = textBox.SelectionLength;
                String newText = String.Empty;
                int count = 0;
                foreach (Char c in textBox.Text.ToCharArray())
                {
                    if (Char.IsDigit(c) || Char.IsControl(c) || (c == '.' && count == 0))
                    {
                        newText += c;
                        if (c == '.')
                            count += 1;
                    }
                }
                textBox.Text = newText;
                textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
            }
        }
    }
}

